using AutoMapper;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados;

namespace SME.CDEP.Aplicacao.Servicos.Fachadas
{
    public record ContextoInfraAcervoSolicitacao(
        ITransacao Transacao,
        IServicoUsuario ServicoUsuario,
        IServicoMensageria ServicoMensageria,
        IMapper Mapper
    );
}
