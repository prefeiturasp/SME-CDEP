﻿using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoArteGrafica : RepositorioBase<AcervoArteGrafica>, IRepositorioAcervoArteGrafica
    {
        public RepositorioAcervoArteGrafica(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<IEnumerable<AcervoArteGrafica>> ObterTodos()
        {
            var query = @"select  ag.id,
                                  ag.localizacao,
                                  ag.procedencia,
                                  ag.copia_digital,
                                  ag.permite_uso_imagem,
                                  ag.conservacao_id,
                                  ag.cromia_id as cromiaId,
                                  ag.largura,
                                  ag.altura,
                                  ag.diametro,
                                  ag.tecnica,
                                  ag.suporte_id as suporteId,
                                  ag.quantidade,
                                  a.descricao,
                                  a.id,
                                  a.titulo,
                                  a.codigo,
                                  a.tipo,
                                  ca.id,
                                  ca.nome
                        from acervo_arte_grafica ag
                        join acervo a on a.id = ag.acervo_id 
                        left join acervo_credito_autor aca on aca.acervo_id = a.id
                        left join credito_autor ca on aca.credito_autor_id = ca.id
                        where not a.excluido ";

            var retorno = await conexao.Obter().QueryAsync<AcervoArteGrafica, Acervo, CreditoAutor,  AcervoArteGrafica>(
                query, (acervoArteGrafica, acervo, creditoAutor) =>
                {
                    acervo.CreditoAutor = creditoAutor;
                    acervoArteGrafica.Acervo = acervo;
                    return acervoArteGrafica;
                });
            
            return retorno;
        }
        
        public async Task<AcervoArteGraficaCompleto> ObterPorId(long id)
        {
            var query = QueryCompletaAcervoArteGrafica();

            query += " and a.id = @id ";
            
            var retorno = (await conexao.Obter().QueryAsync<AcervoArteGraficaCompleto>(query, new { id }));
            if (retorno.Any())
            {
                var acervoArteGrafica = retorno.FirstOrDefault();
                acervoArteGrafica.Arquivos = retorno.Where(w=> w.ArquivoId > 0).Select(s => new ArquivoResumido() { Id = s.ArquivoId, Codigo = s.ArquivoCodigo, Nome = s.ArquivoNome }).DistinctBy(d=> d.Id).ToArray();
                acervoArteGrafica.CreditosAutoresIds = acervoArteGrafica.CreditoAutorId > 0 ? retorno.Select(s => s.CreditoAutorId).Distinct().ToArray() : Array.Empty<long>();
                return acervoArteGrafica;    
            }

            return default;
        }

        private static string QueryCompletaAcervoArteGrafica()
        {
            var query = @"select  ag.id,
                                  ag.localizacao,
                                  ag.procedencia,
                                  ag.copia_digital as CopiaDigital,
                                  ag.permite_uso_imagem as PermiteUsoImagem,
                                  ag.conservacao_id as ConservacaoId,
                                  co.nome as conservacaoNome,
                                  ag.cromia_id as cromiaId,
                                  c.nome as cromiaNome,
                                  ag.largura,
                                  ag.altura,
                                  ag.diametro,
                                  ag.tecnica,
                                  ag.suporte_id as suporteId,
                                  su.nome as suporteNome,
                                  ag.quantidade,
                                  a.descricao,
                                  a.id as AcervoId,
                                  a.titulo,
                                  a.codigo,
                                  a.ano,
                                  a.data_acervo dataacervo,
                                  a.tipo as TipoAcervoId,
                                  a.criado_em as CriadoEm,
                                  a.criado_por as CriadoPor,
                                  a.criado_login as CriadoLogin,
                                  a.alterado_em as AlteradoEm,
                                  a.alterado_por as AlteradoPor,
                                  a.alterado_login as AlteradoLogin,
                                  ca.id as CreditoAutorId,
                                  ca.nome as CreditoAutorNome,
                                  arq.id as arquivoId,
                                  arq.nome as ArquivoNome,
                                  arq.codigo as ArquivoCodigo,
                                  arqMin.nome as ArquivoNomeMiniatura,
                                  arqMin.codigo as ArquivoCodigoMiniatura
                        from acervo_arte_grafica ag
                        join acervo a on a.id = ag.acervo_id 
                        left join acervo_credito_autor aca on aca.acervo_id = a.id
                        left join credito_autor ca on aca.credito_autor_id = ca.id
                        left join acervo_arte_grafica_arquivo aga on aga.acervo_arte_grafica_id = ag.id
                        left join arquivo arq on arq.id = aga.arquivo_id
                        left join arquivo arqMin on arqMin.id = aga.arquivo_miniatura_id
                        join cromia c on c.id = ag.cromia_id
                        join conservacao co on co.id = ag.conservacao_id
                        join suporte su on su.id = ag.suporte_id
                        where not a.excluido  ";
            return query;
        }

        public async Task<AcervoArteGraficaCompleto> ObterDetalhamentoPorCodigo(string filtroCodigo)
        {
            var query = QueryCompletaAcervoArteGrafica();

            query += " and a.codigo = @filtroCodigo";

            var  acervosCompletos = await conexao.Obter().QueryAsync<AcervoArteGraficaCompleto>(query, new { filtroCodigo });
            
            if (acervosCompletos.NaoPossuiElementos())
                return default;
            
            var acervoDetalhado = acervosCompletos.FirstOrDefault();
            
            acervoDetalhado.Imagens = acervosCompletos.Any(s => s.ArquivoNome.EstaPreenchido())
                ? acervosCompletos.Select(s => new ImagemDetalhe()
                {
                    Original = s.ArquivoNome,
                    Thumbnail = s.ArquivoNomeMiniatura
                }).Distinct()
                : default;
            
            acervoDetalhado.CreditosAutores = acervosCompletos.Any(a=> a.CreditoAutorNome.EstaPreenchido())
                ? string.Join(" | ", acervosCompletos.Select(s => s.CreditoAutorNome).Distinct())
                : string.Empty;
            
            return acervoDetalhado;
        }
    }
}