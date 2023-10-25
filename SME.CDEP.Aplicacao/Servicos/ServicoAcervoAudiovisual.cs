using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoAudiovisual : IServicoAcervoAudiovisual
    {
        private readonly IRepositorioAcervoAudiovisual repositorioAcervoAcervoAudiovisual;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        
        public ServicoAcervoAudiovisual(
            IRepositorioAcervoAudiovisual repositorioAcervoAcervoAudiovisual, 
            IMapper mapper,
            ITransacao transacao,
            IServicoAcervo servicoAcervo)
        {
            this.repositorioAcervoAcervoAudiovisual = repositorioAcervoAcervoAudiovisual ?? throw new ArgumentNullException(nameof(repositorioAcervoAcervoAudiovisual));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
        }

        public async Task<long> Inserir(AcervoAudiovisualCadastroDTO acervoAudiovisualCadastroDto)
        {
            var acervo = mapper.Map<Acervo>(acervoAudiovisualCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.Audiovisual;
            acervo.Codigo = ObterCodigoAcervo(acervo.Codigo);
            
            var acervoAudiovisual = mapper.Map<AcervoAudiovisual>(acervoAudiovisualCadastroDto);
            
            var tran = transacao.Iniciar();
            try
            {
                var retornoAcervo = await servicoAcervo.Inserir(acervo);
                acervoAudiovisual.AcervoId = retornoAcervo;
                
                await repositorioAcervoAcervoAudiovisual.Inserir(acervoAudiovisual);
                
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
          
            return acervoAudiovisual.AcervoId;
        }

        private string ObterCodigoAcervo(string codigo)
        {
            return codigo.ContemSigla(Constantes.SIGLA_ACERVO_AUDIOVISUAL) 
                ? codigo
                : $"{codigo}{Constantes.SIGLA_ACERVO_AUDIOVISUAL}";
        }
        
        public async Task<IEnumerable<AcervoAudiovisualDTO>> ObterTodos()
        {
            return (await repositorioAcervoAcervoAudiovisual.ObterTodos()).Select(s=> mapper.Map<AcervoAudiovisualDTO>(s));
        }

        public async Task<AcervoAudiovisualDTO> Alterar(AcervoAudiovisualAlteracaoDTO acervoAudiovisualAlteracaoDto)
        {
            var acervoArteGrafica = mapper.Map<AcervoAudiovisual>(acervoAudiovisualAlteracaoDto);
            var codigo = ObterCodigoAcervo(acervoAudiovisualAlteracaoDto.Codigo);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervoAudiovisualAlteracaoDto.AcervoId,
                    acervoAudiovisualAlteracaoDto.Titulo, 
                    acervoAudiovisualAlteracaoDto.Descricao, 
                    codigo, 
                    acervoAudiovisualAlteracaoDto.CreditosAutoresIds);
                
                await repositorioAcervoAcervoAudiovisual.Atualizar(acervoArteGrafica);
                
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

            return await ObterPorId(acervoAudiovisualAlteracaoDto.AcervoId);
        }

        public async Task<AcervoAudiovisualDTO> ObterPorId(long id)
        {
            var acervoAudiovisualSimples = await repositorioAcervoAcervoAudiovisual.ObterPorId(id);
            if (acervoAudiovisualSimples.NaoEhNulo())
            {
                acervoAudiovisualSimples.Codigo = acervoAudiovisualSimples.Codigo.RemoverSufixo();
                var acervoAudiovisualDto = mapper.Map<AcervoAudiovisualDTO>(acervoAudiovisualSimples);
                acervoAudiovisualDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoAudiovisualSimples);
                return acervoAudiovisualDto;
            }

            return default;
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }
    }
}