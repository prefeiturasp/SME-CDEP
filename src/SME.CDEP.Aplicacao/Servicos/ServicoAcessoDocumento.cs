using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcessoDocumento : ServicoAplicacao<AcessoDocumento, IdNomeExcluidoDTO>,IServicoAcessoDocumento
    {
        private readonly IRepositorioAcessoDocumento repositorioAcessoDocumento;

        public ServicoAcessoDocumento(IRepositorioAcessoDocumento repositorioAcessoDocumento, IMapper mapper) : base(repositorioAcessoDocumento,
            mapper)
        {
            this.repositorioAcessoDocumento = repositorioAcessoDocumento ?? throw new ArgumentNullException(nameof(repositorioAcessoDocumento));
        }

        public Task<long> ObterPorNome(string nome)
        {
            return repositorioAcessoDocumento.ObterPorNome(nome);
        }
    }
}