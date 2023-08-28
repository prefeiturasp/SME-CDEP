using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervo : RepositorioBase<Acervo>, IRepositorioAcervo
    {
        public RepositorioAcervo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IList<Acervo>> PesquisarPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo)
        {
            var query = @"select a.id, a.tipo, a.titulo, a.codigo, a.criado_em, a.criado_por, a.criado_login, a.alterado_em, a.alterado_por, a.alterado_login, 
                                 ca.id, ca.nome, ca.tipo 
							from acervo a
						        join credito_autor ca on a.credito_autor_id = ca.id
						    where not a.excluido ";

            if (!string.IsNullOrEmpty(titulo))
                query += $"and lower(a.titulo) like lower('%{titulo}%') ";
	
            if (!string.IsNullOrEmpty(codigo))
                query += "and lower(a.codigo) = lower('@codigo') ";
	
            if (tipoAcervo > 0)
                query += "and a.Tipo = @tipoAcervo ";
	
            if (creditoAutorId > 0)
                query += "and a.credito_autor_id = @creditoAutorId ";
	
            return (await conexao.Obter().QueryAsync<Acervo, CreditoAutor, Acervo>(query, (acervo, creditoAutor) =>
            {
                acervo.CreditoAutor = creditoAutor;
                return acervo;
            }, new { titulo, codigo, tipoAcervo, creditoAutorId }, splitOn: "id")).ToList();
        }
    }
}