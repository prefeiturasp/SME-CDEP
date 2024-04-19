using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao.Arquivos.Mock
{
    public static class ArquivoSalvarMock
    {
        public static Arquivo GerarArquivoValido(TipoArquivo tipoArquivo)
        {
            var faker = new Faker<Arquivo>("pt_BR");
            faker.RuleFor(x => x.Nome, f => $"{f.Name.FirstName()}.jpeg");
            faker.RuleFor(x => x.Tipo, f => tipoArquivo.NaoEhNulo() ? tipoArquivo : f.PickRandom<TipoArquivo>());
            faker.RuleFor(x => x.TipoConteudo, f => "image/jpeg");
            faker.RuleFor(x => x.Codigo, f => Guid.NewGuid());
            faker.RuleFor(x => x.CriadoEm, f => DateTimeExtension.HorarioBrasilia());
            faker.RuleFor(x => x.CriadoLogin, f => f.Name.FirstName());
            faker.RuleFor(x => x.CriadoPor, f => f.Name.FullName());
            return faker.Generate();
        }
    }
}
