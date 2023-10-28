using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoDocumental : RepositorioBase<AcervoDocumental>, IRepositorioAcervoDocumental
    {
        public RepositorioAcervoDocumental(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<IEnumerable<AcervoDocumental>> ObterTodos()
        {
            var query = @"select  ad.id,
                                  ad.material_id as materialId,
                                  ad.idioma_id as idiomaId,
                                  ad.ano,
                                  ad.numero_pagina numeroPagina,
                                  ad.volume,
                                  ad.tipo_anexo as tipoAnexo,                                  
                                  ad.largura,
                                  ad.altura,
                                  ad.tamanho_arquivo as tamanhoArquivo,
                                  ad.localizacao,
                                  ad.copia_digital as copiaDigital,
                                  ad.conservacao_id as conservacaoId,
                                  a.descricao,
                                  a.id,
                                  a.titulo,
                                  a.codigo,
                                  a.tipo,
                                  ca.id,
                                  ca.nome
                        from acervo_documental ad
                        join acervo a on a.id = ad.acervo_id     
                        left join acervo_credito_autor aca on aca.acervo_id = a.id
                        left join credito_autor ca on aca.credito_autor_id = ca.id
                        where not a.excluido ";

            var retorno = await conexao.Obter().QueryAsync<AcervoDocumental, Acervo, CreditoAutor,  AcervoDocumental>(
                query, (acervoDocumental, acervo, creditoAutor) =>
                {
                    acervo.CreditoAutor = creditoAutor;
                    acervoDocumental.Acervo = acervo;
                    return acervoDocumental;
                });
            
            return retorno;
        }
        
        public async Task<AcervoDocumentalCompleto> ObterPorId(long id)
        {
            var query = @"select  ad.id,
                                  ad.ano,
                                  ad.numero_pagina numeroPagina,
                                  ad.volume,
                                  ad.tipo_anexo as tipoAnexo,                                  
                                  ad.largura,
                                  ad.altura,
                                  ad.tamanho_arquivo as tamanhoArquivo,
                                  ad.localizacao,
                                  ad.copia_digital as copiaDigital,
                                  a.descricao,
                                  a.id as AcervoId,
                                  a.titulo,
                                  a.codigo,
                                  a.codigo_novo CodigoNovo,                                  
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
                                  i.id as IdiomaId,
                                  i.nome as IdiomaNome,
                                  m.id as materialId,
                                  m.nome as materialNome,
                                  c.id as conservacaoId,
                                  c.nome as conservacaoNome,
                                  adoc.id as acessoDocumentoId,
                                  adoc.nome as acessoDocumentoNome
                        from acervo_documental ad
                        join acervo a on a.id = ad.acervo_id 
                        join idioma i on i.id = ad.idioma_id 
                        join acervo_documental_acesso_documento adad on adad.acervo_documental_id = ad.id
                        join acesso_documento adoc on adoc.id = adad.acesso_documento_id
                        left join acervo_credito_autor aca on aca.acervo_id = a.id
                        left join credito_autor ca on aca.credito_autor_id = ca.id
                        left join acervo_documental_arquivo ada on ada.acervo_documental_id = ad.id
                        left join arquivo arq on arq.id = ada.arquivo_id 
                        left join material m on m.id = ad.material_id
                        left join conservacao c on c.id = ad.conservacao_id                         
                        where not a.excluido 
                        and a.id = @id";

            var retorno = (await conexao.Obter().QueryAsync<AcervoDocumentalCompleto>(query, new { id }));
            if (retorno.Any())
            {
                var acervoDocumental = retorno.FirstOrDefault();
                acervoDocumental.Arquivos = retorno.Where(w=> w.ArquivoId > 0).Select(s => new ArquivoResumido() { Id = s.ArquivoId.Value, Codigo = s.ArquivoCodigo, Nome = s.ArquivoNome }).DistinctBy(d=> d.Id).ToArray();
                acervoDocumental.AcessoDocumentosIds = retorno.Select(s => s.AcessoDocumentoId).Distinct().ToArray();
                acervoDocumental.CreditosAutoresIds = acervoDocumental.CreditoAutorId > 0 ? retorno.Select(s => s.CreditoAutorId).Distinct().ToArray() : Array.Empty<long>();
                return acervoDocumental;    
            }

            return default;
        }
        
    }
}