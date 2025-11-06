using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoHistoricoConsultaAcervo : IServicoAplicacao
    {
        Task<HistoricoConsultaAcervoDto> InserirAsync(HistoricoConsultaAcervoDto historicoConsultaAcervo);
    }
}