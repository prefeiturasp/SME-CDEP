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
								  ia.tipo_acervo as TipoAcervo,
								  ia.status as StatusArquivo,
								  ia.conteudo,
								  ia.criado_em criadoEm
						from importacao_arquivo ia
						where not ia.excluido 
						and ia.tipo_acervo = @tipoAcervo
						and ia.status = @status
						order by ia.id desc";

            return await conexao.Obter().QueryFirstOrDefaultAsync<ImportacaoArquivo>(query, new { tipoAcervo, status = (int)ImportacaoStatus.Erros});
        }

        public async Task<long> Salvar(ImportacaoArquivo importacaoArquivo)
        {
	        if (importacaoArquivo.Id.EhMaiorQueZero())
		        return (await Atualizar(importacaoArquivo)).Id;
	        
	        return await Inserir(importacaoArquivo);
        }
    }
}