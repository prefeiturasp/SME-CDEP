using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioImportacaoArquivo : RepositorioBaseAuditavel<ImportacaoArquivo>, IRepositorioImportacaoArquivo
    {
        public RepositorioImportacaoArquivo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<ImportacaoArquivoCompleto> ObterUltimaImportacao()
        {
            var query = @"select ia.id,
								  ia.nome,
								  ia.tipo_acervo as TipoAcervo,
								  ia.status as StatusArquivo,
								  ia.conteudo
						from importacao_arquivo ia
						where not ia.excluido 
						order by ia.id desc";

            return await conexao.Obter().QueryFirstAsync<ImportacaoArquivoCompleto>(query);
        }
    }
}