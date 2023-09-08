using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
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
            var arquivo = new Arquivo()
            {
                Nome = formFile.FileName,
                Codigo = Guid.NewGuid(),
                Tipo = tipoArquivo,
                TipoConteudo = formFile.ContentType,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoLogin = contextoAplicacao.UsuarioLogado,
                CriadoPor = contextoAplicacao.NomeUsuario,
            };

            await repositorioArquivo.SalvarAsync(arquivo);
            
            var path = await servicoArmazenamentoArquivoFisico.Armazenar(formFile, arquivo.Codigo.ToString(), tipoArquivo);

            return new ArquivoArmazenadoDTO(arquivo.Id, arquivo.Codigo,path);
        }
    }
}  