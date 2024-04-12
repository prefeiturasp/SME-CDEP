using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SME.CDEP.Dominio.Extensions;

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

        public static string Descricao(this Enum? enumValue)
        {
            if (enumValue.EhNulo())
                return string.Empty;
            
            return Convert.ToInt32(enumValue) == 0 ? string.Empty : enumValue.ObterAtributo<DisplayAttribute>().Description;
        }
        
        public static string Nome(this Enum enumValue)
            => Convert.ToInt32(enumValue) == 0 ? string.Empty : enumValue.ObterAtributo<DisplayAttribute>().Name;
    }
}