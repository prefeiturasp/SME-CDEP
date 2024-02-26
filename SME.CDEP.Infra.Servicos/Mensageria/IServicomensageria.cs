﻿namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public interface IServicoMensageria
    {
        Task Enviar(string mensagem, string rota, string exchange);
        Task NotificarCancelamentoAtendimento(long acervoSolicitacaoId);
        Task NotificarCancelamentoItemAtendimento(long acervoSolicitacaoItemId);
    }
}
