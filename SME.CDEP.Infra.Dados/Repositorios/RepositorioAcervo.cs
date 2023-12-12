﻿using Dapper;
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
                                 a,ano,
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
        
        public async Task<IEnumerable<PesquisaAcervo>> ObterPorTextoLivreETipoAcervo(string? textoLivre, TipoAcervo? tipoAcervo, int? anoInicial, int? anoFinal)
        {
            var query = $@";with acervosIds as
                         (
                             select   distinct a.id as acervoId,
                                     coalesce(a.codigo,a.codigo_novo)  codigo,              
                                     a.tipo, 
                                     a.titulo,              
                                     ca.nome as creditoAutoria,
                                     ast.nome as assunto,
                                     a.descricao
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
                                     a.data_acervo dataAcervo
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
                    tipoAcervo, 
                    textoLivre = textoLivre.NaoEhNulo() ? textoLivre.ToLower() : string.Empty,
                    anoInicial,
                    anoFinal
                });
            
            return retorno;
        }
        
        private string IncluirFiltroPorAno(int? anoInicial, int? anoFinal)
        {
            if (anoInicial.HasValue && anoFinal.HasValue)
                return "and a.ano between @anoInicial and @anoFinal ";
            
            if (anoInicial.HasValue)
                return "and a.ano >= @anoInicial ";
                
            return anoInicial.HasValue ? "and a.ano <= @anoFinal " : string.Empty;
        }
        
        private string IncluirFiltroPorTextoLivre(string? textoLivre)
        {
            if (textoLivre.EstaPreenchido())
                return " and ( f_unaccent(lower(a.titulo)) LIKE ('%' || f_unaccent(@textoLivre) || '%') Or f_unaccent(lower(ca.nome)) LIKE ('%' || f_unaccent(@textoLivre) || '%')  Or f_unaccent(lower(ast.nome)) LIKE ('%' || f_unaccent(@textoLivre) || '%'))";

            return string.Empty;
        }

        private string IncluirFiltroPorTipoAcervo(TipoAcervo? tipoAcervo)
        {
            if (tipoAcervo.NaoEhNulo())
                return "and a.tipo = @tipoAcervo ";

            return string.Empty;
        }
    }
}