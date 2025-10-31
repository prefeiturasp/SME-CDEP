using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoFormato : ServicoAplicacao<Formato, IdNomeTipoExcluidoDTO>,IServicoFormato
    {
        private readonly IRepositorioFormato repositorio;

        public ServicoFormato(IRepositorioFormato repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<long> ObterPorNomeETipo(string nome, int tipoSuporte)
        {
            return repositorio.ObterPorNomeETipo(nome, tipoSuporte);
        }
    }
}