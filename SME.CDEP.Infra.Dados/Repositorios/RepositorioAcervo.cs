using Dapper;
using Dommel;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Dtos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioAcervo : RepositorioBaseAuditavel<Acervo>, IRepositorioAcervo
    {
        public RepositorioAcervo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto, conexao)
        { }


        public async Task<int> ContarPorFiltro(AcervoFiltroDto filtro)
        {
            var construtorDeConsultas = new SqlBuilder();

            var modelo = construtorDeConsultas.AddTemplate(@"
                SELECT COUNT(distinct a.id)
                FROM acervo a
                LEFT JOIN acervo_credito_autor aca on aca.acervo_id = a.id
                LEFT JOIN acervo_bibliografico ab on a.id = ab.acervo_id 
                /**where**/");

            ConstruirClausulasWhereParaPesquisa(filtro, construtorDeConsultas);

            return await conexao.Obter().ExecuteScalarAsync<int>(modelo.RawSql, modelo.Parameters);
        }

        public async Task<IEnumerable<Acervo>> PesquisarPorFiltroPaginado(AcervoFiltroDto filtro, PaginacaoDto paginacao)
        {
            var construtorDeConsultas = new SqlBuilder();
            var deslocamento = (paginacao.Pagina - 1) * paginacao.QuantidadeRegistros;
            var clausulaOrdenacao = ObterColunaDeOrdenacao(paginacao.OrdenacaoDto, paginacao.DirecaoOrdenacaoDto);

            var modelo = construtorDeConsultas.AddTemplate($@"
                WITH AcervosPaginados AS (
                    SELECT DISTINCT a.id,
                                    a.titulo,
                                    COALESCE(a.alterado_em, a.criado_em) as data_ordenacao,
                                    CASE 
                                        WHEN length(a.codigo_novo) > 0 THEN 
                                            CASE WHEN length(a.codigo) > 0 THEN concat(a.codigo, '/', a.codigo_novo) ELSE a.codigo_novo END
                                        ELSE a.codigo 
                                    END as codigo
                    FROM acervo a
                    LEFT JOIN acervo_credito_autor aca ON aca.acervo_id = a.id
                    LEFT JOIN acervo_bibliografico ab ON ab.acervo_id = a.id
                    /**where**/
                    ORDER BY {clausulaOrdenacao}
                    LIMIT @QuantidadeRegistros OFFSET @Offset
                )
                SELECT
                     a.id, 
                     a.tipo, 
                     a.titulo,
                     a.descricao, 
                     case when length(a.codigo_novo) > 0 then 
                            case when length(a.codigo) > 0 then concat(a.codigo,'/',a.codigo_novo) 
                            else a.codigo_novo 
                            end
                         else a.codigo end codigo,
                     a.data_acervo,
                     a.ano,
                     a.criado_em, 
                     a.criado_por, 
                     a.criado_login, 
                     a.alterado_em, 
                     a.alterado_por, 
                     a.alterado_login,
                     -- Campos do CreditoAutor
                     ca.id, 
                     ca.nome, 
                     ca.tipo,
                     -- Campo da Editora
                     e.nome AS editora,
                     -- Campo da CapaDocumento
                     ad.capa_documento as capaDocumento
                FROM acervo a
                JOIN AcervosPaginados ap ON a.id = ap.id
                LEFT JOIN acervo_credito_autor aca ON aca.acervo_id = a.id
                LEFT JOIN credito_autor ca ON aca.credito_autor_id = ca.id
				LEFT JOIN acervo_documental ad on ad.acervo_id = a.id
                LEFT JOIN acervo_bibliografico ab ON ab.acervo_id = a.id
                LEFT JOIN editora e ON ab.editora_id = e.id
                ORDER BY {clausulaOrdenacao};");

            construtorDeConsultas.AddParameters(new { paginacao.QuantidadeRegistros, Offset = deslocamento });
            ConstruirClausulasWhereParaPesquisa(filtro, construtorDeConsultas);

            var indiceDeAcervos = new Dictionary<long, Acervo>();
            await conexao.Obter().QueryAsync<Acervo, CreditoAutor, string, string, Acervo>(
                modelo.RawSql,
                (acervo, creditoAutor, editora, capaDocumento) =>
                {
                    if (!indiceDeAcervos.TryGetValue(acervo.Id, out var acervoEntrada))
                    {
                        acervoEntrada = acervo;
                        acervoEntrada.CreditosAutores = new List<CreditoAutor>();
                        acervoEntrada.CapaDocumento = capaDocumento;
                        acervoEntrada.Editora = editora;
                        indiceDeAcervos.Add(acervoEntrada.Id, acervoEntrada);
                    }
                    if (creditoAutor != null)
                    {
                        acervoEntrada.CreditosAutores.Add(creditoAutor);
                    }
                    return acervoEntrada;
                },
                modelo.Parameters,
                splitOn: "id,editora,capaDocumento" 
            );

            return indiceDeAcervos.Values;
        }

        private static void ConstruirClausulasWhereParaPesquisa(AcervoFiltroDto filtro, SqlBuilder builder)
        {
            builder.Where("NOT a.excluido");

            if (!string.IsNullOrWhiteSpace(filtro.Titulo))
                builder.Where("lower(a.titulo) LIKE lower(@Titulo)", new { Titulo = $"%{filtro.Titulo}%" });

            if (!string.IsNullOrWhiteSpace(filtro.Codigo))
                builder.Where("(lower(a.codigo) = lower(@Codigo) OR lower(a.codigo_novo) = lower(@Codigo))", new { filtro.Codigo });

            if (filtro.TipoAcervo.HasValue && filtro.TipoAcervo > 0)
                builder.Where("a.tipo = @TipoAcervo", new { filtro.TipoAcervo });

            if (filtro.IdEditora.HasValue && filtro.IdEditora > 0)
                builder.Where("ab.editora_id = @IdEditora", new { filtro.IdEditora });

            if (filtro.CreditoAutorId.HasValue && filtro.CreditoAutorId > 0)
                builder.Where("aca.credito_autor_id = @CreditoAutorId", new { filtro.CreditoAutorId });
        }

        private static string ObterColunaDeOrdenacao(TipoOrdenacaoDto ordenacao, DirecaoOrdenacaoDto direcao)
        {
            string direcaoSql = direcao.ToString();

            switch (ordenacao)
            {
                case TipoOrdenacaoDto.DATA:
                    return $"data_ordenacao {direcaoSql}";
                case TipoOrdenacaoDto.TITULO:
                    return $"a.titulo {direcaoSql}";
                case TipoOrdenacaoDto.CODIGO:
                    return $"codigo {direcaoSql}";
                default:
                    return $"data_ordenacao {direcaoSql}";
            }
        }

        public Task<bool> ExisteCodigo(string codigo, long id, TipoAcervo tipo)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>("select 1 from acervo where (lower(codigo) = @codigo or lower(codigo_novo) = @codigo) and not excluido and id != @id and tipo = @tipo", new { id, codigo = codigo.ToLower(), tipo });
        }

        public async Task<IEnumerable<ArquivoCodigoNomeAcervoId>> ObterArquivosPorAcervoId(long[] acervosIds)
        {
            var query = @"
             select a.nome, a.codigo, aag.acervo_id as acervoId 
             from acervo_arte_grafica_arquivo aaga 
             join acervo_arte_grafica aag on aag.id = aaga.acervo_arte_grafica_id 
             join arquivo a on a.id = aaga.arquivo_id 
             where aag.acervo_id = any(@acervosIds)
             and not a.excluido 
             and aag.permite_uso_imagem
             
             union all
             
             select a.nome, a.codigo, ad.acervo_id as acervoId
             from acervo_documental_arquivo ada
             join acervo_documental ad on ad.id = ada.acervo_documental_id  
             join arquivo a on a.id = ada.arquivo_id 
             where ad.acervo_id = any(@acervosIds)
             and not a.excluido
             
             union all
             
             select a.nome, a.codigo, af.acervo_id as acervoId 
             from acervo_fotografico_arquivo afa
             join acervo_fotografico af on af.id = afa.acervo_fotografico_id  
             join arquivo a on a.id = afa.arquivo_id 
             where af.acervo_id = any(@acervosIds)
             and af.permite_uso_imagem 
             and not a.excluido
             
             union all
             
             select a.nome, a.codigo, at.acervo_id as acervoId
             from acervo_tridimensional_arquivo ata 
             join acervo_tridimensional at  on at.id = ata.acervo_tridimensional_id  
             join arquivo a on a.id = ata.arquivo_id 
             where at.acervo_id = any(@acervosIds)
             and not a.excluido; ";

            return await conexao.Obter().QueryAsync<ArquivoCodigoNomeAcervoId>(query, new { acervosIds });
        }

        public async Task<IEnumerable<Acervo>> ObterAcervosPorIds(long[] ids)
        {
            var query = @"
             select 
                id,
                tipo,
                titulo,
                codigo,
                excluido,
                criado_em,
                criado_por,
                alterado_em,
                alterado_por,
                criado_login,
                alterado_login,
                codigo_novo,
                subtitulo,
                descricao,
                ano,
                data_acervo,
                ano_inicio,
                ano_fim
            from acervo
            where id = any(@ids)
                and not excluido; ";

            return await conexao.Obter().QueryAsync<Acervo>(query, new { ids });
        }

        public async Task<IEnumerable<AcervoSolicitacaoItemCompleto>> ObterAcervosSolicitacoesItensCompletoPorId(long acervoSolicitacaoId, long[] tiposAcervosPermitidos)
        {
            var query = @"
            SELECT DISTINCT ON (asi.id)
              asi.id,  
              a.tipo as tipoAcervo,
              a.titulo,
              a.id as acervoId,
              asi.situacao as situacaoItem,
              asi.tipo_atendimento as tipoAtendimento,
              asi.dt_visita as DataVisita,
              aso.situacao,
              ae.situacao as situacaoEmprestimo,
              ab.situacao_saldo as situacaoSaldo,
              asi.acervo_solicitacao_id as acervoSolicitacaoId
            FROM acervo a 
            JOIN acervo_solicitacao_item asi on asi.acervo_id = a.id
            JOIN acervo_solicitacao aso on aso.id = asi.acervo_solicitacao_id
            LEFT JOIN acervo_emprestimo ae on ae.acervo_solicitacao_item_id = asi.id and not ae.excluido 
		    LEFT JOIN acervo_bibliografico ab on ab.acervo_id = a.id 
            WHERE aso.id = @acervoSolicitacaoId 
                and not a.excluido
                and not aso.excluido
                and not asi.excluido
                and a.tipo = ANY(@tiposAcervosPermitidos)
            ORDER BY asi.id, ae.id desc;
               
            select 
              ca.nome,
              aca.acervo_id as AcervoId
            from acervo_credito_autor aca 
              join credito_autor ca on ca.id = aca.credito_autor_id
            where aca.acervo_id in (select acervo_id from acervo_solicitacao_item a join acervo b on a.acervo_id = b.id where acervo_solicitacao_id = @acervoSolicitacaoId and b.tipo = ANY(@tiposAcervosPermitidos))
            and not ca.excluido; ";

            var retorno = await conexao.Obter().QueryMultipleAsync(query, new { acervoSolicitacaoId, tiposAcervosPermitidos });

            if (retorno.EhNulo())
                return default;

            var acervosSolicitacoes = retorno.Read<AcervoSolicitacaoItemCompleto>();
            var creditosAutoresNomes = retorno.Read<CreditoAutorNomeAcervoId>();

            foreach (var acervoSolicitacao in acervosSolicitacoes)
                acervoSolicitacao.AutoresCreditos = ObterCreditosAutores(creditosAutoresNomes, acervoSolicitacao);

            return acervosSolicitacoes;
        }

        private IEnumerable<ArquivoCodigoNomeAcervoId> ObterArquivos(IEnumerable<ArquivoCodigoNomeAcervoId> arquivosCodigoNome, AcervoSolicitacaoItemCompleto acervoSolicitacao)
        {
            return arquivosCodigoNome.PossuiElementos() ? arquivosCodigoNome.Where(w => w.AcervoId == acervoSolicitacao.AcervoId) : Enumerable.Empty<ArquivoCodigoNomeAcervoId>();
        }

        private IEnumerable<CreditoAutorNomeAcervoId> ObterCreditosAutores(IEnumerable<CreditoAutorNomeAcervoId> creditosAutoresNomes, AcervoSolicitacaoItemCompleto acervoSolicitacao)
        {
            return creditosAutoresNomes.PossuiElementos() ? creditosAutoresNomes.Where(w => w.AcervoId == acervoSolicitacao.AcervoId).Select(s => s) : Enumerable.Empty<CreditoAutorNomeAcervoId>();
        }

        public async Task<IEnumerable<PesquisaAcervo>> ObterPorTextoLivreETipoAcervo(string? textoLivre, TipoAcervo? tipoAcervo, int? anoInicial, int? anoFinal)
        {
            var query = $@";with acervosIds as
                         (
                             select   distinct a.id as acervoId
                           from acervo a
                                left join acervo_credito_autor aca on aca.acervo_id = a.id
                                left join credito_autor ca on aca.credito_autor_id = ca.id
                                left join acervo_bibliografico ab on a.id = ab.acervo_id 
                                left join editora e ON ab.editora_id = e.id
                                left join acervo_bibliografico_assunto aba on aba.acervo_bibliografico_id = ab.id
                                left join assunto ast on ast.id = aba.assunto_id      
                            where not a.excluido
                            {IncluirFiltroPorTipoAcervo(tipoAcervo)}
                            {IncluirFiltroPorTextoLivre(textoLivre)}
                            {IncluirFiltroPorAno(anoInicial, anoFinal)}
                            {IncluirFiltroSituacaoAcervo()}
                         )
                          select   distinct a.id as acervoId,
                                     coalesce(a.codigo,a.codigo_novo)  codigo,              
                                     a.tipo, 
                                     a.titulo,              
                                     ca.nome as creditoAutoria,
                                     ast.nome as assunto,
                                     a.descricao,
                                     a.data_acervo dataAcervo,
                                     a.ano,
                                     coalesce(ab.situacao_saldo,0) as situacaoSaldo,
                                     e.nome as editora
                            from acervo a
                                join acervosIds aid on aid.acervoId = a.id
                                left join acervo_credito_autor aca on aca.acervo_id = a.id
                                left join credito_autor ca on aca.credito_autor_id = ca.id
                                left join acervo_bibliografico ab on a.id = ab.acervo_id 
                                left join editora e on ab.editora_id = e.id
                                left join acervo_bibliografico_assunto aba on aba.acervo_bibliografico_id = ab.id
                                left join assunto ast on ast.id = aba.assunto_id
                         ";

            var retorno = await conexao.Obter().QueryAsync<PesquisaAcervo>(query,
                new
                {
                    tipoAcervo = tipoAcervo.HasValue ? (int)tipoAcervo : (int?)null,
                    textoLivre = textoLivre.NaoEhNulo() ? textoLivre.ToLower() : null,
                    anoInicial,
                    anoFinal
                });

            return retorno;
        }

        private string IncluirFiltroPorAno(int? anoInicial, int? anoFinal)
        {
            if (anoInicial.HasValue && anoFinal.HasValue)
                return " and (a.ano_inicio between @anoInicial and @anoFinal or a.ano_fim between @anoInicial and @anoFinal) ";

            if (anoInicial.HasValue)
                return " and (@anoInicial between a.ano_inicio and a.ano_fim) ";

            return anoFinal.HasValue ? " and (@anoFinal between a.ano_inicio and a.ano_fim) " : string.Empty;
        }

        private string IncluirFiltroPorTextoLivre(string? textoLivre)
        {
            if (textoLivre.EstaPreenchido())
                return " and ( f_unaccent(lower(a.titulo)) LIKE ('%' || f_unaccent(@textoLivre) || '%') " +
                    "Or f_unaccent(lower(ca.nome)) LIKE ('%' || f_unaccent(@textoLivre) || '%') " +
                    "Or f_unaccent(lower(ast.nome)) LIKE ('%' || f_unaccent(@textoLivre) || '%') " +
                    "Or f_unaccent(lower(e.nome)) LIKE ('%' || f_unaccent(@textoLivre) || '%') " +
                    "Or f_unaccent(lower(a.descricao)) LIKE ('%' || f_unaccent(@textoLivre) || '%'))";

            return string.Empty;
        }

        private string IncluirFiltroPorTipoAcervo(TipoAcervo? tipoAcervo)
        {
            return tipoAcervo.NaoEhNulo() ? "and a.tipo = @tipoAcervo " : string.Empty;
        }

        private string IncluirFiltroSituacaoAcervo()
        {
            return " and COALESCE(a.situacao, 1) = 1 ";
        }

        public Task<Acervo> PesquisarAcervoPorCodigoTombo(string codigoTombo, long[] tiposAcervosPermitidos)
        {
            var query = @"
            select id, 
                   titulo,
                   tipo,
                   coalesce(codigo, codigo_novo) as codigo
            from acervo
            where (lower(codigo) = @codigo or lower(codigo_novo) = @codigo)
              and tipo = ANY(@tiposAcervosPermitidos)
              and not excluido ";

            return conexao.Obter().QueryFirstOrDefaultAsync<Acervo>(query, new { codigo = codigoTombo.ToLower(), tiposAcervosPermitidos });
        }
    }
}