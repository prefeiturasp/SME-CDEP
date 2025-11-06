using AutoMapper;
using Microsoft.Extensions.Logging;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoHistoricoConsultaAcervo(
        IRepositorioHistoricoConsultaAcervo repositorioHistoricoConsultaAcervo,
        IMapper mapper,
        ILogger<ServicoHistoricoConsultaAcervo> logger) : IServicoHistoricoConsultaAcervo
    {
        public async Task<HistoricoConsultaAcervoDto> InserirAsync(HistoricoConsultaAcervoDto historicoConsultaAcervo)
        {
            logger.LogInformation("Inserindo histórico de consulta ao acervo");

            var entidade = mapper.Map<HistoricoConsultaAcervo>(historicoConsultaAcervo);
            entidade.Id = await repositorioHistoricoConsultaAcervo.Inserir(entidade);

            logger.LogInformation("Histórico de consulta ao acervo inserido com sucesso. Id: {Id}", entidade.Id);
            return mapper.Map<HistoricoConsultaAcervoDto>(entidade);
        }
    }
}
