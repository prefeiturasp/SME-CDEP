using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao.ServicosFakes
{
    public class ServicoGerarMiniaturaFake : IServicoGerarMiniatura
    {
        private readonly IRepositorioArquivo repositorioArquivo;
        
        public ServicoGerarMiniaturaFake(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<long> GerarMiniatura(string tipoConteudo, string nomeArquivoFisico, string nomeArquivoMiniatura, TipoArquivo tipoArquivo)
        {
            return await repositorioArquivo.SalvarAsync(new Arquivo()
            {
                Nome = nomeArquivoMiniatura,
                TipoConteudo = tipoConteudo,
                Codigo = Guid.NewGuid(),
                Tipo = tipoArquivo
            });
        }
    }
}