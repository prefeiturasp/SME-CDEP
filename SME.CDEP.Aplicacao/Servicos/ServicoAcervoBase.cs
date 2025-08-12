using SME.CDEP.Aplicacao.Extensions;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoBase : IServicoAcervoBase
    {
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IServicoMoverArquivoTemporario servicoMoverArquivoTemporario;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private IEnumerable<Arquivo> ArquivosCompletos { get; set; }
        private IEnumerable<Arquivo> ArquivosExcluirArmazenamento { get; set; }
        private IEnumerable<Arquivo> ArquivosMoverTemporario { get; set; }
        
        public ServicoAcervoBase(IRepositorioAcervo repositorioAcervo,IRepositorioArquivo repositorioArquivo,
            IServicoMoverArquivoTemporario servicoMoverArquivoTemporario, IServicoArmazenamento servicoArmazenamento)
        {
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.servicoMoverArquivoTemporario = servicoMoverArquivoTemporario ?? throw new ArgumentNullException(nameof(servicoMoverArquivoTemporario));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            ArquivosCompletos = Enumerable.Empty<Arquivo>();
            ArquivosExcluirArmazenamento = Enumerable.Empty<Arquivo>();
            ArquivosMoverTemporario = Enumerable.Empty<Arquivo>();
        }

        public async Task<IEnumerable<Arquivo>> ObterArquivosPorIds(long[] ids)
        {
            if (ArquivosCompletos.Any())
                return ArquivosCompletos;
            
            ArquivosCompletos = await repositorioArquivo.ObterPorIds(ids);

            return ArquivosCompletos;
        }

        public async Task MoverArquivosTemporarios(TipoArquivo tipoAcervoFotografico)
        {
            foreach (var arquivo in ArquivosMoverTemporario)
                await servicoMoverArquivoTemporario.Mover(tipoAcervoFotografico, arquivo);
        }
        
        public async Task MoverArquivosTemporarios(TipoArquivo tipoAcervoFotografico, IEnumerable<Arquivo> arquivosAMover)
        {
            ArquivosMoverTemporario = arquivosAMover.Any() ? arquivosAMover : ArquivosMoverTemporario;
            await MoverArquivosTemporarios(tipoAcervoFotografico);
        }
        
        public async Task ExcluirArquivosArmazenamento()
        {
            foreach (var arquivoAExcluir in ArquivosExcluirArmazenamento)
            {
                await ExcluirArquivoArmazenamento(arquivoAExcluir.NomeArquivoFisico);
            }
        }

        public async Task ExcluirArquivoArmazenamento(string urlArquivo)
        {
            if (string.IsNullOrWhiteSpace(urlArquivo))
                return;
            await servicoArmazenamento.Excluir(urlArquivo);
        }


        public async Task<(IEnumerable<long>,IEnumerable<long>)> ObterArquivosInseridosExcluidosMovidos(long[]? arquivosAlterados, long[] arquivosExistentes)
        {
            arquivosAlterados = arquivosAlterados ?? Array.Empty<long>();
            var arquivosAlteradosCompletos = await repositorioArquivo.ObterPorIds(arquivosAlterados);
            var arquivosExistentesCompletos = await repositorioArquivo.ObterPorIds(arquivosExistentes);
            
            var arquivosIdsInserir = arquivosAlterados.Except(arquivosExistentes);
            var arquivosIdsExcluir = arquivosExistentes.Except(arquivosAlterados);
            
            ArquivosMoverTemporario = arquivosAlteradosCompletos.Where(w => arquivosIdsInserir.Contains(w.Id)).Select(s => s);
            ArquivosExcluirArmazenamento = arquivosExistentesCompletos.Where(w => arquivosIdsExcluir.Contains(w.Id)).Select(s => s);
            
            return (arquivosIdsInserir,arquivosIdsExcluir);
        }
        
        public async Task<bool> Excluir(long id)
        {
            await repositorioAcervo.Remover(id);
            return true;
        }

        public async Task<string> ArmazenarImagemCapaDocumento(string capaDocumentoBase64)
        {
            if (capaDocumentoBase64.NaoEstaPreenchido()) return capaDocumentoBase64;
            var anexoInfo = capaDocumentoBase64.ObterContentTypeBase64EExtension();
            var bytes = Convert.FromBase64String(anexoInfo.base64Data);
            var stream = new MemoryStream(bytes);
            var nomeArquivo = $"capa_acervo_{Guid.NewGuid()}.{anexoInfo.extension}";
            await servicoArmazenamento.Armazenar(nomeArquivo, stream, anexoInfo.contentType);
            return nomeArquivo;
        }
    }
}