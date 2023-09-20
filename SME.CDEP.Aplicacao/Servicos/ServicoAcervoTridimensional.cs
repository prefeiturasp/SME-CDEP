using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
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
        
        public ServicoAcervoTridimensional(
            IRepositorioAcervo repositorioAcervo,
            IRepositorioAcervoTridimensional repositorioAcervoTridimensional, 
            IMapper mapper,
            ITransacao transacao,
            IServicoAcervo servicoAcervo,
            IRepositorioArquivo repositorioArquivo,
            IRepositorioAcervoTridimensionalArquivo repositorioAcervoTridimensionalArquivo,
            IServicoMoverArquivoTemporario servicoMoverArquivoTemporario,
            IServicoArmazenamento servicoArmazenamento) : 
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
        }

        public async Task<long> Inserir(AcervoTridimensionalCadastroDTO acervoTridimensionalCadastroDto)
        {
            var arquivosCompletos =  acervoTridimensionalCadastroDto.Arquivos != null
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
                    await repositorioAcervoTridimensionalArquivo.Inserir(new AcervoTridimensionalArquivo()
                    {
                        ArquivoId = arquivo.Id, 
                        AcervoTridimensionalId= acervoTridimensional.Id
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

            await MoverArquivosTemporarios(TipoArquivo.AcervoTridimensional,arquivosCompletos);
          
            return acervoTridimensional.AcervoId;
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
            var arquivosIdsInserir =  Enumerable.Empty<long>();
            var arquivosIdsExcluir =  Enumerable.Empty<long>();
            
            var acervoTridimensional = mapper.Map<AcervoTridimensional>(acervoTridimensionalAlteracaoDto);
            var codigo = ObterCodigoAcervo(acervoTridimensionalAlteracaoDto.Codigo);
            
            var arquivosExistentes = (await repositorioAcervoTridimensionalArquivo.ObterPorAcervoTridimensionalId(acervoTridimensionalAlteracaoDto.Id)).Select(s => s.ArquivoId).ToArray();
            (arquivosIdsInserir, arquivosIdsExcluir) = await ObterArquivosInseridosExcluidosMovidos(acervoTridimensionalAlteracaoDto.Arquivos, arquivosExistentes);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervoTridimensionalAlteracaoDto.AcervoId,
                    acervoTridimensionalAlteracaoDto.Titulo, 
                    codigo, 
                    acervoTridimensionalAlteracaoDto.CreditosAutoresIds);
                
                await repositorioAcervoTridimensional.Atualizar(acervoTridimensional);
                
                foreach (var arquivo in arquivosIdsInserir)
                {
                    await repositorioAcervoTridimensionalArquivo.Inserir(new AcervoTridimensionalArquivo()
                    {
                        ArquivoId = arquivo, 
                        AcervoTridimensionalId = acervoTridimensional.Id 
                    });
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

            await MoverArquivosTemporarios(TipoArquivo.AcervoArteGrafica);

            await ExcluirArquivosArmazenamento();

            return await ObterPorId(acervoTridimensionalAlteracaoDto.AcervoId);
        }

        public async Task<AcervoTridimensionalDTO> ObterPorId(long id)
        {
            var acervoTridimensionalSimples = await repositorioAcervoTridimensional.ObterPorId(id);
            acervoTridimensionalSimples.Codigo = acervoTridimensionalSimples.Codigo.RemoverSufixo();
            var acervoTridimensionalDto = mapper.Map<AcervoTridimensionalDTO>(acervoTridimensionalSimples);
            acervoTridimensionalDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoTridimensionalSimples);
            return acervoTridimensionalDto;
        }

        public async Task<bool> Excluir(long id)
        {
            return await base.Excluir(id);
        }
    }
}