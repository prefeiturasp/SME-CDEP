using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.Mensageria.Exchange;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoMensageria : IServicoMensageria
    {
        private readonly IServicoMensageriaCDEP servicoMensageria;
        private readonly IServicoMensageriaMetricas servicoMensageriaMetricas;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMapper mapper;

        public ServicoMensageria(IServicoMensageriaCDEP servicoMensageria, IServicoMensageriaMetricas servicoMensageriaMetricas,
            IServicoUsuario servicoUsuario,IMapper mapper)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
            this.servicoMensageriaMetricas = servicoMensageriaMetricas ?? throw new ArgumentNullException(nameof(servicoMensageriaMetricas));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        } 

        public async Task<bool> Publicar(string rota, object filtros, Guid? codigoCorrelacao = null, Usuario usuarioLogado = null,
            bool notificarErroUsuario = false, string exchange = null)
        {
            var usuario = mapper.Map<UsuarioDTO>(usuarioLogado) ?? await ObterUsuario();
            
            var mensagem = new MensagemRabbit(filtros,
                codigoCorrelacao,
                usuario?.Nome,
                usuario?.Login,
                null,
                notificarErroUsuario);
            
            await servicoMensageria.Publicar(mensagem, rota, exchange ?? ExchangeRabbit.Cdep, "PublicarFilaCDEP");
            await servicoMensageriaMetricas.Publicado(rota);
            return true;
        }

        private async Task<UsuarioDTO> ObterUsuario()
        {
            try
            {
                return await servicoUsuario.ObterUsuarioLogado();
            }
            catch
            {
                return new UsuarioDTO();
            }
        }
    }
}