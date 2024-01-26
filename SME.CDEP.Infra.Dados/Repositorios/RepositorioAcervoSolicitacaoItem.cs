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
        
        public async Task<IEnumerable<AcervoSolicitacaoItemResumido>> ObterMinhasSolicitacoes(long usuarioId)
        {
            var query = @"
            select 
              asi.acervo_solicitacao_id as AcervoSolicitacaoId,
              asi.criado_em as DataCriacao,              
              asi.situacao,
              a.tipo as TipoAcervo,
              a.titulo 
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
    }
}