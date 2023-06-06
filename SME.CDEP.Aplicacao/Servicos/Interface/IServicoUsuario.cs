using SME.CDEP.Dominio.Dominios;
using SME.CDEP.Infra.Dominio.Dtos;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoUsuario
    {
        Task<long> Inserir(UsuarioDto usuarioDto);
        Task<IList<Usuario>> ObterTodos();
        Task<Usuario> Alterar(UsuarioDto usuarioDto);
        Task<Usuario> ObterPorId(long usuarioId);
    }
}
