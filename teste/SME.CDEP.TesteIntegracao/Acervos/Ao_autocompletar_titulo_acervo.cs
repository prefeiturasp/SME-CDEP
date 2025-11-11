using Bogus;
using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Acervos
{
    public class Ao_autocompletar_titulo_acervo : TesteBase
    {
        private readonly IServicoAcervo _servicoAcervo;
        private readonly Faker _faker;

        public Ao_autocompletar_titulo_acervo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _servicoAcervo = GetServicoAcervo();
            _faker = new Faker();
        }

        //[Fact(DisplayName = "Acervo - Autocompletar titulo quando houver retorno")]
        //public async Task Autocompletar_titulo_quando_houver_retorno()
        //{
        //    // Arrange
        //    await InserirDadosBasicosAleatorios();
        //    var acervoBibliograficoMock = AcervoDocumentalMock.Instance.Gerar().Generate(7);
        //    var tituloParcial = "A Assembleia";
        //    acervoBibliograficoMock[0].Acervo.Titulo = "A Assembleia dos Homens";
        //    acervoBibliograficoMock[1].Acervo.Titulo = "A Assembléia Geral";
        //    acervoBibliograficoMock[2].Acervo.Titulo = "A Arte da Guerra";
        //    acervoBibliograficoMock[3].Acervo.Titulo = "Confusão na assembleia";
        //    acervoBibliograficoMock[4].Acervo.Titulo = "O Senhor dos Anéis";
        //    acervoBibliograficoMock[5].Acervo.Titulo = "a assembleia Constituinte";
        //    acervoBibliograficoMock[6].Acervo.Titulo = "a assembleia principal";

        //    var acervosSolicitacoes = acervoBibliograficoMock
        //        .Where(a => a.Acervo.Titulo != "a assembleia principal")
        //        .Select(a => new AcervoSolicitacao
        //        {
        //            AlteradoEm = DateTime.UtcNow,
        //            AlteradoLogin = _faker.Internet.UserName(),
        //            AlteradoPor = _faker.Person.FullName,
        //            CriadoEm = _faker.Date.Recent(),
        //            CriadoLogin = _faker.Internet.UserName(),
        //            CriadoPor = _faker.Person.FullName,
        //            DataSolicitacao = DateTime.UtcNow,
        //            Excluido = false,
        //            Origem = Origem.Portal,
        //            Situacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO,
        //            UsuarioId = _faker.Random.Long(1, 5),
        //            Itens = [
        //                new(){
        //                    AcervoId = a.Acervo.Id,
        //                    Situacao = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
        //                    AcervoSolicitacaoId = a.Id,
        //                    CriadoEm = DateTime.UtcNow,
        //                    CriadoLogin = _faker.Internet.UserName(),
        //                    CriadoPor = _faker.Person.FullName,
        //                }
        //                ]
        //        }).ToList();

        //    acervosSolicitacoes.Add(new AcervoSolicitacao
        //    {
        //        AlteradoEm = DateTime.UtcNow,
        //        AlteradoLogin = _faker.Internet.UserName(),
        //        AlteradoPor = _faker.Person.FullName,
        //        CriadoEm = _faker.Date.Recent(),
        //        CriadoLogin = _faker.Internet.UserName(),
        //        CriadoPor = _faker.Person.FullName,
        //        DataSolicitacao = DateTime.UtcNow,
        //        Excluido = false,
        //        Origem = Origem.Portal,
        //        Situacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO,
        //        UsuarioId = _faker.Random.Long(1, 5),
        //        Itens = [
        //                new(){
        //                    AcervoId = acervoBibliograficoMock[6].Acervo.Id,
        //                    Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO,
        //                    AcervoSolicitacaoId = acervoBibliograficoMock[6].Id,
        //                    CriadoEm = DateTime.UtcNow,
        //                    CriadoLogin = _faker.Internet.UserName(),
        //                    CriadoPor = _faker.Person.FullName,
        //                }
        //            ]
        //    });

        //    foreach (var acervo in acervoBibliograficoMock)
        //    {
        //        await InserirNaBase(acervo.Acervo);
        //        await InserirNaBase(acervo);
        //    }

        //    foreach (var acervoSolicitacao in acervosSolicitacoes)
        //    {
        //        await InserirNaBase(acervoSolicitacao);
        //    }

        //    // Act
        //    var resultado = await _servicoAcervo.ObterAutocompletacaoTituloAcervosBaixadosAsync(tituloParcial);

        //    // Assert
        //    resultado.ShouldNotBeNull();
        //    resultado.Count().ShouldBe(3);
        //    resultado.ShouldContain(r => r.Equals("A Assembleia dos Homens", StringComparison.OrdinalIgnoreCase));
        //    resultado.ShouldContain(r => r.Equals("A Assembléia Geral", StringComparison.OrdinalIgnoreCase));
        //    resultado.ShouldContain(r => r.Equals("a assembleia Constituinte", StringComparison.OrdinalIgnoreCase));
        //    resultado.ShouldNotContain(r => r.Equals("Confusão na assembleia", StringComparison.OrdinalIgnoreCase));
        //    resultado.ShouldNotContain(r => r.Equals("a assembleia principal", StringComparison.OrdinalIgnoreCase));
        //}
    }
}
