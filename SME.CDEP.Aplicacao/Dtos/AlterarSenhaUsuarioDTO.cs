using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AlterarSenhaUsuarioDTO
    {
        public string SenhaAtual { get; set; }
        public string SenhaNova { get; set; }
        public string ConfirmarSenha { get; set; }
    }
}
