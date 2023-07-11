using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioFormato : RepositorioBase<Formato>, IRepositorioFormato
    {
        public RepositorioFormato(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
    }
}