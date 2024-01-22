using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoItemRetornoDTO 
{
    public string TipoAcervo { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string[] AutoresCreditos { get; set; }
    public string Situacao { get; set; }
    public IEnumerable<ArquivoCodigoNomeDTO> Arquivos { get; set; }
}