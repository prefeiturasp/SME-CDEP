
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase : IExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase
    {
        private IServicoEvento servicoEvento;
        
        public ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase(IServicoEvento servicoEvento)
        {
            this.servicoEvento = servicoEvento ?? throw new ArgumentNullException(nameof(servicoEvento));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var eventoCadastroDto = param.ObterObjetoMensagem<EventoCadastroDTO>();

            await servicoEvento.Inserir(eventoCadastroDto);
            
            return true;
        }
    }
}