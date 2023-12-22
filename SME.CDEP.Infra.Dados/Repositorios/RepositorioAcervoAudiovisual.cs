using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

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
                                  av.copia,
                                  av.permite_uso_imagem as permiteUsoImagem,
                                  av.conservacao_id as conservacaoId,
                                  a.descricao,                                  
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
                                  av.copia,
                                  av.permite_uso_imagem as permiteUsoImagem,
                                  av.conservacao_id as conservacaoId,
                                  a.descricao,  
                                  av.suporte_id as suporteId,
                                  av.duracao,                                  
                                  av.cromia_id as cromiaId,                                  
                                  av.tamanho_arquivo as tamanhoArquivo,
                                  av.acessibilidade,
                                  av.disponibilizacao,
                                  a.id as AcervoId,
                                  a.titulo,
                                  a.codigo,
                                  a.tipo as TipoAcervoId,
                                  a.ano,
                                  a.data_acervo dataacervo,
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
                        left join acervo_credito_autor aca on aca.acervo_id = a.id
                        left join credito_autor ca on aca.credito_autor_id = ca.id                         
                        where not a.excluido 
                        and a.id = @id";

            var retorno = (await conexao.Obter().QueryAsync<AcervoAudiovisualCompleto>(query, new { id }));
            if (retorno.Any())
            {
                var audiovisualCompleto = retorno.FirstOrDefault();
                audiovisualCompleto.CreditosAutoresIds = audiovisualCompleto.CreditoAutorId.HasValue 
                    ? retorno.Select(s => s.CreditoAutorId.Value).Distinct().ToArray() 
                    : Array.Empty<long>();
                return audiovisualCompleto;    
            }

            return default;
        }

        public async Task<AcervoAudiovisualDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo)
        {
            var acervoAudioVisual = await ObterPorCodigo(filtroCodigo);

            if (acervoAudioVisual.EhNulo())
                return default;
            
            acervoAudioVisual.Creditos = await ObterCreditos(acervoAudioVisual.AcervoId);
            
            return acervoAudioVisual;
        }

        private async Task<AcervoAudiovisualDetalhe> ObterPorCodigo(string codigo)
        {
            var query = @"select    av.id,
                                    av.acervo_id acervoId,
                                    a.titulo,
                                    a.codigo,
                                    av.localizacao,
                                    av.procedencia,
                                    a.ano,
                                    a.data_acervo dataAcervo,
                                    av.copia,
                                    av.permite_uso_imagem as permiteUsoImagem,
                                    c.nome as conservacao,
                                    a.descricao,  
                                    s.nome as suporte,
                                    av.duracao,                                  
                                    cr.nome as cromia,
                                    av.tamanho_arquivo as tamanhoArquivo,
                                    av.acessibilidade,
                                    av.disponibilizacao                                  
                          from acervo_audiovisual av
                            join acervo a on a.id = av.acervo_id      
                            join suporte s on s.id = av.suporte_id
                            left join conservacao c on c.id = av.conservacao_id
                            left join cromia cr on cr.id = av.cromia_id
                          where not a.excluido 
                            and not s.excluido 
	                            and not s.excluido
	                            and not c.excluido
	                            and not cr.excluido
                                and a.codigo = @codigo";
            return conexao.Obter().QueryFirstOrDefault<AcervoAudiovisualDetalhe>(query, new { codigo });
        }
    }
}