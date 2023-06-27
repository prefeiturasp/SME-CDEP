using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoPerfilUsuario : IServicoPerfilUsuario
    {
        private readonly IServicoAcessos servicoAcessos;
        
        public ServicoPerfilUsuario(IServicoAcessos servicoAcessos) 
        {
            this.servicoAcessos = servicoAcessos ?? throw new ArgumentNullException(nameof(servicoAcessos));
        }
        
        public Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
        {
            var retorno = servicoAcessos.ObterPerfisUsuario(login);
            return retorno;
        }

        public Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
        {
            var retorno = servicoAcessos.VincularPerfilExternoCoreSSO(login, perfilId);
            return retorno;
        }
    }
}
