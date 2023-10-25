using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
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
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioAcervoDocumentalArquivo repositorioAcervoDocumentalArquivo;
        private readonly IRepositorioAcervoDocumentalAcessoDocumento repositorioAcervoDocumentalAcessoDocumento;
        private readonly IRepositorioAcervoDocumental repositorioAcervoDocumental;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        private readonly IServicoMoverArquivoTemporario servicoMoverArquivoTemporario;
        private readonly IServicoArmazenamento servicoArmazenamento;
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
        }

        public async Task<long> Inserir(AcervoDocumentalCadastroDTO acervoDocumentalCadastroDto)
        {
            var arquivosCompletos =  acervoDocumentalCadastroDto.Arquivos.NaoEhNulo()
                ? await ObterArquivosPorIds(acervoDocumentalCadastroDto.Arquivos) 
                : Enumerable.Empty<Arquivo>();
            
            var acessoDocumentosCompletos =  await repositorioAcessoDocumento.ObterPorIds(acervoDocumentalCadastroDto.AcessoDocumentosIds);
            
            var acervo = mapper.Map<Acervo>(acervoDocumentalCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.DocumentacaoHistorica;
            
            var acervoDocumental = mapper.Map<AcervoDocumental>(acervoDocumentalCadastroDto);
            
            var tran = transacao.Iniciar();
            try
            {
                var retornoAcervo = await servicoAcervo.Inserir(acervo);
                acervoDocumental.AcervoId = retornoAcervo;
                
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
                throw;
            }
            finally
            {
                tran.Dispose();
            }

            await MoverArquivosTemporarios(TipoArquivo.AcervoDocumental,arquivosCompletos);
          
            return acervoDocumental.AcervoId;
        }

        public async Task<IEnumerable<AcervoDocumentalDTO>> ObterTodos()
        {
            return (await repositorioAcervoDocumental.ObterTodos()).Select(s=> mapper.Map<AcervoDocumentalDTO>(s));
        }

        public async Task<AcervoDocumentalDTO> Alterar(AcervoDocumentalAlteracaoDTO acervoDocumentalAlteracaoDto)
        {
            var arquivosIdsInserir =  Enumerable.Empty<long>();
            var arquivosIdsExcluir =  Enumerable.Empty<long>();
            
            var acessosDocumentosIdsInserir =  Enumerable.Empty<long>();
            var acessosDocumentosIdsExcluir =  Enumerable.Empty<long>();
            
            var acervoDocumental = mapper.Map<AcervoDocumental>(acervoDocumentalAlteracaoDto);
            
            var arquivosExistentes = (await repositorioAcervoDocumentalArquivo.ObterPorAcervoDocumentalId(acervoDocumentalAlteracaoDto.Id)).Select(s => s.ArquivoId).ToArray();
            (arquivosIdsInserir, arquivosIdsExcluir) = await ObterArquivosInseridosExcluidosMovidos(acervoDocumentalAlteracaoDto.Arquivos, arquivosExistentes);
            
            var acessoDocumentosExistentes = (await repositorioAcervoDocumentalAcessoDocumento.ObterPorAcervoDocumentalId(acervoDocumentalAlteracaoDto.Id)).Select(s => s.AcessoDocumentoId).ToArray();
            (acessosDocumentosIdsInserir, acessosDocumentosIdsExcluir) = await ObterAcessoDocumentosInseridosExcluidos(acervoDocumentalAlteracaoDto.AcessoDocumentosIds, acessoDocumentosExistentes);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervoDocumentalAlteracaoDto.AcervoId,
                    acervoDocumentalAlteracaoDto.Titulo, 
                    acervoDocumentalAlteracaoDto.Descricao, 
                    acervoDocumentalAlteracaoDto.Codigo,
                    acervoDocumentalAlteracaoDto.CreditosAutoresIds,
                    acervoDocumentalAlteracaoDto.CodigoNovo);
                
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
            var acervoDocumentalSimples = await repositorioAcervoDocumental.ObterPorId(id);
            if (acervoDocumentalSimples.NaoEhNulo())
            {
                var acervoDocumentalDto = mapper.Map<AcervoDocumentalDTO>(acervoDocumentalSimples);
                acervoDocumentalDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoDocumentalSimples);
                return acervoDocumentalDto;
            }

            return default;
        }

        public async Task<bool> Excluir(long id)
        {
            return await base.Excluir(id);
        }
    }
}