using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class DadosSolicitanteDTO
    {
        public long Id { get; set; }
        public string? Nome { get; set; }
        public string? Login { get; set; }
        public string? Telefone { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Tipo { get; set; }
        public TipoUsuario TipoId { get; set; }

        public string ObterEnderecoCompleto(string? numero, string? complemento, string? cidade, string? estado, string? cep)
        {
            if (!string.IsNullOrWhiteSpace(numero))
                Endereco += $", {numero}";

            if (!string.IsNullOrWhiteSpace(complemento))
                Endereco += $" - {complemento}";

            if (!string.IsNullOrWhiteSpace(cidade))
                Endereco += $" - {cidade}";

            if (!string.IsNullOrWhiteSpace(estado))
                Endereco += $"/{estado}";

            if (!string.IsNullOrWhiteSpace(cep))
                Endereco += $" - {cep}";

            return Endereco;
        }
    }
}