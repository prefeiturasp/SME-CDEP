using Dapper;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Repositorios;

[ExcludeFromCodeCoverage]
public class RepositorioPainelGerencial(ICdepConexao conexao) : IRepositorioPainelGerencial
{
    public async Task<List<PainelGerencialAcervosCadastrados>> ObterAcervosCadastrados()
    {
        var sql = @"
            SELECT 
                tipo AS TipoAcervo,
                COUNT(*) AS Quantidade
            FROM acervo
            GROUP BY tipo;
        ";
        var resultado = await conexao.Obter().QueryAsync<PainelGerencialAcervosCadastrados>(sql);
        return [.. resultado];
    }
}
