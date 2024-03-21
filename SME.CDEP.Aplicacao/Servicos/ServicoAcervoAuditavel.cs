using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Extensions;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using Exception = System.Exception;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoAuditavel : IServicoAcervo
    {
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor;
        private readonly IMapper mapper;
        private readonly IContextoAplicacao contextoAplicacao;
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IConfiguration configuration;
        private readonly IRepositorioAcervoBibliografico repositorioAcervoBibliografico;
        private readonly IRepositorioAcervoDocumental repositorioAcervoDocumental;
        private readonly IRepositorioAcervoArteGrafica repositorioAcervoArteGrafica;
        private readonly IRepositorioAcervoAudiovisual repositorioAcervoAudiovisual;
        private readonly IRepositorioAcervoFotografico repositorioAcervoFotografico;
        private readonly IRepositorioAcervoTridimensional repositorioAcervoTridimensional;
        private readonly IRepositorioParametroSistema repositorioParametroSistema;
        
        public ServicoAcervoAuditavel(IRepositorioAcervo repositorioAcervo, IMapper mapper, 
            IContextoAplicacao contextoAplicacao, IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor,
            IRepositorioArquivo repositorioArquivo, IConfiguration configuration,
            IRepositorioAcervoBibliografico repositorioAcervoBibliografico,
            IRepositorioAcervoDocumental repositorioAcervoDocumental,
            IRepositorioAcervoArteGrafica repositorioAcervoArteGrafica,
            IRepositorioAcervoAudiovisual repositorioAcervoAudiovisual,
            IRepositorioAcervoFotografico repositorioAcervoFotografico,
            IRepositorioAcervoTridimensional repositorioAcervoTridimensional,
            IRepositorioParametroSistema repositorioParametroSistema)
        {
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.repositorioAcervoCreditoAutor = repositorioAcervoCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioAcervoCreditoAutor));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioAcervoBibliografico = repositorioAcervoBibliografico ?? throw new ArgumentNullException(nameof(repositorioAcervoBibliografico));
            this.repositorioAcervoDocumental = repositorioAcervoDocumental ?? throw new ArgumentNullException(nameof(repositorioAcervoDocumental));
            this.repositorioAcervoArteGrafica = repositorioAcervoArteGrafica ?? throw new ArgumentNullException(nameof(repositorioAcervoArteGrafica));
            this.repositorioAcervoAudiovisual = repositorioAcervoAudiovisual ?? throw new ArgumentNullException(nameof(repositorioAcervoAudiovisual));
            this.repositorioAcervoFotografico = repositorioAcervoFotografico ?? throw new ArgumentNullException(nameof(repositorioAcervoFotografico));
            this.repositorioAcervoTridimensional = repositorioAcervoTridimensional ?? throw new ArgumentNullException(nameof(repositorioAcervoTridimensional));
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
        }
        
        public async Task<long> Inserir(Acervo acervo)
        {
            ValidarCodigoTomboCodigoNovoAno(acervo);
            
            var tipo = (TipoAcervo)acervo.TipoAcervoId;
            
            await ValidarCodigoTomboCodigoNovoDuplicado(acervo.Codigo, acervo.Id, tipo);
            
            if (acervo.CodigoNovo.EstaPreenchido())
                await ValidarCodigoTomboCodigoNovoDuplicado(acervo.CodigoNovo, acervo.Id, tipo);
            
            if (acervo.CodigoNovo.EstaPreenchido() && acervo.TipoAcervoId.NaoEhAcervoDocumental())
                throw new NegocioException(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO);
                
            acervo.CriadoEm = DateTimeExtension.HorarioBrasilia();
            acervo.CriadoPor = contextoAplicacao.NomeUsuario;
            acervo.CriadoLogin = contextoAplicacao.UsuarioLogado;

            if (!acervo.Ano.EhAnoConformeFormatoABNT())
                throw new NegocioException(MensagemNegocio.O_ANO_NAO_ESTA_SEGUINDO_FORMATO_ABNT);
            
            ObterAnoInicialFinal(acervo);
            
            ValidarAnoFuturo(acervo);
            
            var acervoId = await repositorioAcervo.Inserir(acervo);

            foreach (var creditoAutorId in acervo.CreditosAutoresIds)
                await repositorioAcervoCreditoAutor.Inserir(new AcervoCreditoAutor()
                {
                    AcervoId = acervoId,
                    CreditoAutorId = creditoAutorId
                });
            
            foreach (var coAutor in acervo.CoAutores)
                await repositorioAcervoCreditoAutor.Inserir(new AcervoCreditoAutor()
                {
                    AcervoId = acervoId,
                    CreditoAutorId = coAutor.CreditoAutorId,
                    TipoAutoria = coAutor.TipoAutoria,
                    EhCoAutor = true,
                });

            return acervoId;
        }

        private static void ValidarAnoFuturo(Acervo acervo)
        {
            if (acervo.AnoInicio.EhAnoFuturo())
                throw new NegocioException(MensagemNegocio.NAO_PERMITIDO_ANO_FUTURO);
        }

        private static void ValidarCodigoTomboCodigoNovoAno(Acervo acervo)
        {
            var codigoNaoPreenchido = acervo.Codigo.NaoEstaPreenchido();
            var codigoNovoNaoPreenchido = acervo.CodigoNovo.NaoEstaPreenchido();
            
            if (codigoNaoPreenchido && codigoNovoNaoPreenchido)
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_NAO_INFORMADO, ObterCodigoOuTomboPorTipoAcervo((TipoAcervo)acervo.TipoAcervoId)));
            
            if ((!codigoNaoPreenchido && !codigoNovoNaoPreenchido) && acervo.Codigo.Equals(acervo.CodigoNovo))
                throw new NegocioException(string.Format(MensagemNegocio.REGISTRO_X_DUPLICADO, ObterCodigoOuTomboPorTipoAcervo((TipoAcervo)acervo.TipoAcervoId)));
        }

        private static string ObterCodigoOuTomboPorTipoAcervo(TipoAcervo tipoAcervo)
        {
            switch (tipoAcervo)
            {
                case TipoAcervo.DocumentacaoHistorica:
                    return "Código antigo ou Código novo";
                default:
                    return "Tombo";
            }
        }

        public async Task ValidarCodigoTomboCodigoNovoDuplicado(string codigo, long id, TipoAcervo tipo)
        {
            if (codigo.EstaPreenchido() && await repositorioAcervo.ExisteCodigo(codigo, id, tipo))
                throw new NegocioException(string.Format(MensagemNegocio.REGISTRO_X_DUPLICADO,ObterCodigoOuTomboPorTipoAcervo(tipo)));
        }

        public async Task<IEnumerable<AcervoDTO>> ObterTodos()
        {
            return (await repositorioAcervo.ObterTodos()).Where(w=> !w.Excluido).Select(s=> mapper.Map<AcervoDTO>(s));
        }

        public async Task<AcervoDTO> Alterar(Acervo acervo)
        {
            ValidarCodigoTomboCodigoNovoAno(acervo);

            var tipo = (TipoAcervo)acervo.TipoAcervoId;
            
            await ValidarCodigoTomboCodigoNovoDuplicado(acervo.Codigo, acervo.Id, tipo);

            if (acervo.CodigoNovo.EstaPreenchido())
                await ValidarCodigoTomboCodigoNovoDuplicado(acervo.CodigoNovo, acervo.Id, tipo);
            
            acervo.AlteradoEm = DateTimeExtension.HorarioBrasilia();
            acervo.AlteradoLogin = contextoAplicacao.UsuarioLogado;
            acervo.AlteradoPor = contextoAplicacao.NomeUsuario;
            
            if (!acervo.Ano.EhAnoConformeFormatoABNT())
                throw new NegocioException(MensagemNegocio.O_ANO_NAO_ESTA_SEGUINDO_FORMATO_ABNT);
            
            ObterAnoInicialFinal(acervo);
            
            ValidarAnoFuturo(acervo);

            if (acervo.CodigoNovo.EstaPreenchido() && acervo.TipoAcervoId.NaoEhAcervoDocumental())
                throw new NegocioException(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO);
            
            var acervoAlterado = mapper.Map<AcervoDTO>(await repositorioAcervo.Atualizar(acervo));

            var creditosAutoresPropostos = acervo.CreditosAutoresIds.NaoEhNulo() ? acervo.CreditosAutoresIds : Enumerable.Empty<long>();
            var acervoCreditoAutorAtuais = await repositorioAcervoCreditoAutor.ObterPorAcervoId(acervoAlterado.Id);
            var acervoCreditoAutorAInserir = creditosAutoresPropostos.Select(a => a)
                                         .Except(acervoCreditoAutorAtuais.Select(b => b.CreditoAutorId));
            var creditosAutoresIdsExcluir = acervoCreditoAutorAtuais.Select(a => a.CreditoAutorId)
                                 .Except(creditosAutoresPropostos.Select(b => b)).ToArray();

            foreach (var creditoAutorId in acervoCreditoAutorAInserir)
                await repositorioAcervoCreditoAutor.Inserir(new AcervoCreditoAutor()
                {
                    AcervoId = acervo.Id,
                    CreditoAutorId = creditoAutorId
                });
            
            await repositorioAcervoCreditoAutor.Excluir(creditosAutoresIdsExcluir,acervo.Id);
            
            var coAutoresPropostos = acervo.CoAutores.NaoEhNulo() ? acervo.CoAutores : Enumerable.Empty<CoAutor>();
            var coAutoresAtuais = await repositorioAcervoCreditoAutor.ObterPorAcervoId(acervoAlterado.Id, true);
            var coAutoresAInserir = coAutoresPropostos.Select(a => a).Except(coAutoresAtuais.Select(b => new CoAutor() { CreditoAutorId = b.CreditoAutorId, TipoAutoria = b.TipoAutoria}));
            var coAutoresIdsExcluir = coAutoresAtuais.Select(b => new CoAutor() { CreditoAutorId = b.CreditoAutorId, TipoAutoria = b.TipoAutoria}).Except(coAutoresPropostos.Select(b => b)).ToArray();
            
            foreach (var creditoAutor in coAutoresAInserir)
                await repositorioAcervoCreditoAutor.Inserir(new AcervoCreditoAutor()
                {
                    AcervoId = acervo.Id,
                    CreditoAutorId = creditoAutor.CreditoAutorId,
                    TipoAutoria = creditoAutor.TipoAutoria,
                    EhCoAutor = true,
                });

            foreach (var coAutorExcluir in coAutoresIdsExcluir)
                await repositorioAcervoCreditoAutor.Excluir(coAutorExcluir.CreditoAutorId, coAutorExcluir.TipoAutoria,acervo.Id);
            

            return acervoAlterado;
        }

        private static void ObterAnoInicialFinal(Acervo acervo)
        {
            var ehDecadaOuSeculo = acervo.Ano.ContemDecadaOuSeculoCertoOuPossivel();
            var anoBase = acervo.Ano.ObterAnoNumerico();
            
            acervo.AnoInicio = anoBase;
            acervo.AnoFim = ehDecadaOuSeculo ? anoBase.ObterFimDaDecadaOuSeculo() : anoBase;
        }

        public async Task<AcervoDTO> Alterar(AcervoDTO acervoDTO)
        {
            var acervo = await repositorioAcervo.ObterPorId(acervoDTO.Id);

            if (acervoDTO.CodigoNovo.EstaPreenchido() && acervo.TipoAcervoId != (long)TipoAcervo.DocumentacaoHistorica)
                throw new NegocioException(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO);

            var acervoAlterar = mapper.Map<Acervo>(acervoDTO);
            acervoAlterar.Id = acervo.Id;
            acervoAlterar.TipoAcervoId = acervo.TipoAcervoId;
            acervoAlterar.CriadoEm = acervo.CriadoEm;
            acervoAlterar.CriadoPor = acervo.CriadoPor;
            acervoAlterar.CriadoLogin = acervo.CriadoLogin;
            
            return await Alterar(acervoAlterar);
        }

        public async Task<AcervoDTO> ObterPorId(long acervoId)
        {
            return mapper.Map<AcervoDTO>(await repositorioAcervo.ObterPorId(acervoId));
        }
        
        public async Task<bool> Excluir(long entidaId)
        {
            await repositorioAcervo.Remover(entidaId);
            return true;
        }

        public async Task<PaginacaoResultadoDTO<PesquisaAcervoDTO>> ObterPorTextoLivreETipoAcervo(FiltroTextoLivreTipoAcervoDTO filtroTextoLivreTipoAcervo)
        {
            var paginacao = Paginacao;
            
            var acervos = await repositorioAcervo.ObterPorTextoLivreETipoAcervo(filtroTextoLivreTipoAcervo.TextoLivre, filtroTextoLivreTipoAcervo.TipoAcervo, filtroTextoLivreTipoAcervo.AnoInicial, filtroTextoLivreTipoAcervo.AnoFinal);

            if (acervos.Any())
            {
                var acervosIds = acervos.Where(w=> w.Tipo.EhAcervoArteGraficaOuFotograficoOuTridimensional()).Select(s => s.AcervoId).Distinct().ToArray();

                var miniaturasDosAcervos = await repositorioArquivo.ObterAcervoCodigoNomeArquivoPorAcervoId(acervosIds);

                var imagensPadrao = await ObterImagensPadrao();

                var hostAplicacao = configuration["UrlFrontEnd"];
            
                var acervosAgrupandoCreditoAutor = acervos
                    .GroupBy(g => new { g.AcervoId,g.Codigo, g.Titulo, g.Tipo, g.Descricao, g.TipoAcervoTag, g.DataAcervo, g.Ano })
                    .Select(s => new PesquisaAcervoDTO
                    {
                        AcervoId = s.Key.AcervoId,
                        Codigo = s.Key.Codigo,
                        Tipo = s.Key.Tipo,
                        Titulo = s.Key.Titulo,
                        Descricao = s.Key.Descricao.RemoverTagsHtml(),
                        DataAcervo = s.Key.DataAcervo,
                        Ano = s.Key.Ano,
                        TipoAcervoTag = s.Key.TipoAcervoTag,
                        CreditoAutoria = s.Any(w=> w.CreditoAutoria.NaoEhNulo() ) ? string.Join(", ", s.Select(ca=> ca.CreditoAutoria).Distinct()) : string.Empty,
                        Assunto = s.Any(w=> w.Assunto.NaoEhNulo() ) ? string.Join(", ", s.Select(ca=> ca.Assunto).Distinct()) : string.Empty,
                        EnderecoImagem = miniaturasDosAcervos.Any(f=> f.AcervoId == s.Key.AcervoId) 
                            ? $"{hostAplicacao}{Constantes.BUCKET_CDEP}/{miniaturasDosAcervos.FirstOrDefault(f=> f.AcervoId == s.Key.AcervoId).Thumbnail}"
                            : string.Empty,
                        EnderecoImagemPadrao = $"{hostAplicacao}{Constantes.BUCKET_CDEP}/{imagensPadrao.FirstOrDefault(f=> f.TipoAcervo == s.Key.Tipo).NomeArquivoFisico}",
                        EstaDisponivel = true,/* será tratada no controle de saldo em outra estória */
                        SituacaoDisponibilidade = Constantes.ACERVO_DISPONIVEL, /* será tratada no controle de saldo em outra estória */
                        TemControleDisponibilidade = s.Key.Tipo.EhAcervoBibliografico()
                    });
            
                var totalRegistros = acervosAgrupandoCreditoAutor.Count();
            
                return new PaginacaoResultadoDTO<PesquisaAcervoDTO>()
                {
                    Items = acervosAgrupandoCreditoAutor.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                    TotalRegistros = totalRegistros,
                    TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
                };
            }

            return new PaginacaoResultadoDTO<PesquisaAcervoDTO>()
            {
                Items = new List<PesquisaAcervoDTO>(),
                TotalPaginas = 0,
                TotalRegistros = 0
            };
        }

        private async Task<IEnumerable<TipoAcervoNomeArquivoFisicoDTO>> ObterImagensPadrao()
        {
            var imagensPadrao = new List<TipoAcervoNomeArquivoFisicoDTO>();
            
            var tiposDeAcervos = Enum.GetValues(typeof(TipoAcervo))
                .Cast<TipoAcervo>()
                .OrderBy(O=> O)
                .Select(v => v);

            foreach (var tipoAcervo in tiposDeAcervos)
            {
                var nomeImagemPadrao = ObterNomesDasImagensPadrao(tipoAcervo);
                
                var imagem = await repositorioArquivo.ObterArquivoPorNomeTipoArquivo(nomeImagemPadrao, TipoArquivo.Sistema);
                
                imagensPadrao.Add(new TipoAcervoNomeArquivoFisicoDTO()
                {
                    TipoAcervo = tipoAcervo,
                    NomeArquivoFisico = imagem.NomeArquivoFisico
                });
            }

            return imagensPadrao;
        }

        private string ObterNomesDasImagensPadrao(TipoAcervo tipoAcervo)
        {
            return tipoAcervo switch
            {
                TipoAcervo.Bibliografico => Constantes.IMAGEM_PADRAO_ACERVO_BIBLIOGRAFICO,
                TipoAcervo.DocumentacaoHistorica => Constantes.IMAGEM_PADRAO_ACERVO_DOCUMENTAL,
                TipoAcervo.ArtesGraficas => Constantes.IMAGEM_PADRAO_ACERVO_ARTE_GRAFICA,
                TipoAcervo.Audiovisual => Constantes.IMAGEM_PADRAO_ACERVO_AUDIOVISUAL,
                TipoAcervo.Fotografico => Constantes.IMAGEM_PADRAO_ACERVO_FOTOGRAFICO,
                TipoAcervo.Tridimensional => Constantes.IMAGEM_PADRAO_ACERVO_TRIDIMENSIONAL,
                _ => throw new ArgumentOutOfRangeException(nameof(tipoAcervo), tipoAcervo, null)
            };
        }

        public async Task<PaginacaoResultadoDTO<IdTipoTituloCreditoAutoriaCodigoAcervoDTO>> ObterPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo)
        {
            var paginacao = Paginacao;
            
            var registros = await repositorioAcervo.PesquisarPorFiltro(tipoAcervo, titulo, creditoAutorId, codigo);
            
            var registrosOrdenados = OrdenarRegistros(paginacao, registros);
            
            var acervosAgrupandoCreditoAutor = registrosOrdenados
                .GroupBy(g => new { g.Id, g.Titulo, g.Codigo, g.TipoAcervoId,
                    Data = g.DataAcervo })
                .Select(s => new IdTipoTituloCreditoAutoriaCodigoAcervoDTO
                {
                    Titulo = s.Key.Titulo,
                    AcervoId = s.Key.Id,
                    Codigo = s.Key.Codigo,
                    Data = s.Key.Data,
                    CreditoAutoria = s.Any(w=> w.CreditoAutor.NaoEhNulo() ) ? string.Join(", ", s.Select(ca=> ca.CreditoAutor.Nome)) : string.Empty,
                    TipoAcervo = ((TipoAcervo)s.Key.TipoAcervoId).Descricao(),
                    TipoAcervoId = (TipoAcervo)s.Key.TipoAcervoId,
                });
            
            var totalRegistros = acervosAgrupandoCreditoAutor.Count();
            
            var retornoPaginado = new PaginacaoResultadoDTO<IdTipoTituloCreditoAutoriaCodigoAcervoDTO>()
            {
                Items = acervosAgrupandoCreditoAutor.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
                
            return retornoPaginado;
        }

        private IOrderedEnumerable<Acervo> OrdenarRegistros(Paginacao paginacao, IEnumerable<Acervo> registros)
        {
            return paginacao.Ordenacao switch
            {
                TipoOrdenacao.DATA => registros.OrderByDescending(o => o.AlteradoEm.HasValue ? o.AlteradoEm.Value : o.CriadoEm),
                TipoOrdenacao.AZ => registros.OrderBy(o => o.Titulo),
                TipoOrdenacao.ZA => registros.OrderByDescending(o => o.Titulo),
            };
        }

        public async Task<AcervoDetalheDTO> ObterDetalhamentoPorTipoAcervoECodigo(FiltroDetalharAcervoDTO filtro)
        {
            switch (filtro.Tipo)
            {
                case TipoAcervo.Bibliografico:
                    var retornoBibliografico = mapper.Map<AcervoBibliograficoDetalheDTO>(await repositorioAcervoBibliografico.ObterDetalhamentoPorCodigo(filtro.Codigo));
                    
                    if (retornoBibliografico.EhNulo())
                        throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
                    
                    retornoBibliografico.EnderecoImagemPadrao = await ObterEnderecoImagemPadrao(TipoAcervo.Bibliografico);
                    retornoBibliografico.TemControleDisponibilidade = true;
                    retornoBibliografico.EstaDisponivel = true;/* será tratada no controle de saldo em outra estória */
                    retornoBibliografico.SituacaoDisponibilidade = Constantes.ACERVO_DISPONIVEL; /* será tratada no controle de saldo em outra estória */
                    retornoBibliografico.TipoAcervoId = (int)TipoAcervo.Bibliografico; /* será tratada no controle de saldo em outra estória */
                    return retornoBibliografico;
                
                case TipoAcervo.DocumentacaoHistorica:
                {
                    var retornoDocumental =  mapper.Map<AcervoDocumentalDetalheDTO>(await repositorioAcervoDocumental.ObterDetalhamentoPorCodigo(filtro.Codigo));
                    
                    if (retornoDocumental.EhNulo())
                        throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
                    
                    retornoDocumental.Imagens = await AplicarEndereco(retornoDocumental.Imagens);
                    retornoDocumental.EnderecoImagemPadrao = retornoDocumental.Imagens.PossuiElementos() ? string.Empty : await ObterEnderecoImagemPadrao(TipoAcervo.DocumentacaoHistorica);
                    retornoDocumental.TipoAcervoId = (int)TipoAcervo.DocumentacaoHistorica;
                    return retornoDocumental;
                }
                case TipoAcervo.ArtesGraficas:
                {
                    var retornoArteGrafica =  mapper.Map<AcervoArteGraficaDetalheDTO>(await repositorioAcervoArteGrafica.ObterDetalhamentoPorCodigo(filtro.Codigo));
                    
                    if (retornoArteGrafica.EhNulo())
                        throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
                    
                    retornoArteGrafica.Imagens = await AplicarEndereco(retornoArteGrafica.Imagens);
                    retornoArteGrafica.EnderecoImagemPadrao = retornoArteGrafica.Imagens.PossuiElementos() ? string.Empty : await ObterEnderecoImagemPadrao(TipoAcervo.ArtesGraficas);
                    retornoArteGrafica.TipoAcervoId = (int)TipoAcervo.ArtesGraficas;
                    return retornoArteGrafica;
                }
                case TipoAcervo.Audiovisual:
                    var retornoAudiovisual = mapper.Map<AcervoAudiovisualDetalheDTO>(await repositorioAcervoAudiovisual.ObterDetalhamentoPorCodigo(filtro.Codigo));
                    
                    if (retornoAudiovisual.EhNulo())
                        throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
                    
                    retornoAudiovisual.EnderecoImagemPadrao = await ObterEnderecoImagemPadrao(TipoAcervo.Audiovisual);
                    retornoAudiovisual.TipoAcervoId = (int)TipoAcervo.Audiovisual;
                    return retornoAudiovisual;
                
                case TipoAcervo.Fotografico:
                {
                    var retornoFotografico =  mapper.Map<AcervoFotograficoDetalheDTO>(await repositorioAcervoFotografico.ObterDetalhamentoPorCodigo(filtro.Codigo));
                    
                    if (retornoFotografico.EhNulo())
                        throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
                    
                    retornoFotografico.Imagens = await AplicarEndereco(retornoFotografico.Imagens);
                    retornoFotografico.EnderecoImagemPadrao = retornoFotografico.Imagens.PossuiElementos() ? string.Empty : await ObterEnderecoImagemPadrao(TipoAcervo.Fotografico);
                    retornoFotografico.TipoAcervoId = (int)TipoAcervo.Fotografico;
                    return retornoFotografico;
                }
                case TipoAcervo.Tridimensional:
                {
                    var retornoTridimensional =  mapper.Map<AcervoTridimensionalDetalheDTO>(await repositorioAcervoTridimensional.ObterDetalhamentoPorCodigo(filtro.Codigo));
                    
                    if (retornoTridimensional.EhNulo())
                        throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
                    
                    retornoTridimensional.Imagens = await AplicarEndereco(retornoTridimensional.Imagens);
                    retornoTridimensional.EnderecoImagemPadrao = retornoTridimensional.Imagens.PossuiElementos() ? string.Empty : await ObterEnderecoImagemPadrao(TipoAcervo.Tridimensional);
                    retornoTridimensional.TipoAcervoId = (int)TipoAcervo.Tridimensional;
                    return retornoTridimensional;
                }
                default:
                    throw new Exception("Tipo de Acervo inválido!");
            }
        }
        
        private async Task<string> ObterEnderecoImagemPadrao(TipoAcervo tipoAcervo)
        {
            var hostAplicacao = configuration["UrlFrontEnd"];

           var nomeImagemPadrao = await repositorioArquivo.ObterArquivoPorNomeTipoArquivo(ObterNomesDasImagensPadrao(tipoAcervo), TipoArquivo.Sistema);
                
           return $"{hostAplicacao}{Constantes.BUCKET_CDEP}/{nomeImagemPadrao.NomeArquivoFisico}";
        }

        private async Task<ImagemDTO[]> AplicarEndereco(ImagemDTO[] imagens)
        {
            var hostAplicacao = configuration["UrlFrontEnd"];
            
            foreach (var imagem in imagens)
            {
                imagem.Original = $"{hostAplicacao}{Constantes.BUCKET_CDEP}/{imagem.Original}";
                imagem.Thumbnail = $"{hostAplicacao}{Constantes.BUCKET_CDEP}/{imagem.Thumbnail}";
            }

            return imagens;
        }
        
        private AcervoDetalheDTO AplicarEndereco(AcervoArteGraficaDetalheDTO acervo)
        {
            var hostAplicacao = configuration["UrlFrontEnd"];
            
            foreach (var acervoImagem in acervo.Imagens)
            {
                acervoImagem.Original = $"{hostAplicacao}{Constantes.BUCKET_CDEP}/{acervoImagem.Original}";
                acervoImagem.Thumbnail = $"{hostAplicacao}{Constantes.BUCKET_CDEP}/{acervoImagem.Thumbnail}";
            }

            return acervo;
        }

        public async Task<string> ObterTermoDeCompromisso()
        {
            var retorno = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.TermoCompromissoPesquisador, DateTimeExtension.HorarioBrasilia().Year);

            if (retorno.EhNulo() || !retorno.Ativo)
                return default;

            return retorno.Valor;
        }

        public async Task<IdNomeCodigoTipoParaEmprestimoDTO> PesquisarAcervoPorCodigoTombo(FiltroCodigoTomboDTO filtro)
        {
            var tiposAcervosPermitidos = ObterTiposAcervosPermitidosDoPerfilLogado();
            
            var retorno = await repositorioAcervo.PesquisarAcervoPorCodigoTombo(filtro.CodigoTombo,tiposAcervosPermitidos);

            if (retorno.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
            
            return mapper.Map<IdNomeCodigoTipoParaEmprestimoDTO>(retorno);
        }

        public Paginacao Paginacao
        {
            get
            {
                var numeroPaginaQueryString = contextoAplicacao.ObterVariavel<string>("NumeroPagina");
                var numeroRegistrosQueryString = contextoAplicacao.ObterVariavel<string>("NumeroRegistros");
                var ordenacaoQueryString = contextoAplicacao.ObterVariavel<string>("Ordenacao");

                if (numeroPaginaQueryString.NaoEstaPreenchido() || numeroRegistrosQueryString.NaoEstaPreenchido()|| ordenacaoQueryString.NaoEstaPreenchido())
                    return new Paginacao(0, 0,0);

                var numeroPagina = numeroPaginaQueryString.ConverterParaInteiro();
                var numeroRegistros = numeroRegistrosQueryString.ConverterParaInteiro();
                var ordenacao = ordenacaoQueryString.ConverterParaInteiro();

                return new Paginacao(numeroPagina, numeroRegistros == 0 ? 10 : numeroRegistros,ordenacao);
            }
        }

        public long[] ObterTiposAcervosPermitidosDoPerfilLogado()
        {
            var perfilLogado = new Guid(contextoAplicacao.PerfilUsuario);
            
            var tiposAcervosDisponiveis = ObterTodosTipos().Select(s => s.Id);
            
            switch (true)
            {
                case var _ when perfilLogado.EhPerfilAdminGeral() || perfilLogado.EhPerfilBasico() :
                    return tiposAcervosDisponiveis.ToArray();
                
                case var _ when perfilLogado.EhPerfilAdminBiblioteca():
                    return tiposAcervosDisponiveis.Where(w => w == (long)TipoAcervo.Bibliografico).ToArray();
                
                case var _ when perfilLogado.EhPerfilAdminMemoria():
                    return tiposAcervosDisponiveis.Where(w => w == (long)TipoAcervo.DocumentacaoHistorica).ToArray();
                
                case var _ when perfilLogado.EhPerfilAdminMemorial():
                    return tiposAcervosDisponiveis
                        .Where(w => w == (long)TipoAcervo.ArtesGraficas
                                    || w == (long)TipoAcervo.Fotografico
                                    || w == (long)TipoAcervo.Tridimensional
                                    || w == (long)TipoAcervo.Audiovisual).ToArray();
                
                default:
                    return Array.Empty<long>();
            }
        }

        public IEnumerable<IdNomeDTO> ObterTodosTipos()
        {
            return Enum.GetValues(typeof(TipoAcervo))
                .Cast<TipoAcervo>()
                .Select(v => new IdNomeDTO
                {
                    Id = (int)v,
                    Nome = v.ObterAtributo<DisplayAttribute>().Description,
                });
        }
    }
}  