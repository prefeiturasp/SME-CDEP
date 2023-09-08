using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
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
                                  af.data_acervo,
                                  af.copia_digital,
                                  af.permite_uso_imagem,
                                  af.conservacao_id,
                                  af.descricao,
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
                        join credito_autor ca on ca.id = a.credito_autor_id
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
                                  af.data_acervo as DataAcervo,
                                  af.copia_digital as CopiaDigital,
                                  af.permite_uso_imagem as PermiteUsoImagem,
                                  af.conservacao_id as ConservacaoId,
                                  af.descricao,
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
                                  a.tipo as TipoAcervoId,
                                  a.criado_em as CriadoEm,
                                  a.criado_por as CriadoPor,
                                  a.criado_login as CriadoLogin,
                                  a.alterado_em as AlteradoEm,
                                  a.alterado_por as AlteradoPor,
                                  a.alterado_login as AlteradoLogin,
                                  ca.id as CreditoAutorId,
                                  ca.nome as CreditoAutorNome,
                                  arq.nome as ArquivoNome,
                                  arq.codigo as ArquivoCodigo
                        from acervo_fotografico af
                        join acervo a on a.id = af.acervo_id 
                        join credito_autor ca on ca.id = a.credito_autor_id
                        left join acervo_fotografico_arquivo afa on afa.acervo_fotografico_id = af.id
                        left join arquivo arq on arq.id = afa.arquivo_id 
                        where not a.excluido 
                        and a.id = @id";

            var retorno = (await conexao.Obter().QueryAsync<AcervoFotograficoCompleto>(query, new { id }));
            if (retorno.Any())
            {
                var acervoFotografico = retorno.FirstOrDefault();
                acervoFotografico.Arquivos = retorno.Where(w=> !string.IsNullOrEmpty(w.ArquivoNome)).Select(s => new ArquivoResumido() { Codigo = s.ArquivoCodigo, Nome = s.ArquivoNome }).ToArray();
            
                return acervoFotografico;    
            }

            return default;
        }
        
    }
}