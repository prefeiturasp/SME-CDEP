using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoPerfilUsuario(IServicoAcessos servicoAcessos) : IServicoPerfilUsuario
    {
        private readonly IServicoAcessos servicoAcessos = servicoAcessos ?? throw new ArgumentNullException(nameof(servicoAcessos));

        public async Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
        {
            var retorno = await servicoAcessos.ObterPerfisUsuario(login);
            
            if (retorno.PerfilUsuario.NaoPossuiElementos())
            {
                await VincularPerfilExternoCoreSSO(login,new Guid(Constantes.PERFIL_EXTERNO_GUID));
                retorno = await servicoAcessos.ObterPerfisUsuario(login);
                if (retorno.PerfilUsuario.EhNulo())
                    throw new NegocioException(MensagemNegocio.NAO_FOI_POSSIVEL_VINCULAR_PERFIL_EXTERNO_CORESSO_USUARIO_SEM_PERFIL);
            }

            return retorno;
        }

        public Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
        {
            var retorno = servicoAcessos.VincularPerfilExternoCoreSSO(login, perfilId);
            return retorno;
        }
    }
}
