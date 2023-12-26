using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
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
                                  a.ano,
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
            var query = @"select  a.ano,
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
                                  
                                  ad.id,
                                  ad.numero_pagina numeroPagina,
                                  ad.volume,
                                  ad.tipo_anexo as tipoAnexo,                                  
                                  ad.largura,
                                  ad.altura,
                                  ad.tamanho_arquivo as tamanhoArquivo,
                                  ad.localizacao,
                                  ad.copia_digital as copiaDigital,
                                  ad.idioma_id as IdiomaId,
                                  ad.material_id as MaterialId,
                                  ad.conservacao_id as ConservacaoId
                        from acervo_documental ad
                            join acervo a on a.id = ad.acervo_id 
                            join idioma i on i.id = ad.idioma_id  
                            left join material m on m.id = ad.material_id and not m.excluido
                            left join conservacao c on c.id = ad.conservacao_id and not c.excluido                         
                        where not a.excluido
                            and not i.excluido
                            and a.id = @id;

                        select ca.id as CreditoAutorId
                        from acervo_credito_autor aca
                            join credito_autor ca on aca.credito_autor_id = ca.id
                        where not ca.excluido
                              and aca.acervo_id = @id;                             

                        select arq.id,
                                  arq.nome,
                                  arq.codigo
                        from acervo_documental ad
                            join acervo_documental_arquivo ada on ada.acervo_documental_id = ad.id
                            join arquivo arq on arq.id = ada.arquivo_id
                        where not  arq.excluido
                        and ad.acervo_id = @id;

                        select adoc.id as acessoDocumentoId
                        from acervo_documental ad
                            join acervo_documental_acesso_documento adad on adad.acervo_documental_id = ad.id
	                        join acesso_documento adoc on adoc.id = adad.acesso_documento_id
                        where not  adoc.excluido
                        and ad.acervo_id = @id;	";
            
            var queryMultiple = await conexao.Obter().QueryMultipleAsync(query, new { id });
            var acervoDocumentalCompleto = queryMultiple.ReadFirst<AcervoDocumentalCompleto>();
            acervoDocumentalCompleto.CreditosAutoresIds = queryMultiple.Read<long>().ToArray();
            acervoDocumentalCompleto.Arquivos = queryMultiple.Read<ArquivoResumido>().ToArray();
            acervoDocumentalCompleto.AcessoDocumentosIds = queryMultiple.Read<long>().ToArray();
            
            return acervoDocumentalCompleto;
        }

        public async Task<AcervoDocumentalDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo)
        {
            var acervoDocumental = await ObterPorCodigo(filtroCodigo);

            if (acervoDocumental.EhNulo())
                return default;
            
            acervoDocumental.Autores = await ObterCreditosAutores(acervoDocumental.AcervoId);
            
            acervoDocumental.Imagens = await ObterArquivos(acervoDocumental.Id);
            
            acervoDocumental.AcessosDocumentos = await ObterAcessoDocumentos(acervoDocumental.Id);
            
            return acervoDocumental;
        }
        
        protected async Task<string> ObterAcessoDocumentos(long acervoDocumentalId)
        {
            var query = @" select adoc.nome
                            from acervo_documental_acesso_documento ada 
                                join acesso_documento adoc on adoc.id = ada.acesso_documento_id
                                where not adoc.excluido 
                            and ada.acervo_documental_id  = @acervoDocumentalId";

            var acessoDocumentos = await conexao.Obter().QueryAsync<string>(query, new { acervoDocumentalId });
            
            return  acessoDocumentos.Any(a=> a.EstaPreenchido())
                ? string.Join(" | ", acessoDocumentos.Select(s => s).Distinct())
                : string.Empty;
        } 

        protected async Task<IEnumerable<ImagemDetalhe>> ObterArquivos(long acervoDocumentalId)
        {
            var query = @" select a.nome original
                            from acervo_documental_arquivo ada 
                                join arquivo a on a.id = ada.arquivo_id   
                            where not a.excluido 
                                and ada.acervo_documental_id = @acervoDocumentalId";

            return await conexao.Obter().QueryAsync<ImagemDetalhe>(query, new { acervoDocumentalId });
        }

        private async Task<AcervoDocumentalDetalhe> ObterPorCodigo(string codigo)
        {
            var query = @"select  ad.id,
			                        a.id as AcervoId,
 			                        a.titulo,
                                    a.codigo,
                                    a.codigo_novo CodigoNovo, 
                                    m.nome as material,
                                    i.nome as Idioma,
                                    a.ano,
                                    ad.numero_pagina numeroPagina,
                                    ad.volume,
                                    a.descricao,  
                                    ad.tipo_anexo as tipoAnexo,                                  
                                    ad.largura,
                                    ad.altura,
                                    ad.tamanho_arquivo as tamanhoArquivo,
                                    ad.localizacao,
                                    ad.copia_digital as copiaDigital,          
                                    c.nome as conservacao
                        from acervo_documental ad
                        join acervo a on a.id = ad.acervo_id 
                        join idioma i on i.id = ad.idioma_id 
                        left join material m on m.id = ad.material_id and not m.excluido 
                        left join conservacao c on c.id = ad.conservacao_id and not c.excluido                          
                        where not a.excluido                         
                        and not i.excluido                         
                        and (a.codigo = @codigo or a.codigo_novo = @codigo) ";
            return conexao.Obter().QueryFirstOrDefault<AcervoDocumentalDetalhe>(query, new { codigo });
        }
    }
}