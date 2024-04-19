using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class DadosSolicitanteDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public TipoUsuario TipoId { get; set; }

        public string ObterEnderecoCompleto(string numero, string complemento, string cidade, string estado, string cep)
        {
            if (numero.EstaPreenchido())
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