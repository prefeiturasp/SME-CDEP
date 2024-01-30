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
    public class ServicoAcervoTridimensional : ServicoAcervoBase,IServicoAcervoTridimensional
    {
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioAcervoTridimensionalArquivo repositorioAcervoTridimensionalArquivo;
        private readonly IRepositorioAcervoTridimensional repositorioAcervoTridimensional;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        private readonly IServicoMoverArquivoTemporario servicoMoverArquivoTemporario;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly IServicoGerarMiniatura servicoGerarMiniatura;
        private List<AcervoTridimensionalArquivo> AcervoTridimensionalArquivoInseridos;
        
        public ServicoAcervoTridimensional(
            IRepositorioAcervo repositorioAcervo,
            IRepositorioAcervoTridimensional repositorioAcervoTridimensional, 
            IMapper mapper,
            ITransacao transacao,
            IServicoAcervo servicoAcervo,
            IRepositorioArquivo repositorioArquivo,
            IRepositorioAcervoTridimensionalArquivo repositorioAcervoTridimensionalArquivo,
            IServicoMoverArquivoTemporario servicoMoverArquivoTemporario,
            IServicoArmazenamento servicoArmazenamento,
            IServicoGerarMiniatura servicoGerarMiniatura) : 
            base(repositorioAcervo,
                repositorioArquivo,
                servicoMoverArquivoTemporario,
                servicoArmazenamento)
        {
            this.repositorioAcervoTridimensional = repositorioAcervoTridimensional ?? throw new ArgumentNullException(nameof(repositorioAcervoTridimensional));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioAcervoTridimensionalArquivo = repositorioAcervoTridimensionalArquivo ?? throw new ArgumentNullException(nameof(repositorioAcervoTridimensionalArquivo));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
            this.servicoGerarMiniatura = servicoGerarMiniatura ?? throw new ArgumentNullException(nameof(servicoGerarMiniatura));
            AcervoTridimensionalArquivoInseridos = new List<AcervoTridimensionalArquivo>();
        }

        public async Task<long> Inserir(AcervoTridimensionalCadastroDTO acervoTridimensionalCadastroDto)
        {
            ValidarPreenchimentoAcervoTridimensional(acervoTridimensionalCadastroDto.CreditosAutoresIds, 
                acervoTridimensionalCadastroDto.Altura, acervoTridimensionalCadastroDto.Largura, acervoTridimensionalCadastroDto.Diametro, 
                acervoTridimensionalCadastroDto.Profundidade);
            
            var arquivosCompletos =  acervoTridimensionalCadastroDto.Arquivos.NaoEhNulo()
                ? await ObterArquivosPorIds(acervoTridimensionalCadastroDto.Arquivos) 
                : Enumerable.Empty<Arquivo>();
            
            var acervo = mapper.Map<Acervo>(acervoTridimensionalCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.Tridimensional;
            acervo.Codigo = ObterCodigoAcervo(acervo.Codigo);
            
            var acervoTridimensional = mapper.Map<AcervoTridimensional>(acervoTridimensionalCadastroDto);
            
            var tran = transacao.Iniciar();
            try
            {
                var retornoAcervo = await servicoAcervo.Inserir(acervo);
                acervoTridimensional.AcervoId = retornoAcervo;
                
                await repositorioAcervoTridimensional.Inserir(acervoTridimensional);
                
                foreach (var arquivo in arquivosCompletos)
                {
                    var acervoTridimensionalArquivo = new AcervoTridimensionalArquivo()
                    {
                        ArquivoId = arquivo.Id, 
                        AcervoTridimensionalId= acervoTridimensional.Id
                    };
                    acervoTridimensionalArquivo.Id = await repositorioAcervoTridimensionalArquivo.Inserir(acervoTridimensionalArquivo);
                    AcervoTridimensionalArquivoInseridos.Add(acervoTridimensionalArquivo);
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

            await MoverArquivosTemporarios(TipoArquivo.AcervoTridimensional,arquivosCompletos);
            
            // if (arquivosCompletos.Any())
            //     await GerarMiniaturaEVinculoArquivo(arquivosCompletos);
          
            return acervoTridimensional.AcervoId;
        }

        private async Task GerarMiniaturaEVinculoArquivo(IEnumerable<Arquivo> arquivosAInserir)
        {
            foreach (var arquivo in arquivosAInserir)
            {
                if (arquivo.Nome.EhExtensaoImagemGerarMiniatura())
                {
                    var acervoTridimensionalArquivo = AcervoTridimensionalArquivoInseridos.FirstOrDefault(f => f.ArquivoId == arquivo.Id);
                    acervoTridimensionalArquivo.ArquivoMiniaturaId = await servicoGerarMiniatura.GerarMiniatura(arquivo.TipoConteudo, arquivo.NomeArquivoFisico, arquivo.NomeArquivoFisicoMiniatura, arquivo.Tipo);
                    await repositorioAcervoTridimensionalArquivo.Atualizar(acervoTridimensionalArquivo);
                }
            }
        }
        
        private string ObterCodigoAcervo(string codigo)
        {
            return codigo.ContemSigla(Constantes.SIGLA_ACERVO_TRIDIMENSIONAL) 
                ? codigo
                : $"{codigo}{Constantes.SIGLA_ACERVO_TRIDIMENSIONAL}";
        }

        public async Task<IEnumerable<AcervoTridimensionalDTO>> ObterTodos()
        {
            return (await repositorioAcervoTridimensional.ObterTodos()).Select(s=> mapper.Map<AcervoTridimensionalDTO>(s));
        }

        public async Task<AcervoTridimensionalDTO> Alterar(AcervoTridimensionalAlteracaoDTO acervoTridimensionalAlteracaoDto)
        {
            ValidarPreenchimentoAcervoTridimensional(acervoTridimensionalAlteracaoDto.CreditosAutoresIds, 
                acervoTridimensionalAlteracaoDto.Altura, acervoTridimensionalAlteracaoDto.Largura, acervoTridimensionalAlteracaoDto.Diametro, 
                acervoTridimensionalAlteracaoDto.Profundidade);

            var arquivosIdsInserir =  Enumerable.Empty<long>();
            var arquivosIdsExcluir =  Enumerable.Empty<long>();
            
            var acervoTridimensional = mapper.Map<AcervoTridimensional>(acervoTridimensionalAlteracaoDto);

            var acervoDTO = mapper.Map<AcervoDTO>(acervoTridimensionalAlteracaoDto);
            acervoDTO.Codigo = ObterCodigoAcervo(acervoTridimensionalAlteracaoDto.Codigo);
            
            var arquivosExistentes = (await repositorioAcervoTridimensionalArquivo.ObterPorAcervoTridimensionalId(acervoTridimensionalAlteracaoDto.Id)).Select(s => s.ArquivoId).ToArray();
            (arquivosIdsInserir, arquivosIdsExcluir) = await ObterArquivosInseridosExcluidosMovidos(acervoTridimensionalAlteracaoDto.Arquivos, arquivosExistentes);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervoDTO);
                
                await repositorioAcervoTridimensional.Atualizar(acervoTridimensional);
                
                foreach (var arquivo in arquivosIdsInserir)
                {
                    var acervoTridimensionalArquivo = new AcervoTridimensionalArquivo()
                    {
                        ArquivoId = arquivo,
                        AcervoTridimensionalId = acervoTridimensional.Id
                    };
                    acervoTridimensionalArquivo.Id = await repositorioAcervoTridimensionalArquivo.Inserir(acervoTridimensionalArquivo);
                    AcervoTridimensionalArquivoInseridos.Add(acervoTridimensionalArquivo);
                }

                await repositorioAcervoTridimensionalArquivo.Excluir(arquivosIdsExcluir.ToArray(), acervoTridimensional.Id);
                
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

            await MoverArquivosTemporarios(TipoArquivo.AcervoTridimensional);
            
            // if (arquivosIdsInserir.Any())
            //     await GerarMiniaturaEVinculoArquivo(await ObterArquivosPorIds(arquivosIdsInserir.ToArray()));

            await ExcluirArquivosArmazenamento();

            return await ObterPorId(acervoTridimensionalAlteracaoDto.AcervoId);
        }

        private void ValidarPreenchimentoAcervoTridimensional(long[]? creditosAutoresIds, string? altura, string? largura, string? diametro, string? profundidade)
        {
            if (creditosAutoresIds.NaoEhNulo())
                throw new NegocioException(MensagemNegocio.ESSE_ACERVO_NAO_POSSUI_CREDITO_OU_AUTOR);

            if (largura.EstaPreenchido() && largura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));
            
            if (altura.EstaPreenchido() && altura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA));
            
            if (profundidade.EstaPreenchido() && profundidade.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.PROFUNDIDADE));
            
            if (diametro.EstaPreenchido() && diametro.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.DIAMETRO));
        }

        public async Task<AcervoTridimensionalDTO> ObterPorId(long id)
        {
            var acervoTridimensionalSimples = await repositorioAcervoTridimensional.ObterPorId(id);
            if (acervoTridimensionalSimples.NaoEhNulo())
            {
                acervoTridimensionalSimples.Codigo = acervoTridimensionalSimples.Codigo.RemoverSufixo();
                var acervoTridimensionalDto = mapper.Map<AcervoTridimensionalDTO>(acervoTridimensionalSimples);
                acervoTridimensionalDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoTridimensionalSimples);
                return acervoTridimensionalDto;
            }
            return default;
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }
    }
}