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
           select 
		     aso.id,
             aso.usuario_id as usuarioId,
             aso.criado_em as dataSolicitacao,
             resp.login as responsavelRf,
             aso.situacao 
		   from acervo_solicitacao aso
		     left join usuario resp on resp.id = aso.usuario_responsavel_id and not resp.excluido
		   where aso.id = @acervoSolicitacaoId
		      and not aso.excluido;
		   
		   select 
		     asi.id,
             coalesce(a.codigo, a.codigo_novo) as codigo,
             a.tipo as tipoAcervo,
             a.titulo,
             asi.dt_visita as dataVisita,
             asi.situacao,
             asi.tipo_atendimento as tipoAtendimento,
             a.id as acervoId
		   from acervo_solicitacao_item asi
		     join acervo a on a.id = asi.acervo_id 
		   where not asi.excluido
		     and not a.excluido
		     and asi.acervo_solicitacao_id = @acervoSolicitacaoId; ";
            
	        var queryMultiple = await conexao.Obter().QueryMultipleAsync(query, new { acervoSolicitacaoId });

	        var acervoSolicitacao = queryMultiple.ReadFirst<AcervoSolicitacaoDetalhe>();

	        if (acervoSolicitacao.EhNulo())
		        return default;

	        acervoSolicitacao.Itens = queryMultiple.Read<AcervoSolicitacaoItemDetalheResumido>();

	        return acervoSolicitacao;
        }
    }
}