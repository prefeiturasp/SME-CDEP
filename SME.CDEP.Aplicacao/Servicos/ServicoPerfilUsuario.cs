﻿using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Dominio.Constantes;

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
            
            if (retorno.PerfilUsuario == null)
            {
                await VincularPerfilExternoCoreSSO(login,new Guid(Constantes.PERFIL_EXTERNO_GUID));
                retorno = await servicoAcessos.ObterPerfisUsuario(login);
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
