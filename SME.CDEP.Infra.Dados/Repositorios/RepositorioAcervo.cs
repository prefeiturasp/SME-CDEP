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
                                 case when length(a.codigo_novo) > 0 then 
     	                                case when length(a.codigo) > 0 then concat(a.codigo,'/',a.codigo_novo) 
     	                                else a.codigo_novo 
     	                                end
                                     else a.codigo end codigo,
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
        
        public Task<bool> ExisteCodigo(string codigo, long id)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>("select 1 from acervo where (lower(codigo) = @codigo or lower(codigo_novo) = @codigo) and not excluido and id != @id",new { id, codigo = codigo.ToLower() });
        }
        
        public Task<bool> ExisteTitulo(string titulo, long id, string codigo, string codigoNovo)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>($"select 1 from acervo where lower(titulo) = @titulo and not excluido and id != @id and codigo = @codigo {IncluirCodigoNovo(codigoNovo)}",new { id, titulo = titulo.ToLower(), codigo, codigoNovo });
        }

        public Task<IEnumerable<Acervo>> ObterPorTextoLivreETipoAcervo(string? textoLivre, TipoAcervo? tipoAcervo)
        {
            var query = @"
/*CREATE OR REPLACE FUNCTION public.f_unaccent(text)
 RETURNS text
 LANGUAGE sql
 IMMUTABLE
AS $function$
SELECT public.unaccent('public.unaccent', $1)  -- schema-qualify function and dictionary
$function$
;*/

;with pesquisa as 
(
select    coalesce(a.codigo,a.codigo_novo)  codigo,              
			 a.tipo, 
             a.titulo,              
             ca.nome,
             ass.nome 
from acervo a
    left join acervo_credito_autor aca on aca.acervo_id = a.id
    left join credito_autor ca on aca.credito_autor_id = ca.id
    left join acervo_bibliografico ab on a.id = ab.acervo_id 
    left join acervo_bibliografico_assunto aba on aba.acervo_bibliografico_id = ab.id
    left join assunto ass on ass.id = aba.assunto_id      
where not a.excluido
and ( lower(a.titulo) like '%es%' Or lower(ca.nome) like '%es%'  Or lower(ass.nome) like '%es%')
)
select * from pesquisa




    left join acervo_fotografico af on a.id = af.acervo_id 
    left join acervo_arte_grafica aag on a.id = aag.acervo_id 
    left join acervo_audiovisual aav on a.id = aav.acervo_id 
    left join acervo_audiovisual ad on a.id = ad.acervo_id 
    left join acervo_tridimensional at on a.id = at.acervo_id
    
    
select * from acervo_fotografico --descricao e data_acervo
select * from acervo_arte_grafica aag --descricao e data_acervo
select * from acervo_audiovisual aa --descricao e data_acervo
select * from acervo_documental ad --descricao
select * from acervo_tridimensional at2 --descricao e data_acervo
select * from acervo_bibliografico --subtitulo
select * from acervo

--> mover descrição e data_acervo para acervo
--> mantém subtitulo em campo separado ";

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

        private string IncluirCodigoNovo(string codigoNovo)
        {
            return codigoNovo.EstaPreenchido() ? " and codigo_novo = @codigoNovo " : string.Empty;
        }
    }
}