using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoLinhaRetornoDTO
{
    public ImportacaoStatus Status { get; set; }
    public int NumeroLinha { get; set; }
    public string Mensagem { get; set; }
    public string ErrosCampos { get; set; }
}