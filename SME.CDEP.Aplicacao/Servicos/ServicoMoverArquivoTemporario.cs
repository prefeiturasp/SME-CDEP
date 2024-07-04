using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
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
        
        public async Task Mover(TipoArquivo tipoArquivo, Arquivo arquivo)
        {
            await servicoArmazenamento.Mover(arquivo.NomeArquivoFisico);
            arquivo.Tipo = tipoArquivo;
            arquivo.Id = await repositorioArquivo.SalvarAsync(arquivo);
        }
    }
}