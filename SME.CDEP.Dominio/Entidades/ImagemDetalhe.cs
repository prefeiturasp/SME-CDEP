
namespace SME.CDEP.Dominio.Entidades
{
    public class ImagemDetalhe : IEquatable<ImagemDetalhe>
    {
        public string Original { get; set; }
        public string Thumbnail { get; set; }

        public bool Equals(ImagemDetalhe? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Original == other.Original && Thumbnail == other.Thumbnail;
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
            return HashCode.Combine(Original, Thumbnail);
        }
    }
}
