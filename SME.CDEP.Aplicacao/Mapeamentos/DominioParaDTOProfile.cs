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
            CreateMap<IdNomeExcluidoDTO, AcessoDocumento>().ReverseMap();
            CreateMap<IdNomeExcluidoDTO, Conservacao>().ReverseMap();
            CreateMap<IdNomeExcluidoDTO, Cromia>().ReverseMap();
            CreateMap<IdNomeExcluidoTipoDto, Formato>().ReverseMap();
            CreateMap<IdNomeExcluidoDTO, Idioma>().ReverseMap();
            CreateMap<IdNomeExcluidoTipoDto, Material>().ReverseMap();
            CreateMap<IdNomeExcluidoTipoDto, Suporte>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Credito>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Autor>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Editora>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Assunto>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, SerieColecao>().ReverseMap();
        }
    }
}
