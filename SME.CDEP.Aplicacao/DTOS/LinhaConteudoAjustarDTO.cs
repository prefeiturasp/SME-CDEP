using SME.CDEP.Dominio.Constantes;

namespace SME.CDEP.Aplicacao.DTOS;

public class LinhaConteudoAjustarDTO
{
    public string Conteudo { get; set; }
    public bool PossuiErro { get; set; }
    public string Mensagem { get; set; }
    public int LimiteCaracteres { get; set; }
    public bool EhCampoObrigatorio { get; set; }
    public IEnumerable<string> ValoresPermitidos { get; set; }
    public string FormatoTipoDeCampo { get; set; } = Constantes.FORMATO_STRING;
    public bool PermiteNovoRegistro { get; set; }
    public string ValidarComExpressaoRegular { get; set; }
    public string MensagemValidacao { get; set; }

    public void DefinirComoSucesso()
    {
        PossuiErro = false;
        Mensagem = string.Empty;
    }
}