using Dapper;
using Dommel;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioArquivo : RepositorioBaseAuditavel<Arquivo>, IRepositorioArquivo
    {
        public RepositorioArquivo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<Arquivo> ObterPorCodigo(Guid codigo)
        {
            const string query = @"select * 
                                    from arquivo
                                    where codigo = @codigo";

            return await conexao.Obter().QueryFirstOrDefaultAsync<Arquivo>(query, new { codigo });
        }

        public async Task<IEnumerable<Arquivo>> ObterPorCodigos(Guid[] codigos)
        {
            const string query = @"select * 
                                    from arquivo
                                    where codigo = ANY(@codigos)";

            return await conexao.Obter().QueryAsync<Arquivo>(query, new { codigos });
        }

        public async Task<IEnumerable<Arquivo>> ObterPorIds(long[] ids)
        {
            const string query = @"select * 
                                    from arquivo
                                    where id = ANY(@ids)";

            return await conexao.Obter().QueryAsync<Arquivo>(query, new { ids });
        }

        public async Task<bool> ExcluirArquivoPorCodigo(Guid codigoArquivo)
        {
            var query = "delete from Arquivo where codigo = @codigoArquivo";

            return await conexao.Obter().ExecuteScalarAsync<bool>(query, new { codigoArquivo });
        }

        public async Task<bool> ExcluirArquivoPorId(long id)
        {
            const string query = "delete from Arquivo where id = @id";
            return await conexao.Obter().ExecuteScalarAsync<bool>(query, new { id });
        }
        
        public async Task<bool> ExcluirArquivosPorIds(long[] ids)
        {
            const string query = "delete from Arquivo where id = ANY(@ids)";
            return await conexao.Obter().ExecuteScalarAsync<bool>(query, new { ids });
        }

        public async Task<long> SalvarAsync(Arquivo arquivo)
        {
            if (arquivo.Id > 0)
                await conexao.Obter().UpdateAsync(arquivo);
            else
                arquivo.Id = (long)await conexao.Obter().InsertAsync(arquivo);

            return arquivo.Id;
        }

        public async Task<IEnumerable<AcervoCodigoNomeResumido>> ObterAcervoCodigoNomeArquivoPorAcervoId(long[] acervosIds)
        {
            var query = @"select ag.acervo_id as acervoId, a.codigo, a.nome 
                            from acervo_arte_grafica ag 
                                join acervo_arte_grafica_arquivo aga on aga.acervo_arte_grafica_id = ag.id 
                                join arquivo a on a.id = aga.arquivo_id 
                            where permite_uso_imagem and ag.acervo_id = any(@acervosIds)
                            union all
                            select af.acervo_id as acervoId, a.codigo, a.nome 
                                from acervo_fotografico af 
                                    join acervo_fotografico_arquivo afa on afa.acervo_fotografico_id = af.id 
                                    join arquivo a on a.id = afa.arquivo_id 
                            where permite_uso_imagem and af.acervo_id = any(@acervosIds) ";

            return await conexao.Obter().QueryAsync<AcervoCodigoNomeResumido>(query, new { acervosIds });
        }

        public async Task<Arquivo> ObterArquivoPorNomeTipoArquivo(string nome, TipoArquivo tipoArquivo)
        {
            var query = @"select id,
                                  nome,
                                  codigo,
                                  tipo,
                                  tipo_conteudo tipoConteudo,
                                  criado_em criadoEm,
                                  criado_por criadoPor,
                                  criado_login criadoLogin,
                                  alterado_em alteradoEm,
                                  alterado_por alteradoPor,
                                  alterado_login alteradoLogin
                        from arquivo
                        where not excluido 
                        and tipo = @tipoArquivo
                        and nome = @nome ";

            return await conexao.Obter().QueryFirstAsync<Arquivo>(query, new { tipoArquivo, nome });
        }

        public async Task<long> ObterIdPorCodigo(Guid arquivoCodigo)
        {
            var query = @"select id
                            from arquivo 
                           where codigo = @arquivoCodigo";

            return await conexao.Obter().QueryFirstOrDefaultAsync<long>(query, new { arquivoCodigo });
        }
    }
}