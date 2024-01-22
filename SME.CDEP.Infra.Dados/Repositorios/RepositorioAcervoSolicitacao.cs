using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoSolicitacao : RepositorioBaseAuditavel<AcervoSolicitacao>, IRepositorioAcervoSolicitacao
    {
        public RepositorioAcervoSolicitacao(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<AcervoSolicitacao> ObterAcervoSolicitacaoCompletoPorId(long acervoSolicitacaoId)
        {
            var query = @"
             select 
               id,
               usuario_id,
               situacao,
               criado_em,
               criado_por,
               criado_login,
               alterado_em,
               alterado_por,
               alterado_login,
               excluido
             from acervo_solicitacao 
             where id = @acervoSolicitacaoId
             and not excluido;
             
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
             where acervo_solicitacao_id = @acervoSolicitacaoId
             and not excluido;";
            
            var queryMultiple = await conexao.Obter().QueryMultipleAsync(query, new { acervoSolicitacaoId });

            var retorno = queryMultiple.Read<AcervoSolicitacao>();

            if (retorno.NaoPossuiElementos())
                return default;
            
            var acervoSolicitacao = retorno.FirstOrDefault();
            acervoSolicitacao.Itens = queryMultiple.Read<AcervoSolicitacaoItem>();

            return acervoSolicitacao;
        }
        
        public async Task<IEnumerable<AcervoSolicitacao>> ObterTodosCompletosPorUsuario(long usuarioId)
        {
            var query = @"
             select 
               id,
               usuario_id,
               situacao,
               criado_em,
               criado_por,
               criado_login,
               alterado_em,
               alterado_por,
               alterado_login,
               excluido
             from acervo_solicitacao 
             where usuario_id = @usuarioId
             and not excluido;
             
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
             and not excluido;";
            
            var retorno = await conexao.Obter().QueryMultipleAsync(query, new { usuarioId });

            if (retorno.EhNulo())
                return default;
            
            var acervosSolicitacoes = retorno.Read<AcervoSolicitacao>();
            var acervosSolicitacoesItens = retorno.Read<AcervoSolicitacaoItem>();

            return acervosSolicitacoes
                .Select(acervoSolicitacao =>
                {
                    acervoSolicitacao.Itens = acervosSolicitacoesItens.Where(w => w.AcervoSolicitacaoId == acervoSolicitacao.Id);
                    return acervoSolicitacao;
                });
        }

        public async Task<AcervoSolicitacaoItemResumido> ObterItensDoAcervoPorFiltros(string codigo, TipoAcervo tipo)
        {
            var query = @"
            select 
              a.tipo,
              a.titulo,
              a.id as acervoId
            from acervo a 
            where (lower(a.codigo) = lower(@codigo) or lower(codigo_novo) = lower(@codigo)) 
                and not excluido 
                and tipo = @tipo;";
            
            return await conexao.Obter().QueryFirstOrDefaultAsync<AcervoSolicitacaoItemResumido>(query, new { codigo = codigo, tipo });
        }
    }
}