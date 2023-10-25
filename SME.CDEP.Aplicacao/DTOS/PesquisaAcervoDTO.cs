using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class PesquisaAcervoDTO
{
    public long Id { get; set; }
    public TipoAcervo TipoAcervoId { get; set; }
    public string Titulo { get; set; }
    public string CreditoAutoria { get; set; }
    public string Assunto { get; set; }
    public string Descricao { get; set; }
    public TipoAcervoTag TipoAcervoTag { get; set; }
    public string EnderecoImagem { get; set; }
}