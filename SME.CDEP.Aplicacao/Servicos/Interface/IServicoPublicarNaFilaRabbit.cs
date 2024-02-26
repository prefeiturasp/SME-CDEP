using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoPublicarNaFilaRabbit 
    {
        Task<bool> Publicar(string rota, object filtros, Guid? codigoCorrelacao = null, Usuario usuarioLogado = null,
            bool notificarErroUsuario = false, string exchange = null);
    }
}
