﻿using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Constantes;

namespace SME.CDEP.TesteIntegracao;

public static class AcervoFotograficoLinhaMock
{
    public static Faker<AcervoFotograficoLinhaDTO> GerarAcervoFotograficoLinhaDTO()
        {
            var numeroLinhas = 1;
            var random = new Random();
            var faker = new Faker<AcervoFotograficoLinhaDTO>("pt_BR");
            
            faker.RuleFor(x => x.NumeroLinha, f => numeroLinhas++);
            
            faker.RuleFor(x => x.Titulo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Codigo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Sentence().Limite(12)}.FT",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Credito, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(200)}|{f.Lorem.Sentence().Limite(200)}|{f.Lorem.Word().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                EhCampoObrigatorio = true
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
            
            faker.RuleFor(x => x.CopiaDigital, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = Dominio.Constantes.Constantes.OPCAO_SIM,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_3,
                ValoresPermitidos  = new List<string>() { ConstantesTestes.OPCAO_SIM, ConstantesTestes.OPCAO_NAO }  
            });
            
            faker.RuleFor(x => x.PermiteUsoImagem, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = Dominio.Constantes.Constantes.OPCAO_SIM,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_3,
                ValoresPermitidos  = new List<string>() { ConstantesTestes.OPCAO_SIM, ConstantesTestes.OPCAO_NAO }  
            });
            
            faker.RuleFor(x => x.EstadoConservacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.REGULAR,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Descricao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text(),
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Quantidade, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(45,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_INTEIRO,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Largura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = "50,45",
                ValidarComExpressaoRegular = Dominio.Constantes.Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Dominio.Constantes.Constantes.ALTURA)
            });
            
            faker.RuleFor(x => x.Altura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = "10,20",
                ValidarComExpressaoRegular = Dominio.Constantes.Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Dominio.Constantes.Constantes.ALTURA)
            });
            
            faker.RuleFor(x => x.Suporte, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.PAPEL,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.FormatoImagem, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.JPEG,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.TamanhoArquivo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Cromia, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.PB,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Resolucao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            
            return faker;
        }
}