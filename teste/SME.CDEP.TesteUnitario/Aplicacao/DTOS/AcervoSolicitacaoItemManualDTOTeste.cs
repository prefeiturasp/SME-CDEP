using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemManualDTOTeste
    {
        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoPropriedadesAcervoIdEIdSaoAcessiveis()
        {
            var acervoId = new Faker().Random.Long(1, 1000);
            var id = new Faker().Random.Long(1, 1000);

            var dto = new AcervoSolicitacaoItemManualDTO
            {
                Id = id,
                AcervoId = acervoId
            };

            dto.Id.Should().Be(id);
            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTOSemId_QuandoInstanciar_EntaoIdEhNulo()
        {
            var acervoId = new Faker().Random.Long(1, 1000);

            var dto = new AcervoSolicitacaoItemManualDTO
            {
                AcervoId = acervoId
            };

            dto.Id.Should().BeNull();
            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoHerdaPropriedadesDoBaseDTO()
        {
            var faker = new Faker("pt_BR");
            var dataVisita = faker.Date.Recent();
            var dataEmprestimo = faker.Date.Recent();
            var dataDevolucao = faker.Date.Recent();
            var tipoAcervo = TipoAcervo.Bibliografico;
            var tipoAtendimento = TipoAtendimento.Presencial;
            var acervoId = faker.Random.Long(1, 1000);

            var dto = new AcervoSolicitacaoItemManualDTO
            {
                DataVisita = dataVisita,
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao,
                TipoAcervo = tipoAcervo,
                TipoAtendimento = tipoAtendimento,
                AcervoId = acervoId
            };

            dto.DataVisita.Should().Be(dataVisita);
            dto.DataEmprestimo.Should().Be(dataEmprestimo);
            dto.DataDevolucao.Should().Be(dataDevolucao);
            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.TipoAtendimento.Should().Be(tipoAtendimento);
            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoAcervoIdEhObrigatorio()
        {
            var dto = GerarAcervoSolicitacaoItemManualDTO();

            dto.AcervoId.Should().BeGreaterThan(0);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoAlterarAcervoId_EntaoValorEhAtualizado()
        {
            var dto = GerarAcervoSolicitacaoItemManualDTO();
            var novoAcervoId = new Faker().Random.Long(1001, 2000);

            dto.AcervoId = novoAcervoId;

            dto.AcervoId.Should().Be(novoAcervoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoAlterarId_EntaoValorEhAtualizado()
        {
            var dto = GerarAcervoSolicitacaoItemManualDTO();
            var novoId = new Faker().Random.Long(1001, 2000);

            dto.Id = novoId;

            dto.Id.Should().Be(novoId);
        }

        [Fact]
        public void DadoMultiplosAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoCadaUmTemValoresIndependentes()
        {
            var dto1 = GerarAcervoSolicitacaoItemManualDTO();
            var dto2 = GerarAcervoSolicitacaoItemManualDTO();

            dto1.AcervoId.Should().NotBe(dto2.AcervoId);
            dto1.Should().NotBeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTOComIdNulo_QuandoAlterarParaValorValido_EntaoIdEhAtualizado()
        {
            var dto = new AcervoSolicitacaoItemManualDTO
            {
                AcervoId = new Faker().Random.Long(1, 1000)
            };
            var novoId = new Faker().Random.Long(1, 1000);

            dto.Id = novoId;

            dto.Id.Should().Be(novoId);
            dto.Id.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoTodasAsPropriedadesDosBasesEstaoDisponiveis()
        {
            var faker = new Faker("pt_BR");
            var dto = new AcervoSolicitacaoItemManualDTO();

            typeof(AcervoSolicitacaoItemManualDTO).GetProperty(nameof(AcervoSolicitacaoItemManualDTO.Id)).Should().NotBeNull();
            typeof(AcervoSolicitacaoItemManualDTO).GetProperty(nameof(AcervoSolicitacaoItemManualDTO.AcervoId)).Should().NotBeNull();
            typeof(AcervoSolicitacaoItemManualDTO).GetProperty(nameof(AcervoSolicitacaoItemManualDTO.DataVisita)).Should().NotBeNull();
            typeof(AcervoSolicitacaoItemManualDTO).GetProperty(nameof(AcervoSolicitacaoItemManualDTO.DataEmprestimo)).Should().NotBeNull();
            typeof(AcervoSolicitacaoItemManualDTO).GetProperty(nameof(AcervoSolicitacaoItemManualDTO.DataDevolucao)).Should().NotBeNull();
            typeof(AcervoSolicitacaoItemManualDTO).GetProperty(nameof(AcervoSolicitacaoItemManualDTO.TipoAcervo)).Should().NotBeNull();
            typeof(AcervoSolicitacaoItemManualDTO).GetProperty(nameof(AcervoSolicitacaoItemManualDTO.TipoAtendimento)).Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoEhDoTipoDataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO()
        {
            var dto = GerarAcervoSolicitacaoItemManualDTO();

            dto.Should().BeAssignableTo<DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO>();
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999999L)]
        public void DadoAcervoIdCom_ValoresVariados_QuandoInstanciar_EntaoPropriedadeEhPopulada(long acervoId)
        {
            var dto = new AcervoSolicitacaoItemManualDTO { AcervoId = acervoId };

            dto.AcervoId.Should().Be(acervoId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1L)]
        [InlineData(100L)]
        public void DadoIdComValoresVariados_QuandoInstanciar_EntaoPropriedadeEhPopulada(long? id)
        {
            var dto = new AcervoSolicitacaoItemManualDTO { Id = id, AcervoId = 1L };

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoDataVisitaPodenSerNula()
        {
            var dto = new AcervoSolicitacaoItemManualDTO { AcervoId = 1L };

            dto.DataVisita.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoDataEmprestimoPodenSerNula()
        {
            var dto = new AcervoSolicitacaoItemManualDTO { AcervoId = 1L };

            dto.DataEmprestimo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemManualDTO_QuandoInstanciar_EntaoDataDevolucaoPodenSerNula()
        {
            var dto = new AcervoSolicitacaoItemManualDTO { AcervoId = 1L };

            dto.DataDevolucao.Should().BeNull();
        }
        private static AcervoSolicitacaoItemManualDTO GerarAcervoSolicitacaoItemManualDTO() =>
            new Faker<AcervoSolicitacaoItemManualDTO>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.DataVisita, f => f.Date.Recent())
                .RuleFor(x => x.DataEmprestimo, f => f.Date.Recent())
                .RuleFor(x => x.DataDevolucao, f => f.Date.Future())
                .RuleFor(x => x.TipoAcervo, f => f.PickRandom<TipoAcervo>())
                .RuleFor(x => x.TipoAtendimento, f => f.PickRandom<TipoAtendimento>())
                .Generate();
    }
}
