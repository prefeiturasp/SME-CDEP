using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Constantes;

namespace SME.CDEP.TesteIntegracao;

public static class AcervoBibliograficoLinhaMock
{
    public static  Faker<AcervoBibliograficoLinhaDTO> GerarAcervoBibliograficoLinhaDTO()
        {
            var numeroLinhas = 1;
            var random = new Random();
            var faker = new Faker<AcervoBibliograficoLinhaDTO>("pt_BR");
            
            faker.RuleFor(x => x.NumeroLinha, f => numeroLinhas++);
            
            faker.RuleFor(x => x.Titulo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            faker.RuleFor(x => x.SubTitulo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
            });
            faker.RuleFor(x => x.Material, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.LIVRO,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            faker.RuleFor(x => x.Autor, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(200)}|{f.Lorem.Sentence().Limite(200)}|{f.Lorem.Word().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                EhCampoObrigatorio = true
            });
            faker.RuleFor(x => x.CoAutor, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Person.FirstName.Limite(200)}|{f.Person.FullName.Limite(200)}|{f.Person.LastName.Limite(200)}|{f.Person.UserName.Limite(200)}|{f.Company.CompanyName().Limite(200)}|{f.Company.CompanySuffix().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
            });
            faker.RuleFor(x => x.TipoAutoria, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(15)}|{f.Lorem.Text().Limite(15)}|{f.Lorem.Text().Limite(15)}|{f.Lorem.Text().Limite(15)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
            });
            faker.RuleFor(x => x.Editora, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.EDITORA_A,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
            });
            faker.RuleFor(x => x.Assunto, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Word().Limite(200)}|{f.Lorem.Word().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                EhCampoObrigatorio = true
            });
            faker.RuleFor(x => x.Ano, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Date.Recent().Year.ToString(),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            faker.RuleFor(x => x.Edicao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            faker.RuleFor(x => x.NumeroPaginas, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_INTEIRO
            });
            faker.RuleFor(x => x.Largura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = "50,45",
                ValidarComExpressaoRegular = Dominio.Constantes.Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Dominio.Constantes.Constantes.LARGURA)
            });
            faker.RuleFor(x => x.Altura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = "10,20",
                ValidarComExpressaoRegular = Dominio.Constantes.Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Dominio.Constantes.Constantes.ALTURA)
            });
            faker.RuleFor(x => x.SerieColecao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.SERIE_COLECAO_A,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
            });
            faker.RuleFor(x => x.Volume, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            faker.RuleFor(x => x.Idioma, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.PORTUGUES,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            faker.RuleFor(x => x.LocalizacaoCDD, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(50),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                EhCampoObrigatorio = true
            });
            faker.RuleFor(x => x.LocalizacaoPHA, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(50),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                EhCampoObrigatorio = true
            });
            faker.RuleFor(x => x.NotasGerais, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
            });
            faker.RuleFor(x => x.Isbn, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(50),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
            });
            faker.RuleFor(x => x.Codigo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            return faker;
        }
}