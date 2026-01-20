using Dapper;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioParametroSistema(IContextoAplicacao contexto, ICdepConexao conexao) : RepositorioBaseAuditavel<ParametroSistema>(contexto,conexao), IRepositorioParametroSistema
    {

        public async Task<ParametroSistema> ObterParametroPorTipoEAno(TipoParametroSistema tipo, int ano = 0)
        {
            var query = @"select *
                            from parametro_sistema ps
                           where ano = @ano
                             and tipo = @tipo
                             and ativo";

            var retorno = await conexao.Obter().QueryFirstOrDefaultAsync<ParametroSistema>(query, new { tipo, ano });

            return retorno ?? 
                   throw new NegocioException(string.Format(MensagemNegocio.PARAMETRO_NAO_ENCONTRADO_TIPO_X,tipo));
        }
        public async Task<ParametroSistema?> ObterParametroPorTipoAsync(TipoParametroSistema tipoParametroSistema)
        {
            var query = @"select *
                            from parametro_sistema ps
                           where tipo = @tipo
                             and ativo
                             and not excluido";

            var retorno = await conexao.Obter().QueryFirstOrDefaultAsync<ParametroSistema>(query, new { tipo = tipoParametroSistema });

            return retorno;
        }
    }
}
