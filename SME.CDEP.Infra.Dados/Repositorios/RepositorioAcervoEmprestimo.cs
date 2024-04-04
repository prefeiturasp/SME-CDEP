using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

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
        
        public async Task<AcervoEmprestimo> ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(long acervoSolicitacaoItemId)
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
            AND acervo_solicitacao_item_id = @acervoSolicitacaoItemId 
            ORDER BY acervo_solicitacao_item_id, id DESC;";
            
            return await conexao.Obter().QueryFirstOrDefaultAsync<AcervoEmprestimo>(query, new { acervoSolicitacaoItemId });
        }
        
        public async Task<IEnumerable<AcervoEmprestimo>> ObterItensEmprestadosAtrasados()
        {
            var situacoesEmprestadoOuProrrogado = new []
            {
                (int)SituacaoEmprestimo.EMPRESTADO,
                (int)SituacaoEmprestimo.EMPRESTADO_PRORROGACAO
            };
            
            var query = @"
            ;with acervosEmAtraso as
            (
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
               AND dt_devolucao::date < @dataAtual::date            
               ORDER BY acervo_solicitacao_item_id, id DESC
            )
            SELECT * 
            FROM acervosEmAtraso
            WHERE situacao = ANY(@situacoesEmprestadoOuProrrogado)";
            
            return await conexao.Obter().QueryAsync<AcervoEmprestimo>(query, new { situacoesEmprestadoOuProrrogado, dataAtual = DateTimeExtension.HorarioBrasilia() });
        }
        
        public async Task<IEnumerable<AcervoEmprestimoAntesVencimentoDevolucao>> ObterDetalhamentoDosItensANotificarAntesVencimentoEmprestimo(DateTime dataDevolucaoNotificada)
        {
            var query = @"
            ;with ultimaMovimentacaoAcervos as
            (
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
               ORDER BY acervo_solicitacao_item_id, id desc
            )
               SELECT asi.Id as acervoSolicitacaoItemId, 
                        aso.id as acervoSolicitacaoId,
                        u.nome as solicitante,
                        a.titulo,
                        a.codigo,
                        u.email,      
                        ae.dt_emprestimo as dataEmprestimo,
                        ae.dt_devolucao as dataDevolucao
             FROM ultimaMovimentacaoAcervos ae
                join acervo_solicitacao_item asi on asi.id = ae.acervo_solicitacao_item_id
                join acervo_solicitacao aso on aso.id = asi.acervo_solicitacao_id 
                join usuario u on u.id = aso.usuario_id 
                join acervo a on a.id = asi.acervo_id
            WHERE ae.situacao <> @situacaoDevolvido
              and ae.dt_devolucao::date = @dataDevolucaoNotificada::date "; 
            
            return await conexao.Obter().QueryAsync<AcervoEmprestimoAntesVencimentoDevolucao>(query, new { situacaoDevolvido = (int)SituacaoEmprestimo.DEVOLVIDO, dataDevolucaoNotificada });
        }
    }
}