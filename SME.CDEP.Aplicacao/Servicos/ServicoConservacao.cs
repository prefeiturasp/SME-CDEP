using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoConservacao : ServicoAplicacao<Conservacao, IdNomeExcluidoDTO>,IServicoConservacao
    {
        private readonly IRepositorioConservacao repositorioConservacao;

        public ServicoConservacao(IRepositorioConservacao repositorioConservacao, IMapper mapper) : base(repositorioConservacao, mapper)
        {
            this.repositorioConservacao = repositorioConservacao ?? throw new ArgumentNullException(nameof(repositorioConservacao));
        }

        public Task<long> ObterPorNome(string nome)
        {
            return repositorioConservacao.ObterPorNome(nome);
        }
    }
}