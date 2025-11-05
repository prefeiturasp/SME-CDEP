using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public static class UsuarioDTOMock
{
    public static Faker<UsuarioDTO> GerarUsuarioDTO(TipoUsuario tipoUsuario)
    {
        var faker = new Faker<UsuarioDTO>("pt_BR");
            
        faker.RuleFor(x => x.Login, f => f.Person.FirstName.Limite(45));
        faker.RuleFor(x => x.Nome, f => f.Lorem.Text().Limite(100));
        faker.RuleFor(x => x.Endereco, f => f.Address.FullAddress().Limite(200));
        faker.RuleFor(x => x.Numero, f => f.Address.BuildingNumber().Limite(4));
        faker.RuleFor(x => x.Complemento, f => f.Address.StreetSuffix().Limite(20));
        faker.RuleFor(x => x.Cep, f => f.Address.ZipCode());
        faker.RuleFor(x => x.Cidade, f => f.Address.City());
        faker.RuleFor(x => x.Estado, f => f.Address.StateAbbr());
        faker.RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber("(##) #####-####"));
        faker.RuleFor(x => x.Bairro, f => f.Address.County().Limite(200));
        faker.RuleFor(x => x.TipoUsuario, f => (int)tipoUsuario);
        faker.RuleFor(x => x.Instituicao, f => f.Company.CompanyName());
        return faker;
    }
}