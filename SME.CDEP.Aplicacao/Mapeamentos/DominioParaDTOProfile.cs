using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Mapeamentos
{
    public class DominioParaDTOProfile : Profile
    {
        public DominioParaDTOProfile()
        {
            CreateMap<UsuarioDTO, Usuario>().ReverseMap();
            CreateMap<UsuarioIdNomeLoginDTO, Usuario>().ReverseMap();
            CreateMap<AcessoDocumentoDTO, AcessoDocumento>().ReverseMap();
            CreateMap<ConservacaoDTO, Conservacao>().ReverseMap();
            CreateMap<CromiaDTO, Cromia>().ReverseMap();
            CreateMap<FormatoDTO, Formato>().ReverseMap();
            CreateMap<IdiomaDTO, Idioma>().ReverseMap();
            CreateMap<MaterialDTO, Material>().ReverseMap();
            CreateMap<SuporteDTO, Suporte>().ReverseMap();
            CreateMap<TipoAnexoDTO, TipoAnexo>().ReverseMap();
        }
    }
}
