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
        
        public async Task<IList<AcervoFotografico>> ObterTodos()
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

            var retorno = (await conexao.Obter().QueryAsync<AcervoFotografico, Acervo, CreditoAutor,  AcervoFotografico>(
                query, (acervoFotografico, acervo, creditoAutor) =>
                {
                    acervo.CreditoAutor = creditoAutor;
                    acervoFotografico.Acervo = acervo;
                    return acervoFotografico;
                })).ToList();
            
            return retorno;
        }
        
        public async Task<AcervoFotografico> ObterPorId(long id)
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
                                  a.criado_em,
                                  a.criado_por,
                                  a.criado_login,
                                  a.alterado_em,
                                  a.alterado_por,
                                  a.alterado_login,
                                  ca.id,
                                  ca.nome
                        from acervo_fotografico af
                        join acervo a on a.id = af.acervo_id 
                        join credito_autor ca on ca.id = a.credito_autor_id
                        where not a.excluido 
                        and af.id = @id";

            var retorno = (await conexao.Obter().QueryAsync<AcervoFotografico, Acervo, CreditoAutor,  AcervoFotografico>(
                query, (acervoFotografico, acervo, creditoAutor) =>
                {
                    acervo.CreditoAutor = creditoAutor;
                    acervoFotografico.Acervo = acervo;
                    return acervoFotografico;
                },new {id})).FirstOrDefault();
            
            return retorno;
        }
        
    }
}