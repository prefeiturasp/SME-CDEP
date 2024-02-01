using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public static class AcervoArteGraficaDTOMock
{
    public static Faker<AcervoArteGraficaCadastroDTO> GerarAcervoArteGraficaCadastroDTO()
    {
        var random = new Random();
        var faker = new Faker<AcervoArteGraficaCadastroDTO>("pt_BR");
            
        faker.RuleFor(x => x.Codigo, f => random.Next(1,499).ToString());
        faker.RuleFor(x => x.Titulo, f => f.Lorem.Text().Limite(500));
        faker.RuleFor(x => x.Descricao, f => f.Lorem.Text());
        faker.RuleFor(x => x.SubTitulo, f => f.Lorem.Text().Limite(500));
        faker.RuleFor(x => x.Localizacao, f => f.Lorem.Text().Limite(100));
        faker.RuleFor(x => x.Procedencia, f => f.Lorem.Text().Limite(200));
        faker.RuleFor(x => x.DataAcervo, f => f.Date.Recent().Year.ToString());
        faker.RuleFor(x => x.CopiaDigital, f => true);
        faker.RuleFor(x => x.PermiteUsoImagem, f => true);
        faker.RuleFor(x => x.Largura, f => "50,45");
        faker.RuleFor(x => x.Altura, f => "10,20");
        faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
        faker.RuleFor(x => x.CromiaId, f => random.Next(1,5));
        faker.RuleFor(x => x.Diametro, f => "15,55");
        faker.RuleFor(x => x.Tecnica, f => f.Lorem.Sentence().Limite(100));
        faker.RuleFor(x => x.SuporteId, f => random.Next(1,5));
        faker.RuleFor(x => x.Quantidade, f => random.Next(15,55));
        faker.RuleFor(x => x.CreditosAutoresIds, f => new long[]{1,2,3,4,5});
        faker.RuleFor(x => x.Arquivos, f => new long[]{random.Next(1,10),random.Next(1,10),random.Next(1,10),random.Next(1,10),random.Next(1,10)});
        faker.RuleFor(x => x.Ano, f => $"[{DateTimeExtension.HorarioBrasilia().Year}]");
        return faker;
    }
}