﻿using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class SuporteMap : BaseSemAuditoriaMap<Suporte>
    {
        public SuporteMap()
        {
            ToTable("suporte");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.TipoSuporte).ToColumn("tipo_suporte");
        }
    }
}