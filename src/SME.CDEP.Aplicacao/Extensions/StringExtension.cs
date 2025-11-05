using System.Drawing.Imaging;
using SME.CDEP.Dominio.Excecoes;

namespace SME.CDEP.Aplicacao.Extensions;

public static class StringExtension
{
    public static ImageFormat ObterFormato(this string formato)
    {
        switch (formato)
        {
            case "image/jpeg":
                return ImageFormat.Jpeg;
            case "image/bmp":
                return ImageFormat.Bmp;
            case "image/emf":
                return ImageFormat.Emf;
            case "image/exif":
                return ImageFormat.Exif;
            case "image/gif":
                return ImageFormat.Gif;
            case "image/icon":
                return ImageFormat.Icon;
            case "image/png":
                return ImageFormat.Png;
            case "image/tiff":
                return ImageFormat.Tiff;
            case "image/wmf":
                return ImageFormat.Wmf;
            default: 
                throw new NegocioException("Formato da imagem não identificado");
        }
    }

    public static (string contentType, string base64Data, string extension) ObterContentTypeBase64EExtension(this string base64String)
    {
        var dataParts = base64String.Split(',');
        if (dataParts.Length != 2 || !dataParts[0].StartsWith("data:") || !dataParts[0].Contains(";base64"))
            throw new NegocioException("String Base64 inválida.");
        var contentType = dataParts[0].Substring(5, dataParts[0].IndexOf(";base64") - 5);
        var base64Data = dataParts[1];
        var extension = contentType.Split('/').Last();
        return (contentType, base64Data, extension);
    }
}