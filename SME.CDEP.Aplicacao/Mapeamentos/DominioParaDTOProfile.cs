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
            CreateMap<BaseComNomeDTO, AcessoDocumento>().ReverseMap();
            CreateMap<BaseComNomeDTO, Conservacao>().ReverseMap();
            CreateMap<BaseComNomeDTO, Cromia>().ReverseMap();
            CreateMap<BaseComNomeTipoDto, Formato>().ReverseMap();
            CreateMap<BaseComNomeDTO, Idioma>().ReverseMap();
            CreateMap<BaseComNomeTipoDto, Material>().ReverseMap();
            CreateMap<BaseComNomeTipoDto, Suporte>().ReverseMap();
            CreateMap<BaseComNomeDTO, TipoAnexo>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Credito>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Autor>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Editora>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Assunto>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, SerieColecao>().ReverseMap();
        }
    }
}
