using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Constantes;

namespace SME.CDEP.TesteIntegracao;

public static class AcervoTridimensionalLinhaMock
{
    public static Faker<AcervoTridimensionalLinhaDTO> GerarAcervoTridimensionalLinhaDTO()
        {
            var numeroLinhas = 1;
            var random = new Random();
            var faker = new Faker<AcervoTridimensionalLinhaDTO>("pt_BR");
            
            faker.RuleFor(x => x.NumeroLinha, f => numeroLinhas++);
            
            faker.RuleFor(x => x.Titulo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Codigo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Sentence().Limite(12)}.TD",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Procedencia, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(200),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Ano, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Date.Recent().Year.ToString(),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_4,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Data, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Date.Recent().Year.ToString(),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.EstadoConservacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.OTIMO,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Quantidade, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(45,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_INTEIRO,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Descricao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text(),
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Largura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = "50,45",
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Altura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = "10.20",
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Profundidade, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = "1540",
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Diametro, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = "1801",
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            return faker;
        }
}