using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoFotografico : RepositorioBase<AcervoFotografico>, IRepositorioAcervoFotografico
    {
        public RepositorioAcervoFotografico(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public async Task<IEnumerable<AcervoFotografico>> ObterTodos()
        {
            var query = @"select  af.id,
                                  af.localizacao,
                                  af.procedencia,
                                  af.copia_digital,
                                  af.permite_uso_imagem,
                                  af.conservacao_id,
                                  a.descricao,
                                  af.quantidade,
                                  af.largura,
                                  af.altura,
                                  af.suporte_id as suporteId,
                                  af.formato_id as formatoId,
                                  af.cromia_id as cromiaId,
                                  af.resolucao,
                                  af.tamanho_arquivo,
                                  a.id,
                                  a.titulo,
                                  a.codigo,
                                  a.tipo,
                                  ca.id,
                                  ca.nome
                        from acervo_fotografico af
                        join acervo a on a.id = af.acervo_id 
                        join acervo_credito_autor aca on aca.acervo_id = a.id
                        join credito_autor ca on aca.credito_autor_id = ca.id
                        where not a.excluido ";

            var retorno = await conexao.Obter().QueryAsync<AcervoFotografico, Acervo, CreditoAutor,  AcervoFotografico>(
                query, (acervoFotografico, acervo, creditoAutor) =>
                {
                    acervo.CreditoAutor = creditoAutor;
                    acervoFotografico.Acervo = acervo;
                    return acervoFotografico;
                });
            
            return retorno;
        }
        
        public async Task<AcervoFotograficoCompleto> ObterPorId(long id)
        {
            var query = @"select  af.id,
                                  af.localizacao,
                                  af.procedencia,
                                  af.copia_digital as CopiaDigital,
                                  af.permite_uso_imagem as PermiteUsoImagem,
                                  af.conservacao_id as ConservacaoId,
                                  a.descricao,
                                  af.quantidade,
                                  af.largura,
                                  af.altura,
                                  af.suporte_id as suporteId,
                                  af.formato_id as formatoId,
                                  af.cromia_id as cromiaId,
                                  af.resolucao,
                                  af.tamanho_arquivo as TamanhoArquivo,
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
                                  arq.codigo as ArquivoCodigo
                        from acervo_fotografico af
                        join acervo a on a.id = af.acervo_id 
                        join acervo_credito_autor aca on aca.acervo_id = a.id
                        join credito_autor ca on aca.credito_autor_id = ca.id
                        left join acervo_fotografico_arquivo afa on afa.acervo_fotografico_id = af.id
                        left join arquivo arq on arq.id = afa.arquivo_id 
                        where not a.excluido 
                        and a.id = @id";

            var retorno = (await conexao.Obter().QueryAsync<AcervoFotograficoCompleto>(query, new { id }));
            if (retorno.Any())
            {
                var acervoFotografico = retorno.FirstOrDefault();
                acervoFotografico.Arquivos = retorno.Where(w=> w.ArquivoId > 0).Select(s => new ArquivoResumido() { Id = s.ArquivoId, Codigo = s.ArquivoCodigo, Nome = s.ArquivoNome }).DistinctBy(d=> d.Id).ToArray();
                acervoFotografico.CreditosAutoresIds = retorno.Select(s => s.CreditoAutorId).Distinct().ToArray();
                return acervoFotografico;    
            }

            return default;
        }

        public async Task<AcervoFotograficoDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo)
        {
            var acervoFotografico = await ObterPorCodigo(filtroCodigo);

            if (acervoFotografico.EhNulo())
                return default;
            
            acervoFotografico.Creditos = await ObterCreditos(acervoFotografico.AcervoId);
            
            acervoFotografico.Imagens = await ObterArquivos(acervoFotografico.Id);
            
            return acervoFotografico;
        }

        protected async Task<IEnumerable<ImagemDetalhe>> ObterArquivos(long acervoFotograficoId)
        {
            var query = @" select a.nome original, 
                                am.nome thumbnail
                            from acervo_fotografico_arquivo afa 
                                join arquivo a on a.id = afa.arquivo_id 
                            join arquivo am on am.id = afa.arquivo_miniatura_id  
                            where not a.excluido and not am.excluido 
                                and afa.acervo_fotografico_id  = @acervoFotograficoId";

            return await conexao.Obter().QueryAsync<ImagemDetalhe>(query, new { acervoFotograficoId });
        }

        private async Task<AcervoFotograficoDetalhe> ObterPorCodigo(string codigo)
        {
            var query = @"select  af.id,
                                  af.acervo_id acervoId,
                                   a.titulo,
                                   a.codigo,           
                                   af.localizacao,
                                   af.procedencia,
                                   a.ano,
                                   a.data_acervo dataacervo,           
                                  af.copia_digital as CopiaDigital,
                                  af.permite_uso_imagem as PermiteUsoImagem,
                                  c.nome as Conservacao,          
                                  a.descricao,
                                  af.quantidade,
                                  af.largura,
                                  af.altura,
                                  su.nome as suporte,
                                  fo.nome as formato,
                                  af.tamanho_arquivo as TamanhoArquivo,
                                  cro.nome as cromia,
                                  af.resolucao
                        from acervo_fotografico af
                        join acervo a on a.id = af.acervo_id 
                        join conservacao c on c.id = af.conservacao_id
                        join suporte su on su.id = af.suporte_id
                        join formato fo on fo.id = af.formato_id
                        join cromia cro on cro.id = af.cromia_id
                        where not a.excluido
                          and not su.excluido 
	                      and not c.excluido
	                      and not fo.excluido
	                      and not c.excluido
                          and a.codigo = @codigo ";
            return conexao.Obter().QueryFirstOrDefault<AcervoFotograficoDetalhe>(query, new { codigo });
        }
    }
}