using AutoMapper;
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
    public class Ao_fazer_manutencao_acervo_audiovisual : TesteBase
    {
        public Ao_fazer_manutencao_acervo_audiovisual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Audiovisual - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoAudiovisual();
            var servicoAcervoAudiovisual = GetServicoAcervoAudiovisual();

            var acervoAudiovisualDto = await servicoAcervoAudiovisual.ObterPorId(5);
            acervoAudiovisualDto.CreditosAutoresIds.Any().ShouldBeTrue();
            acervoAudiovisualDto.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Audiovisual - Excluir por Id")]
        public async Task Excluir_por_id()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoAudiovisual();
            var servicoAcervoAudiovisual = GetServicoAcervoAudiovisual();

            var acervoAudiovisualDto = await servicoAcervoAudiovisual.Excluir(5);
            acervoAudiovisualDto.ShouldBeTrue();

            var acervo = ObterTodos<Acervo>();
            acervo.FirstOrDefault(w=> w.Id == 5).Excluido.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Audiovisual - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoAudiovisual();
            var servicoAcervoAudiovisual = GetServicoAcervoAudiovisual();

            var acervoAudiovisualDtos = await servicoAcervoAudiovisual.ObterTodos();
            acervoAudiovisualDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Audiovisual - Atualizar")]
        public async Task Atualizar()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoAudiovisual();
            
            var random = new Random();
            
            var servicoAcervoAudiovisual = GetServicoAcervoAudiovisual();

            var acervoAudiovisualAlteracaoDto = new AcervoAudiovisualAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.AV",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                Copia = faker.Lorem.Text().Limite(100),
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Descricao = faker.Lorem.Text(),
                SuporteId = random.Next(1, 5),
                Duracao = faker.Lorem.Text().Limite(15),
                CromiaId = random.Next(1, 5),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Acessibilidade = faker.Lorem.Text().Limite(100),
                Disponibilizacao = faker.Lorem.Text().Limite(100),
            };
            
            await servicoAcervoAudiovisual.Alterar(acervoAudiovisualAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoAudiovisualAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoAudiovisualAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoAudiovisualAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.Ano.ShouldBe(acervoAudiovisualAlteracaoDto.Ano);
            acervo.DataAcervo.ShouldBe(acervoAudiovisualAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoAudiovisualAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Audiovisual);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
           var acervoAudiovisual = ObterTodos<AcervoAudiovisual>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoAudiovisual.Localizacao.ShouldBe(acervoAudiovisualAlteracaoDto.Localizacao);
            acervoAudiovisual.Procedencia.ShouldBe(acervoAudiovisualAlteracaoDto.Procedencia);
            acervoAudiovisual.Copia.ShouldBe(acervoAudiovisualAlteracaoDto.Copia);
            acervoAudiovisual.PermiteUsoImagem.ShouldBe(acervoAudiovisualAlteracaoDto.PermiteUsoImagem);
            acervoAudiovisual.ConservacaoId.ShouldBe(acervoAudiovisualAlteracaoDto.ConservacaoId);
            acervoAudiovisual.SuporteId.ShouldBe(acervoAudiovisualAlteracaoDto.SuporteId);
            acervoAudiovisual.Duracao.ShouldBe(acervoAudiovisualAlteracaoDto.Duracao);
            acervoAudiovisual.CromiaId.ShouldBe(acervoAudiovisualAlteracaoDto.CromiaId.Value);
            acervoAudiovisual.TamanhoArquivo.ShouldBe(acervoAudiovisualAlteracaoDto.TamanhoArquivo);
            acervoAudiovisual.Acessibilidade.ShouldBe(acervoAudiovisualAlteracaoDto.Acessibilidade);
            acervoAudiovisual.Disponibilizacao.ShouldBe(acervoAudiovisualAlteracaoDto.Disponibilizacao);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Audiovisual - Não deve atualizar com ano futuro")]
        public async Task Nao_deve_atualizar_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoAudiovisual();
            
            var random = new Random();
            
            var servicoAcervoAudiovisual = GetServicoAcervoAudiovisual();

            var acervoAudiovisualAlteracaoDto = new AcervoAudiovisualAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.AV",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Future().AddYears(1).Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                Copia = faker.Lorem.Text().Limite(100),
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Descricao = faker.Lorem.Text(),
                SuporteId = random.Next(1, 5),
                Duracao = faker.Lorem.Text().Limite(15),
                CromiaId = random.Next(1, 5),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Acessibilidade = faker.Lorem.Text().Limite(100),
                Disponibilizacao = faker.Lorem.Text().Limite(100),
            };
            
            await servicoAcervoAudiovisual.Alterar(acervoAudiovisualAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Audiovisual - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoAudiovisual();

            var servicoAcervoAudiovisual = GetServicoAcervoAudiovisual();
            
            var random = new Random();

            var acervoAudiovisualCadastroDto = new AcervoAudiovisualCadastroDTO()
            {
                Codigo = "100",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                Copia = faker.Lorem.Text().Limite(100),
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Descricao = faker.Lorem.Text(),
                SuporteId = random.Next(1, 5),
                Duracao = faker.Lorem.Text().Limite(15),
                CromiaId = random.Next(1, 5),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Acessibilidade = faker.Lorem.Text().Limite(100),
                Disponibilizacao = faker.Lorem.Text().Limite(100),
            };
            
            var acervoAudiovisualInserido = await servicoAcervoAudiovisual.Inserir(acervoAudiovisualCadastroDto);
            acervoAudiovisualInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoAudiovisualCadastroDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoAudiovisualCadastroDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals($"{acervoAudiovisualCadastroDto.Codigo}.AV").ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoAudiovisualCadastroDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoAudiovisualCadastroDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Audiovisual);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeFalse();
            acervo.AlteradoPor.ShouldBeNull();
            
            var acervoAudiovisual = ObterTodos<AcervoAudiovisual>().LastOrDefault();
            acervoAudiovisual.Localizacao.ShouldBe(acervoAudiovisualCadastroDto.Localizacao);
            acervoAudiovisual.Procedencia.ShouldBe(acervoAudiovisualCadastroDto.Procedencia);
            acervoAudiovisual.Copia.ShouldBe(acervoAudiovisualCadastroDto.Copia);
            acervoAudiovisual.PermiteUsoImagem.ShouldBe(acervoAudiovisualCadastroDto.PermiteUsoImagem);
            acervoAudiovisual.ConservacaoId.ShouldBe(acervoAudiovisualCadastroDto.ConservacaoId);
            acervoAudiovisual.SuporteId.ShouldBe(acervoAudiovisualCadastroDto.SuporteId);
            acervoAudiovisual.Duracao.ShouldBe(acervoAudiovisualCadastroDto.Duracao);
            acervoAudiovisual.CromiaId.ShouldBe(acervoAudiovisualCadastroDto.CromiaId.Value);
            acervoAudiovisual.TamanhoArquivo.ShouldBe(acervoAudiovisualCadastroDto.TamanhoArquivo);
            acervoAudiovisual.Acessibilidade.ShouldBe(acervoAudiovisualCadastroDto.Acessibilidade);
            acervoAudiovisual.Disponibilizacao.ShouldBe(acervoAudiovisualCadastroDto.Disponibilizacao);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == acervoAudiovisual.AcervoId);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Audiovisual - Não deve inserir Tombo duplicado")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoAudiovisual();

            var servicoAcervoAudiovisual = GetServicoAcervoAudiovisual();
            
            var random = new Random();

            var acervoAudiovisualCadastroDto = new AcervoAudiovisualCadastroDTO()
            {
                Codigo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                Copia = faker.Lorem.Text().Limite(100),
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Descricao = faker.Lorem.Text(),
                SuporteId = random.Next(1, 5),
                Duracao = faker.Lorem.Text().Limite(15),
                CromiaId = random.Next(1, 5),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Acessibilidade = faker.Lorem.Text().Limite(100),
                Disponibilizacao = faker.Lorem.Text().Limite(100),
            };
            
            await servicoAcervoAudiovisual.Inserir(acervoAudiovisualCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Audiovisual - Não deve inserir com ano futuro")]
        public async Task Nao_deve_inserir_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoAudiovisual();

            var servicoAcervoAudiovisual = GetServicoAcervoAudiovisual();
            
            var random = new Random();

            var acervoAudiovisualCadastroDto = new AcervoAudiovisualCadastroDTO()
            {
                Codigo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Future().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                Copia = faker.Lorem.Text().Limite(100),
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Descricao = faker.Lorem.Text(),
                SuporteId = random.Next(1, 5),
                Duracao = faker.Lorem.Text().Limite(15),
                CromiaId = random.Next(1, 5),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Acessibilidade = faker.Lorem.Text().Limite(100),
                Disponibilizacao = faker.Lorem.Text().Limite(100),
            };
            
            await servicoAcervoAudiovisual.Inserir(acervoAudiovisualCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }

        private async Task InserirAcervoAudiovisual(bool inserirCredor = true)
        {
            var random = new Random();

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = $"{j.ToString()}.AV",
                    Titulo = faker.Lorem.Text().Limite(500),
                    Descricao = faker.Lorem.Text(),
                    TipoAcervoId = (int)TipoAcervo.Audiovisual,
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

                await InserirNaBase(new AcervoAudiovisual()
                {
                    AcervoId = j,
                    Localizacao = faker.Lorem.Text().Limite(100),
                    Procedencia = faker.Lorem.Text().Limite(200),
                    Copia = faker.Lorem.Text().Limite(100),
                    PermiteUsoImagem = true,
                    ConservacaoId = random.Next(1,5),
                    SuporteId = random.Next(1,5),
                    Duracao = faker.Lorem.Text().Limite(15),
                    CromiaId = random.Next(1,5),
                    TamanhoArquivo = faker.Lorem.Text().Limite(15),
                    Acessibilidade = faker.Lorem.Text().Limite(100),
                    Disponibilizacao = faker.Lorem.Text().Limite(100),
                });
            }
        }
    }
}