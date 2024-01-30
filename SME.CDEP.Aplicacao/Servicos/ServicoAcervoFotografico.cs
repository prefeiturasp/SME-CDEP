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
    public class ServicoAcervoFotografico : ServicoAcervoBase,IServicoAcervoFotografico
    {
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioAcervoFotograficoArquivo repositorioAcervoFotograficoArquivo;
        private readonly IRepositorioAcervoFotografico repositorioAcervoFotografico;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        private readonly IServicoMoverArquivoTemporario servicoMoverArquivoTemporario;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly IServicoGerarMiniatura servicoGerarMiniatura;
        private List<AcervoFotograficoArquivo> AcervoFotograficoArquivoInseridos;
        
        public ServicoAcervoFotografico(
            IRepositorioAcervo repositorioAcervo,
            IRepositorioAcervoFotografico repositorioAcervoFotografico, 
            IMapper mapper,
            ITransacao transacao,
            IServicoAcervo servicoAcervo,
            IRepositorioArquivo repositorioArquivo,
            IRepositorioAcervoFotograficoArquivo repositorioAcervoFotograficoArquivo,
            IServicoMoverArquivoTemporario servicoMoverArquivoTemporario,
            IServicoArmazenamento servicoArmazenamento,
            IServicoGerarMiniatura servicoGerarMiniatura) : 
            base(repositorioAcervo,
                repositorioArquivo,
                servicoMoverArquivoTemporario,
                servicoArmazenamento)
        {
            this.repositorioAcervoFotografico = repositorioAcervoFotografico ?? throw new ArgumentNullException(nameof(repositorioAcervoFotografico));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioAcervoFotograficoArquivo = repositorioAcervoFotograficoArquivo ?? throw new ArgumentNullException(nameof(repositorioAcervoFotograficoArquivo));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
            this.servicoGerarMiniatura = servicoGerarMiniatura ?? throw new ArgumentNullException(nameof(servicoGerarMiniatura));
            this.AcervoFotograficoArquivoInseridos = new List<AcervoFotograficoArquivo>();
        }

        public async Task<long> Inserir(AcervoFotograficoCadastroDTO acervoFotograficoCadastroDto)
        {
            ValidarPreenchimentoAcervoFotografico(acervoFotograficoCadastroDto.CreditosAutoresIds, 
                acervoFotograficoCadastroDto.Altura, acervoFotograficoCadastroDto.Largura);
            
            var arquivosCompletos =  acervoFotograficoCadastroDto.Arquivos.NaoEhNulo()
                ? await ObterArquivosPorIds(acervoFotograficoCadastroDto.Arquivos) 
                : Enumerable.Empty<Arquivo>();
            
            var acervo = mapper.Map<Acervo>(acervoFotograficoCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.Fotografico;
            acervo.Codigo = ObterCodigoAcervo(acervo.Codigo);
            
            var acervoFotografico = mapper.Map<AcervoFotografico>(acervoFotograficoCadastroDto);
            
            var tran = transacao.Iniciar();
            try
            {
                var retornoAcervo = await servicoAcervo.Inserir(acervo);
                acervoFotografico.AcervoId = retornoAcervo;
                
                await repositorioAcervoFotografico.Inserir(acervoFotografico);
                
                foreach (var arquivo in arquivosCompletos)
                {
                    var acervoFotograficoArquivo = new AcervoFotograficoArquivo()
                    {
                        ArquivoId = arquivo.Id, 
                        AcervoFotograficoId = acervoFotografico.Id
                    };
                    acervoFotograficoArquivo.Id = await repositorioAcervoFotograficoArquivo.Inserir(acervoFotograficoArquivo);
                    AcervoFotograficoArquivoInseridos.Add(acervoFotograficoArquivo);
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
            
            await MoverArquivosTemporarios(TipoArquivo.AcervoFotografico,arquivosCompletos);
            
            // if (arquivosCompletos.Any())
            //     await GerarMiniaturaEVinculoArquivo(arquivosCompletos);
          
            return acervoFotografico.AcervoId;
        }
        
        private void ValidarPreenchimentoAcervoFotografico(long[]? creditosAutoresIds, string? altura, string? largura)
        {
            if (creditosAutoresIds.EhNulo())
                throw new NegocioException(MensagemNegocio.CREDITO_OU_AUTORES_SAO_OBRIGATORIOS);

            if (largura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));

            if (altura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA));
        }
        
        private async Task GerarMiniaturaEVinculoArquivo(IEnumerable<Arquivo> arquivosAInserir)
        {
            foreach (var arquivo in arquivosAInserir)
            {
                if (arquivo.Nome.EhExtensaoImagemGerarMiniatura())
                {
                    var acervoFotograficoArquivo = AcervoFotograficoArquivoInseridos.FirstOrDefault(f => f.ArquivoId == arquivo.Id);
                    acervoFotograficoArquivo.ArquivoMiniaturaId = await servicoGerarMiniatura.GerarMiniatura(arquivo.TipoConteudo, arquivo.NomeArquivoFisico, arquivo.NomeArquivoFisicoMiniatura, arquivo.Tipo);
                    await repositorioAcervoFotograficoArquivo.Atualizar(acervoFotograficoArquivo);
                }
            }
        }

        private string ObterCodigoAcervo(string codigo)
        {
            return codigo.ContemSigla(Constantes.SIGLA_ACERVO_FOTOGRAFICO) 
                ? codigo
                : $"{codigo}{Constantes.SIGLA_ACERVO_FOTOGRAFICO}";
        }

        public async Task<IEnumerable<AcervoFotograficoDTO>> ObterTodos()
        {
            return (await repositorioAcervoFotografico.ObterTodos()).Select(s=> mapper.Map<AcervoFotograficoDTO>(s));
        }

        public async Task<AcervoFotograficoDTO> Alterar(AcervoFotograficoAlteracaoDTO acervoFotograficoAlteracaoDto)
        {
            ValidarPreenchimentoAcervoFotografico(acervoFotograficoAlteracaoDto.CreditosAutoresIds, 
                acervoFotograficoAlteracaoDto.Altura, acervoFotograficoAlteracaoDto.Largura);
            
            var arquivosIdsInserir =  Enumerable.Empty<long>();
            var arquivosIdsExcluir =  Enumerable.Empty<long>();
            
            var acervoFotografico = mapper.Map<AcervoFotografico>(acervoFotograficoAlteracaoDto);

            var acervoDTO = mapper.Map<AcervoDTO>(acervoFotograficoAlteracaoDto);
            acervoDTO.Codigo = ObterCodigoAcervo(acervoFotograficoAlteracaoDto.Codigo);
            
            var arquivosExistentes = (await repositorioAcervoFotograficoArquivo.ObterPorAcervoFotograficoId(acervoFotograficoAlteracaoDto.Id)).Select(s => s.ArquivoId).ToArray();
            (arquivosIdsInserir, arquivosIdsExcluir) = await ObterArquivosInseridosExcluidosMovidos(acervoFotograficoAlteracaoDto.Arquivos, arquivosExistentes);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervoDTO);
                
                await repositorioAcervoFotografico.Atualizar(acervoFotografico);
                
                foreach (var arquivo in arquivosIdsInserir)
                {
                    var acervoFotograficoArquivo = new AcervoFotograficoArquivo()
                    {
                        ArquivoId = arquivo,
                        AcervoFotograficoId = acervoFotografico.Id 
                    };
                    acervoFotograficoArquivo.Id = await repositorioAcervoFotograficoArquivo.Inserir(acervoFotograficoArquivo);
                    AcervoFotograficoArquivoInseridos.Add(acervoFotograficoArquivo);
                }

                await repositorioAcervoFotograficoArquivo.Excluir(arquivosIdsExcluir.ToArray(), acervoFotografico.Id);
                
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
            
            await MoverArquivosTemporarios(TipoArquivo.AcervoFotografico);
            
            // if (arquivosIdsInserir.Any())
            //     await GerarMiniaturaEVinculoArquivo(await ObterArquivosPorIds(arquivosIdsInserir.ToArray()));

            await ExcluirArquivosArmazenamento();

            return await ObterPorId(acervoFotograficoAlteracaoDto.AcervoId);
        }

        public async Task<AcervoFotograficoDTO> ObterPorId(long id)
        {
            var acervoFotograficoSimples = await repositorioAcervoFotografico.ObterPorId(id);
            if (acervoFotograficoSimples.NaoEhNulo())
            {
               acervoFotograficoSimples.Codigo = acervoFotograficoSimples.Codigo.RemoverSufixo();
               var acervoFotograficoDto = mapper.Map<AcervoFotograficoDTO>(acervoFotograficoSimples);
               acervoFotograficoDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoFotograficoSimples);
               return acervoFotograficoDto; 
            }
            return default;
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }
    }
}