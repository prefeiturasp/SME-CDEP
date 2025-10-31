using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades;

public class AcervoAudiovisualDetalhe 
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string Codigo { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public string Ano { get; set; }
    public string DataAcervo { get; set; }
    public string Copia { get; set; }
    public bool PermiteUsoImagem { get; set; }
    public string Conservacao { get; set; }
    public string Descricao { get; set; }
    public string Suporte { get; set; }
    public string Duracao { get; set; }
    public string Cromia { get; set; }
    public string TamanhoArquivo { get; set; }
    public string Acessibilidade { get; set; }
    public string Disponibilizacao { get; set; }
    public string Creditos { get; set; }
    public SituacaoAcervo SituacaoAcervo { get; set; }
}