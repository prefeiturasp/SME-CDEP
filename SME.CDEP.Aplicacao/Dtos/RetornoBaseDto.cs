using FluentValidation.Results;

namespace SME.CDEP.Aplicacao.Dtos;

   public class RetornoBaseDto
    {
        public RetornoBaseDto(IEnumerable<ValidationFailure> validationFailures)
        {
            if (validationFailures != null && validationFailures.Any())
                Mensagens = validationFailures.Select(c => c.ErrorMessage).ToList();
        }
        public RetornoBaseDto()
        {
            Mensagens = new List<string>();
        }

        public RetornoBaseDto(string mensagem)
        {
            Mensagens = new List<string>() { mensagem };
        }

        public List<string> Mensagens { get; set; }
        public bool ExistemErros => Mensagens?.Any() ?? false;
    }