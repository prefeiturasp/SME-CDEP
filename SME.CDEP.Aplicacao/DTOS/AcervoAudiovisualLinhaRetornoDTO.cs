using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAudiovisualLinhaRetornoDTO : LinhaDTO
{
    public LinhaConteudoAjustarRetornoDTO Titulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Tombo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Credito { get; set; }
    public LinhaConteudoAjustarRetornoDTO Localizacao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Procedencia { get; set; }
    public LinhaConteudoAjustarRetornoDTO Data { get; set; }
    public LinhaConteudoAjustarRetornoDTO Copia { get; set; }
    public LinhaConteudoAjustarRetornoDTO AutorizacaoUsoDeImagem { get; set; }
    public LinhaConteudoAjustarRetornoDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Descricao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Suporte { get; set; }
    public LinhaConteudoAjustarRetornoDTO Duracao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Cromia { get; set; }
    public LinhaConteudoAjustarRetornoDTO TamanhoArquivo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Acessibilidade { get; set; }
    public LinhaConteudoAjustarRetornoDTO Disponibilizacao { get; set; }
    public ImportacaoStatus Status { get; set; }
    public int NumeroLinha { get; set; }
}