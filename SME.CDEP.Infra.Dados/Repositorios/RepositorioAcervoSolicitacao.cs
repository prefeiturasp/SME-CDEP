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
    }
}