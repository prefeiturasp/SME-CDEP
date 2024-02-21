namespace SME.CDEP.Aplicacao.DTOS
{
    public class SemanaDTO
    {
        public int Numero { get; set; }
        public List<DiaDTO> Dias { get; set; } = new ();
    }
}