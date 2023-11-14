using Bogus;
using Bogus.Extensions.Brazil;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;

namespace SME.CDEP.TesteIntegracao
{
    public abstract class BaseMock
    {
        public static long GerarIdAleatorio()
        {
            return new Faker().Random.Long(1);
        }

        protected static void AuditoriaFaker<T>(Faker<T> faker) where T : EntidadeBaseAuditavel
        {
            faker.RuleFor(x => x.CriadoPor, f => f.Name.FullName());
            faker.RuleFor(x => x.CriadoEm, DateTimeExtension.HorarioBrasilia());
            faker.RuleFor(x => x.CriadoLogin, f => f.Name.FirstName());
            faker.RuleFor(x => x.AlteradoPor, f => f.Name.FullName());
            faker.RuleFor(x => x.AlteradoEm, DateTimeExtension.HorarioBrasilia());
            faker.RuleFor(x => x.AlteradoLogin, f => f.Name.FirstName());
        }
        
        protected static Faker<Acervo> GerarAcervo(TipoAcervo tipoAcervo)
        {
            var random = new Random();
            var faker = new Faker<Acervo>("pt_BR");
            faker.RuleFor(x => x.Titulo, f => f.Lorem.Text().Limite(500));
            faker.RuleFor(x => x.Descricao, f => f.Lorem.Text());
            faker.RuleFor(x => x.Codigo, f => random.Next(1,499).ToString());
            
            if (((long)tipoAcervo).EhAcervoDocumental())
                faker.RuleFor(x => x.CodigoNovo, f => random.Next(500,999).ToString());
            
            faker.RuleFor(x => x.SubTitulo, f => f.Lorem.Text().Limite(500));
            faker.RuleFor(x => x.TipoAcervoId, f => (int)tipoAcervo);
            AuditoriaFaker(faker);
            return faker;
        }
    
        protected Faker<AcervoArteGrafica> GerarAcervoArteGrafica()
        {
            var random = new Random();
            var faker = new Faker<AcervoArteGrafica>("pt_BR");
            
            faker.RuleFor(x => x.Localizacao, f => f.Lorem.Text().Limite(100));
            faker.RuleFor(x => x.Procedencia, f => f.Lorem.Text().Limite(200));
            faker.RuleFor(x => x.DataAcervo, f => f.Date.Recent().Year.ToString());
            faker.RuleFor(x => x.CopiaDigital, f => true);
            faker.RuleFor(x => x.PermiteUsoImagem, f => true);
            faker.RuleFor(x => x.Largura, f => random.Next(15,55));
            faker.RuleFor(x => x.Altura, f => random.Next(15,55));
            faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
            faker.RuleFor(x => x.CromiaId, f => random.Next(1,5));
            faker.RuleFor(x => x.Diametro, f => random.Next(15,55));
            faker.RuleFor(x => x.Tecnica, f => f.Lorem.Sentence().Limite(100));
            faker.RuleFor(x => x.SuporteId, f => random.Next(1,5));
            faker.RuleFor(x => x.Quantidade, f => random.Next(15,55));
            faker.RuleFor(x => x.Acervo, f => GerarAcervo(TipoAcervo.ArtesGraficas).Generate());
            return faker;
        }
    
        protected Faker<AcervoArteGraficaCadastroDTO> GerarAcervoArteGraficaCadastroDTO()
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
            faker.RuleFor(x => x.Largura, f => random.Next(15,55));
            faker.RuleFor(x => x.Altura, f => random.Next(15,55));
            faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
            faker.RuleFor(x => x.CromiaId, f => random.Next(1,5));
            faker.RuleFor(x => x.Diametro, f => random.Next(15,55));
            faker.RuleFor(x => x.Tecnica, f => f.Lorem.Sentence().Limite(100));
            faker.RuleFor(x => x.SuporteId, f => random.Next(1,5));
            faker.RuleFor(x => x.Quantidade, f => random.Next(15,55));
            faker.RuleFor(x => x.CreditosAutoresIds, f => new long[]{1,2,3,4,5});
            faker.RuleFor(x => x.Arquivos, f => new long[]{random.Next(1,10),random.Next(1,10),random.Next(1,10),random.Next(1,10),random.Next(1,10)});
            return faker;
        }
        
        protected Faker<Arquivo> GerarArquivo(TipoArquivo tipoArquivo)
        {
            var faker = new Faker<Arquivo>("pt_BR");
            
            faker.RuleFor(x => x.Codigo, f => Guid.NewGuid());
            faker.RuleFor(x => x.Nome, f => f.Lorem.Text().Limite(20));
            faker.RuleFor(x => x.Tipo, f => tipoArquivo);
            faker.RuleFor(x => x.TipoConteudo, f => "image/jpeg");
            AuditoriaFaker(faker);
            return faker;
        }
        
        protected Faker<UsuarioDTO> GerarUsuarioDTO(TipoUsuario tipoUsuario)
        {
            var faker = new Faker<UsuarioDTO>("pt_BR");
            
            faker.RuleFor(x => x.Login, f => f.Person.FirstName.Limite(45));
            faker.RuleFor(x => x.Nome, f => f.Lorem.Text().Limite(100));
            faker.RuleFor(x => x.Endereco, f => f.Address.FullAddress().Limite(200));
            faker.RuleFor(x => x.Numero, f => int.Parse(f.Address.BuildingNumber().Limite(4)));
            faker.RuleFor(x => x.Complemento, f => f.Address.StreetSuffix().Limite(20));
            faker.RuleFor(x => x.Cep, f => f.Address.ZipCode());
            faker.RuleFor(x => x.Cidade, f => f.Address.City());
            faker.RuleFor(x => x.Estado, f => f.Address.StateAbbr());
            faker.RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber("(##) #####-####"));
            faker.RuleFor(x => x.Bairro, f => f.Address.County().Limite(200));
            faker.RuleFor(x => x.TipoUsuario, f => (int)tipoUsuario);
            return faker;
        }
        
        protected Faker<AcervoBibliograficoLinhaDTO> GerarAcervoBibliograficoLinhaDTO()
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
                Conteudo = f.Lorem.Sentence().Limite(500),
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
                Conteudo = $"{f.Lorem.Text().Limite(200)}|{f.Lorem.Text().Limite(200)}|{f.Lorem.Text().Limite(200)}|{f.Lorem.Text().Limite(200)}|{f.Lorem.Text().Limite(200)}|{f.Lorem.Text().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
            });
            faker.RuleFor(x => x.TipoAutoria, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(15)}|{f.Lorem.Text().Limite(15)}|{f.Lorem.Text().Limite(15)}|{f.Lorem.Text().Limite(15)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
            });
            faker.RuleFor(x => x.Editora, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Commerce.Department().Limite(200),
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
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
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
            faker.RuleFor(x => x.SerieColecao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(200),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
            });
            faker.RuleFor(x => x.Volume, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            faker.RuleFor(x => x.Idioma, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(500),
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
            faker.RuleFor(x => x.Tombo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            return faker;
        }
        
        protected Faker<AcervoDocumentalLinhaDTO> GerarAcervoDocumentalLinhaDTO()
        {
            var numeroLinhas = 1;
            var random = new Random();
            var faker = new Faker<AcervoDocumentalLinhaDTO>("pt_BR");
            
            faker.RuleFor(x => x.NumeroLinha, f => numeroLinhas++);
            
            faker.RuleFor(x => x.Titulo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.CodigoAntigo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            
            faker.RuleFor(x => x.CodigoNovo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            
            faker.RuleFor(x => x.Material, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
            });
            
            faker.RuleFor(x => x.Idioma, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Autor, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(200)}|{f.Lorem.Sentence().Limite(200)}|{f.Lorem.Word().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
            });
            
            faker.RuleFor(x => x.Ano, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Date.Recent().Year.ToString(),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            
            faker.RuleFor(x => x.NumeroPaginas, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_4,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Volume, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            
            faker.RuleFor(x => x.Descricao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text(),
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.TipoAnexo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(50),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
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
            
            faker.RuleFor(x => x.TamanhoArquivo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            
            faker.RuleFor(x => x.AcessoDocumento, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(500)}|{f.Lorem.Sentence().Limite(500)}|{f.Lorem.Word().Limite(500)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Localizacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Sentence().Limite(100),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_100,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.CopiaDigital, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.OPCAO_SIM,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_3,
            });
            
            faker.RuleFor(x => x.EstadoConservacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
            });
            return faker;
        }

        protected Faker<AcervoArteGraficaLinhaDTO> GerarAcervoArteGraficaLinhaDTO()
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
            
            faker.RuleFor(x => x.Tombo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Sentence().Limite(12)}.AG",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Credito, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(200)}|{f.Lorem.Sentence().Limite(200)}|{f.Lorem.Word().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
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
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.AutorizacaoUsoDeImagem, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.OPCAO_SIM,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_3,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.EstadoConservacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Cromia, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
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
                Conteudo = f.Lorem.Text().Limite(500),
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
        
        protected Faker<AcervoAudiovisualLinhaDTO> GerarAcervoAudiovisualLinhaDTO()
        {
            var numeroLinhas = 1;
            var faker = new Faker<AcervoAudiovisualLinhaDTO>("pt_BR");
            
            faker.RuleFor(x => x.NumeroLinha, f => numeroLinhas++);
            
            faker.RuleFor(x => x.Titulo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Tombo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Sentence().Limite(12)}.AV",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Credito, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = $"{f.Lorem.Text().Limite(200)}|{f.Lorem.Sentence().Limite(200)}|{f.Lorem.Word().Limite(200)}",
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
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
            
            faker.RuleFor(x => x.Copia, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(100),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_100,
            });
            
            faker.RuleFor(x => x.AutorizacaoUsoDeImagem, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = ConstantesTestes.OPCAO_SIM,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_3,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.EstadoConservacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
            });
            
            faker.RuleFor(x => x.Descricao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text(),
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Suporte, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.Duracao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            
            faker.RuleFor(x => x.Cromia, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
            });
            
            faker.RuleFor(x => x.TamanhoArquivo, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(15),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
            });
            
            faker.RuleFor(x => x.Acessibilidade, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(100),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_100,
            });
            
            faker.RuleFor(x => x.Disponibilizacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(100),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_100,
            });
            
            return faker;
        }
        
        protected Faker<AcervoFotograficoLinhaDTO> GerarAcervoFotograficoLinhaDTO()
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
            
            faker.RuleFor(x => x.Tombo, f => new LinhaConteudoAjustarDTO()
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
            });
            
            faker.RuleFor(x => x.AutorizacaoUsoDeImagem, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = Dominio.Constantes.Constantes.OPCAO_SIM,
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_3,
            });
            
            faker.RuleFor(x => x.EstadoConservacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
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
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Altura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Suporte, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.FormatoImagem, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
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
                Conteudo = f.Lorem.Text().Limite(500),
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
        
        protected Faker<AcervoTridimensionalLinhaDTO> GerarAcervoTridimensionalLinhaDTO()
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
            
            faker.RuleFor(x => x.Tombo, f => new LinhaConteudoAjustarDTO()
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
            
            faker.RuleFor(x => x.Data, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Date.Recent().Year.ToString(),
                LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                EhCampoObrigatorio = true
            });
            
            faker.RuleFor(x => x.EstadoConservacao, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = f.Lorem.Text().Limite(500),
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
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Altura, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Profundidade, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            faker.RuleFor(x => x.Diametro, f => new LinhaConteudoAjustarDTO()
            {
                Conteudo = random.Next(15,55).ToString(),
                FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
            });
            
            return faker;
        }
        
        protected Faker<AcervoBibliografico> GerarAcervoBibliografico()
        {
            var random = new Random();
            var faker = new Faker<AcervoBibliografico>("pt_BR");
            
            faker.RuleFor(x => x.MaterialId, f => random.Next(1,5));
            faker.RuleFor(x => x.EditoraId, f => random.Next(1,5));
            faker.RuleFor(x => x.Ano, f => f.Date.Recent().Year.ToString());
            faker.RuleFor(x => x.Edicao, f => f.Lorem.Sentence().Limite(15));
            faker.RuleFor(x => x.NumeroPagina, f => random.Next(15,55));
            faker.RuleFor(x => x.Largura, f => random.Next(15,55));
            faker.RuleFor(x => x.Altura, f => random.Next(15,55));
            faker.RuleFor(x => x.Volume, f => f.Lorem.Sentence().Limite(15));
            faker.RuleFor(x => x.IdiomaId, f => random.Next(1,5));
            faker.RuleFor(x => x.LocalizacaoCDD, f => f.Lorem.Sentence().Limite(50));
            faker.RuleFor(x => x.LocalizacaoPHA, f => f.Lorem.Sentence().Limite(50));
            faker.RuleFor(x => x.NotasGerais, f => f.Lorem.Text().Limite(500));
            faker.RuleFor(x => x.Isbn, f => f.Lorem.Sentence().Limite(50));
            faker.RuleFor(x => x.Acervo, f => GerarAcervo(TipoAcervo.Bibliografico).Generate());
            return faker;
        }
    }
}