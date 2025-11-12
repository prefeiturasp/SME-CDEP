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
    public class ServicoAcervoArteGrafica : ServicoAcervoBase,IServicoAcervoArteGrafica
    {
        private readonly IRepositorioAcervoArteGraficaArquivo repositorioAcervoArteGraficaArquivo;
        private readonly IRepositorioAcervoArteGrafica repositorioAcervoArteGrafica;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        private readonly IServicoGerarMiniatura servicoGerarMiniatura;
        private List<AcervoArteGraficaArquivo> AcervoArteGraficaArquivoInseridos;
        
        public ServicoAcervoArteGrafica(
            IRepositorioAcervo repositorioAcervo,
            IRepositorioAcervoArteGrafica repositorioAcervoArteGrafica, 
            IMapper mapper,
            ITransacao transacao,
            IServicoAcervo servicoAcervo,
            IRepositorioArquivo repositorioArquivo,
            IRepositorioAcervoArteGraficaArquivo repositorioAcervoArteGraficaArquivo,
            IServicoMoverArquivoTemporario servicoMoverArquivoTemporario,
            IServicoArmazenamento servicoArmazenamento,
            IServicoGerarMiniatura servicoGerarMiniatura) : 
            base(repositorioAcervo,
                repositorioArquivo,
                servicoMoverArquivoTemporario,
                servicoArmazenamento)
        {
            this.repositorioAcervoArteGrafica = repositorioAcervoArteGrafica ?? throw new ArgumentNullException(nameof(repositorioAcervoArteGrafica));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioAcervoArteGraficaArquivo = repositorioAcervoArteGraficaArquivo ?? throw new ArgumentNullException(nameof(repositorioAcervoArteGraficaArquivo));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
            this.servicoGerarMiniatura = servicoGerarMiniatura ?? throw new ArgumentNullException(nameof(servicoGerarMiniatura));
            this.AcervoArteGraficaArquivoInseridos = new List<AcervoArteGraficaArquivo>();
        }

        public async Task<long> Inserir(AcervoArteGraficaCadastroDTO acervoArteGraficaCadastroDto)
        {
            ValidarPreenchimentoAcervoArteGrafica(acervoArteGraficaCadastroDto.Altura, acervoArteGraficaCadastroDto.Largura, acervoArteGraficaCadastroDto.Diametro);
            
            var arquivosCompletos =  acervoArteGraficaCadastroDto.Arquivos.NaoEhNulo()
                ? await ObterArquivosPorIds(acervoArteGraficaCadastroDto.Arquivos) 
                : Enumerable.Empty<Arquivo>();
            
            var acervo = mapper.Map<Acervo>(acervoArteGraficaCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.ArtesGraficas;
            acervo.Codigo = ObterCodigoAcervo(acervo.Codigo);
            
            var acervoArteGrafica = mapper.Map<AcervoArteGrafica>(acervoArteGraficaCadastroDto);
            
            var tran = transacao.Iniciar();
            try
            {
                var retornoAcervo = await servicoAcervo.Inserir(acervo);
                acervoArteGrafica.AcervoId = retornoAcervo;
                
                await repositorioAcervoArteGrafica.Inserir(acervoArteGrafica);
                
                foreach (var arquivo in arquivosCompletos)
                {
                    var arquivoArteGraficaArquivo = new AcervoArteGraficaArquivo()
                    {
                        ArquivoId = arquivo.Id, 
                        AcervoArteGraficaId = acervoArteGrafica.Id
                    };
                    arquivoArteGraficaArquivo.Id = await repositorioAcervoArteGraficaArquivo.Inserir(arquivoArteGraficaArquivo);
                    AcervoArteGraficaArquivoInseridos.Add(arquivoArteGraficaArquivo);
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

            await MoverArquivosTemporarios(TipoArquivo.AcervoArteGrafica,arquivosCompletos);

            // if (arquivosCompletos.Any())
            //     await GerarMiniaturaEVinculoArquivo(arquivosCompletos);
            
            return acervoArteGrafica.AcervoId;
        }
        
        private void ValidarPreenchimentoAcervoArteGrafica(string? altura, string? largura, string? diametro)
        {
            if (largura.EstaPreenchido() && largura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));

            if (altura.EstaPreenchido() && altura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA));

            if (diametro.EstaPreenchido() && diametro.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.DIAMETRO));
        }

        private string ObterCodigoAcervo(string codigo)
        {
            return codigo.ContemSigla(Constantes.SIGLA_ACERVO_ARTE_GRAFICA) 
                ? codigo
                : $"{codigo}{Constantes.SIGLA_ACERVO_ARTE_GRAFICA}";
        }

        public async Task<IEnumerable<AcervoArteGraficaDTO>> ObterTodos()
        {
            return (await repositorioAcervoArteGrafica.ObterTodos()).Select(s=> mapper.Map<AcervoArteGraficaDTO>(s));
        }

        public async Task<AcervoArteGraficaDTO> Alterar(AcervoArteGraficaAlteracaoDTO acervoArteGraficaAlteracaoDto)
        {
            ValidarPreenchimentoAcervoArteGrafica(acervoArteGraficaAlteracaoDto.Altura, acervoArteGraficaAlteracaoDto.Largura, acervoArteGraficaAlteracaoDto.Diametro);
            
            var arquivosIdsInserir =  Enumerable.Empty<long>();
            var arquivosIdsExcluir =  Enumerable.Empty<long>();
            
            var acervoArteGrafica = mapper.Map<AcervoArteGrafica>(acervoArteGraficaAlteracaoDto);

            var acervoDTO = mapper.Map<AcervoDTO>(acervoArteGraficaAlteracaoDto);
            acervoDTO.Codigo = ObterCodigoAcervo(acervoArteGraficaAlteracaoDto.Codigo);
            
            
            var arquivosExistentes = (await repositorioAcervoArteGraficaArquivo.ObterPorAcervoArteGraficaId(acervoArteGraficaAlteracaoDto.Id)).Select(s => s.ArquivoId).ToArray();
            (arquivosIdsInserir, arquivosIdsExcluir) = await ObterArquivosInseridosExcluidosMovidos(acervoArteGraficaAlteracaoDto.Arquivos, arquivosExistentes);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervoDTO);
                    
                await repositorioAcervoArteGrafica.Atualizar(acervoArteGrafica);
                
                foreach (var arquivo in arquivosIdsInserir)
                {
                    var arquivoArteGraficaArquivo = new AcervoArteGraficaArquivo()
                    {
                        ArquivoId = arquivo,
                        AcervoArteGraficaId = acervoArteGrafica.Id
                    };
                    arquivoArteGraficaArquivo.Id = await repositorioAcervoArteGraficaArquivo.Inserir(arquivoArteGraficaArquivo);
                    AcervoArteGraficaArquivoInseridos.Add(arquivoArteGraficaArquivo);
                }

                await repositorioAcervoArteGraficaArquivo.Excluir(arquivosIdsExcluir.ToArray(), acervoArteGrafica.Id);
                
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

            await MoverArquivosTemporarios(TipoArquivo.AcervoArteGrafica);

            await ExcluirArquivosArmazenamento();

            return await ObterPorId(acervoArteGraficaAlteracaoDto.AcervoId);
        }

        public async Task<AcervoArteGraficaDTO> ObterPorId(long id)
        {
            var acervoArteGraficaSimples = await repositorioAcervoArteGrafica.ObterPorId(id);
            if (acervoArteGraficaSimples is not null)
            {
                acervoArteGraficaSimples.Codigo = acervoArteGraficaSimples.Codigo.RemoverSufixo();
                var acervoArteGraficaDto = mapper.Map<AcervoArteGraficaDTO>(acervoArteGraficaSimples);
                acervoArteGraficaDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoArteGraficaSimples);
                return acervoArteGraficaDto;
            }

            return default!;
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }
    }
}