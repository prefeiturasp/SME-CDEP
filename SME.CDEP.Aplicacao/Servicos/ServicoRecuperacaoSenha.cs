using System.Text.RegularExpressions;
using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoRecuperacaoSenha : IServicoRecuperacaoSenha 
    {
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoAcessos servicoAcessos;
        
        public ServicoRecuperacaoSenha(IServicoUsuario servicoUsuario,IServicoAcessos servicoAcessos) 
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAcessos = servicoAcessos ?? throw new ArgumentNullException(nameof(servicoAcessos));
        }

        public Task<string> SolicitarRecuperacaoSenha(string login)
        {
            var loginRecuperar = login.Replace(" ", "");
            return servicoAcessos.SolicitarRecuperacaoSenha(loginRecuperar);
        }

        public Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token)
        {
            return servicoAcessos.TokenRecuperacaoSenhaEstaValido(token);
        }

        public async Task<UsuarioAutenticacaoRetornoDTO> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
            var login = await servicoAcessos.AlterarSenhaComTokenRecuperacao(recuperacaoSenhaDto);
            return await servicoUsuario.Autenticar(login, recuperacaoSenhaDto.NovaSenha);
        }
    }
}
