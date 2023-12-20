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
}