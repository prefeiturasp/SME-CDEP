using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class EnderecoTelefoneUsuarioExternoDTO
    {
        public string Login { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string? Complemento { get; set; }
        public int Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
    }
}
