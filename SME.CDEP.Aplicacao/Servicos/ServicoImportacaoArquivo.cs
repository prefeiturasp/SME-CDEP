using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivo : IServicoImportacaoArquivo
    {
        private readonly IRepositorioImportacaoArquivo repositorioImportacaoArquivo;
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico;
        private readonly IServicoMaterial servicoMaterial;
        private readonly IServicoEditora servicoEditora;
        private readonly IServicoSerieColecao servicoSerieColecao;
        private readonly IServicoIdioma servicoIdioma;
        private readonly IServicoAssunto servicoAssunto;
        private readonly IServicoCreditoAutor servicoCreditoAutor;
        private readonly IMapper mapper;

        public ServicoImportacaoArquivo(IRepositorioImportacaoArquivo repositorioImportacaoArquivo,
            IMapper mapper, IRepositorioMaterial repositorioMaterial, IRepositorioEditora repositorioEditora,
            IRepositorioSerieColecao repositorioSerieColecao, IRepositorioIdioma repositorioIdioma, 
            IRepositorioAssunto repositorioAssunto, IRepositorioCreditoAutor repositorioCreditoAutor,
            IServicoAcervoBibliografico servicoAcervoBibliografico)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
            this.repositorioImportacaoArquivo = repositorioImportacaoArquivo ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
            this.repositorioImportacaoArquivo = repositorioImportacaoArquivo ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
            this.servicoMaterial = servicoMaterial ?? throw new ArgumentNullException(nameof(servicoMaterial));
            this.servicoEditora = servicoEditora ?? throw new ArgumentNullException(nameof(servicoEditora));
            this.servicoSerieColecao = servicoSerieColecao ?? throw new ArgumentNullException(nameof(servicoSerieColecao));
            this.servicoIdioma = servicoIdioma ?? throw new ArgumentNullException(nameof(servicoIdioma));
            this.servicoAssunto = servicoAssunto ?? throw new ArgumentNullException(nameof(servicoAssunto));
            this.servicoCreditoAutor = servicoCreditoAutor ?? throw new ArgumentNullException(nameof(servicoCreditoAutor));
            this.servicoAcervoBibliografico = servicoAcervoBibliografico ?? throw new ArgumentNullException(nameof(servicoAcervoBibliografico));
        }

        public async Task<long> Inserir(ImportacaoArquivo importacaoArquivo)
        {
            return await repositorioImportacaoArquivo.Inserir(importacaoArquivo);
        }

        public async Task<ImportacaoArquivoDTO> Alterar(ImportacaoArquivo importacaoArquivo)
        {
            return mapper.Map<ImportacaoArquivoDTO>(await repositorioImportacaoArquivo.Atualizar(importacaoArquivo));
        }

        public async Task<bool> Excluir(long importacaoArquivoId)
        {
            await repositorioImportacaoArquivo.Remover(importacaoArquivoId);
            return true;
        }

        public async Task<ImportacaoArquivoCompleto> ObterUltimaImportacao()
        {
            return await repositorioImportacaoArquivo.ObterUltimaImportacao();
        }

        public async Task<ImportacaoArquivoDTO> ImportarArquivoPorTipoAcervo(IFormFile file, TipoAcervo tipoAcervo)
        {
            if (file == null || file.Length == 0)
                throw new NegocioException(MensagemNegocio.ARQUIVO_VAZIO);

            if (file.ContentType.NaoEhArquivoXlsx())
                throw new NegocioException(MensagemNegocio.SOMENTE_ARQUIVO_XLSX_SUPORTADO);

            switch (tipoAcervo)
            {
                case TipoAcervo.Bibliografico:
                    return await ObterServicoImportacaoArquivoAcervoBibliografico().ImportarArquivo(file,tipoAcervo);
                
                case TipoAcervo.DocumentacaoHistorica:
                    break;
                case TipoAcervo.ArtesGraficas:
                    break;
                case TipoAcervo.Audiovisual:
                    break;
                case TipoAcervo.Fotografico:
                    break;
                case TipoAcervo.Tridimensional:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoAcervo), tipoAcervo, null);
            }
            return default;
        }

        private ServicoImportacaoArquivoAcervoBibliografico ObterServicoImportacaoArquivoAcervoBibliografico()
        {
            return new ServicoImportacaoArquivoAcervoBibliografico(repositorioImportacaoArquivo, mapper, servicoMaterial,
                servicoEditora, servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor, servicoAcervoBibliografico);
        }
    }
}