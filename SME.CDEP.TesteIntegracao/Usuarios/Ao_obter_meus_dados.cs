using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.TesteIntegracao.ServicosFakes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
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
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_99999999998, 
                Nome = ConstantesTestes.USUARIO_EXTERNO_99999999998,
                Endereco = ConstantesTestes.RUA_99999999998,
                Numero = Int32.Parse(ConstantesTestes.NUMERO_98), 
                Complemento = ConstantesTestes.COMPLEMENTO_CASA_98, 
                Cep = ConstantesTestes.CEP_88058998,
                Cidade = ConstantesTestes.CIDADE_99999999998, 
                Estado = ConstantesTestes.ESTADO_SC, 
                Telefone = ConstantesTestes.TELEFONE_99_99999_9998, 
                Bairro = ConstantesTestes.BAIRRO_99999999998,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, 
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                TipoUsuario = ConstantesTestes.TIPO_USUARIO_SERVIDOR_PUBLICO
            });

            var retorno = await GetServicoUsuario().ObterMeusDados(ConstantesTestes.LOGIN_99999999998);
            retorno.Endereco.ShouldBe(ConstantesTestes.RUA_99999999998);
            retorno.Numero.ShouldBe(ConstantesTestes.NUMERO_98);
            retorno.Complemento.ShouldBe(ConstantesTestes.COMPLEMENTO_CASA_98);
            retorno.Cep.ShouldBe(ConstantesTestes.CEP_88058998);
            retorno.Cidade.ShouldBe(ConstantesTestes.CIDADE_99999999998);
            retorno.Bairro.ShouldBe(ConstantesTestes.BAIRRO_99999999998);
            retorno.Telefone.ShouldBe(ConstantesTestes.TELEFONE_99_99999_9998);
            retorno.Email.ShouldBe(ConstantesTestes.EMAIL_EXTERNO);
        }
        
        [Fact(DisplayName = "Usuário - Obtendo dados de usuário interno")]
        public async Task ObterDadosUsuarioInterno()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_99999999999, 
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, 
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                TipoUsuario = ConstantesTestes.TIPO_USUARIO_CORESSO
            });

            var retorno = await GetServicoUsuario().ObterMeusDados(ConstantesTestes.LOGIN_99999999999);
            retorno.Endereco.ShouldBe(ConstantesTestes.RUA_99999999999);
            retorno.Numero.ShouldBe(ConstantesTestes.NUMERO_99);
            retorno.Complemento.ShouldBe(ConstantesTestes.COMPLEMENTO_CASA_99);
            retorno.Cep.ShouldBe(ConstantesTestes.CEP_88058999);
            retorno.Cidade.ShouldBe(ConstantesTestes.CIDADE_99999999999);
            retorno.Bairro.ShouldBe(ConstantesTestes.BAIRRO_99999999999);
            retorno.Telefone.ShouldBe(ConstantesTestes.TELEFONE_99_99999_9999);
            retorno.Email.ShouldBe(ConstantesTestes.EMAIL_INTERNO);
        }
    }
}