using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.SGP.Infra.Dominio.Atributos;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoMenu : IServicoMenu
    {
        private readonly IServicoUsuario servicoUsuario;

        public ServicoMenu(IServicoUsuario servicoUsuario)
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<MenuRetornoDTO>> ObterMenu()
        {
            var permissoes = servicoUsuario.ObterPermissoes();

            var agrupamentos = permissoes.Where(c => Enum.IsDefined(typeof(Permissao), c)).GroupBy(item => new
            {
                Descricao = item.ObterAtributo<PermissaoMenuAttribute>().Agrupamento,
                Ordem = item.ObterAtributo<PermissaoMenuAttribute>().OrdemAgrupamento
            }).OrderBy(a => a.Key.Ordem);

            var listaRetorno = new List<MenuRetornoDTO>();

            foreach (var agrupamento in agrupamentos)
            {
                var permissao = agrupamento.First();
                var atributoEnumerado = permissao.ObterAtributo<PermissaoMenuAttribute>();

                var menuRetornoDto = new MenuRetornoDTO()
                {
                    Codigo = (int)permissao,
                    Descricao = atributoEnumerado.Agrupamento,
                    Icone = atributoEnumerado.Icone,
                    EhMenu = atributoEnumerado.EhMenu
                };

                var permissoesMenu = agrupamento.GroupBy(item => new
                {
                    item.ObterAtributo<PermissaoMenuAttribute>().Menu,
                    Ordem = item.ObterAtributo<PermissaoMenuAttribute>().OrdemMenu
                }).OrderBy(a => a.Key.Ordem);

                foreach (var permissaoMenu in permissoesMenu)
                {
                    var menu = permissaoMenu.First();
                    var menuEnumerado = menu.ObterAtributo<PermissaoMenuAttribute>();

                    if (menuEnumerado.EhSubMenu)
                    {
                        var menuPai = new MenuPermissaoDTO
                        {
                            Codigo = (int)menu,
                            Descricao = menuEnumerado.Menu,
                        };

                        foreach (var subMenu in permissaoMenu.GroupBy(a => a.ObterAtributo<PermissaoMenuAttribute>().Url))
                        {
                            if (!menuEnumerado.EhSubMenu) 
                                continue;
                            
                            var menuSubEnumerado = subMenu.FirstOrDefault();
                            var menuSubEnumeradoComAtributo = menuSubEnumerado.ObterAtributo<PermissaoMenuAttribute>();

                            var url = ObterUrlComRedirect(menuSubEnumeradoComAtributo);

                            var permissoesSubMenu = permissaoMenu.Where(c =>
                                c.ObterAtributo<PermissaoMenuAttribute>().Url == subMenu.Key);

                            menuPai.SubMenus.Add(new MenuPermissaoDTO()
                            {
                                Codigo = (int)menuSubEnumerado,
                                Url = url,
                                Descricao = menuSubEnumeradoComAtributo.SubMenu,
                                Ordem = menuSubEnumeradoComAtributo.OrdemSubMenu,
                                PodeConsultar = permissoesSubMenu.Any(a => a.ObterAtributo<PermissaoMenuAttribute>().EhConsulta),
                                PodeAlterar = permissoesSubMenu.Any(a => a.ObterAtributo<PermissaoMenuAttribute>().EhAlteracao),
                                PodeIncluir = permissoesSubMenu.Any(a => a.ObterAtributo<PermissaoMenuAttribute>().EhInclusao),
                                PodeExcluir = permissoesSubMenu.Any(a => a.ObterAtributo<PermissaoMenuAttribute>().EhExclusao),
                            });
                        }

                        menuRetornoDto.Menus.Add(menuPai);
                    }
                    else
                    {
                        var url = ObterUrlComRedirect(menuEnumerado);
                        menuRetornoDto.Menus.Add(new MenuPermissaoDTO()
                        {
                            Codigo = (int)menu,
                            Url = url,
                            Descricao = menuEnumerado.Menu,
                            PodeAlterar = permissaoMenu.Any(a => a.ObterAtributo<PermissaoMenuAttribute>().EhAlteracao),
                            PodeIncluir = permissaoMenu.Any(a => a.ObterAtributo<PermissaoMenuAttribute>().EhInclusao),
                            PodeExcluir = permissaoMenu.Any(a => a.ObterAtributo<PermissaoMenuAttribute>().EhExclusao),
                            PodeConsultar = permissaoMenu.Any(a => a.ObterAtributo<PermissaoMenuAttribute>().EhConsulta),
                        });
                    }
                }
                
                menuRetornoDto.Menus = menuRetornoDto.Menus.OrderBy(a => a.Ordem).ToList();
                listaRetorno.Add(menuRetornoDto);
            }
            
            return listaRetorno;
        }

        private string ObterUrlComRedirect(PermissaoMenuAttribute permissaoMenuAttribute)
        {
            return permissaoMenuAttribute.Url;
        }
    }
}