using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioEvento : RepositorioBaseAuditavel<Evento>, IRepositorioEvento
    {
        public RepositorioEvento(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public Task<bool> ExisteFeriadoOuSuspensaoNoDia(DateTime data, long? id)
        {
            var tipoFeriadoOuSuspensao = new []
            {
                (int)TipoEvento.FERIADO, 
                (int)TipoEvento.SUSPENSAO
            };
            
            var query = @"select 1 
                          from evento 
                          where tipo = any(@tipoFeriadoOuSuspensao) 
                            and data::date = @data::date 
                            and not excluido 
                            and id <> @id";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>(query,new { data,tipoFeriadoOuSuspensao, id });

        }

        public Task<IEnumerable<Evento>> ObterEventosTagPorData(DateTime data)
        {
            var query = @"select id,
                                 data,
                                 tipo,
                                 descricao,
                                 acervo_solicitacao_item_id,
                                 justificativa                                 
                          from evento 
                          where data::date = @data::date 
                            and not excluido ";
            
            return conexao.Obter().QueryAsync<Evento>(query,new { data });
        }

        public Task<IEnumerable<Evento>> ObterEventosTagPorMesAno(int mes, int ano, long[] tiposAcervosPermitidos)
        {
            var dataInicial = new DateTime(ano, mes, 1);
            var dataFinal = dataInicial.AddMonths(1).AddDays(-1);
            var eventoSuspensaoFeriado = new int[] { (int)TipoEvento.FERIADO, (int)TipoEvento.SUSPENSAO };
            var eventoTipoVisita = (int)TipoEvento.VISITA;
            
            var query = @"
            ;with eventosFixosMoveis as 
            (
            	select id,
            	         data,
            	         tipo,
            	         descricao,
            	         acervo_solicitacao_item_id,
            	         justificativa                                 
            	  from evento 
            	  where data::date between @dataInicial::date and @dataFinal::date 
            	    and not excluido
            	    and tipo = ANY(@eventoSuspensaoFeriado)
            ),
            eventosVisita as 
            (
            select distinct e.id,
                     e.data,
                     e.tipo,
                     e.descricao,
                     e.acervo_solicitacao_item_id,
                     e.justificativa
              from evento e 
                join acervo_solicitacao_item asi on e.acervo_solicitacao_item_id = asi.id and not asi.excluido 
                join acervo a on asi.acervo_id = a.id and not a.excluido and a.tipo = ANY(@tiposAcervosPermitidos) 
              where e.data::date between @dataInicial::date and @dataFinal::date 
                and not e.excluido 
                and e.tipo = @eventoTipoVisita
             )
             select * from eventosFixosMoveis 
             union all
             select * from eventosVisita";
            
            return conexao.Obter().QueryAsync<Evento>(query,new { dataInicial, dataFinal, tiposAcervosPermitidos, eventoSuspensaoFeriado, eventoTipoVisita });
        }
        
        public Task<IEnumerable<EventoDetalhe>> ObterDetalhesDoDiaPorData(DateTime data, long[] tiposAcervosPermitidos)
        {
            var eventoSuspensaoFeriado = new int[] { (int)TipoEvento.FERIADO, (int)TipoEvento.SUSPENSAO };
            var eventoTipoVisita = (int)TipoEvento.VISITA;
            
            var query = @"            
            ;with eventosFixosMoveis as 
            (
                select id,
                       data,
                       tipo,
                       descricao,
                       0 as acervoSolicitacaoId,
                       justificativa,
                       '' as titulo,
                       '' as codigo, 
                       '' as codigoNovo,
                       '' as solicitante,
                       0 as SituacaoSolicitacaoItem                                 
                from evento 
                where data::date between @data::date and @data::date 
                   and not excluido
                   and tipo = ANY(@eventoSuspensaoFeriado)
            ),
            eventosVisita as 
            (
                select distinct 
                       e.id,
                       e.data,
                       e.tipo,
                       e.descricao,
                       e.acervo_solicitacao_item_id as acervoSolicitacaoId,
                       e.justificativa,
                       a.titulo,
                       a.codigo, 
                       a.codigo_novo as codigoNovo,
                       u.nome as solicitante,
                       asi.situacao as SituacaoSolicitacaoItem
                from evento e 
                  join acervo_solicitacao_item asi on e.acervo_solicitacao_item_id = asi.id and not asi.excluido 
                  join acervo a on asi.acervo_id = a.id and not a.excluido and a.tipo = ANY(@tiposAcervosPermitidos) 
                  join usuario u on u.id = asi.usuario_responsavel_id  and not u.excluido  
                where e.data::date between @data::date and @data::date 
                   and not e.excluido 
                   and e.tipo = @eventoTipoVisita
            )
            select * from eventosFixosMoveis 
            union all
            select * from eventosVisita";
            
            return conexao.Obter().QueryAsync<EventoDetalhe>(query,new { data, tiposAcervosPermitidos, eventoSuspensaoFeriado, eventoTipoVisita });
        }

        public Task<IEnumerable<DateTime>> ObterEventosDeFeriadoESuspensaoPorDatas(DateTime[] datasDasVisitas)
        {
            var feriadoOuSuspensao = new []
            {
                (int)TipoEvento.FERIADO, 
                (int)TipoEvento.SUSPENSAO
            };
            
            var query = @"select data
                          from evento
                          where data::date = any(@datasDasVisitas::date[])
                          and tipo = any(@feriadoOuSuspensao)
                            and not excluido";
            
            return conexao.Obter().QueryAsync<DateTime>(query,new { datasDasVisitas, feriadoOuSuspensao });
        }

        public Task<Evento> ObterPorAtendimentoItemId(long atendimentoItemId)
        {
            var query = @"select id,
                                 data,
                                 tipo,
                                 descricao,
                                 acervo_solicitacao_item_id,
                                 justificativa,
                                 excluido,
                                 criado_em,
                                 criado_por,
                                 criado_login,
                                 alterado_em,
                                 alterado_por,
                                 alterado_login
                          from evento 
                          where acervo_solicitacao_item_id = @atendimentoItemId 
                            and not excluido ";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<Evento>(query,new { atendimentoItemId });
        }
    }
}