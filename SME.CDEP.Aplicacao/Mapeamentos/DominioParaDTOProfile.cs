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
            CreateMap<IdNomeTipoExcluidoDTO, Formato>().ReverseMap();
            CreateMap<IdNomeExcluidoDTO, Idioma>().ReverseMap();
            CreateMap<IdNomeTipoExcluidoDTO, Material>().ReverseMap();
            CreateMap<IdNomeTipoExcluidoDTO, Suporte>().ReverseMap();
            CreateMap<IdNomeTipoExcluidoAuditavelDTO, CreditoAutor>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Editora>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, Assunto>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, SerieColecao>().ReverseMap();
            CreateMap<AcervoDTO, Acervo>().ReverseMap();
            CreateMap<AcervoFotograficoDTO, Acervo>().ReverseMap();
            CreateMap<AcervoFotograficoDTO, AcervoFotografico>().ReverseMap();
            CreateMap<CreditoAutorDTO, CreditoAutor>().ReverseMap();
            CreateMap<AcervoFotograficoDTO, AcervoFotograficoCompleto>().ReverseMap();
            CreateMap<AuditoriaDTO, AcervoFotograficoCompleto>().ReverseMap();
            CreateMap<ArquivoResumidoDTO, ArquivoResumido>().ReverseMap();
            CreateMap<AcervoFotograficoCadastroDTO, AcervoFotografico>().ReverseMap();
            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoFotografico>().ReverseMap();
            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoFotograficoDTO>().ReverseMap();
            CreateMap<AcervoFotograficoCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoArteGraficaCadastroDTO, AcervoArteGrafica>().ReverseMap();
            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoArteGrafica>().ReverseMap();
            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoArteGraficaDTO>().ReverseMap();
            CreateMap<AcervoArteGraficaCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoArteGraficaDTO, AcervoArteGraficaCompleto>().ReverseMap();
            CreateMap<AuditoriaDTO, AcervoArteGraficaCompleto>().ReverseMap();
            CreateMap<AcervoArteGraficaDTO, Acervo>().ReverseMap();
            CreateMap<AcervoArteGraficaDTO, AcervoArteGrafica>().ReverseMap();
            
            CreateMap<AcervoTridimensionalCadastroDTO, AcervoTridimensional>().ReverseMap();
            CreateMap<AcervoTridimensionalAlteracaoDTO, AcervoTridimensional>().ReverseMap();
            CreateMap<AcervoTridimensionalAlteracaoDTO, AcervoTridimensionalDTO>().ReverseMap();
            CreateMap<AcervoTridimensionalCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoTridimensionalDTO, AcervoTridimensionalCompleto>().ReverseMap();
            CreateMap<AuditoriaDTO, AcervoTridimensionalCompleto>().ReverseMap();
            CreateMap<AcervoTridimensionalDTO, Acervo>().ReverseMap();
            CreateMap<AcervoTridimensionalDTO, AcervoTridimensional>().ReverseMap();
            
            CreateMap<AcervoAudiovisualCadastroDTO, AcervoAudiovisual>().ReverseMap();
            CreateMap<AcervoAudiovisualAlteracaoDTO, AcervoAudiovisual>().ReverseMap();
            CreateMap<AcervoAudiovisualAlteracaoDTO, AcervoAudiovisualDTO>().ReverseMap();
            CreateMap<AcervoAudiovisualCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoAudiovisualDTO, AcervoAudiovisualCompleto>().ReverseMap();
            CreateMap<AuditoriaDTO, AcervoAudiovisualCompleto>().ReverseMap();
            CreateMap<AcervoAudiovisualDTO, Acervo>().ReverseMap();
            CreateMap<AcervoAudiovisualDTO, AcervoAudiovisual>().ReverseMap();
            
            CreateMap<AcervoDocumentalCadastroDTO, AcervoDocumental>().ReverseMap();
            CreateMap<AcervoDocumentalAlteracaoDTO, AcervoDocumental>().ReverseMap();
            CreateMap<AcervoDocumentalAlteracaoDTO, AcervoDocumentalDTO>().ReverseMap();
            CreateMap<AcervoDocumentalCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoDocumentalDTO, AcervoDocumentalCompleto>().ReverseMap();
            CreateMap<AuditoriaDTO, AcervoDocumentalCompleto>().ReverseMap();
            CreateMap<AcervoDocumentalDTO, Acervo>().ReverseMap();
            CreateMap<AcervoDocumentalDTO, AcervoDocumental>().ReverseMap();
            
            CreateMap<CoAutorDTO, CoAutor>().ReverseMap();
            
            CreateMap<AcervoBibliograficoCadastroDTO, AcervoBibliografico>().ReverseMap();
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoBibliografico>().ReverseMap();
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoBibliograficoDTO>().ReverseMap();
            CreateMap<AcervoBibliograficoCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, AcervoBibliograficoCompleto>().ReverseMap();
            CreateMap<AuditoriaDTO, AcervoBibliograficoCompleto>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, Acervo>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, AcervoBibliografico>().ReverseMap();
            
            CreateMap<AcervoBibliograficoCompleto, AcervoBibliograficoDTO>()
                .ForMember(dest => dest.CoAutores, opt => opt.MapFrom(o => o.CoAutores))
                .ReverseMap();
        }
    }
}
