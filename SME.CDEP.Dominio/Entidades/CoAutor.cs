
namespace SME.CDEP.Dominio.Entidades
{
    public class CoAutor : IEquatable<CoAutor>
    {
        public long CreditoAutorId { get; set; }
        public string TipoAutoria { get; set; }
        public string CreditoAutorNome { get; set; }

        public bool Equals(CoAutor? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CreditoAutorId == other.CreditoAutorId && TipoAutoria == other.TipoAutoria;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CoAutor)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CreditoAutorId, TipoAutoria);
        }
    }
}
