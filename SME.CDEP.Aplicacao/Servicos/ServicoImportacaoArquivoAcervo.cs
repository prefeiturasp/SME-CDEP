using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervo : IServicoImportacaoArquivoAcervo 
    {
        private readonly IRepositorioImportacaoArquivo repositorioImportacaoArquivo;
        
        public ServicoImportacaoArquivoAcervo(IRepositorioImportacaoArquivo repositorioImportacaoArquivo)
        {
            this.repositorioImportacaoArquivo = repositorioImportacaoArquivo ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
        }

        public async Task<bool> Excluir(long id)
        {
            await repositorioImportacaoArquivo.Remover(id);
            return true;
        }
    }
}