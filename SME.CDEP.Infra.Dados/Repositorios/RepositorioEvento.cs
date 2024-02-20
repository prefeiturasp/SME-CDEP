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
    }
}