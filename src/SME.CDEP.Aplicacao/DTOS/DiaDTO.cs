namespace SME.CDEP.Aplicacao.DTOS
{
    public class DiaDTO
    {
        public int Dia { get; set; }
        public int DayOfWeek { get; set; }
        public bool Desabilitado { get; set; }
        public IEnumerable<EventoTagDTO> EventosTag { get; set; } = Enumerable.Empty<EventoTagDTO>();
    }
}