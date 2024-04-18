using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioImportacaoArquivo : RepositorioBaseAuditavel<ImportacaoArquivo>, IRepositorioImportacaoArquivo
    {
        public RepositorioImportacaoArquivo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<ImportacaoArquivo> ObterUltimaImportacao(TipoAcervo tipoAcervo)
        {
            var query = @"select ia.id,
								  ia.nome,
								  ia.tipo_acervo,
								  ia.status,
								  ia.conteudo,
								  ia.criado_em
						from importacao_arquivo ia
						where not ia.excluido 
						and ia.tipo_acervo = @tipoAcervo
						order by ia.id desc";

            var importacao = await conexao.Obter().QueryFirstOrDefaultAsync<ImportacaoArquivo>(query, new { tipoAcervo});

            if (importacao.EhNulo())
	            return default;

            if (importacao.Status == ImportacaoStatus.Sucesso)
	            return default;

            return importacao;
        }

        public async Task<long> Salvar(ImportacaoArquivo importacaoArquivo)
        {
	        if (importacaoArquivo.Id.EhMaiorQueZero())
		        return (await Atualizar(importacaoArquivo)).Id;
	        
	        return await Inserir(importacaoArquivo);
        }

        public async Task<ImportacaoArquivo> ObterImportacaoPorId(long id)
        {
	        var query = @"select ia.id,
								  ia.nome,
								  ia.tipo_acervo as TipoAcervo,
								  ia.status as StatusArquivo,
								  ia.conteudo,
								  ia.criado_em criadoEm
						from importacao_arquivo ia
						where not ia.excluido						
						and ia.id = @id ";

	        var importacao = await conexao.Obter().QueryFirstOrDefaultAsync<ImportacaoArquivo>(query, new { id});

	        return importacao;
        }
    }
}