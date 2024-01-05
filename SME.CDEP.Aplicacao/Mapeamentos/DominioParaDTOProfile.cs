using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;

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
            
            CreateMap<AcervoFotograficoCompleto,AcervoFotograficoDTO>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AuditoriaDTO, AcervoFotograficoCompleto>().ReverseMap();
            CreateMap<ArquivoResumidoDTO, ArquivoResumido>().ReverseMap();
            
            CreateMap<AcervoFotograficoCadastroDTO, AcervoFotografico>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoFotografico>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoFotograficoAlteracaoDTO,AcervoFotograficoDTO>().ReverseMap();
            
            CreateMap<AcervoFotograficoCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<AcervoArteGraficaCadastroDTO, AcervoArteGrafica>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Diametro, opt => opt.MapFrom(o => o.Diametro.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoArteGrafica>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Diametro, opt => opt.MapFrom(o => o.Diametro.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoArteGraficaDTO>().ReverseMap();
            CreateMap<AcervoArteGraficaCadastroDTO, Acervo>().ReverseMap();
            
            CreateMap<AcervoArteGraficaCompleto,AcervoArteGraficaDTO>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.Diametro, opt => opt.MapFrom(o => o.Diametro.FormatarDoubleComCasasDecimais()))
                .ReverseMap();
                
            CreateMap<AuditoriaDTO, AcervoArteGraficaCompleto>().ReverseMap();
            CreateMap<AcervoArteGraficaDTO, Acervo>().ReverseMap();
            CreateMap<AcervoArteGraficaDTO, AcervoArteGrafica>().ReverseMap();
            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<AcervoTridimensionalCadastroDTO, AcervoTridimensional>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Diametro, opt => opt.MapFrom(o => o.Diametro.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Profundidade, opt => opt.MapFrom(o => o.Profundidade.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();

            CreateMap<AcervoTridimensionalAlteracaoDTO, AcervoTridimensional>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Diametro, opt => opt.MapFrom(o => o.Diametro.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Profundidade, opt => opt.MapFrom(o => o.Profundidade.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();

            CreateMap<AcervoTridimensionalAlteracaoDTO, AcervoTridimensionalDTO>().ReverseMap();
            
            CreateMap<AcervoTridimensionalCadastroDTO, Acervo>().ReverseMap();
            
            CreateMap<AcervoTridimensionalCompleto,AcervoTridimensionalDTO>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.Diametro, opt => opt.MapFrom(o => o.Diametro.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.Profundidade, opt => opt.MapFrom(o => o.Profundidade.FormatarDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoTridimensionalCompleto,AuditoriaDTO>().ReverseMap();
            CreateMap<AcervoTridimensionalDTO, Acervo>().ReverseMap();
            CreateMap<AcervoTridimensionalDTO, AcervoTridimensional>().ReverseMap();
            CreateMap<AcervoTridimensionalAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<AcervoAudiovisualCadastroDTO, AcervoAudiovisual>().ReverseMap();
            CreateMap<AcervoAudiovisualAlteracaoDTO, AcervoAudiovisual>().ReverseMap();
            CreateMap<AcervoAudiovisualAlteracaoDTO, AcervoAudiovisualDTO>().ReverseMap();
            CreateMap<AcervoAudiovisualCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoAudiovisualDTO, AcervoAudiovisualCompleto>().ReverseMap();
            CreateMap<AuditoriaDTO, AcervoAudiovisualCompleto>().ReverseMap();
            CreateMap<AcervoAudiovisualDTO, Acervo>().ReverseMap();
            CreateMap<AcervoAudiovisualDTO, AcervoAudiovisual>().ReverseMap();
            CreateMap<AcervoAudiovisualAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<AcervoDocumentalCadastroDTO, AcervoDocumental>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoDocumentalAlteracaoDTO, AcervoDocumental>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoDocumentalAlteracaoDTO, AcervoDocumentalDTO>().ReverseMap();
            CreateMap<AcervoDocumentalCadastroDTO, Acervo>().ReverseMap();
            
            CreateMap<AcervoDocumentalCompleto,AcervoDocumentalDTO>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarDoubleComCasasDecimais()))
                .ReverseMap();
                
            CreateMap<AuditoriaDTO, AcervoDocumentalCompleto>().ReverseMap();
            CreateMap<AcervoDocumentalDTO, Acervo>().ReverseMap();
            CreateMap<AcervoDocumentalDTO, AcervoDocumental>().ReverseMap();
            CreateMap<AcervoDocumentalAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<CoAutorDTO, CoAutor>().ReverseMap();
            
            CreateMap<AcervoBibliograficoCadastroDTO, AcervoBibliografico>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoBibliografico>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarParaDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarParaDoubleComCasasDecimais()))
                .ReverseMap();
            
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoBibliograficoDTO>().ReverseMap();
            CreateMap<AcervoBibliograficoCadastroDTO, Acervo>().ReverseMap();
            
            CreateMap<AcervoBibliograficoCompleto,AcervoBibliograficoDTO>()
                .ForMember(dest => dest.Largura, opt => opt.MapFrom(o => o.Largura.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.Altura, opt => opt.MapFrom(o => o.Altura.FormatarDoubleComCasasDecimais()))
                .ForMember(dest => dest.CoAutores, opt => opt.MapFrom(o => o.CoAutores))
                .ReverseMap();
            
            CreateMap<AuditoriaDTO, AcervoBibliograficoCompleto>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, Acervo>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, AcervoBibliografico>().ReverseMap();
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<ImportacaoArquivoDTO, ImportacaoArquivo>().ReverseMap();
            CreateMap<IdNomeTipoExcluidoDTO, IdNomeTipoDTO>().ReverseMap();
            CreateMap<IdNomeExcluidoDTO, IdNomeDTO>().ReverseMap();
            CreateMap<IdNomeTipoExcluidoAuditavelDTO, IdNomeTipoDTO>().ReverseMap();
            CreateMap<IdNomeExcluidoAuditavelDTO, IdNomeDTO>().ReverseMap();
            CreateMap<IdNomeTipoDTO, IdNomeDTO>().ReverseMap();
            CreateMap<ImagemDetalhe, ImagemDTO>().ReverseMap();
            
            CreateMap<AcervoArteGraficaDetalhe, AcervoArteGraficaDetalheDTO>()
                .ForMember(dest => dest.CopiaDigital, opt => opt.MapFrom(o => o.CopiaDigital.ObterSimNao()))
                .ForMember(dest => dest.PermiteUsoImagem, opt => opt.MapFrom(o => o.PermiteUsoImagem.ObterSimNao()))
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Creditos))
                .ForMember(dest => dest.Dimensoes, opt => opt.MapFrom(o => 
                    $"{o.Largura.ToString().ObterValorOuZero()} x {o.Altura.ToString().ObterValorOuZero()} x {o.Diametro.ToString().ObterValorOuZero()}")
                ).ReverseMap();
            
            CreateMap<AcervoAudiovisualDetalhe, AcervoAudiovisualDetalheDTO>()
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Creditos))
                .ForMember(dest => dest.PermiteUsoImagem, opt => opt.MapFrom(o => o.PermiteUsoImagem.ObterSimNao()))
                .ReverseMap();
            
            CreateMap<AcervoFotograficoDetalhe, AcervoFotograficoDetalheDTO>()
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Creditos))
                .ForMember(dest => dest.CopiaDigital, opt => opt.MapFrom(o => o.CopiaDigital.ObterSimNaoVazio()))
                .ForMember(dest => dest.PermiteUsoImagem, opt => opt.MapFrom(o => o.PermiteUsoImagem.ObterSimNaoVazio()))
                .ForMember(dest => dest.Dimensoes, opt => opt.MapFrom(o => 
                    $"{o.Largura.ToString().ObterValorOuZero()} x {o.Altura.ToString().ObterValorOuZero()}")
                ).ReverseMap();
            
            CreateMap<AcervoTridimensionalDetalhe, AcervoTridimensionalDetalheDTO>()
                .ForMember(dest => dest.Dimensoes, opt => opt.MapFrom(o => 
                    $"{o.Largura.ToString().ObterValorOuZero()} x {o.Altura.ToString().ObterValorOuZero()} x {o.Profundidade.ToString().ObterValorOuZero()} x {o.Diametro.ToString().ObterValorOuZero()}")
                    ).ReverseMap(); 
            
            CreateMap<AcervoDocumentalDetalhe, AcervoDocumentalDetalheDTO>()
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Autores))
                .ForMember(dest => dest.CopiaDigital, opt => opt.MapFrom(o => o.CopiaDigital.ObterSimNaoVazio()))
                .ForMember(dest => dest.Dimensoes, opt => opt.MapFrom(o => 
                    $"{o.Largura.ToString().ObterValorOuZero()} x {o.Altura.ToString().ObterValorOuZero()}")
                ).ReverseMap();
            
            CreateMap<AcervoBibliograficoDetalhe, AcervoBibliograficoDetalheDTO>()
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Autores))
                .ForMember(dest => dest.Dimensoes, opt => opt.MapFrom(o => 
                    $"{o.Largura.ToString().ObterValorOuZero()} x {o.Altura.ToString().ObterValorOuZero()}")
                ).ReverseMap(); 
        }
    }
}
