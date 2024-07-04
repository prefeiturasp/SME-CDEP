using System.Collections.Generic;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class MenuRetornoDTO
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public bool EhMenu { get; set; }
        public string Icone { get; set; }
        public List<MenuPermissaoDTO> Menus { get; set; }
        public int QuantidadeMenus { get { return Menus.Count; } }
    }
}