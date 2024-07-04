using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Extensions;
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
        
        public async Task<ArquivoArmazenadoDTO> Armazenar(IFormFile formFile, TipoArquivo tipoArquivo)
        {
            var path = string.Empty;
            
            var stream = formFile.OpenReadStream();
            
            var codigo = Guid.NewGuid();
            
            var nomeArquivo = $"{codigo}{Path.GetExtension(formFile.FileName)}";

            if (formFile.ContentType.EhImagemTiff())
            {
                nomeArquivo = $"{codigo.ToString()}.jpeg";
                
                Bitmap imagem = new Bitmap(stream);
                
                ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L); 
                
                using(MemoryStream ms = new MemoryStream())
                {
                    imagem.Save(ms, jpegCodec, encoderParameters);
                    ms.Position = 0;
                    path = await Armazenar(tipoArquivo,nomeArquivo, ms, Constantes.CONTENT_TYPE_JPEG);
                    
                    return new ArquivoArmazenadoDTO(path,codigo, nomeArquivo, Constantes.CONTENT_TYPE_JPEG, tipoArquivo);
                }
            }

            path = await Armazenar(tipoArquivo, nomeArquivo, stream,formFile.ContentType);
            
            return new ArquivoArmazenadoDTO(path,codigo, formFile.FileName, formFile.ContentType, tipoArquivo);
        }

        private async Task<string> Armazenar(TipoArquivo tipoArquivo, string nomeArquivo, Stream stream, string contentType)
        {
            if (tipoArquivo == TipoArquivo.Temp || tipoArquivo == TipoArquivo.Editor)
                return await servicoArmazenamento.ArmazenarTemporaria(nomeArquivo, stream, contentType);

            return await servicoArmazenamento.Armazenar(nomeArquivo, stream, contentType);
        }

        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}  