﻿using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class BaseMap<T> : DommelEntityMap<T> where T : EntidadeBase
    {
        public BaseMap()
        {
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.CriadoPor).ToColumn("criado_por");
            Map(c => c.AlteradoEm).ToColumn("alterado_em");
            Map(c => c.AlteradoPor).ToColumn("alterado_por");
            Map(c => c.AlteradoLogin).ToColumn("alterado_login");
            Map(c => c.CriadoLogin).ToColumn("criado_login");
        }
    }
}
