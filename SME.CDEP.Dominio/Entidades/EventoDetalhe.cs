using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class EventoDetalhe
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
        public TipoEvento Tipo { get; set; }
        public string Descricao { get; set; }
        public long? AcervoSolicitacaoId { get; set; }
        public string Justificativa { get; set; }
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public string CodigoNovo { get; set; }
        public string Solicitante { get; set; }

        public string CodigoTombo
        {
            get
            {
                if (Codigo.PossuiElementos() && CodigoNovo.PossuiElementos())
                    return $"{Codigo}/{CodigoNovo}";

                return Codigo;
            }
        }
    }
}
