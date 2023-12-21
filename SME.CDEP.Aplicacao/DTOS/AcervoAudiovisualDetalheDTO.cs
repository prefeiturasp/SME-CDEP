namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAudiovisualDetalheDTO : AcervoDetalheDTO
{
    public string DataAcervo { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public string CopiaDigital { get; set; }
    public string PermiteUsoImagem { get; set; }
    public string Conservacao { get; set; }
    public string Cromia { get; set; }
    public string Suporte { get; set; }
    public string Duracao { get; set; }
    public string TamanhoArquivo { get; set; }
    public string Acessibilidade { get; set; }
    public string Disponibilizacao { get; set; }
    public ImagemDTO[] Imagens { get; set; }
}