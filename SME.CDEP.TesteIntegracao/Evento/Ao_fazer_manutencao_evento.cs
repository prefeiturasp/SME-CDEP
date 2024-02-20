using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Infra.Dominio.Enumerados;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_evento : TesteBase
    {
        public Ao_fazer_manutencao_evento(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Evento - Inserir")]
        public async Task Inserir()
        {
            var servicoEvento = GetServicoEvento();
            var eventoDto = EventoCadastroDTO.GerarEventoDTO(TipoEvento.VISITA).Generate(); 

            var eventoId = await servicoEvento.Inserir(eventoDto);
            eventoId.ShouldBeGreaterThan(0);
            
            var retorno = ObterTodos<Dominio.Entidades.Evento>().LastOrDefault();
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(eventoId);
            retorno.Data.ShouldBe(eventoDto.Data);
            retorno.Descricao.ShouldBe(eventoDto.Descricao);
            retorno.Tipo.ShouldBe(eventoDto.Tipo);
            retorno.Justificativa.ShouldBe(eventoDto.Justificativa);
        }

        [Fact(DisplayName = "Evento - Obter todos")]
        public async Task ObterTodosEventos()
        {
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.VISITA);

            var eventoDtos = await servicoEvento.ObterTodos();
            eventoDtos.ShouldNotBeNull();
            eventoDtos.Count().ShouldBe(10);
        }

        [Fact(DisplayName = "Evento - Obter por id")]
        public async Task ObterEventoPorId()
        {
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.SUSPENSAO);

            var usuario = await servicoEvento.ObterPorId(1);
            usuario.ShouldNotBeNull();
            usuario.Id.ShouldBe(1);
        }

        [Fact(DisplayName = "Evento - Atualizar")]
        public async Task Atualizar()
        {
            CriarClaimUsuario();
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.FERIADO);

            var evento = await servicoEvento.ObterPorId(1);

            var data = DateTimeExtension.HorarioBrasilia().AddDays(35);
            
            var eventoAlteracao = new Aplicacao.DTOS.EventoCadastroDTO()
            {
                Id = evento.Id,
                Dia = data.Day,
                Mes = data.Month,
                Descricao = evento.Descricao.Limite(50) + "_alterado",
                Tipo = evento.Tipo,
                Justificativa = evento.Justificativa.Limite(50) + "_alterado"
            };
            
            var retorno = await servicoEvento.Alterar(eventoAlteracao);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(eventoAlteracao.Id);
            retorno.Data.ShouldBe(eventoAlteracao.Data);
            retorno.Descricao.ShouldBe(eventoAlteracao.Descricao);
            retorno.Tipo.ShouldBe(eventoAlteracao.Tipo);
            retorno.Justificativa.ShouldBe(eventoAlteracao.Justificativa);
        }
        
        [Fact(DisplayName = "Evento - Obter Eventos tag")]
        public async Task Obter_eventos_tag()
        {
            CriarClaimUsuario();
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.FERIADO);

            var eventos = ObterTodos<Dominio.Entidades.Evento>();

            var diaMes = new DiaMesDTO()
            {
                Dia = eventos.FirstOrDefault().Data.Day,
                Mes = eventos.FirstOrDefault().Data.Month
            };

            var eventosTag = await servicoEvento.ObterEventosTagPorData(diaMes);
            eventosTag.ShouldNotBeNull();
            eventosTag.All(a=> a.Tipo.EstaPreenchido()).ShouldBeTrue();
        }

        private async Task<IServicoEvento> CadastrarVarios(TipoEvento tipoEvento)
        {
            var servicoEvento = GetServicoEvento();

            var eventos = EventoMock.Instance.GerarEvento(tipoEvento).Generate(10);

            var contador = 1;

            foreach (var eventoDto in eventos)
            {
                eventoDto.Data.AddDays(contador);
                await InserirNaBase(eventoDto);
            }

            return servicoEvento;
        }
    }
}