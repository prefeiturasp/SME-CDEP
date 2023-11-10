using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoBase
    {
        void ValidarArquivo(IFormFile file);
        Task<long> PersistirImportacao(ImportacaoArquivo importacaoArquivo);
        Task<long> AtualizarImportacao(long id, string conteudo, ImportacaoStatus? status = null);
        Task ValidarOuInserirMateriais(IEnumerable<string> materiais, TipoMaterial tipoMaterial);
        Task ValidarOuInserirEditoras(IEnumerable<string> editoras);
        Task ValidarOuInserirSeriesColecoes(IEnumerable<string> seriesColecoes);
        Task ValidarOuInserirIdiomas(IEnumerable<string> idiomas);
        Task ValidarOuInserirAssuntos(IEnumerable<string> assuntos);
        Task ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(IEnumerable<string> creditosAutoresCoautores, TipoCreditoAutoria tipoCreditoAutoria);
        void ValidarPreenchimentoLimiteCaracteres(LinhaConteudoAjustarDTO campo, string nomeCampo);
        Task<bool> RemoverLinhaDoArquivo<T>(long id, int linhaDoArquivo, TipoAcervo tipoAcervoEsperado) where T: AcervoLinhaDTO;
        Task<bool> Remover(long id);
    }
}
