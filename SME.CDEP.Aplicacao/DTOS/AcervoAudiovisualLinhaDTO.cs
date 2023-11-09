using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAudiovisualLinhaDTO: LinhaDTO
{
    public LinhaConteudoAjustarDTO Titulo { get; set; }
    public LinhaConteudoAjustarDTO Tombo { get; set; }
    public LinhaConteudoAjustarDTO Credito { get; set; }
    public LinhaConteudoAjustarDTO Localizacao { get; set; }
    public LinhaConteudoAjustarDTO Procedencia { get; set; }
    public LinhaConteudoAjustarDTO Data { get; set; }
    public LinhaConteudoAjustarDTO Copia { get; set; }
    public LinhaConteudoAjustarDTO AutorizacaoUsoDeImagem { get; set; }
    public LinhaConteudoAjustarDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarDTO Descricao { get; set; }
    public LinhaConteudoAjustarDTO Suporte { get; set; }
    public LinhaConteudoAjustarDTO Duracao { get; set; }
    public LinhaConteudoAjustarDTO Cromia { get; set; }
    public LinhaConteudoAjustarDTO TamanhoArquivo { get; set; }
    public LinhaConteudoAjustarDTO Acessibilidade { get; set; }
    public LinhaConteudoAjustarDTO Disponibilizacao { get; set; }
    public ImportacaoStatus Status { get; set; }
    public string Mensagem { get; set; }
    public int NumeroLinha { get; set; }
    public bool PossuiErros { get; set; }
}