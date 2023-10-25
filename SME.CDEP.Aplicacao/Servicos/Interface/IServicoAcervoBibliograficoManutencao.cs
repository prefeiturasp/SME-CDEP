using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoBibliograficoManutencao
    {
        Task<long> Inserir(AcervoBibliograficoCadastroDTO acervoBibliograficoCadastroDto);
        Task<IEnumerable<AcervoBibliograficoDTO>> ObterTodos();
        Task<AcervoBibliograficoDTO> Alterar(AcervoBibliograficoAlteracaoDTO acervoBibliograficoAlteracaoDto);
        Task<AcervoBibliograficoDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
