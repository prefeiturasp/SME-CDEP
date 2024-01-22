﻿namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoSolicitacaoMap : BaseMapAuditavel<CDEP.Dominio.Entidades.AcervoSolicitacao>
    {
        public AcervoSolicitacaoMap()
        {
            ToTable("acervo_solicitacao");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.Situacao).ToColumn("situacao");
        }
    }
}