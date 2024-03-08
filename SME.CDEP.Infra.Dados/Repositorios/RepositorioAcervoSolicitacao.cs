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

        public async Task<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutores>> ObterItensDoAcervoPorAcervosIds(long[] acervosIds)
        {
            var query = @"
            select 
              a.tipo as tipoAcervo,
              a.titulo,
              a.id as acervoId
            from acervo a 
            where a.id = any(@acervosIds)
                  and not a.excluido;

            select 
            	  ca.nome,
            	  aca.acervo_id as acervoId
            	from acervo_credito_autor aca 
            	  join credito_autor ca on ca.id = aca.credito_autor_id
            	where aca.acervo_id = any(@acervosIds) 
            	      and not ca.excluido ";
            
            var queryMultiple = await conexao.Obter().QueryMultipleAsync(query, new { acervosIds });

            var acervos = queryMultiple.Read<AcervoTipoTituloAcervoIdCreditosAutores>();

            if (acervos.NaoPossuiElementos())
                return default;

            var creditosAutores = queryMultiple.Read<CreditoAutorNomeAcervoId>();

            foreach (var acervo in acervos)
                acervo.AutoresCreditos = creditosAutores.PossuiElementos()
                    ? creditosAutores.Where(w => w.AcervoId == acervo.AcervoId).Select(s => s)
                    : Enumerable.Empty<CreditoAutorNomeAcervoId>();

            return acervos;
        }

        public Task Excluir(long acervoSolicitacaoId)
        {
            var parametros = new
            {
                AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                AlteradoPor = contexto.NomeUsuario,
                AlteradoLogin = contexto.UsuarioLogado,
                acervoSolicitacaoId
            };

            var query = @"update
	                        acervo_solicitacao_item
                        set
	                        excluido = true,	                        
	                        alterado_em = @AlteradoEm,
	                        alterado_por = @AlteradoPor,
	                        alterado_login = @AlteradoLogin
                        where acervo_solicitacao_id = @acervoSolicitacaoId;

                        update
	                        acervo_solicitacao
                        set
	                        excluido = true,                        
	                        alterado_em = @AlteradoEm,
	                        alterado_por = @AlteradoPor,
	                        alterado_login = @AlteradoLogin
                        where id = @acervoSolicitacaoId; ";

            return conexao.Obter().ExecuteAsync(query, parametros);
        }

        public async Task<AcervoSolicitacaoDetalhe> ObterDetalhesPorId(long acervoSolicitacaoId)
        {
	        var query = @"
           SELECT 
		     aso.id,
             aso.usuario_id as usuarioId,
             aso.data_solicitacao as dataSolicitacao,
             aso.situacao 
		   FROM acervo_solicitacao aso		     
		   WHERE aso.id = @acervoSolicitacaoId
		      and not aso.excluido;
		   
		   SELECT DISTINCT ON (asi.id)
		     asi.id,
             coalesce(a.codigo, a.codigo_novo) as codigo,
             a.tipo as tipoAcervo,
             a.titulo,
             asi.dt_visita as dataVisita,
             asi.situacao,
             asi.tipo_atendimento as tipoAtendimento,
             a.id as acervoId,
             resp.nome as responsavel,
             ae.dt_emprestimo as DataEmprestimo, 
             ae.dt_devolucao as Datadevolucao,
             ae.situacao as situacaoEmprestimo
		   FROM acervo_solicitacao_item asi
		     JOIN acervo a on a.id = asi.acervo_id
		     LEFT JOIN usuario resp on resp.id = asi.usuario_responsavel_id and not resp.excluido
		     LEFT JOIN acervo_emprestimo ae on ae.acervo_solicitacao_item_id = asi.id and not ae.excluido 
		   WHERE not asi.excluido
		     and not a.excluido
		     and asi.acervo_solicitacao_id = @acervoSolicitacaoId
		   ORDER BY asi.id, ae.id desc; ";
            
	        var queryMultiple = await conexao.Obter().QueryMultipleAsync(query, new { acervoSolicitacaoId });

	        var acervoSolicitacao = await queryMultiple.ReadFirstOrDefaultAsync<AcervoSolicitacaoDetalhe>();

	        if (acervoSolicitacao.EhNulo())
		        return default;

	        acervoSolicitacao.Itens = queryMultiple.Read<AcervoSolicitacaoItemDetalheResumido>();

	        return acervoSolicitacao;
        }
    }
}