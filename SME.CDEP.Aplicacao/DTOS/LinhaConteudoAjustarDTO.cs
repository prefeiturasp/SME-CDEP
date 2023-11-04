using SME.CDEP.Dominio.Constantes;

namespace SME.CDEP.Aplicacao.DTOS;

public class LinhaConteudoAjustarDTO : LinhaDTO
{
    public string Conteudo { get; set; }
    public bool Validado { get; set; }
    public string Mensagem { get; set; }
    public int LimiteCaracteres { get; set; }
    public bool EhCampoObrigatorio { get; set; }
    public string FormatoTipoDeCampo { get; set; } = Constantes.FORMATO_STRING;
}