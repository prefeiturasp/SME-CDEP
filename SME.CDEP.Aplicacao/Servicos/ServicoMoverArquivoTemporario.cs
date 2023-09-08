using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoMoverArquivoTemporario : IServicoMoverArquivoTemporario
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly IRepositorioArquivo repositorioArquivo;
        
        public ServicoMoverArquivoTemporario(IServicoArmazenamento servicoArmazenamento,IRepositorioArquivo repositorioArquivo)
        {
            this.servicoArmazenamento = servicoArmazenamento?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.repositorioArquivo = repositorioArquivo?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }
        
        public async Task Mover(TipoArquivo tipoArquivo, Guid codigoArquivo)
        {
            var arquivo = await repositorioArquivo.ObterPorCodigo(codigoArquivo);
            
            if (arquivo != null )
            {
                var extensao = Path.GetExtension(arquivo.Nome);
                var nomeArquivoBucket= $"{arquivo.Codigo.ToString()}{extensao}";
             
                await servicoArmazenamento.Mover(nomeArquivoBucket);
                
                arquivo.Tipo = tipoArquivo;
                await repositorioArquivo.SalvarAsync(arquivo);
            }
        }
    }
}