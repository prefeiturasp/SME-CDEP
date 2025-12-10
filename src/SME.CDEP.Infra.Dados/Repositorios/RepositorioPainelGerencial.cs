using Dapper;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
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

    public async Task<List<PainelGerencialQuantidadeDeSolicitacoesPorTipoAcervo>> ObterQuantidadeDeSolicitacoesPorTipoAcervoAsync()
    {
        var sql = @"
        SELECT acervo.TIPO tipoAcervo, COUNT(1) quantidade
        FROM   acervo_solicitacao_item
               INNER JOIN acervo ON ACERVO_ID = acervo.ID 
        WHERE  NOT acervo_solicitacao_item.EXCLUIDO 
        GROUP  BY acervo.TIPO;";

        var resultado = await conexao.Obter().QueryAsync<PainelGerencialQuantidadeDeSolicitacoesPorTipoAcervo>(sql);
        return [.. resultado];
    }

    public async Task<List<PainelGerencialQuantidadeAcervoEmprestadoPorSituacao>> ObterQuantidadeAcervoEmprestadoPorSituacaoAsync()
    {
        var sql = @"
        SELECT SITUACAO, COUNT (1) quantidade
        FROM acervo_emprestimo
        WHERE SITUACAO <> @situacao
        AND NOT excluido
        GROUP BY situacao;";

        var resultado = await conexao.Obter().QueryAsync<PainelGerencialQuantidadeAcervoEmprestadoPorSituacao>(sql, new { situacao = SituacaoEmprestimo.DEVOLVIDO });
        return [.. resultado];
    }

    public async Task<List<PainelGerencialQuantidadeSolicitacaoPorSituacao>> ObterQuantidadeSolicitacaoPorSituacaoAsync()
    {
        var sql = @"
        SELECT SITUACAO, COUNT (1) quantidade
        FROM acervo_solicitacao_item
        WHERE NOT excluido
        GROUP BY situacao;";

        var resultado = await conexao.Obter().QueryAsync<PainelGerencialQuantidadeSolicitacaoPorSituacao>(sql);
        return [.. resultado];
    }
}
