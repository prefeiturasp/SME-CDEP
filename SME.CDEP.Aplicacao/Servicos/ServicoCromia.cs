using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoCromia : IServicoCromia
    {
        private readonly IRepositorioCromia repositorioCromia;
        private readonly IMapper mapper;
        
        public ServicoCromia(IRepositorioCromia repositorioCromia, IMapper mapper) 
        {
            this.repositorioCromia = repositorioCromia ?? throw new ArgumentNullException(nameof(repositorioCromia));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(CromiaDTO cromiaDTO)
        {
            var cromia = mapper.Map<Cromia>(cromiaDTO);
            return await repositorioCromia.Inserir(cromia);
        }

        public async Task<IList<CromiaDTO>> ObterTodos()
        {
            return (await repositorioCromia.ObterTodos()).ToList().Where(w=> !w.Excluido).Select(s=> mapper.Map<CromiaDTO>(s)).ToList();
        }

        public async Task<CromiaDTO> Alterar(CromiaDTO cromiaDTO)
        {
            var cromia = mapper.Map<Cromia>(cromiaDTO);
            return mapper.Map<CromiaDTO>(await repositorioCromia.Atualizar(cromia));
        }

        public async Task<CromiaDTO> ObterPorId(long cromiaId)
        {
            return mapper.Map<CromiaDTO>(await repositorioCromia.ObterPorId(cromiaId));
        }

        public async Task<bool> Excluir(long cromiaId)
        {
            var cromia = await ObterPorId(cromiaId);
            cromia.Excluido = true;
            await Alterar(cromia);
            return true;
        }
    }
}
