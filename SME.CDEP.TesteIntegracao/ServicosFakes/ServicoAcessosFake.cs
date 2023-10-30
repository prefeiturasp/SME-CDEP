using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;

namespace SME.CDEP.TesteIntegracao.ServicosFakes;

public class ServicoAcessosFake : BaseMock, IServicoAcessos
{
    protected readonly Faker faker;

    public ServicoAcessosFake()
    {
        faker = new Faker("pt_BR");
    }
    
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
        return Task.FromResult(new RetornoPerfilUsuarioDTO()
        {
            Email = ConstantesTestes.EMAIL_INTERNO,
            Autenticado = true,
            Token = Guid.NewGuid().ToString(),
            PerfilUsuario = new List<PerfilUsuarioDTO>()
            {
                new PerfilUsuarioDTO()
                {
                    Perfil = new Guid(ConstantesTestes.PERFIL_EXTERNO_GUID),
                    PerfilNome = ConstantesTestes.PERFIL_EXTERNO_DESCRICAO
                }
            },
            UsuarioLogin = ConstantesTestes.LOGIN_99999999999,
            UsuarioNome = ConstantesTestes.USUARIO_INTERNO_99999999999,
            DataHoraExpiracao = DateTimeExtension.HorarioBrasilia().AddMinutes(20)
        });
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
                Email = faker.Person.Email,
            }),
            ConstantesTestes.LOGIN_99999999999 => Task.FromResult(new DadosUsuarioDTO()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                Email = faker.Person.Email,
                Endereco = faker.Address.FullAddress(),
                Numero = faker.Address.BuildingNumber(),
                Complemento = faker.Address.StreetSuffix(),
                Cep = faker.Address.ZipCode(),
                Cidade = faker.Address.City(),
                Estado = faker.Address.StateAbbr(),
                Telefone = faker.Phone.PhoneNumber("(##) #####-####"),
                Bairro = faker.Address.County(),
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
    