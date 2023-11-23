using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAudiovisualLinhaRetornoDTO : AcervoLinhaRetornoDTO
{
    public LinhaConteudoAjustarRetornoDTO Titulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Codigo { get; set; }
    public LinhaConteudoAjustarRetornoDTO CreditosAutoresIds { get; set; }
    public LinhaConteudoAjustarRetornoDTO Localizacao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Procedencia { get; set; }
    public LinhaConteudoAjustarRetornoDTO DataAcervo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Copia { get; set; }
    public LinhaConteudoAjustarRetornoDTO PermiteUsoImagem { get; set; }
    public LinhaConteudoAjustarRetornoDTO ConservacaoId { get; set; }
    public LinhaConteudoAjustarRetornoDTO Descricao { get; set; }
    public LinhaConteudoAjustarRetornoDTO SuporteId { get; set; }
    public LinhaConteudoAjustarRetornoDTO Duracao { get; set; }
    public LinhaConteudoAjustarRetornoDTO CromiaId { get; set; }
    public LinhaConteudoAjustarRetornoDTO TamanhoArquivo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Acessibilidade { get; set; }
    public LinhaConteudoAjustarRetornoDTO Disponibilizacao { get; set; }
}