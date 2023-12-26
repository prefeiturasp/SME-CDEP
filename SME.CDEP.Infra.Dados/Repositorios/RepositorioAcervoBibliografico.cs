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
            var query =  @"select  a.id as AcervoId,
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
                                   a.ano,
                                   a.descricao,            
                                   
                                   ab.id,                                   
                                   ab.edicao,          
                                   ab.numero_pagina numeroPagina,
                                   ab.largura,
                                   ab.altura,                                  
                                   ab.volume,
                                   ab.localizacao_cdd as localizacaoCDD,
                                   ab.localizacao_pha as localizacaoPHA,                                  
                                   ab.notas_gerais as notasGerais,
                                   ab.isbn,
                                   ab.idioma_id as IdiomaId,
                                   ab.material_id as MaterialId,
                                   ab.editora_id as EditoraId,
                                   ab.serie_colecao_id as SerieColecaoId          
                        from acervo_bibliografico ab
                            join acervo a on a.id = ab.acervo_id 
                            join idioma i on i.id = ab.idioma_id                          
                            join material m on m.id = ab.material_id
                            left join editora e on e.id = ab.editora_id and not e.excluido
                            left join serie_colecao sc on sc.id = ab.serie_colecao_id and not sc.excluido
                        where not a.excluido
                          and not i.excluido
                          and not m.excluido
                          and a.id = @id;

                        select aba.assunto_id as assuntoId
                        from acervo_bibliografico_assunto aba
                            join acervo_bibliografico ab on ab.id = aba.acervo_bibliografico_id
                            join assunto ast on ast.id = aba.assunto_id
                        where not  ast.excluido
                            and ab.acervo_id = @id;

                        select ca.id as CreditoAutorId,
                                  ca.nome as CreditoAutorNome,             
                                  aca.tipo_autoria as TipoAutoria,
                                  aca.ehcoautor
                        from acervo_credito_autor aca
                        join credito_autor ca on aca.credito_autor_id = ca.id
                        where not ca.excluido
                            and aca.acervo_id = @id; ";

            var queryMultiple = await conexao.Obter().QueryMultipleAsync(query, new { id });
            
            var acervoBibliograficoCompleto = queryMultiple.ReadFirst<AcervoBibliograficoCompleto>();
            acervoBibliograficoCompleto.AssuntosIds = queryMultiple.Read<long>().ToArray();
            var creditosAutores = queryMultiple.Read<CoAutor>();
            
            acervoBibliograficoCompleto.CreditosAutoresIds = creditosAutores.Where(w=> !w.EhCoautor).Select(s => s.CreditoAutorId).ToArray();
            acervoBibliograficoCompleto.CoAutores = creditosAutores.Where(w=> w.EhCoautor).ToArray();
            
            return acervoBibliograficoCompleto;
        }

        public async Task<AcervoBibliograficoDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo)
        {
            var acervoBibliografico = await ObterPorCodigo(filtroCodigo);

            if (acervoBibliografico.EhNulo())
                return default;
            
            acervoBibliografico.Autores = await ObterCreditosAutores(acervoBibliografico.AcervoId);
            
            acervoBibliografico.Assuntos = await ObterAssuntos(acervoBibliografico.Id);
            
            return acervoBibliografico;
        }

        protected async Task<string> ObterAssuntos(long acervoBibliograficoId)
        {
            var query = @" select a.nome
                                from acervo_bibliografico_assunto aba 
                            join assunto a on a.id = aba.assunto_id
                            where aba.acervo_bibliografico_id = @acervoDocumentalId
                            and not a.excluido";

            var assuntos = await conexao.Obter().QueryAsync<string>(query, new { acervoDocumentalId = acervoBibliograficoId });
            
            return  assuntos.Any(a=> a.EstaPreenchido())
                ? string.Join(" | ", assuntos.Select(s => s).Distinct())
                : string.Empty;
        } 

        private async Task<AcervoBibliograficoDetalhe> ObterPorCodigo(string codigo)
        {
            var query = @"select  ab.id,
                                  a.id as AcervoId,
                                  a.titulo,
                                  a.subTitulo,
                                  m.nome as material,
                                  e.nome as editora,          
                                  a.ano,          
                                  ab.edicao,
                                  ab.numero_pagina numeroPagina,
                                  ab.largura,
                                  ab.altura,
                                  ab.volume,
                                  i.nome as Idioma,
                                  ab.localizacao_cdd as localizacaoCDD,
                                  ab.localizacao_pha as localizacaoPHA,                                  
		                          ab.notas_gerais as notasGerais,
                                  ab.isbn,           
                                  a.codigo,
                                  sc.nome serieColecao
                        from acervo_bibliografico ab
                        join acervo a on a.id = ab.acervo_id 
                        join idioma i on i.id = ab.idioma_id 
                        join material m on m.id = ab.material_id
                        left join editora e on e.id = ab.editora_id and not i.excluido 
                        left join serie_colecao sc on sc.id = ab.serie_colecao_id and not sc.excluido
                        where not a.excluido  
                        and a.codigo = @codigo ";
            return conexao.Obter().QueryFirstOrDefault<AcervoBibliograficoDetalhe>(query, new { codigo });
        }
    }
}