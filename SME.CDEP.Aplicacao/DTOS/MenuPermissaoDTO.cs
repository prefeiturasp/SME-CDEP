using System.Collections.Generic;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class MenuPermissaoDTO
    {
        public MenuPermissaoDTO()
        {
            SubMenus = new List<MenuPermissaoDTO>();
        }

        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public bool PodeAlterar { get; set; }
        public bool PodeConsultar { get; set; }
        public bool PodeExcluir { get; set; }
        public bool PodeIncluir { get; set; }
        public IList<MenuPermissaoDTO> SubMenus { get; set; }
        public string Url { get; set; }
        public int Ordem { get; set; }
        public string AjudaDoSistema { get; set; }
    }
}