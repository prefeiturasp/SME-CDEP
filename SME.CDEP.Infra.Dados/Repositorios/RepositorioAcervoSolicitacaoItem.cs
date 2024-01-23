using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

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
              excluido
            from acervo_solicitacao_item 
            where acervo_solicitacao_id in (select id from acervo_solicitacao where usuario_id = @usuario_id)
            and not excluido
            order by criado_em desc";
            
            return await conexao.Obter().QueryAsync<AcervoSolicitacaoItem>(query, new { usuarioId });
        }
    }
}