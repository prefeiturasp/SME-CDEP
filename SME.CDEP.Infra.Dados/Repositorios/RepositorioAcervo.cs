using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervo : RepositorioBaseAuditavel<Acervo>, IRepositorioAcervo
    {
        public RepositorioAcervo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<Acervo>> PesquisarPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo)
        {
            var query = @"select a.id, 
                                 a.tipo, 
                                 a.titulo,
                                 a.descricao, 
                                 case when length(a.codigo_novo) > 0 then 
     	                                case when length(a.codigo) > 0 then concat(a.codigo,'/',a.codigo_novo) 
     	                                else a.codigo_novo 
     	                                end
                                     else a.codigo end codigo,
                                 a.data_acervo,
                                 a.ano,
                                 a.criado_em, 
                                 a.criado_por, 
                                 a.criado_login, 
                                 a.alterado_em, 
                                 a.alterado_por, 
                                 a.alterado_login, 
                                 ca.id, 
                                 ca.nome, 
                                 ca.tipo 
							from acervo a
							    left join acervo_credito_autor aca on aca.acervo_id = a.id
						        left join credito_autor ca on aca.credito_autor_id = ca.id
						    where not a.excluido ";

            if (titulo.EstaPreenchido())
                query += $"and lower(a.titulo) like lower('%{titulo}%') ";
	
            if (codigo.EstaPreenchido())
                query += $"and (lower(a.codigo) = lower('{codigo}') or lower(a.codigo_novo) = lower('{codigo}') )";
	
            if (tipoAcervo > 0)
                query += "and a.Tipo = @tipoAcervo ";
	
            if (creditoAutorId > 0)
                query += "and aca.credito_autor_id = @creditoAutorId ";
	
            return (await conexao.Obter().QueryAsync<Acervo, CreditoAutor, Acervo>(query, (acervo, creditoAutor) =>
            {
                acervo.CreditoAutor = creditoAutor;
                return acervo;
            }, new { tipoAcervo, creditoAutorId }, splitOn: "id"));
        }
        
        public Task<bool> ExisteCodigo(string codigo, long id, TipoAcervo tipo)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>("select 1 from acervo where (lower(codigo) = @codigo or lower(codigo_novo) = @codigo) and not excluido and id != @id and tipo = @tipo",new { id, codigo = codigo.ToLower(), tipo });
        }

        public async Task<IEnumerable<ArquivoCodigoNomeAcervoId>> ObterArquivosPorAcervoId(long[] acervosIds)
        {
             var query = @"
             select a.nome, a.codigo, aag.acervo_id as acervoId 
             from acervo_arte_grafica_arquivo aaga 
             join acervo_arte_grafica aag on aag.id = aaga.acervo_arte_grafica_id 
             join arquivo a on a.id = aaga.arquivo_id 
             where aag.acervo_id = any(@acervosIds)
             and not a.excluido 
             and aag.permite_uso_imagem
             
             union all
             
             select a.nome, a.codigo, ad.acervo_id as acervoId
             from acervo_documental_arquivo ada
             join acervo_documental ad on ad.id = ada.acervo_documental_id  
             join arquivo a on a.id = ada.arquivo_id 
             where ad.acervo_id = any(@acervosIds)
             and not a.excluido
             
             union all
             
             select a.nome, a.codigo, af.acervo_id as acervoId 
             from acervo_fotografico_arquivo afa
             join acervo_fotografico af on af.id = afa.acervo_fotografico_id  
             join arquivo a on a.id = afa.arquivo_id 
             where af.acervo_id = any(@acervosIds)
             and af.permite_uso_imagem 
             and not a.excluido
             
             union all
             
             select a.nome, a.codigo, at.acervo_id as acervoId
             from acervo_tridimensional_arquivo ata 
             join acervo_tridimensional at  on at.id = ata.acervo_tridimensional_id  
             join arquivo a on a.id = ata.arquivo_id 
             where at.acervo_id = any(@acervosIds)
             and not a.excluido; ";
            
            return await conexao.Obter().QueryAsync<ArquivoCodigoNomeAcervoId>(query, new { acervosIds });
        }
        
        public async Task<IEnumerable<Acervo>> ObterAcervosPorIds(long[] ids)
        {
            var query = @"
             select 
                id,
                tipo,
                titulo,
                codigo,
                excluido,
                criado_em,
                criado_por,
                alterado_em,
                alterado_por,
                criado_login,
                alterado_login,
                codigo_novo,
                subtitulo,
                descricao,
                ano,
                data_acervo,
                ano_inicio,
                ano_fim
            from acervo
            where id = any(@ids)
                and not excluido; ";
            
            return await conexao.Obter().QueryAsync<Acervo>(query, new { ids });
        }

        public async Task<IEnumerable<AcervoSolicitacaoItemCompleto>> ObterAcervosSolicitacoesItensCompletoPorId(long acervoSolicitacaoId, long[] tiposAcervosPermitidos)
        {
            var query = @"
            select 
              asi.id,  
              a.tipo as tipoAcervo,
              a.titulo,
              a.id as acervoId,
              asi.situacao as situacaoItem,
              asi.tipo_atendimento as tipoAtendimento,
              asi.dt_visita as DataVisita,
              aso.situacao
            from acervo a 
            join acervo_solicitacao_item asi on asi.acervo_id = a.id
            join acervo_solicitacao aso on aso.id = asi.acervo_solicitacao_id
            where aso.id = @acervoSolicitacaoId 
                and not a.excluido
                and not aso.excluido
                and not asi.excluido
                and a.tipo = ANY(@tiposAcervosPermitidos);
               
            select 
              ca.nome,
              aca.acervo_id as AcervoId
            from acervo_credito_autor aca 
              join credito_autor ca on ca.id = aca.credito_autor_id
            where aca.acervo_id in (select acervo_id from acervo_solicitacao_item a join acervo b on a.acervo_id = b.id where acervo_solicitacao_id = @acervoSolicitacaoId and b.tipo = ANY(@tiposAcervosPermitidos))
            and not ca.excluido; ";
            
            var retorno = await conexao.Obter().QueryMultipleAsync(query, new { acervoSolicitacaoId, tiposAcervosPermitidos });

            if (retorno.EhNulo())
                return default;
            
            var acervosSolicitacoes = retorno.Read<AcervoSolicitacaoItemCompleto>();
            var creditosAutoresNomes = retorno.Read<CreditoAutorNomeAcervoId>();

            foreach (var acervoSolicitacao in acervosSolicitacoes)
                acervoSolicitacao.AutoresCreditos = ObterCreditosAutores(creditosAutoresNomes, acervoSolicitacao);
            
            return acervosSolicitacoes;
        }

        private IEnumerable<ArquivoCodigoNomeAcervoId> ObterArquivos(IEnumerable<ArquivoCodigoNomeAcervoId> arquivosCodigoNome, AcervoSolicitacaoItemCompleto acervoSolicitacao)
        {
            return arquivosCodigoNome.PossuiElementos() ? arquivosCodigoNome.Where(w => w.AcervoId == acervoSolicitacao.AcervoId) : Enumerable.Empty<ArquivoCodigoNomeAcervoId>();
        }

        private IEnumerable<CreditoAutorNomeAcervoId> ObterCreditosAutores(IEnumerable<CreditoAutorNomeAcervoId> creditosAutoresNomes, AcervoSolicitacaoItemCompleto acervoSolicitacao)
        {
            return creditosAutoresNomes.PossuiElementos() ? creditosAutoresNomes.Where(w => w.AcervoId == acervoSolicitacao.AcervoId).Select(s=> s) : Enumerable.Empty<CreditoAutorNomeAcervoId>();
        }

        public async Task<IEnumerable<PesquisaAcervo>> ObterPorTextoLivreETipoAcervo(string? textoLivre, TipoAcervo? tipoAcervo, int? anoInicial, int? anoFinal)
        {
            var query = $@";with acervosIds as
                         (
                             select   distinct a.id as acervoId
                           from acervo a
                                left join acervo_credito_autor aca on aca.acervo_id = a.id
                                left join credito_autor ca on aca.credito_autor_id = ca.id
                                left join acervo_bibliografico ab on a.id = ab.acervo_id 
                                left join acervo_bibliografico_assunto aba on aba.acervo_bibliografico_id = ab.id
                                left join assunto ast on ast.id = aba.assunto_id      
                            where not a.excluido
                            {IncluirFiltroPorTipoAcervo(tipoAcervo)}
                            {IncluirFiltroPorTextoLivre(textoLivre)}
                            {IncluirFiltroPorAno(anoInicial, anoFinal)}
                         )
                          select   distinct a.id as acervoId,
                                     coalesce(a.codigo,a.codigo_novo)  codigo,              
                                     a.tipo, 
                                     a.titulo,              
                                     ca.nome as creditoAutoria,
                                     ast.nome as assunto,
                                     a.descricao,
                                     a.data_acervo dataAcervo,
                                     a.ano,
                                     coalesce(ab.situacao_saldo,0) as situacaoSaldo
                            from acervo a
                                join acervosIds aid on aid.acervoId = a.id
                                left join acervo_credito_autor aca on aca.acervo_id = a.id
                                left join credito_autor ca on aca.credito_autor_id = ca.id
                                left join acervo_bibliografico ab on a.id = ab.acervo_id 
                                left join acervo_bibliografico_assunto aba on aba.acervo_bibliografico_id = ab.id
                                left join assunto ast on ast.id = aba.assunto_id
                         ";
            
	        var retorno  = await conexao.Obter().QueryAsync<PesquisaAcervo>(query, 
                new
                {
                    tipoAcervo = tipoAcervo.HasValue ? (int)tipoAcervo : (int?)null, 
                    textoLivre = textoLivre.NaoEhNulo() ? textoLivre.ToLower() : null,
                    anoInicial,
                    anoFinal
                });
            
            return retorno;
        }
        
        private string IncluirFiltroPorAno(int? anoInicial, int? anoFinal)
        {
            if (anoInicial.HasValue && anoFinal.HasValue)
                return " and (a.ano_inicio between @anoInicial and @anoFinal or a.ano_fim between @anoInicial and @anoFinal) ";
            
            if (anoInicial.HasValue)
                return " and (@anoInicial between a.ano_inicio and a.ano_fim) ";
                
            return anoFinal.HasValue ? " and (@anoFinal between a.ano_inicio and a.ano_fim) " : string.Empty;
        }
        
        private string IncluirFiltroPorTextoLivre(string? textoLivre)
        {
            if (textoLivre.EstaPreenchido())
                return " and ( f_unaccent(lower(a.titulo)) LIKE ('%' || f_unaccent(@textoLivre) || '%') Or f_unaccent(lower(ca.nome)) LIKE ('%' || f_unaccent(@textoLivre) || '%')  Or f_unaccent(lower(ast.nome)) LIKE ('%' || f_unaccent(@textoLivre) || '%'))";

            return string.Empty;
        }

        private string IncluirFiltroPorTipoAcervo(TipoAcervo? tipoAcervo)
        {
            return tipoAcervo.NaoEhNulo() ? "and a.tipo = @tipoAcervo " : string.Empty;
        }

        public Task<Acervo> PesquisarAcervoPorCodigoTombo(string codigoTombo, long[] tiposAcervosPermitidos)
        {
            var query = @"
            select id, 
                   titulo,
                   tipo,
                   coalesce(codigo, codigo_novo) as codigo
            from acervo
            where (lower(codigo) = @codigo or lower(codigo_novo) = @codigo)
              and tipo = ANY(@tiposAcervosPermitidos)
              and not excluido ";
            
            return conexao.Obter().QueryFirstOrDefaultAsync<Acervo>(query,new { codigo = codigoTombo.ToLower(), tiposAcervosPermitidos });
        }
    }
}