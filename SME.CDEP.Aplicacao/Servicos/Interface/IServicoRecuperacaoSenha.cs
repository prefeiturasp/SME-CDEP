using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoRecuperacaoSenha : IServicoAplicacao
    {
        Task<string> SolicitarRecuperacaoSenha(string login);
        Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token);
        Task<UsuarioAutenticacaoRetornoDTO?> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto);
    }
}
