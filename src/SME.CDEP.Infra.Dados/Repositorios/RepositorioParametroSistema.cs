using Dapper;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioParametroSistema(IContextoAplicacao contexto, ICdepConexao conexao) :
        RepositorioBaseAuditavel<ParametroSistema>(contexto, conexao), IRepositorioParametroSistema
    {
        public async Task<ParametroSistema> ObterParametroPorTipoEAno(TipoParametroSistema tipoParametroSistema, int ano = 0)
        {
            const string query = """
                SELECT *
                FROM  parametro_sistema
                WHERE tipo = @Tipo
                  AND ativo = true
                ORDER BY
                    CASE WHEN ano = @Ano THEN 0 ELSE 1 END ASC, -- Prioridade 0: Ano solicitado
                    ano DESC                                    -- Prioridade 1: Maior ano (fallback)
                LIMIT 1
                """;

            var retorno = await conexao.Obter().QueryFirstOrDefaultAsync<ParametroSistema>(query, new { tipoParametroSistema, ano });

            if (retorno is null)
                throw new NegocioException(string.Format(MensagemNegocio.PARAMETRO_NAO_ENCONTRADO_TIPO_X, tipoParametroSistema));

            return retorno;
        }
    }
}
