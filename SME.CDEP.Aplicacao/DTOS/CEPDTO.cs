using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class CEPDTO
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
    }
}
