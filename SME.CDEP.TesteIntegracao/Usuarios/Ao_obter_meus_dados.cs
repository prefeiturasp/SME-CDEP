using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.TesteIntegracao.ServicosFakes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_obter_meus_dados : TesteBase
    {
        public Ao_obter_meus_dados(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact(DisplayName = "Usuário - Obtendo dados de usuário externo")]
        public async Task ObterDadosUsuarioExterno()
        {
            CriarClaimUsuario();
            var usuarioInserir = new Dominio.Entidades.Usuario()
            {
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA,
                CriadoPor = ConstantesTestes.SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Login = faker.Person.FirstName,
                Nome = faker.Person.FullName,
                Endereco = faker.Address.FullAddress(),
                Numero = faker.Address.BuildingNumber(),
                Complemento = faker.Address.StreetSuffix(),
                Cep = faker.Address.ZipCode(),
                Cidade = faker.Address.City(),
                Estado = faker.Address.StateAbbr(),
                Telefone = faker.Phone.PhoneNumber("(##) #####-####"),
                Bairro = faker.Address.County(),
                TipoUsuario = ConstantesTestes.TIPO_USUARIO_SERVIDOR_PUBLICO
            };
            
            await InserirNaBase(usuarioInserir);

            var retorno = await GetServicoUsuario().ObterMeusDados(usuarioInserir.Login);
            retorno.Endereco.ShouldBe(usuarioInserir.Endereco);
            retorno.Numero.ShouldBe(usuarioInserir.Numero.ToString());
            retorno.Complemento.ShouldBe(usuarioInserir.Complemento);
            retorno.Cep.ShouldBe(usuarioInserir.Cep);
            retorno.Cidade.ShouldBe(usuarioInserir.Cidade);
            retorno.Bairro.ShouldBe(usuarioInserir.Bairro);
            retorno.Telefone.ShouldBe(usuarioInserir.Telefone);
        }
        
        [Fact(DisplayName = "Usuário - Obtendo dados de usuário interno")]
        public async Task ObterDadosUsuarioInterno()
        {
            CriarClaimUsuario();

            var usuario = new Dominio.Entidades.Usuario()
            {
                Login = faker.Person.FirstName,
                Nome = faker.Person.FullName,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA,
                CriadoPor = ConstantesTestes.SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                TipoUsuario = ConstantesTestes.TIPO_USUARIO_CORESSO
            };
            await InserirNaBase(usuario);

            var retorno = await GetServicoUsuario().ObterMeusDados(usuario.Login);
            retorno.Endereco.ShouldBeNull();
            retorno.Numero.ShouldBeNull();
            retorno.Complemento.ShouldBeNull();
            retorno.Cep.ShouldBeNull();
            retorno.Cidade.ShouldBeNull();
            retorno.Bairro.ShouldBeNull();
            retorno.Telefone.ShouldBeNull();
            retorno.Tipo.ShouldBe((int)TipoUsuario.CORESSO);
        }
    }
}