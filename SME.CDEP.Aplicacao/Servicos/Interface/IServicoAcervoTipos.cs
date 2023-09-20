using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoTipos : IServicoAplicacao, IServicoAcervoManutencao
    {
        IEnumerable<IdNomeDTO> ObterTodosTipos();
    }
}
