using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoUploadArquivo : IServicoUploadArquivo
    {
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IServicoArmazenamentoArquivoFisico servicoArmazenamentoArquivoFisico;
        private readonly IContextoAplicacao contextoAplicacao;
        
        public ServicoUploadArquivo(IRepositorioArquivo repositorioArquivo, IContextoAplicacao contextoAplicacao,IServicoArmazenamentoArquivoFisico servicoArmazenamentoArquivoFisico)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.servicoArmazenamentoArquivoFisico = servicoArmazenamentoArquivoFisico ?? throw new ArgumentNullException(nameof(servicoArmazenamentoArquivoFisico));
        }

        public async Task<ArquivoArmazenadoDTO> Upload(IFormFile formFile, TipoArquivo tipoArquivo = TipoArquivo.Temp)
        {
            var arquivoArmazenado = await servicoArmazenamentoArquivoFisico.Armazenar(formFile, tipoArquivo);
            
            var arquivo = new Arquivo()
            {
                Nome = arquivoArmazenado.Nome,
                Codigo = arquivoArmazenado.Codigo,
                Tipo = arquivoArmazenado.TipoArquivo,
                TipoConteudo = arquivoArmazenado.ContentType,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoLogin = contextoAplicacao.UsuarioLogado,
                CriadoPor = contextoAplicacao.NomeUsuario,
            };
            
            arquivo.Id = await repositorioArquivo.SalvarAsync(arquivo);
            
            return new ArquivoArmazenadoDTO(arquivo.Id, arquivo.Codigo,arquivoArmazenado.Path);
        }
    }
}  