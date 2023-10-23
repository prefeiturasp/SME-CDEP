using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

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

        private string IncluirCodigoNovo(string codigoNovo)
        {
            return codigoNovo.EstaPreenchido() ? " and codigo_novo = @codigoNovo " : string.Empty;
        }
    }
}