using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class PesquisaAcervoDTO
{
    public string Codigo { get; set; }
    public TipoAcervo Tipo { get; set; }
    public string Titulo { get; set; }
    public string CreditoAutoria { get; set; }
    public string Assunto { get; set; }
    public string Descricao { get; set; }
    public TipoAcervoTag TipoAcervoTag { get; set; }
    public string EnderecoImagem { get; set; }
    public string DataAcervo { get; set; }
    public string Ano { get; set; }
    public long AcervoId { get; set; }
    public string EnderecoImagemPadrao { get; set; }
}