using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

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
            
            CreateMap<AcervoFotograficoCompleto,AcervoFotograficoDTO>().ReverseMap();
            
            CreateMap<AuditoriaDTO, AcervoFotograficoCompleto>().ReverseMap();
            CreateMap<ArquivoResumidoDTO, ArquivoResumido>().ReverseMap();
            
            CreateMap<AcervoFotograficoCadastroDTO, AcervoFotografico>().ReverseMap();
            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoFotografico>().ReverseMap();
            
            CreateMap<AcervoFotograficoAlteracaoDTO,AcervoFotograficoDTO>().ReverseMap();
            
            CreateMap<AcervoFotograficoCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<AcervoArteGraficaCadastroDTO, AcervoArteGrafica>().ReverseMap();
            
            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoArteGrafica>().ReverseMap();
            
            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoArteGraficaDTO>().ReverseMap();
            CreateMap<AcervoArteGraficaCadastroDTO, Acervo>().ReverseMap();
            
            CreateMap<AcervoArteGraficaCompleto,AcervoArteGraficaDTO>().ReverseMap();
                
            CreateMap<AuditoriaDTO, AcervoArteGraficaCompleto>().ReverseMap();
            CreateMap<AcervoArteGraficaDTO, Acervo>().ReverseMap();
            CreateMap<AcervoArteGraficaDTO, AcervoArteGrafica>().ReverseMap();
            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<AcervoTridimensionalCadastroDTO, AcervoTridimensional>().ReverseMap();

            CreateMap<AcervoTridimensionalAlteracaoDTO, AcervoTridimensional>().ReverseMap();

            CreateMap<AcervoTridimensionalAlteracaoDTO, AcervoTridimensionalDTO>().ReverseMap();
            
            CreateMap<AcervoTridimensionalCadastroDTO, Acervo>().ReverseMap();
            
            CreateMap<AcervoTridimensionalCompleto,AcervoTridimensionalDTO>().ReverseMap();
            
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
            
            CreateMap<AcervoDocumentalCadastroDTO, AcervoDocumental>().ReverseMap();
            
            CreateMap<AcervoDocumentalAlteracaoDTO, AcervoDocumental>().ReverseMap();
            
            CreateMap<AcervoDocumentalAlteracaoDTO, AcervoDocumentalDTO>().ReverseMap();
            CreateMap<AcervoDocumentalCadastroDTO, Acervo>().ReverseMap();
            
            CreateMap<AcervoDocumentalCompleto,AcervoDocumentalDTO>().ReverseMap();
                
            CreateMap<AuditoriaDTO, AcervoDocumentalCompleto>().ReverseMap();
            CreateMap<AcervoDocumentalDTO, Acervo>().ReverseMap();
            CreateMap<AcervoDocumentalDTO, AcervoDocumental>().ReverseMap();
            CreateMap<AcervoDocumentalAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<CoAutorDTO, CoAutor>().ReverseMap();
            
            CreateMap<AcervoBibliograficoCadastroDTO, AcervoBibliografico>().ReverseMap();
            
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoBibliografico>().ReverseMap();
            
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoBibliograficoDTO>().ReverseMap();
            CreateMap<AcervoBibliograficoCadastroDTO, Acervo>().ReverseMap();
            
            CreateMap<AcervoBibliograficoCompleto,AcervoBibliograficoDTO>().ReverseMap();
            
            CreateMap<AuditoriaDTO, AcervoBibliograficoCompleto>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, Acervo>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, AcervoBibliografico>().ReverseMap();
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();
            
            CreateMap<ImportacaoArquivoDTO, ImportacaoArquivo>().ReverseMap();
            
            CreateMap<IdNomeTipoExcluidoDTO, IdNomeTipoDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o=> o.Nome.Trim()))
                .ReverseMap();
            
            CreateMap<IdNomeExcluidoDTO, IdNomeDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o=> o.Nome.Trim()))
                .ReverseMap();
            
            CreateMap<IdNomeTipoExcluidoAuditavelDTO, IdNomeTipoDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o=> o.Nome.Trim()))
                .ReverseMap();
            
            CreateMap<IdNomeExcluidoAuditavelDTO, IdNomeDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o=> o.Nome.Trim()))
                .ReverseMap();
            
            CreateMap<IdNomeTipoDTO, IdNomeDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o=> o.Nome.Trim()))
                .ReverseMap();
            
            CreateMap<ImagemDetalhe, ImagemDTO>().ReverseMap();
            
            CreateMap<AcervoArteGraficaDetalhe, AcervoArteGraficaDetalheDTO>()
                .ForMember(dest => dest.CopiaDigital, opt => opt.MapFrom(o => o.CopiaDigital.ObterSimNao()))
                .ForMember(dest => dest.PermiteUsoImagem, opt => opt.MapFrom(o => o.PermiteUsoImagem.ObterSimNao()))
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Creditos))
                .ReverseMap();
            
            CreateMap<AcervoAudiovisualDetalhe, AcervoAudiovisualDetalheDTO>()
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Creditos))
                .ForMember(dest => dest.PermiteUsoImagem, opt => opt.MapFrom(o => o.PermiteUsoImagem.ObterSimNao()))
                .ReverseMap();
            
            CreateMap<AcervoFotograficoDetalhe, AcervoFotograficoDetalheDTO>()
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Creditos))
                .ForMember(dest => dest.CopiaDigital, opt => opt.MapFrom(o => o.CopiaDigital.ObterSimNaoVazio()))
                .ForMember(dest => dest.PermiteUsoImagem, opt => opt.MapFrom(o => o.PermiteUsoImagem.ObterSimNaoVazio()))
                .ReverseMap();
            
            CreateMap<AcervoTridimensionalDetalhe, AcervoTridimensionalDetalheDTO>()
                .ReverseMap(); 
            
            CreateMap<AcervoDocumentalDetalhe, AcervoDocumentalDetalheDTO>()
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Autores))
                .ForMember(dest => dest.CopiaDigital, opt => opt.MapFrom(o => o.CopiaDigital.ObterSimNaoVazio()))
                .ReverseMap();
            
            CreateMap<AcervoBibliograficoDetalhe, AcervoBibliograficoDetalheDTO>()
                .ForMember(dest => dest.CreditosAutores, opt => opt.MapFrom(o => o.Autores))
                .ForMember(dest => dest.Localizacao, opt => opt.MapFrom(o => $"{o.Localizacaocdd} - {o.Localizacaopha}"))
                .ReverseMap(); 

            CreateMap<Usuario,DadosSolicitanteDTO>()
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(o => o.TipoUsuario.EhCoreSSO() ? null : o.Login))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(o => o.TipoUsuario.Descricao()))
                .ForMember(dest => dest.TipoId, opt => opt.MapFrom(o => o.TipoUsuario))
                .ReverseMap();
            
            CreateMap<AcervoSolicitacaoItemCompleto,AcervoSolicitacaoItemRetornoCadastroDTO>()
                .ForMember(dest => dest.AutoresCreditos, opt => opt.MapFrom(o => o.AutoresCreditos.Select(s=> s.Nome).ToArray()))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.SituacaoItem.Descricao()))
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.TipoAtendimento, opt => opt.MapFrom(o => o.TipoAtendimento.Descricao()))
                .ForMember(dest => dest.AlteraDataVisita, opt => opt.MapFrom(o => o.SituacaoItem.EstaAguardandoVisita() && o.TipoAtendimento.EhAtendimentoPresencial() && o.SituacaoItem.NaoEstaCancelado()))
                .ReverseMap();
            
            CreateMap<ArquivoCodigoNomeDTO,ArquivoCodigoNomeAcervoId>().ReverseMap();
            CreateMap<AcervoSolicitacaoItemCadastroDTO,AcervoSolicitacaoItem>().ReverseMap();
            
            CreateMap<AcervoTipoTituloAcervoIdCreditosAutores,AcervoTipoTituloAcervoIdCreditosAutoresDTO>()
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.AutoresCreditos, opt => opt.MapFrom(o => o.AutoresCreditos.Select(s=> s.Nome).ToArray()))
                .ReverseMap();
            
            CreateMap<AcervoSolicitacaoItemResumido,MinhaSolicitacaoDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.Situacao.Descricao()))
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ReverseMap();
            
            CreateMap<AcervoSolicitacaoItemDetalhe,SolicitacaoDTO>()
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.Situacao.Descricao()))
                .ReverseMap();
            
            CreateMap<UsuarioExternoDTO,Usuario>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(o => o.Cpf))
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(o => o.Tipo))
                .ReverseMap();
            
            CreateMap<AcervoSolicitacaoDetalhe,AcervoSolicitacaoDetalheDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.Situacao.Descricao()))
                .ForMember(dest => dest.SituacaoId, opt => opt.MapFrom(o => o.Situacao))
                .ReverseMap();
            
            CreateMap<AcervoSolicitacaoItemDetalheResumido,AcervoSolicitacaoItemDetalheResumidoDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.Situacao.Descricao()))
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.SituacaoId, opt => opt.MapFrom(o => o.Situacao))
                .ReverseMap();
            
        }
    }
}
