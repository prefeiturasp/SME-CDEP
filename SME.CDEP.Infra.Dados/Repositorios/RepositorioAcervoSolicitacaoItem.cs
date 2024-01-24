using System.Text;
using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoSolicitacaoItem : RepositorioBaseAuditavel<AcervoSolicitacaoItem>, IRepositorioAcervoSolicitacaoItem
    {
        public RepositorioAcervoSolicitacaoItem(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<IEnumerable<AcervoSolicitacaoItem>> ObterMinhasSolicitacoes(long usuarioId)
        {
            var query = @"
            select 
              id,
              acervo_solicitacao_id,
              acervo_id,
              situacao,
              criado_em,
              criado_por,
              criado_login,
              alterado_em,
              alterado_por,
              alterado_login,
              excluido,
              situacao
            from acervo_solicitacao_item 
            where acervo_solicitacao_id in (select id from acervo_solicitacao where usuario_id = @usuario_id)
            and not excluido
            order by criado_em desc";
            
            return await conexao.Obter().QueryAsync<AcervoSolicitacaoItem>(query, new { usuarioId });
        }

        public async Task<IEnumerable<AcervoSolicitacaoItemDetalhe>> ObterSolicitacoesPorFiltro(long? acervoSolicitacaoId, TipoAcervo? tipoAcervo, DateTime? dataSolicitacao,
            string? responsavel, SituacaoSolicitacaoItem? situacaoItem, DateTime? dataVisitaInicio, DateTime? dataVisitaFim)
        {
            var query = new StringBuilder();
            
            query.AppendLine(@"
            select 
                 asi.id,
                 asi.acervo_solicitacao_id as AcervoSolicitacaoId,
                 a.tipo as tipoAcervo,
                 asi.criado_em as dataCriacao,
                 asi.dt_visita as DataVisita, 
                 u.nome as solicitante,
                 asi.situacao
            from acervo_solicitacao_item asi
               join acervo_solicitacao aso on aso.id = asi.acervo_solicitacao_id
               join acervo a on a.id = asi.acervo_id
               join usuario u on u.id = aso.usuario_id
            where not asi.excluido
              and not aso.excluido
              and not a.excluido 
              and not u.excluido ");

            if (acervoSolicitacaoId.HasValue)
                query.AppendLine(" and aso.id = @acervoSolicitacaoId ");
            
            if (tipoAcervo.HasValue)
                query.AppendLine(" and a.tipo = @tipoAcervo ");
            
            if (situacaoItem.HasValue)
                query.AppendLine(" and asi.situacao = @situacaoItem ");
            
            if (dataSolicitacao.HasValue)
                query.AppendLine(" and asi.criado_em::Date = @dataSolicitacao::Date ");
            
            if (dataVisitaInicio.HasValue && dataVisitaFim.HasValue)
                query.AppendLine(" and asi.dt_visita is not null and asi.dt_visita::Date between @dataVisitaInicio::Date and @dataVisitaFim::Date ");

            query.AppendLine(" order by asi.criado_em desc ");
            
            return await conexao.Obter().QueryAsync<AcervoSolicitacaoItemDetalhe>(query.ToString(), 
                new { acervoSolicitacaoId, tipoAcervo, situacaoItem, dataSolicitacao, dataVisitaInicio, dataVisitaFim});
        }
    }
}