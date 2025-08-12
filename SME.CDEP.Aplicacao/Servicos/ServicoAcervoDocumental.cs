using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Extensions;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoDocumental : ServicoAcervoBase,IServicoAcervoDocumental
    {
        private readonly IRepositorioAcervoDocumentalArquivo repositorioAcervoDocumentalArquivo;
        private readonly IRepositorioAcervoDocumentalAcessoDocumento repositorioAcervoDocumentalAcessoDocumento;
        private readonly IRepositorioAcervoDocumental repositorioAcervoDocumental;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        private readonly IRepositorioAcessoDocumento repositorioAcessoDocumento;
        
        public ServicoAcervoDocumental(
            IRepositorioAcervo repositorioAcervo,
            IRepositorioAcervoDocumental repositorioAcervoDocumental, 
            IMapper mapper,
            ITransacao transacao,
            IServicoAcervo servicoAcervo,
            IRepositorioArquivo repositorioArquivo,
            IRepositorioAcessoDocumento repositorioAcessoDocumento,
            IRepositorioAcervoDocumentalArquivo repositorioAcervoDocumentalArquivo,
            IServicoMoverArquivoTemporario servicoMoverArquivoTemporario,
            IServicoArmazenamento servicoArmazenamento,
            IRepositorioAcervoDocumentalAcessoDocumento repositorioAcervoDocumentalAcessoDocumento) : 
            base(repositorioAcervo,
                repositorioArquivo,
                servicoMoverArquivoTemporario,
                servicoArmazenamento)
        {
            this.repositorioAcervoDocumental = repositorioAcervoDocumental ?? throw new ArgumentNullException(nameof(repositorioAcervoDocumental));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioAcervoDocumentalArquivo = repositorioAcervoDocumentalArquivo ?? throw new ArgumentNullException(nameof(repositorioAcervoDocumentalAcessoDocumento));
            this.repositorioAcervoDocumentalAcessoDocumento = repositorioAcervoDocumentalAcessoDocumento ?? throw new ArgumentNullException(nameof(repositorioAcervoDocumentalAcessoDocumento));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
            this.repositorioAcessoDocumento = repositorioAcessoDocumento ?? throw new ArgumentNullException(nameof(repositorioAcessoDocumento));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }

        public async Task<long> Inserir(AcervoDocumentalCadastroDTO acervoDocumentalCadastroDto)
        {
            ValidarPreenchimentoAcervoDocumental(acervoDocumentalCadastroDto.Altura, acervoDocumentalCadastroDto.Largura);
            
            var arquivosCompletos =  acervoDocumentalCadastroDto.Arquivos.NaoEhNulo()
                ? await ObterArquivosPorIds(acervoDocumentalCadastroDto.Arquivos) 
                : Enumerable.Empty<Arquivo>();
            
            var acessoDocumentosCompletos =  await repositorioAcessoDocumento.ObterPorIds(acervoDocumentalCadastroDto.AcessoDocumentosIds);
            
            var acervo = mapper.Map<Acervo>(acervoDocumentalCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.DocumentacaoTextual;
            
            var acervoDocumental = mapper.Map<AcervoDocumental>(acervoDocumentalCadastroDto);
            var tran = transacao.Iniciar();
            var urlCapaDocumento = "";
            try
            {
                var retornoAcervo = await servicoAcervo.Inserir(acervo);
                acervoDocumental.AcervoId = retornoAcervo;
                urlCapaDocumento = await ArmazenarImagemCapaDocumento(acervoDocumentalCadastroDto.CapaDocumento);
                acervoDocumental.CapaDocumento = urlCapaDocumento;

                await repositorioAcervoDocumental.Inserir(acervoDocumental);
                
                foreach (var arquivo in arquivosCompletos)
                {
                    await repositorioAcervoDocumentalArquivo.Inserir(new AcervoDocumentalArquivo()
                    {
                        ArquivoId = arquivo.Id, 
                        AcervoDocumentalId= acervoDocumental.Id
                    });
                }
                
                foreach (var acessoDocumento in acessoDocumentosCompletos)
                {
                    await repositorioAcervoDocumentalAcessoDocumento.Inserir(new AcervoDocumentalAcessoDocumento()
                    {
                        AcessoDocumentoId = acessoDocumento.Id, 
                        AcervoDocumentalId= acervoDocumental.Id
                    });
                }
                
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                if (urlCapaDocumento.EstaPreenchido())
                    await servicoArmazenamento.Excluir(urlCapaDocumento);
                throw;
            }
            finally
            {
                tran.Dispose();
            }

            await MoverArquivosTemporarios(TipoArquivo.AcervoDocumental,arquivosCompletos);
          
            return acervoDocumental.AcervoId;
        }

        private void ValidarPreenchimentoAcervoDocumental(string? altura, string? largura)
        {
            if (largura.EstaPreenchido() && largura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));

            if (altura.EstaPreenchido() && altura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA));
        }

        public async Task<IEnumerable<AcervoDocumentalDTO>> ObterTodos()
        {
            return (await repositorioAcervoDocumental.ObterTodos()).Select(s=> mapper.Map<AcervoDocumentalDTO>(s));
        }

        public async Task<AcervoDocumentalDTO> Alterar(AcervoDocumentalAlteracaoDTO acervoDocumentalAlteracaoDto)
        {
            ValidarPreenchimentoAcervoDocumental(acervoDocumentalAlteracaoDto.Altura, acervoDocumentalAlteracaoDto.Largura);
            
            var arquivosIdsInserir =  Enumerable.Empty<long>();
            var arquivosIdsExcluir =  Enumerable.Empty<long>();
            
            var acessosDocumentosIdsInserir =  Enumerable.Empty<long>();
            var acessosDocumentosIdsExcluir =  Enumerable.Empty<long>();
            
            var acervoDocumental = mapper.Map<AcervoDocumental>(acervoDocumentalAlteracaoDto);

            var arquivosExistentes = (await repositorioAcervoDocumentalArquivo.ObterPorAcervoDocumentalId(acervoDocumentalAlteracaoDto.Id)).Select(s => s.ArquivoId).ToArray();
            (arquivosIdsInserir, arquivosIdsExcluir) = await ObterArquivosInseridosExcluidosMovidos(acervoDocumentalAlteracaoDto.Arquivos, arquivosExistentes);
            
            var acessoDocumentosExistentes = (await repositorioAcervoDocumentalAcessoDocumento.ObterPorAcervoDocumentalId(acervoDocumentalAlteracaoDto.Id)).Select(s => s.AcessoDocumentoId).ToArray();
            (acessosDocumentosIdsInserir, acessosDocumentosIdsExcluir) = await ObterAcessoDocumentosInseridosExcluidos(acervoDocumentalAlteracaoDto.AcessoDocumentosIds, acessoDocumentosExistentes);

            var acervoDTO = mapper.Map<AcervoDTO>(acervoDocumentalAlteracaoDto);
            var tran = transacao.Iniciar();
            var urlCapaDocumento = "";
            try
            {
                acervoDTO = await servicoAcervo.Alterar(acervoDTO);
                urlCapaDocumento = await AtualizarImagemCapaDocumento(acervoDocumental.Id, acervoDocumentalAlteracaoDto.CapaDocumento);
                acervoDocumental.CapaDocumento = urlCapaDocumento;
                await repositorioAcervoDocumental.Atualizar(acervoDocumental);
                
                foreach (var arquivo in arquivosIdsInserir)
                {
                    await repositorioAcervoDocumentalArquivo.Inserir(new AcervoDocumentalArquivo()
                    {
                        ArquivoId = arquivo, 
                        AcervoDocumentalId = acervoDocumental.Id 
                    });
                }
                
                foreach (var acessoDocumento in acessosDocumentosIdsInserir)
                {
                    await repositorioAcervoDocumentalAcessoDocumento.Inserir(new AcervoDocumentalAcessoDocumento()
                    {
                        AcessoDocumentoId = acessoDocumento, 
                        AcervoDocumentalId= acervoDocumental.Id
                    });
                }

                await repositorioAcervoDocumentalArquivo.Excluir(arquivosIdsExcluir.ToArray(), acervoDocumental.Id);
                
                await repositorioAcervoDocumentalAcessoDocumento.Excluir(acessosDocumentosIdsExcluir.ToArray(), acervoDocumental.Id);
                
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                if (urlCapaDocumento.EstaPreenchido())
                    await servicoArmazenamento.Excluir(urlCapaDocumento);
                throw;
            }
            finally
            {
                tran.Dispose();
            }

            await MoverArquivosTemporarios(TipoArquivo.AcervoDocumental);

            await ExcluirArquivosArmazenamento();

            return await ObterPorId(acervoDocumentalAlteracaoDto.AcervoId);
        }

        private async Task<(IEnumerable<long>, IEnumerable<long>)> ObterAcessoDocumentosInseridosExcluidos(long[] acessoDocumentosAlterados, long[] acessoDocumentosExistentes)
        {
            var acessodocumentosIdsInserir = acessoDocumentosAlterados.Except(acessoDocumentosExistentes);
            var acessoDocumentosIdsExcluir = acessoDocumentosExistentes.Except(acessoDocumentosAlterados);
            
            return (acessodocumentosIdsInserir,acessoDocumentosIdsExcluir);
        }

        public async Task<AcervoDocumentalDTO> ObterPorId(long id)
        {
            var acervoDocumentalSimples = await repositorioAcervoDocumental.ObterComDetalhesPorId(id);
            if (acervoDocumentalSimples.NaoEhNulo())
            {
                var acervoDocumentalDto = mapper.Map<AcervoDocumentalDTO>(acervoDocumentalSimples);
                acervoDocumentalDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoDocumentalSimples);
                acervoDocumentalDto.CapaDocumento = await servicoAcervo.ObterImagemBase64(acervoDocumentalSimples.CapaDocumento);
                return acervoDocumentalDto;
            }

            return default;
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }

        private async Task<string> AtualizarImagemCapaDocumento(long idAcervoDocumental, string capaDocumentoBase64)
        {
            var acervoDocumental = await repositorioAcervoDocumental.ObterPorId(idAcervoDocumental);
            if (acervoDocumental == null) return await ArmazenarImagemCapaDocumento(capaDocumentoBase64);
            if (acervoDocumental.CapaDocumento.EstaPreenchido())
                await servicoArmazenamento.Excluir(acervoDocumental.CapaDocumento);
            return await ArmazenarImagemCapaDocumento(capaDocumentoBase64);
        }

        private async Task<string> ArmazenarImagemCapaDocumento(string capaDocumentoBase64)
        {
            if (capaDocumentoBase64.NaoEstaPreenchido()) return capaDocumentoBase64;
            var anexoInfo = capaDocumentoBase64.ObterContentTypeBase64EExtension();
            var bytes = Convert.FromBase64String(anexoInfo.base64Data);
            var stream = new MemoryStream(bytes);
            var nomeArquivo = $"capa_acervo_{Guid.NewGuid()}.{anexoInfo.extension}";
            await servicoArmazenamento.Armazenar(nomeArquivo, stream, anexoInfo.contentType);
            return nomeArquivo;
        }
    }
}