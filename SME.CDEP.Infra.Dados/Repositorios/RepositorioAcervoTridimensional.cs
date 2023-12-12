using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
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
            var query = @"select  at.id,
                                  at.procedencia,
                                  at.conservacao_id as conservacaoId,
                                  at.quantidade,
                                  a.descricao,                                  
                                  at.largura,
                                  at.altura,
                                  at.profundidade,
                                  at.diametro,
                                  a.id as AcervoId,
                                  a.titulo,
                                  a.codigo,
                                  a.ano,
                                  a.tipo as TipoAcervoId,
                                  a.criado_em as CriadoEm,
                                  a.criado_por as CriadoPor,
                                  a.criado_login as CriadoLogin,
                                  a.alterado_em as AlteradoEm,
                                  a.alterado_por as AlteradoPor,
                                  a.alterado_login as AlteradoLogin,                                  
                                  arq.id as arquivoId,
                                  arq.nome as ArquivoNome,
                                  arq.codigo as ArquivoCodigo
                        from acervo_tridimensional at
                        join acervo a on a.id = at.acervo_id 
                        left join acervo_tridimensional_arquivo ata on ata.acervo_tridimensional_id = at.id
                        left join arquivo arq on arq.id = ata.arquivo_id 
                        where not a.excluido 
                        and a.id = @id";

            var retorno = (await conexao.Obter().QueryAsync<AcervoTridimensionalCompleto>(query, new { id }));
            if (retorno.Any())
            {
                var acervoTridimensionalCompleto = retorno.FirstOrDefault();
                acervoTridimensionalCompleto.Arquivos = retorno.Where(w=> w.ArquivoId > 0).Select(s => new ArquivoResumido() { Id = s.ArquivoId, Codigo = s.ArquivoCodigo, Nome = s.ArquivoNome }).DistinctBy(d=> d.Id).ToArray();
                return acervoTridimensionalCompleto;    
            }

            return default;
        }
        
    }
}