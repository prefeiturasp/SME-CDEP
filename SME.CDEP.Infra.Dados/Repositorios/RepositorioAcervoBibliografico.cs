using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoBibliografico : RepositorioBase<AcervoBibliografico>, IRepositorioAcervoBibliografico
    {
        public RepositorioAcervoBibliografico(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<IEnumerable<AcervoBibliografico>> ObterTodos()
        {
            var query = @"select  ab.id,
                                  ab.material_id as materialId,
                                  ab.editora_id as editoraId,
                                  ab.idioma_id as idiomaId,
                                  a.ano,
                                  ab.edicao,
                                  ab.numero_pagina numeroPagina,                                  
                                  ab.largura,
                                  ab.altura,
                                  ab.serie_colecao_id as serieColecaoId,
                                  ab.volume,
                                  ab.localizacao_cdd as LocalizacaoCDD,                                  
                                  ab.localizacao_pha as LocalizacaoPHA,
                                  ab.notas_gerais as notasGerais,
                                  ab.isbn,
                                  a.id,
                                  a.titulo,
                                  a.subtitulo,
                                  a.codigo,
                                  a.tipo,
                                  ca.id,
                                  ca.nome
                        from acervo_bibliografico ab
                        join acervo a on a.id = ab.acervo_id     
                        join acervo_credito_autor aca on aca.acervo_id = a.id
                        join credito_autor ca on aca.credito_autor_id = ca.id
                        where not a.excluido ";

            var retorno = await conexao.Obter().QueryAsync<AcervoBibliografico, Acervo, CreditoAutor,  AcervoBibliografico>(
                query, (acervoBibliografico, acervo, creditoAutor) =>
                {
                    acervo.CreditoAutor = creditoAutor;
                    acervoBibliografico.Acervo = acervo;
                    
                    return acervoBibliografico;
                });
            
            return retorno;
        }
        
        public async Task<AcervoBibliograficoCompleto> ObterPorId(long id)
        {
            var query = @"select  ab.id,
                                  a.ano,
                                  ab.edicao,
                                  ab.numero_pagina numeroPagina,
                                  ab.largura,
                                  ab.altura,                                  
                                  ab.volume,
                                  ab.localizacao_cdd as localizacaoCDD,
                                  ab.localizacao_pha as localizacaoPHA,                                  
                                  ab.notas_gerais as notasGerais,
                                  ab.isbn, 
                                  a.id as AcervoId,
                                  a.titulo,
                                  a.subTitulo,
                                  a.codigo,                                                                    
                                  a.tipo as TipoAcervoId,                                  
                                  a.criado_em as CriadoEm,
                                  a.criado_por as CriadoPor,
                                  a.criado_login as CriadoLogin,
                                  a.alterado_em as AlteradoEm,
                                  a.alterado_por as AlteradoPor,
                                  a.alterado_login as AlteradoLogin,  
                                  ca.id as CreditoAutorId,
                                  ca.nome as CreditoAutorNome,    
                                  aca.tipo_autoria as TipoAutoria,
                                  aca.ehcoautor,
                                  i.id as IdiomaId,
                                  i.nome as IdiomaNome,
                                  m.id as materialId,
                                  m.nome as materialNome,
                                  e.id as editoraId,
                                  e.nome as editoraNome,
                                  aba.assunto_id as assuntoId,
                                  sc.id as serieColecaoId,
                                  sc.nome as serieColecaoNome
                        from acervo_bibliografico ab
                        join acervo a on a.id = ab.acervo_id 
                        join idioma i on i.id = ab.idioma_id 
                        join acervo_bibliografico_assunto aba on aba.acervo_bibliografico_id = ab.id                              
                        join acervo_credito_autor aca on aca.acervo_id = a.id
                        join credito_autor ca on aca.credito_autor_id = ca.id                        
                        join material m on m.id = ab.material_id
                        left join editora e on e.id = ab.editora_id
                        left join serie_colecao sc on sc.id = ab.serie_colecao_id
                        where not a.excluido 
                        and a.id = @id";

            var retorno = (await conexao.Obter().QueryAsync<AcervoBibliograficoCompleto>(query, new { id }));
            if (retorno.Any())
            {
                var acervoBi = retorno.FirstOrDefault();
                acervoBi.CreditosAutoresIds = acervoBi.CreditoAutorId.EhMaiorQueZero() ? retorno.Where(w=> !w.ehcoautor).Select(s => s.CreditoAutorId).Distinct().ToArray() : Array.Empty<long>();
                acervoBi.CoAutores = acervoBi.CreditoAutorId.EhMaiorQueZero() ? retorno.Where(w=> w.ehcoautor).Select(s => new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = s.TipoAutoria, CreditoAutorNome = s.CreditoAutorNome}).Distinct().ToArray() : Array.Empty<CoAutor>();
                acervoBi.AssuntosIds = acervoBi.AssuntoId.EhMaiorQueZero() ? retorno.Select(s => s.AssuntoId).Distinct().ToArray() : Array.Empty<long>();
                return acervoBi;    
            }

            return default;
        }
        
    }
}