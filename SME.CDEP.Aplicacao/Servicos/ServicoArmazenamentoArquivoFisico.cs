using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoArmazenamentoArquivoFisico : IServicoArmazenamentoArquivoFisico
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ServicoArmazenamentoArquivoFisico(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }
        
        public async Task<string> Armazenar(IFormFile formFile, string nomeFisico, TipoArquivo tipoArquivo)
        {
            var enderecoArquivo = string.Empty;

            var stream = formFile.OpenReadStream();
            
            var nomeArquivo = $"{nomeFisico}{Path.GetExtension(formFile.FileName)}";

            if (tipoArquivo == TipoArquivo.Temp || tipoArquivo == TipoArquivo.Editor)
                enderecoArquivo = await servicoArmazenamento.ArmazenarTemporaria(nomeArquivo, stream, formFile.ContentType);
            else
                enderecoArquivo = await servicoArmazenamento.Armazenar(nomeArquivo, stream, formFile.ContentType);
             
            return enderecoArquivo;
        }
    }
}  