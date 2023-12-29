
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Dominio.Entidades
{
    public class ImagemDetalhe : IEquatable<ImagemDetalhe>
    {
        public string NomeOriginal { get; set; }
        public Guid CodigoOriginal { get; set; }
        public Guid CodigoThumbnail { get; set; }

        public string Original
        {
            get
            {
                return $"{CodigoOriginal}{NomeOriginal.ObterExtensao()}";
            }
        }
        
        public string Thumbnail
        {
            get
            {
                return $"{CodigoThumbnail}{NomeOriginal.ObterExtensao()}";
            }
        }   

        public bool Equals(ImagemDetalhe? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return NomeOriginal == other.NomeOriginal && CodigoOriginal.Equals(other.CodigoOriginal) && CodigoThumbnail.Equals(other.CodigoThumbnail);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ImagemDetalhe)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NomeOriginal, CodigoOriginal, CodigoThumbnail);
        }
    }
}
