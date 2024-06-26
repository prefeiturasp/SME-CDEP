﻿using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Integracoes.Interfaces;

public interface IServicoAcessos
{
    Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha);
    Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login, Guid? perfilUsuarioId);
    Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login);
    Task<bool> UsuarioCadastradoCoreSSO(string login);
    Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha);
    Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId);
    Task<DadosUsuarioDTO> ObterMeusDados(string login);
    Task<bool> AlterarSenha(string login, string senhaAtual, string senhaNova);
    Task<bool> AlterarEmail(string login, string email);
    Task<string> SolicitarRecuperacaoSenha(string login);
    Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token);
    Task<string> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto);
    Task<RetornoPerfilUsuarioDTO> RevalidarToken(string token);
    Task<IEnumerable<ResponsavelDTO>> ObterUsuariosComPerfisResponsavel(Guid[] perfis);
}