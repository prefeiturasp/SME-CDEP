using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoCadastroDTO 
{
    [Required(ErrorMessage = "É necessário informar o identificador do usuário para solicitação do acervo")]
    public long UsuarioId { get; set; }
    public AcervoSolicitacaoItemCadastroDTO[] Itens { get; set; }
}