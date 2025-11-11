using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoMensageria 
    {
        Task<bool> Publicar(string rota, object filtros, Guid? codigoCorrelacao = null, Usuario? usuarioLogado = null,
            bool notificarErroUsuario = false, string? exchange = null);
    }
}
