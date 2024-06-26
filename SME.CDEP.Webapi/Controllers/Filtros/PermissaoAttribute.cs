﻿using Microsoft.AspNetCore.Authorization;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Webapi.Filtros
{
    public class PermissaoAttribute : AuthorizeAttribute
    {
        public PermissaoAttribute(params Permissao[] permissoes)
        {
            var permissoesIds = permissoes.Select(x => (int)x);
            Roles = string.Join(",", permissoesIds);
        }
    }
}