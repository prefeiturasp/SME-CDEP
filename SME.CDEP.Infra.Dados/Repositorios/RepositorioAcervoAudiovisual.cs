using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoAudiovisual : RepositorioBase<AcervoAudiovisual>, IRepositorioAcervoAudiovisual
    {
        public RepositorioAcervoAudiovisual(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<IEnumerable<AcervoAudiovisual>> ObterTodos()
        {
            var query = @"select  av.id,
                                  av.localizacao,
                                  av.procedencia,
                                  av.data_acervo dataAcervo,
                                  av.copia,
                                  av.permite_uso_imagem as permiteUsoImagem,
                                  av.conservacao_id as conservacaoId,
                                  av.descricao,                                  
                                  av.suporte_id as suporteId,
                                  av.duracao,                                  
                                  av.cromia_id as cromiaId,                                  
                                  av.tamanho_arquivo,
                                  av.acessibilidade,
                                  av.disponibilizacao,
                                  a.id,
                                  a.titulo,
                                  a.codigo,
                                  a.tipo,
                                  ca.id,
                                  ca.nome
                        from acervo_audiovisual av
                        join acervo a on a.id = av.acervo_id 
                        join acervo_credito_autor aca on aca.acervo_id = a.id
                        join credito_autor ca on aca.credito_autor_id = ca.id
                        where not a.excluido ";

            var retorno = await conexao.Obter().QueryAsync<AcervoAudiovisual, Acervo, CreditoAutor,  AcervoAudiovisual>(
                query, (acervoAudiovisual, acervo, creditoAutor) =>
                {
                    acervo.CreditoAutor = creditoAutor;
                    acervoAudiovisual.Acervo = acervo;
                    return acervoAudiovisual;
                });
            
            return retorno;
        }
        
        public async Task<AcervoAudiovisualCompleto> ObterPorId(long id)
        {
            var query = @"select  av.id,
                                  av.localizacao,
                                  av.procedencia,
                                  av.data_acervo dataAcervo,
                                  av.copia,
                                  av.permite_uso_imagem as permiteUsoImagem,
                                  av.conservacao_id as conservacaoId,
                                  av.descricao,                                  
                                  av.suporte_id as suporteId,
                                  av.duracao,                                  
                                  av.cromia_id as cromiaId,                                  
                                  av.tamanho_arquivo,
                                  av.acessibilidade,
                                  av.disponibilizacao,
                                  a.id as AcervoId,
                                  a.titulo,
                                  a.codigo,
                                  a.tipo as TipoAcervoId,
                                  a.criado_em as CriadoEm,
                                  a.criado_por as CriadoPor,
                                  a.criado_login as CriadoLogin,
                                  a.alterado_em as AlteradoEm,
                                  a.alterado_por as AlteradoPor,
                                  a.alterado_login as AlteradoLogin,
                                  ca.id as CreditoAutorId,
                                  ca.nome as CreditoAutorNome                                  
                        from acervo_audiovisual av
                        join acervo a on a.id = av.acervo_id 
                        join acervo_credito_autor aca on aca.acervo_id = a.id
                        join credito_autor ca on aca.credito_autor_id = ca.id                         
                        where not a.excluido 
                        and a.id = @id";

            var retorno = (await conexao.Obter().QueryAsync<AcervoAudiovisualCompleto>(query, new { id }));
            if (retorno.Any())
            {
                var audiovisualCompleto = retorno.FirstOrDefault();
                audiovisualCompleto.CreditosAutoresIds = retorno.Select(s => s.CreditoAutorId).Distinct().ToArray();
                return audiovisualCompleto;    
            }

            return default;
        }
        
    }
}