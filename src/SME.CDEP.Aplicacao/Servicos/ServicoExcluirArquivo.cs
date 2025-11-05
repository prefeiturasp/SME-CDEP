using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoExcluirArquivo : IServicoExcluirArquivo
    {
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ServicoExcluirArquivo(IRepositorioArquivo repositorioArquivo,IServicoArmazenamento servicoArmazenamento)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }
       
        public async Task<bool> Excluir(long[] ids)
        {
             var arquivos = await repositorioArquivo.ObterPorIds(ids);
             if (arquivos.EhNulo())
                 throw new NegocioException(MensagemNegocio.ARQUIVO_INF0RMADO_NAO_ENCONTRADO);
            
             var arquivosIds = arquivos.Select(s => s.Id).ToArray();
            
             await repositorioArquivo.ExcluirArquivosPorIds(arquivosIds);
            
             foreach (var arquivo in arquivos)
                 await servicoArmazenamento.Excluir(arquivo.Nome);
            
            return true;
        }
    }
}  