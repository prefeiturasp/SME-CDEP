using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoMoverArquivoTemporario
    {
        Task Mover(TipoArquivo tipoArquivo, Arquivo arquivo);
    }
}
