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
            if (enumValue.EhNulo() && !Enum.IsDefined(enumValue.GetType(), enumValue))
                return string.Empty;
            
            return enumValue.ObterAtributo<DisplayAttribute>().Description;
        }
    }
}