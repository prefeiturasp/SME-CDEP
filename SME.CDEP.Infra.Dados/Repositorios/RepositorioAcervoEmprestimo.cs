using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoEmprestimo : RepositorioBaseAuditavel<AcervoEmprestimo>, IRepositorioAcervoEmprestimo
    {
        public RepositorioAcervoEmprestimo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcervoEmprestimo>> ObterUltimoEmprestimoPorAcervoSolicitacaoItemIds(long[] acervoSolicitacaoItemIds)
        {
            var query = @"
            SELECT DISTINCT ON (acervo_solicitacao_item_id)
                   id,
                   acervo_solicitacao_item_id,
                   dt_emprestimo,
                   dt_devolucao,
                   situacao,
                   criado_em,
                   criado_por,
                   criado_login,
                   alterado_em,
                   alterado_por,
                   alterado_login
            FROM acervo_emprestimo
            WHERE NOT excluido
            AND acervo_solicitacao_item_id = ANY(@acervoSolicitacaoItemIds) 
            ORDER BY acervo_solicitacao_item_id, id DESC;";
            
            return await conexao.Obter().QueryAsync<AcervoEmprestimo>(query, new { acervoSolicitacaoItemIds });
        }
    }
}