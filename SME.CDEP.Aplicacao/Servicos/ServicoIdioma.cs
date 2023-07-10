using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoIdioma : IServicoIdioma
    {
        private readonly IRepositorioIdioma repositorioIdioma;
        private readonly IMapper mapper;
        
        public ServicoIdioma(IRepositorioIdioma repositorioIdioma, IMapper mapper) 
        {
            this.repositorioIdioma = repositorioIdioma ?? throw new ArgumentNullException(nameof(repositorioIdioma));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(IdiomaDTO idiomaDTO)
        {
            var idioma = mapper.Map<Idioma>(idiomaDTO);
            return await repositorioIdioma.Inserir(idioma);
        }

        public async Task<IList<IdiomaDTO>> ObterTodos()
        {
            return (await repositorioIdioma.ObterTodos()).ToList().Where(w=> !w.Excluido).Select(s=> mapper.Map<IdiomaDTO>(s)).ToList();
        }

        public async Task<IdiomaDTO> Alterar(IdiomaDTO idiomaDTO)
        {
            var idioma = mapper.Map<Idioma>(idiomaDTO);
            return mapper.Map<IdiomaDTO>(await repositorioIdioma.Atualizar(idioma));
        }

        public async Task<IdiomaDTO> ObterPorId(long idiomaId)
        {
            return mapper.Map<IdiomaDTO>(await repositorioIdioma.ObterPorId(idiomaId));
        }

        public async Task<bool> Excluir(long idiomaId)
        {
            var idioma = await ObterPorId(idiomaId);
            idioma.Excluido = true;
            await Alterar(idioma);
            return true;
        }
    }
}
