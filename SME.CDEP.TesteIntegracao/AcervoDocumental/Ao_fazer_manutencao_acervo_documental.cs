using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo_documental : TesteBase
    {
        public Ao_fazer_manutencao_acervo_documental(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo documental - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoDocumental();
            var servicoAcervoDocumental = GetServicoAcervoDocumental();

            var acervoDocumentalDto = await servicoAcervoDocumental.ObterPorId(5);
            acervoDocumentalDto.ShouldNotBeNull();
            acervoDocumentalDto.CreditosAutoresIds.Any().ShouldBeTrue();
            acervoDocumentalDto.Arquivos.Any().ShouldBeTrue();
            acervoDocumentalDto.AcessoDocumentosIds.Any().ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo documental - Obter por Id sem credor")]
        public async Task Obter_por_id_sem_credor()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoDocumental(false);
            var acervoDocumental = GetServicoAcervoDocumental();

            var acervo = await acervoDocumental.ObterPorId(5);
            acervo.ShouldNotBeNull();
            acervo.CreditosAutoresIds.Any().ShouldBeFalse();
            acervo.Arquivos.Any().ShouldBeTrue();
            acervo.AcessoDocumentosIds.Any().ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo documental - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoDocumental();
            var acervoDocumental = GetServicoAcervoDocumental();

            var acervoDocumentalDtos = await acervoDocumental.ObterTodos();
            acervoDocumentalDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo documental - Atualizar (Adicionando 4 novos arquivos/documentos, sendo 1 existente)")]
        public async Task Atualizar_com_4_novos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();
            
            var random = new Random();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoDocumental = GetServicoAcervoDocumental();

            var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100",
                CodigoNovo = "100.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
            
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoDocumentalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoDocumentalAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoDocumentalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.CodigoNovo.Equals(acervoDocumentalAlteracaoDto.CodigoNovo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.DocumentacaoHistorica);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            acervo.Ano.ShouldBe(acervoDocumentalAlteracaoDto.Ano);
            
           var acervoDocumental = ObterTodos<AcervoDocumental>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoDocumental.Localizacao.ShouldBe(acervoDocumentalAlteracaoDto.Localizacao);
            acervoDocumental.ConservacaoId.ShouldBe(acervoDocumentalAlteracaoDto.ConservacaoId);
            acervoDocumental.Largura.ShouldBe(acervoDocumentalAlteracaoDto.Largura.Value);
            acervoDocumental.Altura.ShouldBe(acervoDocumentalAlteracaoDto.Altura.Value);
            acervoDocumental.MaterialId.ShouldBe(acervoDocumentalAlteracaoDto.MaterialId);
            acervoDocumental.IdiomaId.ShouldBe(acervoDocumentalAlteracaoDto.IdiomaId);
            acervoDocumental.NumeroPagina.ShouldBe(acervoDocumentalAlteracaoDto.NumeroPagina);
            acervoDocumental.Volume.ShouldBe(acervoDocumentalAlteracaoDto.Volume);
            acervoDocumental.TipoAnexo.ShouldBe(acervoDocumentalAlteracaoDto.TipoAnexo);
            acervoDocumental.TamanhoArquivo.ShouldBe(acervoDocumentalAlteracaoDto.TamanhoArquivo);            
            acervoDocumental.CopiaDigital.ShouldBe(acervoDocumentalAlteracaoDto.CopiaDigital);
            
            var acervoDocumentalArquivos = ObterTodos<AcervoDocumentalArquivo>();
            var acervoDocumentalArquivosInseridos = acervoDocumentalArquivos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
            
            var acervoDocumentalAcessoDocumentos = ObterTodos<AcervoDocumentalAcessoDocumento>();
            var acervoDocumentalAcessoDocumentosInseridos = acervoDocumentalAcessoDocumentos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalAcessoDocumentosInseridos.Count().ShouldBe(acessoDocumentosSelecionados.Count());
            acervoDocumentalAcessoDocumentosInseridos.FirstOrDefault().AcessoDocumentoId.ShouldBe(1);
            acervoDocumentalAcessoDocumentosInseridos.LastOrDefault().AcessoDocumentoId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo documental - Atualizar (Adicionando 4 novos arquivos/documentos, sendo 1 existente sem código)")]
        public async Task Atualizar_com_4_novos_sem_codigo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();
            
            var random = new Random();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoDocumental = GetServicoAcervoDocumental();

            var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                CodigoNovo = "100.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
            
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoDocumentalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoDocumentalAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.ShouldBeNull();
            acervo.CodigoNovo.Equals(acervoDocumentalAlteracaoDto.CodigoNovo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.DocumentacaoHistorica);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            acervo.Ano.ShouldBe(acervoDocumentalAlteracaoDto.Ano);
            
           var acervoDocumental = ObterTodos<AcervoDocumental>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoDocumental.Localizacao.ShouldBe(acervoDocumentalAlteracaoDto.Localizacao);
            acervoDocumental.ConservacaoId.ShouldBe(acervoDocumentalAlteracaoDto.ConservacaoId);
            acervoDocumental.Largura.ShouldBe(acervoDocumentalAlteracaoDto.Largura.Value);
            acervoDocumental.Altura.ShouldBe(acervoDocumentalAlteracaoDto.Altura.Value);
            acervoDocumental.MaterialId.ShouldBe(acervoDocumentalAlteracaoDto.MaterialId);
            acervoDocumental.IdiomaId.ShouldBe(acervoDocumentalAlteracaoDto.IdiomaId);
            acervoDocumental.NumeroPagina.ShouldBe(acervoDocumentalAlteracaoDto.NumeroPagina);
            acervoDocumental.Volume.ShouldBe(acervoDocumentalAlteracaoDto.Volume);
            acervoDocumental.TipoAnexo.ShouldBe(acervoDocumentalAlteracaoDto.TipoAnexo);
            acervoDocumental.TamanhoArquivo.ShouldBe(acervoDocumentalAlteracaoDto.TamanhoArquivo);
            acervoDocumental.CopiaDigital.ShouldBe(acervoDocumentalAlteracaoDto.CopiaDigital);
            
            var acervoDocumentalArquivos = ObterTodos<AcervoDocumentalArquivo>();
            var acervoDocumentalArquivosInseridos = acervoDocumentalArquivos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
            
            var acervoDocumentalAcessoDocumentos = ObterTodos<AcervoDocumentalAcessoDocumento>();
            var acervoDocumentalAcessoDocumentosInseridos = acervoDocumentalAcessoDocumentos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalAcessoDocumentosInseridos.Count().ShouldBe(acessoDocumentosSelecionados.Count());
            acervoDocumentalAcessoDocumentosInseridos.FirstOrDefault().AcessoDocumentoId.ShouldBe(1);
            acervoDocumentalAcessoDocumentosInseridos.LastOrDefault().AcessoDocumentoId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo documental - Atualizar (Adicionando 4 novos arquivos/documentos, sendo 1 existente sem código novo)")]
        public async Task Atualizar_com_4_novos_sem_codigo_novo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();
            
            var random = new Random();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoDocumental = GetServicoAcervoDocumental();

            var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
            
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoDocumentalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoDocumentalAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.CodigoNovo.ShouldBeNull();
            acervo.Codigo.Equals(acervoDocumentalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.DocumentacaoHistorica);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            acervo.Ano.ShouldBe(acervoDocumentalAlteracaoDto.Ano);
            
           var acervoDocumental = ObterTodos<AcervoDocumental>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoDocumental.Localizacao.ShouldBe(acervoDocumentalAlteracaoDto.Localizacao);
            acervoDocumental.ConservacaoId.ShouldBe(acervoDocumentalAlteracaoDto.ConservacaoId);
            acervoDocumental.Largura.ShouldBe(acervoDocumentalAlteracaoDto.Largura.Value);
            acervoDocumental.Altura.ShouldBe(acervoDocumentalAlteracaoDto.Altura.Value);
            acervoDocumental.MaterialId.ShouldBe(acervoDocumentalAlteracaoDto.MaterialId);
            acervoDocumental.IdiomaId.ShouldBe(acervoDocumentalAlteracaoDto.IdiomaId);
            acervoDocumental.NumeroPagina.ShouldBe(acervoDocumentalAlteracaoDto.NumeroPagina);
            acervoDocumental.Volume.ShouldBe(acervoDocumentalAlteracaoDto.Volume);
            acervoDocumental.TipoAnexo.ShouldBe(acervoDocumentalAlteracaoDto.TipoAnexo);
            acervoDocumental.TamanhoArquivo.ShouldBe(acervoDocumentalAlteracaoDto.TamanhoArquivo);
            acervoDocumental.CopiaDigital.ShouldBe(acervoDocumentalAlteracaoDto.CopiaDigital);
            
            var acervoDocumentalArquivos = ObterTodos<AcervoDocumentalArquivo>();
            var acervoDocumentalArquivosInseridos = acervoDocumentalArquivos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
            
            var acervoDocumentalAcessoDocumentos = ObterTodos<AcervoDocumentalAcessoDocumento>();
            var acervoDocumentalAcessoDocumentosInseridos = acervoDocumentalAcessoDocumentos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalAcessoDocumentosInseridos.Count().ShouldBe(acessoDocumentosSelecionados.Count());
            acervoDocumentalAcessoDocumentosInseridos.FirstOrDefault().AcessoDocumentoId.ShouldBe(1);
            acervoDocumentalAcessoDocumentosInseridos.LastOrDefault().AcessoDocumentoId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo documental - Atualizar (Removendo 1 arquivo/documento, adicionando 5 novos)")]
        public async Task Atualizar_removendo_1_()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();
            
            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.OrderByDescending(o=> o.Id).Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();
            
            var random = new Random();
            
            var servicoAcervoDocumental = GetServicoAcervoDocumental();

            var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100",
                CodigoNovo = "100.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{2,3},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
                
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto);
	
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoDocumentalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoDocumentalAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoDocumentalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.CodigoNovo.Equals(acervoDocumentalAlteracaoDto.CodigoNovo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.DocumentacaoHistorica);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            acervo.Ano.ShouldBe(acervoDocumentalAlteracaoDto.Ano);
            
            var acervoDocumental = ObterTodos<AcervoDocumental>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoDocumental.Localizacao.ShouldBe(acervoDocumentalAlteracaoDto.Localizacao);
            acervoDocumental.ConservacaoId.ShouldBe(acervoDocumentalAlteracaoDto.ConservacaoId);
            acervoDocumental.Largura.ShouldBe(acervoDocumentalAlteracaoDto.Largura.Value);
            acervoDocumental.Altura.ShouldBe(acervoDocumentalAlteracaoDto.Altura.Value);
            acervoDocumental.MaterialId.ShouldBe(acervoDocumentalAlteracaoDto.MaterialId);
            acervoDocumental.IdiomaId.ShouldBe(acervoDocumentalAlteracaoDto.IdiomaId);
            acervoDocumental.NumeroPagina.ShouldBe(acervoDocumentalAlteracaoDto.NumeroPagina);
            acervoDocumental.Volume.ShouldBe(acervoDocumentalAlteracaoDto.Volume);
            acervoDocumental.TipoAnexo.ShouldBe(acervoDocumentalAlteracaoDto.TipoAnexo);
            acervoDocumental.TamanhoArquivo.ShouldBe(acervoDocumentalAlteracaoDto.TamanhoArquivo);
            acervoDocumental.CopiaDigital.ShouldBe(acervoDocumentalAlteracaoDto.CopiaDigital);
            
            var acervoDocumentalArquivos = ObterTodos<AcervoDocumentalArquivo>();
            var acervoDocumentalArquivosInseridos = acervoDocumentalArquivos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(2);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(3);
            
            var acervoDocumentalAcessoDocumentos = ObterTodos<AcervoDocumentalAcessoDocumento>();
            var acervoDocumentalAcessoDocumentosInseridos = acervoDocumentalAcessoDocumentos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalAcessoDocumentosInseridos.Count().ShouldBe(acessoDocumentosSelecionados.Count());
            acervoDocumentalAcessoDocumentosInseridos.FirstOrDefault().AcessoDocumentoId.ShouldBe(1);
            acervoDocumentalAcessoDocumentosInseridos.LastOrDefault().AcessoDocumentoId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo documental - Atualizar (Removendo todos)")]
        public async Task Atualizar_removendo_todos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();
            
            var random = new Random();
            
            var arquivosSelecionados = Array.Empty<long>();
            var acessoDocumentosSelecionados = Array.Empty<long>();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();

            var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100",
                CodigoNovo = "100.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
                
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto);
	
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoDocumentalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoDocumentalAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoDocumentalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.CodigoNovo.Equals(acervoDocumentalAlteracaoDto.CodigoNovo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.DocumentacaoHistorica);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            acervo.Ano.ShouldBe(acervoDocumentalAlteracaoDto.Ano);
            
            var acervoDocumental = ObterTodos<AcervoDocumental>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoDocumental.Localizacao.ShouldBe(acervoDocumentalAlteracaoDto.Localizacao);
            acervoDocumental.ConservacaoId.ShouldBe(acervoDocumentalAlteracaoDto.ConservacaoId);
            acervoDocumental.Largura.ShouldBe(acervoDocumentalAlteracaoDto.Largura.Value);
            acervoDocumental.Altura.ShouldBe(acervoDocumentalAlteracaoDto.Altura.Value);
            acervoDocumental.MaterialId.ShouldBe(acervoDocumentalAlteracaoDto.MaterialId);
            acervoDocumental.IdiomaId.ShouldBe(acervoDocumentalAlteracaoDto.IdiomaId);
            acervoDocumental.NumeroPagina.ShouldBe(acervoDocumentalAlteracaoDto.NumeroPagina);
            acervoDocumental.Volume.ShouldBe(acervoDocumentalAlteracaoDto.Volume);
            acervoDocumental.TipoAnexo.ShouldBe(acervoDocumentalAlteracaoDto.TipoAnexo);
            acervoDocumental.TamanhoArquivo.ShouldBe(acervoDocumentalAlteracaoDto.TamanhoArquivo);
            acervoDocumental.CopiaDigital.ShouldBe(acervoDocumentalAlteracaoDto.CopiaDigital);
            
            var acervoDocumentalArquivos = ObterTodos<AcervoDocumentalArquivo>();
            var acervoDocumentalArquivosInseridos = acervoDocumentalArquivos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(1);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
            
            var acervoDocumentalAcessoDocumentos = ObterTodos<AcervoDocumentalAcessoDocumento>();
            var acervoDocumentalAcessoDocumentosInseridos = acervoDocumentalAcessoDocumentos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalAcessoDocumentosInseridos.Count().ShouldBe(acessoDocumentosSelecionados.Count());
        }
        
        [Fact(DisplayName = "Acervo documental - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervodocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

            var acervoDocumentalCadastroDto = new AcervoDocumentalCadastroDTO()
            {
                Codigo = "100",
                CodigoNovo = "100.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
            
            var acervoDocumentalInserido = await servicoAcervodocumental.Inserir(acervoDocumentalCadastroDto);
            acervoDocumentalInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoDocumentalCadastroDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoDocumentalCadastroDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoDocumentalCadastroDto.Codigo).ShouldBeTrue();
            acervo.CodigoNovo.Equals(acervoDocumentalCadastroDto.CodigoNovo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.DocumentacaoHistorica);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeFalse();
            acervo.AlteradoPor.ShouldBeNull();
            acervo.Ano.ShouldBe(acervoDocumentalCadastroDto.Ano);
            
            var acervoDocumental = ObterTodos<AcervoDocumental>().LastOrDefault();
            acervoDocumental.Localizacao.ShouldBe(acervoDocumentalCadastroDto.Localizacao);
            acervoDocumental.ConservacaoId.ShouldBe(acervoDocumentalCadastroDto.ConservacaoId);
            acervoDocumental.Largura.ShouldBe(acervoDocumentalCadastroDto.Largura.Value);
            acervoDocumental.Altura.ShouldBe(acervoDocumentalCadastroDto.Altura.Value);
            acervoDocumental.MaterialId.ShouldBe(acervoDocumentalCadastroDto.MaterialId);
            acervoDocumental.IdiomaId.ShouldBe(acervoDocumentalCadastroDto.IdiomaId);
            acervoDocumental.NumeroPagina.ShouldBe(acervoDocumentalCadastroDto.NumeroPagina);
            acervoDocumental.Volume.ShouldBe(acervoDocumentalCadastroDto.Volume);
            acervoDocumental.TipoAnexo.ShouldBe(acervoDocumentalCadastroDto.TipoAnexo);
            acervoDocumental.TamanhoArquivo.ShouldBe(acervoDocumentalCadastroDto.TamanhoArquivo);
            acervoDocumental.CopiaDigital.ShouldBe(acervoDocumentalCadastroDto.CopiaDigital);
            
            var acervoDocumentalArquivos = ObterTodos<AcervoDocumentalArquivo>();
            var acervoDocumentalArquivosInseridos = acervoDocumentalArquivos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == acervoDocumental.AcervoId);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
            
            var acervoDocumentalAcessoDocumentos = ObterTodos<AcervoDocumentalAcessoDocumento>();
            var acervoDocumentalAcessoDocumentosInseridos = acervoDocumentalAcessoDocumentos.Where(w => w.AcervoDocumentalId == acervoDocumental.Id);
            acervoDocumentalAcessoDocumentosInseridos.Count().ShouldBe(acessoDocumentosSelecionados.Count());
            acervoDocumentalAcessoDocumentosInseridos.FirstOrDefault().AcessoDocumentoId.ShouldBe(1);
            acervoDocumentalAcessoDocumentosInseridos.LastOrDefault().AcessoDocumentoId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve inserir código duplicado")]
        public async Task Nao_deve_inserir_codigo_duplicado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

            var acervoDocumentalCadastroDto = new AcervoDocumentalCadastroDTO()
            {
                Codigo = "1",
                CodigoNovo = "100.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
            
            await servicoAcervoDocumental.Inserir(acervoDocumentalCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve inserir sem código")]
        public async Task Nao_deve_inserir_sem_codigo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

            var acervoDocumentalCadastroDto = new AcervoDocumentalCadastroDTO()
            {
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true
            };
            
            await servicoAcervoDocumental.Inserir(acervoDocumentalCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve inserir sem código novo")]
        public async Task Nao_deve_inserir_sem_codigo_novo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

            var acervoDocumentalCadastroDto = new AcervoDocumentalCadastroDTO()
            {
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
            
            await servicoAcervoDocumental.Inserir(acervoDocumentalCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve inserir código novo a código existente")]
        public async Task Nao_deve_inserir_codigo_novo_a_codigo_existente()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

            var acervoDocumentalCadastroDto = new AcervoDocumentalCadastroDTO()
            {
                Codigo = "1",
                CodigoNovo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
            
            await servicoAcervoDocumental.Inserir(acervoDocumentalCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve inserir com ano futuro")]
        public async Task Nao_deve_inserir_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

            var acervoDocumentalCadastroDto = new AcervoDocumentalCadastroDTO()
            {
                Codigo = "1",
                CodigoNovo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Future().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
            
            await servicoAcervoDocumental.Inserir(acervoDocumentalCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve alterar para código existente")]
        public async Task Nao_deve_alterar_para_codigo_existente()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

           var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "1",
                CodigoNovo = "100.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
                
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve alterar para código novo existente")]
        public async Task Nao_deve_alterar_para_codigo_novo_existente()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

           var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100",
                CodigoNovo = "1.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
                
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve alterar sem código")]
        public async Task Nao_deve_alterar_sem_codigo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

           var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
                
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve alterar sem código novo")]
        public async Task Nao_deve_alterar_sem_codigo_novo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

           var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
                
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto).ShouldThrowAsync<NegocioException>();
           
        }
        
         [Fact(DisplayName = "Acervo documental - Não deve alterar com código e código novo iguais")]
        public async Task Nao_deve_alterar_com_codigo_e_codigo_novo_iguais()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

           var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "3.NOVO",
                CodigoNovo = "3.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Past().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
                
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo documental - Não deve alterar com ano futuro")]
        public async Task Nao_deve_alterar_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoDocumental();

            var servicoAcervoDocumental = GetServicoAcervoDocumental();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();
            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var acessoDocumentos = ObterTodos<AcessoDocumento>();
            var acessoDocumentosSelecionados = acessoDocumentos.Take(5).Select(s => s.Id).ToArray();

           var acervoDocumentalAlteracaoDto = new AcervoDocumentalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "3.NOVO",
                CodigoNovo = "3.NOVO",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                ConservacaoId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados,
                AcessoDocumentosIds = acessoDocumentosSelecionados,
                MaterialId = random.Next(1,5),
                IdiomaId = random.Next(1,5),
                Ano = faker.Date.Future().Year,
                NumeroPagina = faker.Lorem.Text().Limite(4),
                Volume = faker.Lorem.Text().Limite(15),
                TipoAnexo = faker.Lorem.Text().Limite(50),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                CopiaDigital = true,
            };
                
            await servicoAcervoDocumental.Alterar(acervoDocumentalAlteracaoDto).ShouldThrowAsync<NegocioException>();
           
        }

        private async Task InserirAcervoDocumental(bool inserirCredor = true,bool inserirAcessoDocumento = true)
        {
            var random = new Random();

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = j.ToString(),
                    CodigoNovo = $"{j.ToString()}.NOVO",
                    Titulo = faker.Lorem.Text().Limite(500),
                    Descricao = faker.Lorem.Text(),
                    TipoAcervoId = (int)TipoAcervo.DocumentacaoHistorica,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                });

                if (inserirCredor)
                {
                    await InserirNaBase(new AcervoCreditoAutor()
                    {
                        AcervoId = j,
                        CreditoAutorId = 1
                    });

                    await InserirNaBase(new AcervoCreditoAutor()
                    {
                        AcervoId = j,
                        CreditoAutorId = 2
                    });

                    await InserirNaBase(new AcervoCreditoAutor()
                    {
                        AcervoId = j,
                        CreditoAutorId = 3
                    });
                }

                await InserirNaBase(new AcervoDocumental()
                {
                    AcervoId = j,
                    MaterialId = random.Next(1,5),
                    IdiomaId = random.Next(1,5),
                    NumeroPagina = faker.Lorem.Text().Limite(4),
                    Volume = faker.Lorem.Text().Limite(15),
                    TipoAnexo = faker.Lorem.Text().Limite(50),
                    Largura = random.Next(15,55),
                    Altura = random.Next(15,55),
                    TamanhoArquivo = faker.Lorem.Text().Limite(15),
                    Localizacao = faker.Lorem.Text().Limite(100),
                    CopiaDigital = true,
                    ConservacaoId = random.Next(1,5),
                });
                
                await InserirNaBase(new Arquivo()
                {
                    Nome = faker.Lorem.Text(),
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.AcervoDocumental,
                    TipoConteudo = ConstantesTestes.MIME_TYPE_JPG,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                });
                
                await InserirNaBase(new AcervoDocumentalArquivo()
                {
                    ArquivoId = j,
                    AcervoDocumentalId = j
                });

                if (inserirAcessoDocumento)
                {
                    await InserirNaBase(new AcervoDocumentalAcessoDocumento()
                    {
                        AcervoDocumentalId = j,
                        AcessoDocumentoId = 1
                    });
                
                    await InserirNaBase(new AcervoDocumentalAcessoDocumento()
                    {
                        AcervoDocumentalId = j,
                        AcessoDocumentoId = 2
                    });
                
                    await InserirNaBase(new AcervoDocumentalAcessoDocumento()
                    {
                        AcervoDocumentalId = j,
                        AcessoDocumentoId = 3
                    });                    
                }
            }
        }
    }
}