using SME.CDEP.Dominio.Constantes;

namespace SME.CDEP.Aplicacao.DTOS;

public class LinhaConteudoAjustarDTO
{
    public string Conteudo { get; set; }
    public bool PossuiErro { get; set; }
    public string Mensagem { get; set; }
    public int LimiteCaracteres { get; set; }
    public bool EhCampoObrigatorio { get; set; }
    public string FormatoTipoDeCampo { get; set; } = Constantes.FORMATO_STRING;

    public void DefinirComoSucesso()
    {
        PossuiErro = false;
        Mensagem = string.Empty;
    }
}