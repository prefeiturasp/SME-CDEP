using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SME.CDEP.Infra.Dominio.Enumerados
{
    public static class EnumExtensao
    {
        public static TAttribute ObterAtributo<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }

        public static string Descricao(this Enum enumValue)
            => enumValue.ObterAtributo<DisplayAttribute>().Description;
    }
}