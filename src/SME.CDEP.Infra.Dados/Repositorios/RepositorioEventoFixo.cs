using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioEventoFixo : RepositorioBaseAuditavel<EventoFixo>, IRepositorioEventoFixo
    {
        public RepositorioEventoFixo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
    }
}