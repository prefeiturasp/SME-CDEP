using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoMaterial : IServicoMaterial
    {
        private readonly IRepositorioMaterial repositorioMaterial;
        private readonly IMapper mapper;
        
        public ServicoMaterial(IRepositorioMaterial repositorioMaterial, IMapper mapper) 
        {
            this.repositorioMaterial = repositorioMaterial ?? throw new ArgumentNullException(nameof(repositorioMaterial));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(MaterialDTO materialDTO)
        {
            var material = mapper.Map<Material>(materialDTO);
            return await repositorioMaterial.Inserir(material);
        }

        public async Task<IList<MaterialDTO>> ObterTodos()
        {
            return (await repositorioMaterial.ObterTodos()).ToList().Where(w=> !w.Excluido).Select(s=> mapper.Map<MaterialDTO>(s)).ToList();
        }

        public async Task<MaterialDTO> Alterar(MaterialDTO materialDTO)
        {
            var material = mapper.Map<Material>(materialDTO);
            return mapper.Map<MaterialDTO>(await repositorioMaterial.Atualizar(material));
        }

        public async Task<MaterialDTO> ObterPorId(long materialId)
        {
            return mapper.Map<MaterialDTO>(await repositorioMaterial.ObterPorId(materialId));
        }

        public async Task<bool> Excluir(long materialId)
        {
            var material = await ObterPorId(materialId);
            material.Excluido = true;
            await Alterar(material);
            return true;
        }
    }
}
