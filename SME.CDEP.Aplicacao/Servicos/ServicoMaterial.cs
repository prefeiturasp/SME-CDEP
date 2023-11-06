using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoMaterial : ServicoAplicacao<Material, IdNomeTipoExcluidoDTO>,IServicoMaterial
    {
        private readonly IRepositorioMaterial repositorio;

        public ServicoMaterial(IRepositorioMaterial repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<long> ObterPorNomeTipo(string nome, TipoMaterial tipo)
        {
            return repositorio.ObterPorNomeTipo(nome, tipo);
        }
    }
}
