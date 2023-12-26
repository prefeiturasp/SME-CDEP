using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoTridimensional : RepositorioBase<AcervoTridimensional>, IRepositorioAcervoTridimensional
    {
        public RepositorioAcervoTridimensional(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<IEnumerable<AcervoTridimensional>> ObterTodos()
        {
            var query = @"select  at.id,
                                  at.procedencia,
                                  at.conservacao_id ConservacaoId,
                                  at.quantidade,
                                  a.descricao,                                  
                                  at.largura,
                                  at.altura,
                                  at.profundidade,
                                  at.diametro,
                                  a.id,
                                  a.titulo,
                                  a.codigo,
                                  a.tipo,
                                  ca.id,
                                  ca.nome
                        from acervo_tridimensional at
                        join acervo a on a.id = at.acervo_id 
                        join acervo_credito_autor aca on aca.acervo_id = a.id
                        join credito_autor ca on aca.credito_autor_id = ca.id
                        where not a.excluido ";

            var retorno = await conexao.Obter().QueryAsync<AcervoTridimensional, Acervo, CreditoAutor,  AcervoTridimensional>(
                query, (acervoTridimensional, acervo, creditoAutor) =>
                {
                    acervo.CreditoAutor = creditoAutor;
                    acervoTridimensional.Acervo = acervo;
                    return acervoTridimensional;
                });
            
            return retorno;
        }
        
        public async Task<AcervoTridimensionalCompleto> ObterPorId(long id)
        {
            var query = @"select a.id as AcervoId,
                                  a.titulo,
                                  a.codigo,
                                  a.descricao,
                                  a.ano,
                                  a.data_acervo dataacervo,
                                  a.tipo as TipoAcervoId,
                                  a.criado_em as CriadoEm,
                                  a.criado_por as CriadoPor,
                                  a.criado_login as CriadoLogin,
                                  a.alterado_em as AlteradoEm,
                                  a.alterado_por as AlteradoPor,
                                  a.alterado_login as AlteradoLogin,                                  

		                          at.id,
                                  at.procedencia,
                                  at.conservacao_id as conservacaoId,
                                  at.quantidade,                                            
                                  at.largura,
                                  at.altura,
                                  at.profundidade,
                                  at.diametro
                        from acervo_tridimensional at
                        join acervo a on a.id = at.acervo_id  
                        where not a.excluido 
                        and a.id = @id;
                            
                             
                        select arq.id,
                                  arq.nome,
                                  arq.codigo
                        from acervo_tridimensional at
                        join acervo_tridimensional_arquivo ata on ata.acervo_tridimensional_id = at.id
                        join arquivo arq on arq.id = ata.arquivo_id
                        where not  arq.excluido
                        and at.acervo_id = @id;";

            var queryMultiple = await conexao.Obter().QueryMultipleAsync(query, new { id });
            var acervoTridimensionalCompleto = queryMultiple.ReadFirst<AcervoTridimensionalCompleto>();
            acervoTridimensionalCompleto.Arquivos = queryMultiple.Read<ArquivoResumido>().ToArray();
            
            return acervoTridimensionalCompleto;
        }

        public async Task<AcervoTridimensionalDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo)
        {
            var acervoTridimensional = await ObterPorCodigo(filtroCodigo);

            if (acervoTridimensional.EhNulo())
                return default;
            
            acervoTridimensional.Imagens = await ObterArquivos(acervoTridimensional.Id);
            
            return acervoTridimensional;
        }

        private async Task<AcervoTridimensionalDetalhe> ObterPorCodigo(string codigo)
        {
            var query = @"select  at.id,
                                  at.acervo_id acervoId,
                                  a.titulo, 
                                  a.codigo,           
                                  at.procedencia,
                                  a.ano,
                                  a.data_acervo dataacervo,          
                                  c.nome as conservacao,
                                  at.quantidade,
                                  a.descricao,                                  
                                  at.largura,
                                  at.altura,
                                  at.profundidade,
                                  at.diametro
                        from acervo_tridimensional at
                        join acervo a on a.id = at.acervo_id 
                        join conservacao c on c.id = at.conservacao_id 
                        where not a.excluido 
	                      and not c.excluido
                          and a.codigo = @codigo ";
            return conexao.Obter().QueryFirstOrDefault<AcervoTridimensionalDetalhe>(query, new { codigo });
        }
        
        protected async Task<IEnumerable<ImagemDetalhe>> ObterArquivos(long acervoTridimensionalId)
        {
            var query = @" select a.nome original, 
                                 am.nome thumbnail
                            from acervo_tridimensional_arquivo at 
                                join arquivo a on a.id = at.arquivo_id 
                            join arquivo am on am.id = at.arquivo_miniatura_id  
                            where not a.excluido and not am.excluido 
                                and at.acervo_tridimensional_id  = @acervoTridimensionalId";

            return await conexao.Obter().QueryAsync<ImagemDetalhe>(query, new { acervoTridimensionalId });
        }
    }
}