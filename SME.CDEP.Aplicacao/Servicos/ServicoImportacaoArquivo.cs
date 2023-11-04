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
        private readonly IMapper mapper;
        private readonly IRepositorioMaterial repositorioMaterial;
        private readonly IRepositorioEditora repositorioEditora;
        private readonly IRepositorioSerieColecao repositorioSerieColecao;
        private readonly IRepositorioIdioma repositorioIdioma;
        private readonly IRepositorioAssunto repositorioAssunto;
        private readonly IRepositorioCreditoAutor repositorioCreditoAutor;
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico;

        public ServicoImportacaoArquivo(IRepositorioImportacaoArquivo repositorioImportacaoArquivo,
            IMapper mapper, IRepositorioMaterial repositorioMaterial, IRepositorioEditora repositorioEditora,
            IRepositorioSerieColecao repositorioSerieColecao, IRepositorioIdioma repositorioIdioma, 
            IRepositorioAssunto repositorioAssunto, IRepositorioCreditoAutor repositorioCreditoAutor,IServicoAcervoBibliografico servicoAcervoBibliografico)
        {
            this.repositorioImportacaoArquivo = repositorioImportacaoArquivo ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.repositorioImportacaoArquivo = repositorioImportacaoArquivo ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
            this.repositorioMaterial = repositorioMaterial ?? throw new ArgumentNullException(nameof(repositorioMaterial));
            this.repositorioEditora = repositorioEditora ?? throw new ArgumentNullException(nameof(repositorioEditora));
            this.repositorioSerieColecao = repositorioSerieColecao ?? throw new ArgumentNullException(nameof(repositorioSerieColecao));
            this.repositorioIdioma = repositorioIdioma ?? throw new ArgumentNullException(nameof(repositorioIdioma));
            this.repositorioAssunto = repositorioAssunto ?? throw new ArgumentNullException(nameof(repositorioAssunto));
            this.repositorioCreditoAutor = repositorioCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioCreditoAutor));
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

        public async Task<bool> UploadPorTipoAcervo(IFormFile file, TipoAcervo tipoAcervo)
        {
            if (file == null || file.Length == 0)
                throw new NegocioException(MensagemNegocio.ARQUIVO_VAZIO);

            if (file.ContentType.NaoEhArquivoXlsx())
                throw new NegocioException(MensagemNegocio.SOMENTE_ARQUIVO_XLSX_SUPORTADO);

            switch (tipoAcervo)
            {
                case TipoAcervo.Bibliografico:
                    return await new ServicoImportacaoArquivoAcervoBibliografico(repositorioImportacaoArquivo,mapper, repositorioMaterial, repositorioEditora,
                        repositorioSerieColecao, repositorioIdioma, repositorioAssunto, repositorioCreditoAutor,servicoAcervoBibliografico).Processar(file,tipoAcervo);
                
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
            return true;
        }
    }
}