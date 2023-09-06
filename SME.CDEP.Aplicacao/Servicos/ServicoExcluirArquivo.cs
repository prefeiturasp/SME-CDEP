using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.Mensageria.Exchange;
using SME.CDEP.Infra.Servicos.Mensageria.Rotas;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoExcluirArquivo : IServicoExcluirArquivo
    {
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IServicoMensageria servicoMensageria;
        
        public ServicoExcluirArquivo(IRepositorioArquivo repositorioArquivo,IServicoMensageria servicoMensageria)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }
       
        public async Task<bool> Excluir(Guid[] codigos)
        {
            var arquivos = await repositorioArquivo.ObterPorCodigos(codigos);
            if (arquivos == null)
                throw new NegocioException(MensagemNegocio.ARQUIVO_INF0RMADO_NAO_ENCONTRADO);
            
            var arquivosIds = arquivos.Select(s => s.Id).ToArray();
            
            await repositorioArquivo.ExcluirArquivosPorIds(arquivosIds);

            foreach (var arquivo in arquivos)
            {
                var extencao = Path.GetExtension(arquivo.Nome);

                await servicoMensageria.Enviar(arquivo.Codigo + extencao, RotasRabbitSgp.RemoverArquivoArmazenamento, ExchangeRabbit.Sgp);

            }
            
            return true;
        }
    }
}  