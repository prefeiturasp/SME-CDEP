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
        
        public async Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
        {
            var retorno = await servicoAcessos.ObterPerfisUsuario(login);
            return retorno;
        }

        public async Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
        {
            var retorno = await servicoAcessos.VincularPerfilExternoCoreSSO(login, perfilId);
            return retorno;
        }
    }
}
