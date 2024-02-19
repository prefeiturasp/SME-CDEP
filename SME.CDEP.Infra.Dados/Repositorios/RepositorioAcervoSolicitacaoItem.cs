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
            DateTime? dataSolicitacaoFim, string? responsavel, SituacaoSolicitacaoItem? situacaoItem, DateTime? dataVisitaInicio, DateTime? dataVisitaFim)
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
                 asi.situacao,
                 ur.nome as Responsavel
            from acervo_solicitacao_item asi
               join acervo_solicitacao aso on aso.id = asi.acervo_solicitacao_id
               join acervo a on a.id = asi.acervo_id
               join usuario u on u.id = aso.usuario_id
               left join usuario ur on ur.id = aso.usuario_responsavel_id and not ur.excluido
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
            
            if (dataSolicitacaoInicio.HasValue && dataSolicitacaoFim.HasValue)
                query.AppendLine(" and asi.criado_em::Date between @dataSolicitacaoInicio::Date and @dataSolicitacaoFim::Date ");
            
            if (dataVisitaInicio.HasValue && dataVisitaFim.HasValue)
                query.AppendLine(" and asi.dt_visita is not null and asi.dt_visita::Date between @dataVisitaInicio::Date and @dataVisitaFim::Date ");

            if (responsavel.EstaPreenchido())
                query.AppendLine(" and ur.login = @responsavel ");
            
            query.AppendLine(" order by asi.criado_em desc ");
            
            return await conexao.Obter().QueryAsync<AcervoSolicitacaoItemDetalhe>(query.ToString(), 
                new { acervoSolicitacaoId, tipoAcervo, situacaoItem, dataSolicitacaoInicio, dataSolicitacaoFim, dataVisitaInicio, dataVisitaFim, responsavel});
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
               tipo_atendimento
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
               tipo_atendimento
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
               tipo_atendimento
            from acervo_solicitacao_item
            where acervo_solicitacao_id = @acervoSolicitacaoId 
              and not excluido";
            
            return conexao.Obter().QueryAsync<AcervoSolicitacaoItem>(query, new { acervoSolicitacaoId });
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
        
        public Task<bool> PossuiItensQueForamAtendidosParcialmente(long acervoSolicitacaoId)
        {
            var situacoesForamAtendidosParcialmente = new []
            {
                (int)SituacaoSolicitacaoItem.AGUARDANDO_VISITA,
                (int)SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE,
                (int)SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
            };
            
            var query = @"
             select 1
            from acervo_solicitacao_item
            where acervo_solicitacao_id = @acervoSolicitacaoId
            and not excluido
            and situacao = any(@situacoesForamAtendidosParcialmente) ";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>(query, new { acervoSolicitacaoId, situacoesForamAtendidosParcialmente });
        }

        public Task<bool> AtendimentoPossuiSituacaoAguardandoVisitaEItemSituacaoFinalizadoAutomaticamenteOuCancelado(long acervoSolicitacaoItemId)
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
             and (a.situacao <> @situacaoAguardandoVisita or i.situacao = any(@situacoesItensConfirmadas)) ";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>(query, new { acervoSolicitacaoItemId, situacaoAguardandoVisita = (int)SituacaoSolicitacao.AGUARDANDO_VISITA, situacoesItensConfirmadas });
        }
    }
}