using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;
using Xunit.Sdk;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo_fotografico : TesteBase
    {
        public Ao_fazer_manutencao_acervo_fotografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        
        [Fact(DisplayName = "Acervo fotográfico - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicos();
            await InserirAcervoFotografico();
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoDtos = await servicoAcervoFotografico.ObterPorId(5);
            acervoFotograficoDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicos();
            await InserirAcervoFotografico();
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoDtos = await servicoAcervoFotografico.ObterTodos();
            acervoFotograficoDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Pesquisar por Nome")]
        public async Task Pesquisar_por_nome()
        {
            var servicoCreditoAutor = GetServicoCreditoAutor();

            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.LIVRO,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.PAPEL,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.VOB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            var retorno = await servicoCreditoAutor.ObterPaginado("o");
            retorno.Items.Count().ShouldBe(ConstantesTestes.QUANTIDADE_3);
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Atualizar")]
        public async Task Atualizar()
        {
            await InserirDadosBasicos();

            await InserirAcervoFotografico();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoDto = await servicoAcervoFotografico.ObterPorId(3);
            acervoFotograficoDto.Acervo.Titulo = string.Format(ConstantesTestes.TITULO_X, 100);
            
            var acervoFotograficoAlterado = await servicoAcervoFotografico.Alterar(acervoFotograficoDto);
            
            acervoFotograficoAlterado.ShouldNotBeNull();
            acervoFotograficoAlterado.Acervo.Titulo = string.Format(ConstantesTestes.TITULO_X, 100);
        }

        private async Task InserirAcervoFotografico()
        {
            var random = new Random();

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = string.Format(ConstantesTestes.CODIGO_X, j),
                    Titulo = string.Format(ConstantesTestes.TITULO_X, j),
                    CreditoAutorId = random.Next(1, 5),
                    TipoAcervoId = (int)TipoAcervo.Fotografico,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = ConstantesTestes.SISTEMA,
                    AlteradoLogin = ConstantesTestes.LOGIN_123456789
                });

                await InserirNaBase(new AcervoFotografico()
                {
                    AcervoId = j,
                    Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, j),
                    Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X,j),
                    DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                    CopiaDigital = true,
                    PermiteUsoImagem = true,
                    ConservacaoId = random.Next(1,5),
                    Descricao = string.Format(ConstantesTestes.DESCRICAO_X,j),
                    Quantidade = random.Next(15,55),
                    Largura = random.Next(15,55),
                    Altura = random.Next(15,55),
                    SuporteId = random.Next(1,5),
                    FormatoId = random.Next(1,5),
                    CromiaId = random.Next(1,5),
                    Resolucao = string.Format(ConstantesTestes.RESOLUCAO_X,j),
                    TamanhoArquivo = string.Format(ConstantesTestes.TAMANHO_ARQUIVO_X_MB,j),
                });
            }
        }
    }
}