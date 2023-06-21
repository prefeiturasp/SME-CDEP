using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Dominios;

namespace SME.CDEP.Aplicacao.Mapeamentos
{
    public class DominioParaDTOProfile : Profile
    {
        public DominioParaDTOProfile()
        {
            CreateMap<RetornoUsuarioDTO, Usuario>().ReverseMap();
            CreateMap<UsuarioDTO, Usuario>().ReverseMap();
        }
    }
}
