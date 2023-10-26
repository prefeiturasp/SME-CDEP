using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoBibliograficoAssunto : RepositorioBaseSomenteId<AcervoBibliograficoAssunto>, IRepositorioAcervoBibliograficoAssunto
    {
        public RepositorioAcervoBibliograficoAssunto(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcervoBibliograficoAssunto>> ObterPorAcervoBibliograficoId(long id)
        => await conexao.Obter()
                .QueryAsync<AcervoBibliograficoAssunto>(@"select id, 
                                                                  acervo_bibliografico_id, 
                                                                  assunto_id
                                                            from acervo_bibliografico_assunto 
                                                            where acervo_bibliografico_id = @id", new { id });

        public async Task Excluir(long[] acessoBibliograficoAssuntoIdsExcluir, long acervoBibliograficoId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_bibliografico_assunto where acervo_bibliografico_id = @acervoBibliograficoId and assunto_id = any(@acessoBibliograficoAssuntoIdsExcluir)", 
                new { acervoBibliograficoId, acessoBibliograficoAssuntoIdsExcluir });
        }
    }
}