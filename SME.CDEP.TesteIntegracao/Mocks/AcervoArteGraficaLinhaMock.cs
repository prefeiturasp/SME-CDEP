using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Constantes;

namespace SME.CDEP.TesteIntegracao;

public static class AcervoArteGraficaLinhaMock
{
    public static Faker<AcervoArteGraficaLinhaDTO> GerarAcervoArteGraficaLinhaDTO()
        {
            var numeroLinhas = 1;
            var random = new Random();
            var faker = new Faker<AcervoArteGraficaLinhaDTO>("pt_BR");
            
            faker.RuleFor(x => x.NumeroLinha, f => numeroLinhas++);
            
            faker.RuleFor(x => x.Titulo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Codigo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Sentence().Limite(12)}.AG",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Credito, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(200)}|{f.Lorem.Sentence().Limite(200)}|{f.Lorem.Word().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                PermiteNovoRegistro = true,
            });
            
            faker.RuleFor(x => x.Localizacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(100),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_100
            });
            
            faker.RuleFor(x => x.Procedencia, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(200),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
            });
            
            faker.RuleFor(x => x.Data, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Date.Recent().Year.ToString(),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.CopiaDigital, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.OPCAO_SIM,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_3,
                EhCampoObrigatorio = true,
                ValoresPermitidos  = new List<string>() { ConstantesTestes.OPCAO_SIM, ConstantesTestes.OPCAO_NAO }
            });
            
            faker.RuleFor(x => x.PermiteUsoImagem, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.OPCAO_SIM,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_3,
                EhCampoObrigatorio = true,
                ValoresPermitidos  = new List<string>() { ConstantesTestes.OPCAO_SIM, ConstantesTestes.OPCAO_NAO }
            });
            
            faker.RuleFor(x => x.EstadoConservacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.OTIMO,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Cromia, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.COLOR,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Largura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Altura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Diametro, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Tecnica, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(100),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_100,
            });
            
            faker.RuleFor(x => x.Suporte, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.PAPEL,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Quantidade, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_INTEIRO,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Descricao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text(),
                EhCampoObrigatorio = true
            });
            
            return faker;
        }
}