using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase : IExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase
    {
        private IRepositorioAcervoEmprestimo repositorioAcervoEmprestimo;
        
        public ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase(IRepositorioAcervoEmprestimo repositorioAcervoEmprestimo)
        {
            this.repositorioAcervoEmprestimo = repositorioAcervoEmprestimo ?? throw new ArgumentNullException(nameof(repositorioAcervoEmprestimo));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var itensEmprestadosAtrasados = await repositorioAcervoEmprestimo.ObterItensEmprestadosAtrasados();

            if (itensEmprestadosAtrasados.Any())
            {
                foreach (var itemEmprestadoAtrasado in itensEmprestadosAtrasados)
                {
                    itemEmprestadoAtrasado.DefinirDevoluvaoEmAtraso();
                    await repositorioAcervoEmprestimo.Inserir(itemEmprestadoAtrasado);
                }
            } 
            
            return true;
        }
    }
}