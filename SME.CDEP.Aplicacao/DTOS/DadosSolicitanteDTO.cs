using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class DadosSolicitanteDTO
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public TipoUsuario TipoId { get; set; }

        public string ObterEnderecoCompleto(int numero, string complemento, string cidade, string estado, string cep)
        {
            if (numero.EhMaiorQueZero())
                Endereco += $", {numero}";
            
            if (complemento.EstaPreenchido())
                Endereco += $" - {complemento}";
            
            if (cidade.EstaPreenchido())
                Endereco += $" - {cidade}";
            
            if (estado.EstaPreenchido())
                Endereco += $"/{estado}";
            
            if (cep.EstaPreenchido())
                Endereco += $" - {cep}";

            return Endereco;    
        }
    }
}