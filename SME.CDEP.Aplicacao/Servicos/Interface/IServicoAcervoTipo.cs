using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoTipo
    {
        IEnumerable<IdNomeDTO> ObterTodosTipos();
    }
}
