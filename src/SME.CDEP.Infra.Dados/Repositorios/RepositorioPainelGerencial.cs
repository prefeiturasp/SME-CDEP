using Dapper;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Repositorios;

[ExcludeFromCodeCoverage]
public class RepositorioPainelGerencial(ICdepConexao conexao) : IRepositorioPainelGerencial
{
    public async Task<List<PainelGerencialAcervosCadastrados>> ObterAcervosCadastradosAsync()
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

    public async Task<List<SumarioConsultaMensal>> ObterSumarioConsultasMensalAsync(int ano)
    {
        var sql = @"
            SELECT 
                mes_referencia AS MesReferencia,
                total_consultas AS TotalConsultas
            FROM sumario_consultas_mensal
            WHERE EXTRACT(YEAR FROM mes_referencia) = @ano
            ORDER BY MesReferencia;";
        var resultado = await conexao.Obter().QueryAsync<SumarioConsultaMensal>(sql, new { ano });
        return [.. resultado];
    }

    public async Task<List<PainelGerencialQuantidadeSolicitacaoMensal>> ObterQuantidadeSolicitacoesMensaisAsync(int ano)
    {
        const string sql = @"
            SELECT mes_referencia AS MesReferencia,
                   total_solicitacoes AS totalSolicitacoes
             FROM sumario_solicitacoes_mensal
            WHERE EXTRACT(YEAR FROM mes_referencia) = @ano
            ORDER BY MesReferencia;";
        var resultado = await conexao.Obter().QueryAsync<PainelGerencialQuantidadeSolicitacaoMensal>(sql, new { ano });
        return [.. resultado];
    }
}
