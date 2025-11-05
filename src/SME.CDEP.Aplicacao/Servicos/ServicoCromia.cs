using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoCromia  : ServicoAplicacao<Cromia, IdNomeExcluidoDTO>,IServicoCromia
    {
        private readonly IRepositorioCromia repositorio;
        public ServicoCromia(IRepositorioCromia repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<long> ObterPorNome(string nome)
        {
            return repositorio.ObterPorNome(nome);
        }
    }
}
