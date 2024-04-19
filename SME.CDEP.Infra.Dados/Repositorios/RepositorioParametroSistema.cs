using Dapper;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioParametroSistema : RepositorioBaseAuditavel<ParametroSistema>, IRepositorioParametroSistema
    {
        public RepositorioParametroSistema(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<ParametroSistema> ObterParametroPorTipoEAno(TipoParametroSistema tipo, int ano = 0)
        {
            var query = @"select *
                            from parametro_sistema ps
                           where ano = @ano
                             and tipo = @tipo
                             and ativo";

            var retorno = await conexao.Obter().QueryFirstOrDefaultAsync<ParametroSistema>(query, new { tipo, ano });
            
            if (retorno.EhNulo())
                throw new NegocioException(string.Format(MensagemNegocio.PARAMETRO_NAO_ENCONTRADO_TIPO_X,tipo));

            return retorno;
        }
    }
}
