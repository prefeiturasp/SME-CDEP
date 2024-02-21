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

        public Task<IEnumerable<Evento>> ObterEventosTagPorMesAno(int mes, int ano)
        {
            var dataInicial = new DateTime(ano, mes, 1);
            var dataFinal = dataInicial.AddMonths(1).AddDays(-1);
            
            var query = @"select id,
                                 data,
                                 tipo,
                                 descricao,
                                 acervo_solicitacao_item_id,
                                 justificativa                                 
                          from evento 
                          where data::date between @dataInicial::date and @dataFinal::date 
                            and not excluido ";
            
            return conexao.Obter().QueryAsync<Evento>(query,new { dataInicial, dataFinal });
        }
        
        public Task<IEnumerable<EventoDetalhe>> ObterDetalhesDoDiaPorData(DateTime data)
        {
            var query = @"select e.id,
                                 e.data,
                                 e.tipo,
                                 e.descricao,
                                 e.acervo_solicitacao_item_id as acervoSolicitacaoItemId,
                                 e.justificativa,
                                 a.titulo,
                                 a.codigo, 
                                 a.codigo_novo as codigoNovo,
                                 u.nome as solicitante
                          from evento e
                            left join acervo_solicitacao_item asi on e.acervo_solicitacao_item_id = asi.id and not asi.excluido 
                            left join acervo a on asi.acervo_id = a.id and not a.excluido 
                            left join acervo_solicitacao aso on aso.id = asi.acervo_solicitacao_id  and not aso.excluido  
                            left join usuario u on u.id = aso.usuario_id  and not u.excluido                          
                          where e.data::date = @data::date
                            and not e.excluido ";
            
            return conexao.Obter().QueryAsync<EventoDetalhe>(query,new { data });
        }
    }
}