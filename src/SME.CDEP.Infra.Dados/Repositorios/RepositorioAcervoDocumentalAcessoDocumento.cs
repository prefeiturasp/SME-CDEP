using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoDocumentalAcessoDocumento : RepositorioBaseSomenteId<AcervoDocumentalAcessoDocumento>, IRepositorioAcervoDocumentalAcessoDocumento
    {
        public RepositorioAcervoDocumentalAcessoDocumento(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcervoDocumentalAcessoDocumento>> ObterPorAcervoDocumentalId(long id)
        => await conexao.Obter()
                .QueryAsync<AcervoDocumentalAcessoDocumento>(@"select id, 
                                                                  acervo_documental_id, 
                                                                  acesso_documento_id
                                                            from acervo_documental_acesso_documento 
                                                            where acervo_documental_id = @id", new { id });

        public async Task Excluir(long[] acessoDocumentosIdsExcluir, long acervoDocumentalId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_documental_acesso_documento where acervo_documental_id = @acervoDocumentalId and acesso_documento_id = any(@arquivosIdsExcluir)", 
                new { acervoDocumentalId, arquivosIdsExcluir = acessoDocumentosIdsExcluir });
        }
    }
}