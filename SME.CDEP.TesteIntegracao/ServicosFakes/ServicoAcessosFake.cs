﻿using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.TesteIntegracao.Constantes;

namespace SME.CDEP.TesteIntegracao.ServicosFakes;

public class ServicoAcessosFake : IServicoAcessos
{
    public Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
    {
        return Task.FromResult(new UsuarioAutenticacaoRetornoDTO()
        {
            Email = ConstantesTestes.EMAIL_INTERNO,
            Login = ConstantesTestes.LOGIN_99999999999,
            Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
        });
    }

    public Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UsuarioCadastradoCoreSSO(string login)
    {
        return Task.FromResult(login.Equals(ConstantesTestes.LOGIN_99999999998));
    }

    public Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha)
    {
        return Task.FromResult(true);
    }

    public Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
    {
        return Task.FromResult(true);
    }

    public Task<bool> AlterarSenha(string login, string senhaAtual, string senhaNova)
    {
        return Task.FromResult(true);
    }

    public Task<bool> AlterarEmail(string login, string email)
    {
        return Task.FromResult(true);
    }

    public Task<DadosUsuarioDTO> ObterMeusDados(string login)
    {
        return login switch
        {
            ConstantesTestes.LOGIN_99999999998 => Task.FromResult(new DadosUsuarioDTO()
            {
                Login = ConstantesTestes.LOGIN_99999999998,
                Nome = ConstantesTestes.USUARIO_EXTERNO_99999999998,
                Email = ConstantesTestes.EMAIL_EXTERNO,
            }),
            ConstantesTestes.LOGIN_99999999999 => Task.FromResult(new DadosUsuarioDTO()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                Email = ConstantesTestes.EMAIL_INTERNO,
                Endereco = ConstantesTestes.RUA_99999999999,
                Numero = ConstantesTestes.NUMERO_99,
                Complemento = ConstantesTestes.COMPLEMENTO_CASA_99,
                Cep = ConstantesTestes.CEP_88058999,
                Cidade = ConstantesTestes.CIDADE_99999999999,
                Estado = ConstantesTestes.ESTADO_SC,
                Telefone = ConstantesTestes.TELEFONE_99_99999_9999,
                Bairro = ConstantesTestes.BAIRRO_99999999999,
            }),
            _ => Task.FromResult(new DadosUsuarioDTO())
        };
    }

    public Task<string> SolicitarRecuperacaoSenha(string login)
    {
        return Task.FromResult(string.Empty);
    }

    public Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token)
    {
        return Task.FromResult(true);
    }

    public Task<string> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto)
    {
        return Task.FromResult(string.Empty);
    }
}
    