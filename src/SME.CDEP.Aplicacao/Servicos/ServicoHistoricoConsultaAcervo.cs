using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoHistoricoConsultaAcervo(
        IRepositorioHistoricoConsultaAcervo repositorioHistoricoConsultaAcervo,
        IMapper mapper) : IServicoHistoricoConsultaAcervo
    {
        public async Task<HistoricoConsultaAcervoDto> InserirAsync(HistoricoConsultaAcervoDto historicoConsultaAcervo)
        {
            var entidade = mapper.Map<HistoricoConsultaAcervo>(historicoConsultaAcervo);
            entidade.Id = await repositorioHistoricoConsultaAcervo.Inserir(entidade);
            return mapper.Map<HistoricoConsultaAcervoDto>(entidade);
        }
    }
}
