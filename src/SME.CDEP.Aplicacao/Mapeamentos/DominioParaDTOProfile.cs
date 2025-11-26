using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Dtos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SME.CDEP.Aplicacao.Mapeamentos
{
    [ExcludeFromCodeCoverage]
    public class DominioParaDTOProfile : Profile
    {
        public DominioParaDTOProfile()
        {
            var culturaBrasil = new CultureInfo("pt-BR");
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

            CreateMap<AcervoFotograficoCompleto, AcervoFotograficoDTO>().ReverseMap();

            CreateMap<AuditoriaDTO, AcervoFotograficoCompleto>().ReverseMap();
            CreateMap<ArquivoResumidoDTO, ArquivoResumido>().ReverseMap();

            CreateMap<AcervoFotograficoCadastroDTO, AcervoFotografico>().ReverseMap();
            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoFotografico>().ReverseMap();

            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoFotograficoDTO>().ReverseMap();

            CreateMap<AcervoFotograficoCadastroDTO, Acervo>().ReverseMap();
            CreateMap<AcervoFotograficoAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();

            CreateMap<AcervoArteGraficaCadastroDTO, AcervoArteGrafica>().ReverseMap();

            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoArteGrafica>().ReverseMap();

            CreateMap<AcervoArteGraficaAlteracaoDTO, AcervoArteGraficaDTO>().ReverseMap();
            CreateMap<AcervoArteGraficaCadastroDTO, Acervo>().ReverseMap();

            CreateMap<AcervoArteGraficaCompleto, AcervoArteGraficaDTO>().ReverseMap();

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

            CreateMap<AcervoTridimensionalCompleto, AcervoTridimensionalDTO>().ReverseMap();

            CreateMap<AcervoTridimensionalCompleto, AuditoriaDTO>().ReverseMap();
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

            CreateMap<AcervoDocumentalCompleto, AcervoDocumentalDTO>().ReverseMap();

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

            CreateMap<AcervoBibliograficoCompleto, AcervoBibliograficoDTO>().ReverseMap();

            CreateMap<AuditoriaDTO, AcervoBibliograficoCompleto>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, Acervo>().ReverseMap();
            CreateMap<AcervoBibliograficoDTO, AcervoBibliografico>().ReverseMap();
            CreateMap<AcervoBibliograficoAlteracaoDTO, AcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.AcervoId))
                .ReverseMap();

            CreateMap<ImportacaoArquivoDTO, ImportacaoArquivo>().ReverseMap();

            CreateMap<IdNomeTipoExcluidoDTO, IdNomeTipoDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => o.Nome.Trim()))
                .ReverseMap();

            CreateMap<IdNomeExcluidoDTO, IdNomeDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => o.Nome.Trim()))
                .ReverseMap();

            CreateMap<IdNomeTipoExcluidoAuditavelDTO, IdNomeTipoDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => o.Nome.Trim()))
                .ReverseMap();

            CreateMap<IdNomeExcluidoAuditavelDTO, IdNomeDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => o.Nome.Trim()))
                .ReverseMap();

            CreateMap<IdNomeTipoDTO, IdNomeDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => o.Nome.Trim()))
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
                .ForMember(dest => dest.EstaDisponivel, opt => opt.MapFrom(o => o.SituacaoSaldo.EstaDisponivel()))
                .ReverseMap();

            CreateMap<Usuario, DadosSolicitanteDTO>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(o => o.TipoUsuario.Descricao()))
                .ForMember(dest => dest.TipoId, opt => opt.MapFrom(o => o.TipoUsuario))
                .ReverseMap();

            CreateMap<AcervoSolicitacaoItemCompleto, AcervoSolicitacaoItemRetornoCadastroDTO>()
                .ForMember(dest => dest.AutoresCreditos,
                    opt => opt.MapFrom(o => o.AutoresCreditos.Select(s => s.Nome).ToArray()))
                .ForMember(dest => dest.SituacaoId, opt => opt.MapFrom(o => o.SituacaoItem))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.SituacaoItem.Descricao()))
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.TipoAtendimento, opt => opt.MapFrom(o => o.TipoAtendimento.Descricao()))
                .ForMember(dest => dest.AlteraDataVisita,
                    opt => opt.MapFrom((o, dest, _, context) =>
                    {
                        var perfilUsuarioLogadoDesabilitaDataVisita = context.Items.ContainsKey(Constantes.PERFIL_USUARIO_LOGADO_DESABILITA_DATA_VISITA)
                                                                     && (bool)context.Items[Constantes.PERFIL_USUARIO_LOGADO_DESABILITA_DATA_VISITA];
                        return o.SituacaoItem.EstaAguardandoVisita() && o.TipoAtendimento.EhAtendimentoPresencial() &&
                        o.SituacaoItem.NaoEstaCancelado() && !perfilUsuarioLogadoDesabilitaDataVisita;
                    }))
                .ForMember(dest => dest.TemControleDisponibilidade, opt => opt.MapFrom(o => o.TipoAcervo.EhAcervoBibliografico()))
                .ForMember(dest => dest.EstaDisponivel, opt => opt.MapFrom(o => o.SituacaoSaldo.EstaDisponivel()))
                .ForMember(dest => dest.SituacaoDisponibilidade, opt => opt.MapFrom(o => o.SituacaoSaldoDescricao()))
                .ReverseMap();

            CreateMap<ArquivoCodigoNomeDTO, ArquivoCodigoNomeAcervoId>().ReverseMap();
            CreateMap<AcervoSolicitacaoItemCadastroDTO, AcervoSolicitacaoItem>().ReverseMap();

            CreateMap<AcervoTipoTituloAcervoIdCreditosAutores, AcervoTipoTituloAcervoIdCreditosAutoresDTO>()
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.AutoresCreditos, opt => opt.MapFrom(o => o.AutoresCreditos.Select(s => s.Nome).ToArray()))
                .ForMember(dest => dest.SituacaoDisponibilidade, opt => opt.MapFrom(o => o.SituacaoSaldo.EstaDisponivel() ? Constantes.ACERVO_DISPONIVEL : Constantes.ACERVO_INDISPONIVEL))
                .ForMember(dest => dest.TemControleDisponibilidade, opt => opt.MapFrom(o => o.TipoAcervo.EhAcervoBibliografico()))
                .ForMember(dest => dest.EstaDisponivel, opt => opt.MapFrom(o => o.SituacaoSaldo.EstaDisponivel()))
                .ForMember(dest => dest.TipoAcervoId, opt => opt.MapFrom(o => o.TipoAcervo))
                .ReverseMap();

            CreateMap<AcervoSolicitacaoItemResumido, MinhaSolicitacaoDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.Situacao.Descricao()))
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(o => o.DataCriacao.ToString("dd/MM HH:mm")))
                .ForMember(dest => dest.DataVisita, opt => opt.MapFrom(o => o.DataVisita.HasValue ? o.DataVisita.Value.ToString("dd/MM HH:mm") : string.Empty))
                .ReverseMap();

            CreateMap<AcervoSolicitacaoItemDetalhe, SolicitacaoDTO>()
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.Situacao.Descricao()))
                .ForMember(dest => dest.SituacaoEmprestimo, opt => opt.MapFrom(o => o.SituacaoEmprestimo.HasValue ? o.SituacaoEmprestimo.Descricao() : string.Empty))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(o => o.DataCriacao.ToString("dd/MM HH:mm")))
                .ForMember(dest => dest.DataVisita, opt => opt.MapFrom(o => o.DataVisita.HasValue ? o.DataVisita.Value.ToString("dd/MM HH:mm") : string.Empty))
                .ReverseMap();

            CreateMap<UsuarioExternoDTO, Usuario>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(o => o.Cpf))
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(o => o.Tipo))
                .ReverseMap();

            CreateMap<AcervoSolicitacaoDetalhe, AcervoSolicitacaoDetalheDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.Situacao.Descricao()))
                .ForMember(dest => dest.SituacaoId, opt => opt.MapFrom(o => o.Situacao))
                .ForMember(dest => dest.DataSolicitacaoFormatado, opt => opt.MapFrom(o => o.DataSolicitacao.ToString("dd/MM HH:mm")))
                .ReverseMap();

            CreateMap<AcervoSolicitacaoItemDetalheResumido, AcervoSolicitacaoItemDetalheResumidoDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(o => o.Situacao.Descricao()))
                .ForMember(dest => dest.TipoAcervo, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.TipoAcervoId, opt => opt.MapFrom(o => o.TipoAcervo))
                .ForMember(dest => dest.SituacaoId, opt => opt.MapFrom(o => o.Situacao))
                .ForMember(dest => dest.TemControleDisponibilidade, opt => opt.MapFrom(o => o.TipoAcervo.EhAcervoBibliografico()))
                .ForMember(dest => dest.EstaDisponivel, opt => opt.MapFrom(o => o.SituacaoSaldo.EstaDisponivel()))
                .ForMember(dest => dest.SituacaoDisponibilidade, opt => opt.MapFrom(o => o.SituacaoSaldoDescricao()))
                .ForMember(dest => dest.DataVisitaFormatada, opt => opt.MapFrom(o => o.DataVisita.HasValue ? o.DataVisita.Value.ToString("dd/MM HH:mm") : string.Empty))
                .ForMember(dest => dest.DataEmprestimoFormatada, opt => opt.MapFrom(o => o.DataEmprestimo.HasValue ? o.DataEmprestimo.Value.ToString("dd/MM HH:mm") : string.Empty))
                .ForMember(dest => dest.DataDevolucaoFormatada, opt => opt.MapFrom(o => o.DataDevolucao.HasValue ? o.DataDevolucao.Value.ToString("dd/MM HH:mm") : string.Empty))
                .ForMember(dest => dest.PodeFinalizarItem, opt => opt.MapFrom(o => o.TipoAtendimento.EhAtendimentoPresencial() && o.DataVisita.HasValue && o.TipoAcervo.NaoEhAcervoBibliografico() && o.DataVisita.NaoEhDataFutura()))
                .ReverseMap();

            CreateMap<Acervo, IdNomeCodigoTipoDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => o.Titulo))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(o => o.TipoAcervoId))
                .ReverseMap();

            CreateMap<Acervo, IdNomeCodigoTipoParaEmprestimoDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => o.Titulo))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(o => o.TipoAcervoId))
                .ForMember(dest => dest.TemControleDisponibilidade, opt => opt.MapFrom(o => ((TipoAcervo)o.TipoAcervoId).EhAcervoBibliografico()))
                .ForMember(dest => dest.EstaDisponivel, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.SituacaoDisponibilidade, opt => opt.MapFrom(o => Constantes.ACERVO_DISPONIVEL))
                .ReverseMap();

            CreateMap<AcervoSolicitacao, AcervoSolicitacaoManualDTO>().ReverseMap();
            CreateMap<AcervoSolicitacaoItem, AcervoSolicitacaoItemManualDTO>().ReverseMap();

            CreateMap<AcervoSolicitacaoItemManualDTO, AcervoSolicitacaoItem>().ReverseMap();

            CreateMap<DiaMesDTO, EventoCadastroDTO>().ReverseMap();

            CreateMap<EventoCadastroDTO, Evento>().ReverseMap();

            CreateMap<Evento, EventoTagDTO>()
                .ForMember(dest => dest.TipoId, opt => opt.MapFrom(o => o.Tipo))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(o => o.Tipo.Descricao()))
                .ReverseMap();

            CreateMap<Evento, EventoDTO>().ReverseMap();

            CreateMap<EventoDetalhe, EventoDetalheDTO>()
                .ForMember(dest => dest.TipoId, opt => opt.MapFrom(o => o.Tipo))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(o => o.Tipo.Descricao()))
                .ForMember(dest => dest.SituacaoSolicitacaoItemDescricao, opt => opt.MapFrom(o => o.SituacaoSolicitacaoItem > 0 ? o.SituacaoSolicitacaoItem.Descricao() : string.Empty))
                .ForMember(dest => dest.SituacaoSolicitacaoItemId, opt => opt.MapFrom(o => o.SituacaoSolicitacaoItem))
                .ForMember(dest => dest.Horario, opt => opt.MapFrom(o => o.Data.ToString("HH:mm")))
                .ReverseMap();

            CreateMap<Paginacao, PaginacaoDto>()
                .ForMember(dest => dest.OrdenacaoDto, opt => opt.MapFrom(o =>
                    o.Ordenacao == Enumerados.TipoOrdenacao.AZ || o.Ordenacao == Enumerados.TipoOrdenacao.ZA
                        ? TipoOrdenacaoDto.TITULO :
                    o.Ordenacao == Enumerados.TipoOrdenacao.CODIGO_ASCENDENTE || o.Ordenacao == Enumerados.TipoOrdenacao.CODIGO_DESCENDENTE
                        ? TipoOrdenacaoDto.CODIGO :
                    TipoOrdenacaoDto.DATA))
                .ForMember(dest => dest.DirecaoOrdenacaoDto, opt => opt.MapFrom(o =>
                   o.Ordenacao == Enumerados.TipoOrdenacao.ZA || o.Ordenacao == Enumerados.TipoOrdenacao.CODIGO_DESCENDENTE
                        ? DirecaoOrdenacaoDto.DESC
                        : DirecaoOrdenacaoDto.ASC))
                .ReverseMap();

            CreateMap<HistoricoConsultaAcervo, HistoricoConsultaAcervoDto>().ReverseMap();

            CreateMap<PainelGerencialAcervosCadastrados, PainelGerencialAcervosCadastradosDto>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => o.TipoAcervo.Descricao()))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(o => o.Quantidade))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.TipoAcervo))
                .ReverseMap();

            CreateMap<SumarioConsultaMensal, PainelGerencialQuantidadePesquisasMensaisDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.MesReferencia.Month))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => culturaBrasil.TextInfo.ToTitleCase(o.MesReferencia.ToString("MMMM", culturaBrasil))))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(o => o.TotalConsultas));

            CreateMap<PainelGerencialQuantidadeSolicitacaoMensal, PainelGerencialQuantidadeSolicitacaoMensalDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.MesReferencia.Month))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(o => culturaBrasil.TextInfo.ToTitleCase(o.MesReferencia.ToString("MMMM", culturaBrasil))))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(o => o.TotalSolicitacoes));
        }
    }
}
