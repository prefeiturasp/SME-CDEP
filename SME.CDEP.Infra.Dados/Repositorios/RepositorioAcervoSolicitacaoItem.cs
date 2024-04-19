using System.Text;
using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoSolicitacaoItem : RepositorioBaseAuditavel<AcervoSolicitacaoItem>, IRepositorioAcervoSolicitacaoItem
    {
        public RepositorioAcervoSolicitacaoItem(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<IEnumerable<AcervoSolicitacaoItemResumido>> ObterMinhasSolicitacoes(long usuarioId)
        {
            var query = @"
            select 
              asi.acervo_solicitacao_id as AcervoSolicitacaoId,
              asi.criado_em as DataCriacao,              
              asi.situacao,
              a.tipo as TipoAcervo,
              a.titulo,
              asi.dt_visita dataVisita
            from acervo_solicitacao_item asi
            join acervo_solicitacao aso on asi.acervo_solicitacao_id = aso.id 
            join acervo a on a.id = asi.acervo_id 
            where aso.usuario_id = @usuarioId
            and not asi.excluido
            and not aso.excluido
            and not a.excluido
            order by asi.criado_em desc";
            
            return await conexao.Obter().QueryAsync<AcervoSolicitacaoItemResumido>(query, new { usuarioId });
        }

        public async Task<IEnumerable<AcervoSolicitacaoItemDetalhe>> ObterSolicitacoesPorFiltro(long? acervoSolicitacaoId, TipoAcervo? tipoAcervo, DateTime? dataSolicitacaoInicio,
            DateTime? dataSolicitacaoFim, string? responsavel, SituacaoSolicitacaoItem? situacaoItem, DateTime? dataVisitaInicio, DateTime? dataVisitaFim, string? solicitanteRf,
            SituacaoEmprestimo? situacaoEmprestimo, long[] tiposAcervosPermitidos)
        {
            var query = new StringBuilder();
            query.AppendLine(@"
            SELECT DISTINCT ON (asi.id)
                 asi.id,
                 asi.acervo_solicitacao_id as AcervoSolicitacaoId,
                 a.tipo as tipoAcervo,
                 asi.criado_em as dataCriacao,
                 asi.dt_visita as DataVisita, 
                 u.nome as solicitante,
                 asi.situacao,
                 ur.nome as Responsavel,
                 situacao_emprestimo.situacao as SituacaoEmprestimo,
                 a.titulo
            FROM acervo_solicitacao_item asi
               JOIN acervo_solicitacao aso on aso.id = asi.acervo_solicitacao_id
               JOIN acervo a on a.id = asi.acervo_id
               JOIN usuario u on u.id = aso.usuario_id
               LEFT JOIN usuario ur on ur.id = asi.usuario_responsavel_id and not ur.excluido
               LEFT JOIN LATERAL (
                   SELECT situacao
                   FROM acervo_emprestimo ae
                   WHERE ae.acervo_solicitacao_item_id = asi.id
                   AND not ae.excluido
                   ORDER BY ae.id DESC
                   LIMIT 1
               ) situacao_emprestimo ON true 
            where not asi.excluido
              and not aso.excluido
              and not a.excluido 
              and not u.excluido 
              and a.tipo = ANY(@tiposAcervosPermitidos) ");

            if (acervoSolicitacaoId.HasValue)
                query.AppendLine(" and aso.id = @acervoSolicitacaoId ");
            
            if (tipoAcervo.HasValue)
                query.AppendLine(" and a.tipo = @tipoAcervo ");
            
            if (situacaoItem.HasValue)
                query.AppendLine(" and asi.situacao = @situacaoItem ");
            
            if (dataSolicitacaoInicio.HasValue && dataSolicitacaoFim.HasValue)
                query.AppendLine(" and asi.criado_em::Date between @dataSolicitacaoInicio::Date and @dataSolicitacaoFim::Date ");
            
            if (dataVisitaInicio.HasValue && dataVisitaFim.HasValue)
                query.AppendLine(" and asi.dt_visita is not null and asi.dt_visita::Date between @dataVisitaInicio::Date and @dataVisitaFim::Date ");

            if (responsavel.EstaPreenchido())
                query.AppendLine(" and ur.login = @responsavel ");
            
            if (solicitanteRf.EstaPreenchido())
                query.AppendLine(" and u.login = @solicitanteRf ");
            
            if (situacaoEmprestimo.HasValue)
                query.AppendLine(" and situacao_emprestimo.situacao = @situacaoEmprestimo ");
            
            query.AppendLine(" order by asi.id desc ");
            
            return await conexao.Obter().QueryAsync<AcervoSolicitacaoItemDetalhe>(query.ToString(), 
                new { acervoSolicitacaoId, tipoAcervo, situacaoItem, dataSolicitacaoInicio, dataSolicitacaoFim, 
                    dataVisitaInicio, dataVisitaFim, responsavel, solicitanteRf, situacaoEmprestimo, tiposAcervosPermitidos});
        }

        public Task<IEnumerable<AcervoSolicitacaoItem>> ObterItensEmSituacaoAguardandoAtendimentoOuVisitaOuFinalizadoManualmentePorSolicitacaoId(long acervoSolicitacaoId)
        {
            var situacoesItensAguardandoAtendimentoEVisitaOuFinalizadoManualmente = new []
            {
                (int)SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO,
                (int)SituacaoSolicitacaoItem.AGUARDANDO_VISITA,
                (int)SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE,
            };
            
            var query = @"
             select id,
               acervo_solicitacao_id,
               acervo_id,
               situacao,
               dt_visita,
               criado_em,
               criado_por,
               criado_login,
               tipo_atendimento,
               usuario_responsavel_id
            from acervo_solicitacao_item
            where acervo_solicitacao_id = @acervoSolicitacaoId
              and situacao = any(@situacoesItensAguardandoAtendimentoEVisitaOuFinalizadoManualmente) 
              and not excluido";
            
            return conexao.Obter().QueryAsync<AcervoSolicitacaoItem>(query, new { acervoSolicitacaoId, situacoesItensAguardandoAtendimentoEVisitaOuFinalizadoManualmente });
        }
        
        public Task<IEnumerable<AcervoSolicitacaoItem>> ObterItensEmSituacaoAguardandoVisitaPorSolicitacaoId(long acervoSolicitacaoId)
        {
            var query = @"
             select id,
               acervo_solicitacao_id,
               acervo_id,
               situacao,
               dt_visita,
               criado_em,
               criado_por,
               criado_login,
               tipo_atendimento,
               usuario_responsavel_id
            from acervo_solicitacao_item
            where acervo_solicitacao_id = @acervoSolicitacaoId
              and situacao = @situacaoAguardandoVisita 
              and not excluido";
            
            return conexao.Obter().QueryAsync<AcervoSolicitacaoItem>(query, new { acervoSolicitacaoId, situacaoAguardandoVisita = (int)SituacaoSolicitacaoItem.AGUARDANDO_VISITA });
        }
        
        public Task<IEnumerable<AcervoSolicitacaoItem>> ObterItensPorSolicitacaoId(long acervoSolicitacaoId)
        {
            var query = @"
             select id,
               acervo_solicitacao_id,
               acervo_id,
               situacao,
               dt_visita,
               criado_em,
               criado_por,
               criado_login,
               tipo_atendimento,
               usuario_responsavel_id
            from acervo_solicitacao_item
            where acervo_solicitacao_id = @acervoSolicitacaoId 
              and not excluido";
            
            return conexao.Obter().QueryAsync<AcervoSolicitacaoItem>(query, new { acervoSolicitacaoId });
        }

        public Task<Acervo> ObterAcervoPorAcervoSolicitacaoItemId(long acervoSolicitacaoItemId)
        {
            var query = @"
             select a.id,
                    a.tipo, 
                    a.titulo, 
                    a.codigo, 
                    a.codigo_novo 
             from acervo_solicitacao_item asi
             join acervo a on a.id = asi.acervo_id 
             where asi.id = @acervoSolicitacaoItemId 
               and not asi.excluido
               and not a.excluido";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<Acervo>(query, new { acervoSolicitacaoItemId });
        }

        public Task<bool> PossuiItensEmSituacaoAguardandoAtendimentoOuAguardandoVisitaComDataFutura(long acervoSolicitacaoId)
        {
            var query = @"
             select 1
            from acervo_solicitacao_item
            where acervo_solicitacao_id = @acervoSolicitacaoId
            and not excluido
            and (situacao = @situacaoAguardandoAtendimento or situacao = @situacaoAguardandoVisita and dt_visita::date > @dataAtual::date )";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>(query, new { acervoSolicitacaoId, 
                situacaoAguardandoAtendimento = (int)SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO,
                situacaoAguardandoVisita = (int)SituacaoSolicitacaoItem.AGUARDANDO_VISITA, dataAtual = DateTimeExtension.HorarioBrasilia().Date });
        }

        public Task<bool> PossuiItensEmSituacaoFinalizadoAutomaticamenteOuCancelado(long acervoSolicitacaoItemId)
        {
            var situacoesItensNaoCancelaveis = new []
            {
                (int)SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
                (int)SituacaoSolicitacaoItem.CANCELADO
            };
            
            var query = @"
             select 1
            from acervo_solicitacao_item
            where id = @acervoSolicitacaoItemId
            and not excluido
            and situacao = any(@situacoesItensNaoCancelaveis) ";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>(query, new { acervoSolicitacaoItemId, situacoesItensNaoCancelaveis });
        }
        
        public Task<bool> PossuiItensFinalizadosAutomaticamente(long acervoSolicitacaoId)
        {
            var situacoesItensFinalizados = new []
            {
                (int)SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
                (int)SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
            };
            
            var query = @"
             select 1
            from acervo_solicitacao_item
            where acervo_solicitacao_id = @acervoSolicitacaoId
            and not excluido
            and situacao = any(@situacoesItensFinalizados) ";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>(query, new
            {
                acervoSolicitacaoId, 
                situacoesItensFinalizados
            });
        }

        public Task<bool> AtendimentoPossuiItemSituacaoFinalizadoAutomaticamenteOuCancelado(long acervoSolicitacaoItemId)
        {
            var situacoesItensConfirmadas = new []
            {
                (int)SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
                (int)SituacaoSolicitacaoItem.CANCELADO
            };
            
            var query = @"
             select 1
            from acervo_solicitacao_item i
            join acervo_solicitacao a on a.id = i.acervo_solicitacao_id 
            where i.id = @acervoSolicitacaoItemId
             and not i.excluido
             and not a.excluido
             and i.situacao = any(@situacoesItensConfirmadas) ";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>(query, new { acervoSolicitacaoItemId, situacoesItensConfirmadas });
        }
        
        public Task<IEnumerable<AcervoSolicitacaoItemDetalhe>> ObterDetalhamentoDosItensPorSolicitacaoOuItem(long? acervoSolicitacaoId,long? acervoSolicitacaoItemId)
        {
            var query = @"
            select asi.Id, 
                   aso.id as acervoSolicitacaoId,                   
                   a.tipo as TipoAcervo,
                   asi.dt_visita as dataVisita,
                   u.nome as solicitante,
                   a.titulo,
                   a.codigo,
                   a.codigo_novo as codigoNovo,
                   u.email,
                   asi.situacao,
                   asi.tipo_atendimento as tipoAtendimento
            from acervo_solicitacao aso
              join acervo_solicitacao_item asi on aso.id = asi.acervo_solicitacao_id 
              join usuario u on u.id = aso.usuario_id 
              join acervo a on a.id = asi.acervo_id 
            where not aso.excluido
              and asi.situacao <> @finalizadoAutomaticamente ";

            if (acervoSolicitacaoId.HasValue)
                query += " and aso.id = @acervoSolicitacaoId";
            
            if (acervoSolicitacaoItemId.HasValue)
                query += " and asi.id = @acervoSolicitacaoItemId";
            
            return conexao.Obter().QueryAsync<AcervoSolicitacaoItemDetalhe>(query, 
                new
                {
                    acervoSolicitacaoId,
                    acervoSolicitacaoItemId, 
                    finalizadoAutomaticamente = (int)SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE
                });
        }
    }
}