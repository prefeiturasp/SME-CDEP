using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Extensions;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Dtos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
using System.ComponentModel.DataAnnotations;
using Exception = System.Exception;
#nullable disable
namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoAuditavel(IRepositorioAcervo repositorioAcervo, IMapper mapper,
        IContextoAplicacao contextoAplicacao, IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor,
        IRepositorioArquivo repositorioArquivo,
        IRepositorioAcervoBibliografico repositorioAcervoBibliografico,
        IRepositorioAcervoDocumental repositorioAcervoDocumental,
        IRepositorioAcervoArteGrafica repositorioAcervoArteGrafica,
        IRepositorioAcervoAudiovisual repositorioAcervoAudiovisual,
        IRepositorioAcervoFotografico repositorioAcervoFotografico,
        IRepositorioAcervoTridimensional repositorioAcervoTridimensional,
        IRepositorioParametroSistema repositorioParametroSistema,
        IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions,
        IServicoArmazenamento servicoArmazenamento,
        IServicoHistoricoConsultaAcervo servicoHistoricoConsultaAcervo,
        ILogger<ServicoAcervoAuditavel> logger) : IServicoAcervo
    {
        private readonly IRepositorioAcervo repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
        private readonly IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor = repositorioAcervoCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioAcervoCreditoAutor));
        private readonly IMapper mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IContextoAplicacao contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        private readonly IRepositorioArquivo repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        private readonly IRepositorioAcervoBibliografico repositorioAcervoBibliografico = repositorioAcervoBibliografico ?? throw new ArgumentNullException(nameof(repositorioAcervoBibliografico));
        private readonly IRepositorioAcervoDocumental repositorioAcervoDocumental = repositorioAcervoDocumental ?? throw new ArgumentNullException(nameof(repositorioAcervoDocumental));
        private readonly IRepositorioAcervoArteGrafica repositorioAcervoArteGrafica = repositorioAcervoArteGrafica ?? throw new ArgumentNullException(nameof(repositorioAcervoArteGrafica));
        private readonly IRepositorioAcervoAudiovisual repositorioAcervoAudiovisual = repositorioAcervoAudiovisual ?? throw new ArgumentNullException(nameof(repositorioAcervoAudiovisual));
        private readonly IRepositorioAcervoFotografico repositorioAcervoFotografico = repositorioAcervoFotografico ?? throw new ArgumentNullException(nameof(repositorioAcervoFotografico));
        private readonly IRepositorioAcervoTridimensional repositorioAcervoTridimensional = repositorioAcervoTridimensional ?? throw new ArgumentNullException(nameof(repositorioAcervoTridimensional));
        private readonly IRepositorioParametroSistema repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        private readonly IServicoArmazenamento servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        private readonly IServicoHistoricoConsultaAcervo servicoHistoricoConsultaAcervo = servicoHistoricoConsultaAcervo ?? throw new ArgumentNullException(nameof(servicoHistoricoConsultaAcervo));

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
            return tipoAcervo switch
            {
                TipoAcervo.DocumentacaoTextual => "Código antigo ou Código novo",
                _ => "Tombo",
            };
        }

        public async Task ValidarCodigoTomboCodigoNovoDuplicado(string codigo, long id, TipoAcervo tipo)
        {
            if (codigo.EstaPreenchido() && await repositorioAcervo.ExisteCodigo(codigo, id, tipo))
                throw new NegocioException(string.Format(MensagemNegocio.REGISTRO_X_DUPLICADO, ObterCodigoOuTomboPorTipoAcervo(tipo)));
        }

        public async Task<IEnumerable<AcervoDTO>> ObterTodos()
        {
            return (await repositorioAcervo.ObterTodos()).Where(w => !w.Excluido).Select(s => mapper.Map<AcervoDTO>(s));
        }

        public async Task<AcervoDTO> AlterarCreditoAutor(Acervo acervo)
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

            await repositorioAcervoCreditoAutor.Excluir(creditosAutoresIdsExcluir, acervo.Id);

            var coAutoresPropostos = acervo.CoAutores.NaoEhNulo() ? acervo.CoAutores : [];
            var coAutoresAtuais = await repositorioAcervoCreditoAutor.ObterPorAcervoId(acervoAlterado.Id, true);
            var coAutoresAInserir = coAutoresPropostos.Select(a => a).Except(coAutoresAtuais.Select(b => new CoAutor() { CreditoAutorId = b.CreditoAutorId, TipoAutoria = b.TipoAutoria }));
            var coAutoresIdsExcluir = coAutoresAtuais.Select(b => new CoAutor() { CreditoAutorId = b.CreditoAutorId, TipoAutoria = b.TipoAutoria }).Except(coAutoresPropostos.Select(b => b)).ToArray();

            foreach (var creditoAutor in coAutoresAInserir)
                await repositorioAcervoCreditoAutor.Inserir(new AcervoCreditoAutor()
                {
                    AcervoId = acervo.Id,
                    CreditoAutorId = creditoAutor.CreditoAutorId,
                    TipoAutoria = creditoAutor.TipoAutoria,
                    EhCoAutor = true,
                });

            foreach (var coAutorExcluir in coAutoresIdsExcluir)
                await repositorioAcervoCreditoAutor.Excluir(coAutorExcluir.CreditoAutorId, coAutorExcluir.TipoAutoria, acervo.Id);


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

            if (acervoDTO.CodigoNovo.EstaPreenchido() && acervo.TipoAcervoId != (long)TipoAcervo.DocumentacaoTextual)
                throw new NegocioException(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO);

            var acervoAlterar = mapper.Map<Acervo>(acervoDTO);
            acervoAlterar.Id = acervo.Id;
            acervoAlterar.TipoAcervoId = acervo.TipoAcervoId;
            acervoAlterar.CriadoEm = acervo.CriadoEm;
            acervoAlterar.CriadoPor = acervo.CriadoPor;
            acervoAlterar.CriadoLogin = acervo.CriadoLogin;

            return await AlterarCreditoAutor(acervoAlterar);
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
            logger.LogInformation("Iniciando pesquisa de acervo por texto livre e tipo de acervo. Filtro: {@filtroTextoLivreTipoAcervo}", filtroTextoLivreTipoAcervo);
            var paginacao = Paginacao;

            var acervos = await repositorioAcervo.ObterPorTextoLivreETipoAcervo(filtroTextoLivreTipoAcervo.TextoLivre, filtroTextoLivreTipoAcervo.TipoAcervo, filtroTextoLivreTipoAcervo.AnoInicial, filtroTextoLivreTipoAcervo.AnoFinal);

            await RegistrarHistoricoDePesquisaAsync(filtroTextoLivreTipoAcervo, acervos.Count());

            if (acervos.Any())
            {
                var acervosIds = acervos.Where(w => w.Tipo.EhAcervoArteGraficaOuFotograficoOuTridimensional()).Select(s => s.AcervoId).Distinct().ToArray();

                var miniaturasDosAcervos = await repositorioArquivo.ObterAcervoCodigoNomeArquivoPorAcervoId(acervosIds);

                var imagensPadrao = await ObterImagensPadrao();

                var acervosAgrupandoCreditoAutor = acervos
                    .GroupBy(g => new { g.AcervoId, g.Codigo, g.Titulo, g.Tipo, g.Descricao, g.TipoAcervoTag, g.DataAcervo, g.Ano, g.SituacaoSaldo, g.Editora })
                    .Select(s => new PesquisaAcervoDTO
                    {
                        AcervoId = s.Key.AcervoId,
                        Codigo = s.Key.Codigo,
                        Tipo = s.Key.Tipo,
                        Titulo = s.Key.Titulo,
                        Descricao = s.Key.Descricao.RemoverTagsHtml(),
                        DataAcervo = s.Key.DataAcervo,
                        Editora = s.Key.Editora,
                        Ano = s.Key.Ano,
                        TipoAcervoTag = s.Key.TipoAcervoTag,
                        CreditoAutoria = s.Any(w => w.CreditoAutoria.NaoEhNulo()) ? string.Join(", ", s.Select(ca => ca.CreditoAutoria).Distinct()) : string.Empty,
                        Assunto = s.Any(w => w.Assunto.NaoEhNulo()) ? string.Join(", ", s.Select(ca => ca.Assunto).Distinct()) : string.Empty,
                        EnderecoImagem = miniaturasDosAcervos.Any(f => f.AcervoId == s.Key.AcervoId)
                            ? $"{configuracaoArmazenamentoOptions.EnderecoCompletoBucketArquivos()}{miniaturasDosAcervos.FirstOrDefault(f => f.AcervoId == s.Key.AcervoId).Thumbnail}"
                            : string.Empty,
                        EnderecoImagemPadrao = $"{configuracaoArmazenamentoOptions.EnderecoCompletoBucketArquivos()}{imagensPadrao.FirstOrDefault(f => f.TipoAcervo == s.Key.Tipo).NomeArquivoFisico}",
                        EstaDisponivel = s.Key.SituacaoSaldo.EstaDisponivel(),
                        SituacaoDisponibilidade = s.Key.SituacaoSaldo.EstaDisponivel() ? Constantes.ACERVO_DISPONIVEL : Constantes.ACERVO_INDISPONIVEL,
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
                Items = [],
                TotalPaginas = 0,
                TotalRegistros = 0
            };
        }

        private async Task RegistrarHistoricoDePesquisaAsync(FiltroTextoLivreTipoAcervoDTO filtro, int quantidadeResultados)
        {
            logger.LogInformation("Registrando histórico de pesquisa de acervo. Filtro: {@filtro}, QuantidadeResultados: {quantidadeResultados}", filtro, quantidadeResultados);
            await servicoHistoricoConsultaAcervo.InserirAsync(new()
            {
                TermoPesquisado = filtro.TextoLivre,
                TipoAcervo = filtro.TipoAcervo,
                AnoInicial = (short?)filtro.AnoInicial,
                AnoFinal = (short?)filtro.AnoFinal,
                QuantidadeResultados = quantidadeResultados,
                DataConsulta = DateTime.UtcNow
            });
            logger.LogInformation("Histórico de pesquisa de acervo registrado com sucesso.");
        }

        private async Task<IEnumerable<TipoAcervoNomeArquivoFisicoDTO>> ObterImagensPadrao()
        {
            var imagensPadrao = new List<TipoAcervoNomeArquivoFisicoDTO>();

            var tiposDeAcervos = Enum.GetValues(typeof(TipoAcervo))
                .Cast<TipoAcervo>()
                .OrderBy(O => O)
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

        private static string ObterNomesDasImagensPadrao(TipoAcervo tipoAcervo)
        {
            return tipoAcervo switch
            {
                TipoAcervo.Bibliografico => Constantes.IMAGEM_PADRAO_ACERVO_BIBLIOGRAFICO,
                TipoAcervo.DocumentacaoTextual => Constantes.IMAGEM_PADRAO_ACERVO_DOCUMENTAL,
                TipoAcervo.ArtesGraficas => Constantes.IMAGEM_PADRAO_ACERVO_ARTE_GRAFICA,
                TipoAcervo.Audiovisual => Constantes.IMAGEM_PADRAO_ACERVO_AUDIOVISUAL,
                TipoAcervo.Fotografico => Constantes.IMAGEM_PADRAO_ACERVO_FOTOGRAFICO,
                TipoAcervo.Tridimensional => Constantes.IMAGEM_PADRAO_ACERVO_TRIDIMENSIONAL,
                _ => throw new ArgumentOutOfRangeException(nameof(tipoAcervo), tipoAcervo, null)
            };
        }

        public async Task<PaginacaoResultadoDTO<AcervoTableRowDTO>> ObterPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo, int? idEditora)
        {
            var paginacao = Paginacao;

            var filtroDto = new AcervoFiltroDto(tipoAcervo, titulo, creditoAutorId, codigo, idEditora);
            var paginacaoDto = mapper.Map<PaginacaoDto>(paginacao);

            var totalRegistros = await repositorioAcervo.ContarPorFiltro(filtroDto);

            if (totalRegistros == 0)
                return new PaginacaoResultadoDTO<AcervoTableRowDTO>()
                {
                    Items = [],
                    TotalRegistros = 0,
                    TotalPaginas = 0
                };

            var registros = await repositorioAcervo.PesquisarPorFiltroPaginado(filtroDto, paginacaoDto);
            var itensDto = registros.Select(async acervo => new AcervoTableRowDTO
            {
                Titulo = acervo.Titulo,
                AcervoId = acervo.Id,
                Codigo = acervo.Codigo,
                Data = acervo.DataAcervo,
                Editora = acervo.Editora,
                CreditoAutoria = acervo.CreditosAutores is not null ? string.Join(", ", acervo.CreditosAutores.Select(c => c.Nome)) : string.Empty,
                TipoAcervo = ((TipoAcervo)acervo.TipoAcervoId).Descricao(),
                TipoAcervoId = (TipoAcervo)acervo.TipoAcervoId,
                CapaDocumento = acervo.TipoAcervoId == (long)TipoAcervo.DocumentacaoTextual && !string.IsNullOrEmpty(acervo.CapaDocumento)
                                ? await ObterImagemBase64(acervo.CapaDocumento)
                                : null

            }).Select(t => t.Result).ToList();

            return new PaginacaoResultadoDTO<AcervoTableRowDTO>()
            {
                Items = itensDto,
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / Paginacao.QuantidadeRegistros)
            };
        }
        public async Task<string> ObterImagemBase64(string nomeArquivo)
        {
            try
            {
                var metadados = await servicoArmazenamento.ObterMetadadosObjeto(nomeArquivo);
                if (metadados == null) return null;

                using var stream = await servicoArmazenamento.ObterStream(nomeArquivo);
                if (stream == null || stream.Length == 0) return null;

                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var conteudoBase64 = Convert.ToBase64String(memoryStream.ToArray());
                return $"data:{metadados.ContentType};base64,{conteudoBase64}";
            }
            catch (Exception)
            {
                return null;
            }
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
                    retornoBibliografico.EstaDisponivel = retornoBibliografico.EstaDisponivel;
                    retornoBibliografico.SituacaoDisponibilidade = retornoBibliografico.EstaDisponivel ? Constantes.ACERVO_DISPONIVEL : Constantes.ACERVO_INDISPONIVEL;
                    retornoBibliografico.TipoAcervoId = (int)TipoAcervo.Bibliografico;
                    return retornoBibliografico;

                case TipoAcervo.DocumentacaoTextual:
                    {
                        var retornoDocumental = mapper.Map<AcervoDocumentalDetalheDTO>(await repositorioAcervoDocumental.ObterDetalhamentoPorCodigo(filtro.Codigo));

                        if (retornoDocumental.EhNulo())
                            throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);

                        retornoDocumental.Imagens = AplicarEndereco(retornoDocumental.Imagens);
                        retornoDocumental.EnderecoImagemPadrao = retornoDocumental.Imagens.PossuiElementos() ? string.Empty : await ObterEnderecoImagemPadrao(TipoAcervo.DocumentacaoTextual);
                        retornoDocumental.TipoAcervoId = (int)TipoAcervo.DocumentacaoTextual;
                        return retornoDocumental;
                    }
                case TipoAcervo.ArtesGraficas:
                    {
                        var retornoArteGrafica = mapper.Map<AcervoArteGraficaDetalheDTO>(await repositorioAcervoArteGrafica.ObterDetalhamentoPorCodigo(filtro.Codigo));

                        if (retornoArteGrafica.EhNulo())
                            throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);

                        retornoArteGrafica.Imagens = AplicarEndereco(retornoArteGrafica.Imagens);
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
                        var retornoFotografico = mapper.Map<AcervoFotograficoDetalheDTO>(await repositorioAcervoFotografico.ObterDetalhamentoPorCodigo(filtro.Codigo));

                        if (retornoFotografico.EhNulo())
                            throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);

                        retornoFotografico.Imagens = AplicarEndereco(retornoFotografico.Imagens);
                        retornoFotografico.EnderecoImagemPadrao = retornoFotografico.Imagens.PossuiElementos() ? string.Empty : await ObterEnderecoImagemPadrao(TipoAcervo.Fotografico);
                        retornoFotografico.TipoAcervoId = (int)TipoAcervo.Fotografico;
                        return retornoFotografico;
                    }
                case TipoAcervo.Tridimensional:
                    {
                        var retornoTridimensional = mapper.Map<AcervoTridimensionalDetalheDTO>(await repositorioAcervoTridimensional.ObterDetalhamentoPorCodigo(filtro.Codigo));

                        if (retornoTridimensional.EhNulo())
                            throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);

                        retornoTridimensional.Imagens = AplicarEndereco(retornoTridimensional.Imagens);
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
            var nomeImagemPadrao = await repositorioArquivo.ObterArquivoPorNomeTipoArquivo(ObterNomesDasImagensPadrao(tipoAcervo), TipoArquivo.Sistema);

            return $"{configuracaoArmazenamentoOptions.EnderecoCompletoBucketArquivos()}{nomeImagemPadrao.NomeArquivoFisico}";
        }

        private ImagemDTO[] AplicarEndereco(ImagemDTO[] imagens)
        {
            foreach (var imagem in imagens)
            {
                imagem.Original = $"{configuracaoArmazenamentoOptions.EnderecoCompletoBucketArquivos()}/{imagem.Original}";
                imagem.Thumbnail = $"{configuracaoArmazenamentoOptions.EnderecoCompletoBucketArquivos()}/{imagem.Thumbnail}";
            }

            return imagens;
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

            var retorno = await repositorioAcervo.PesquisarAcervoPorCodigoTombo(filtro.CodigoTombo, tiposAcervosPermitidos);

            if (retorno.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);

            if (retorno.TipoAcervoId.EhAcervoBibliografico())
            {
                var acervoBibliografico = await repositorioAcervoBibliografico.ObterPorAcervoId(retorno.Id);

                if (acervoBibliografico.SituacaoSaldo.EstaIndisponivel())
                    throw new NegocioException(MensagemNegocio.ACERVO_INDISPONIVEL);
            }

            return mapper.Map<IdNomeCodigoTipoParaEmprestimoDTO>(retorno);
        }

        private Paginacao Paginacao
        {
            get
            {
                var numeroPaginaQueryString = contextoAplicacao.ObterVariavel<string>("NumeroPagina");
                var numeroRegistrosQueryString = contextoAplicacao.ObterVariavel<string>("NumeroRegistros");
                var ordenacaoQueryString = contextoAplicacao.ObterVariavel<string>("Ordenacao") ?? "0";

                if (numeroPaginaQueryString.NaoEstaPreenchido() || numeroRegistrosQueryString.NaoEstaPreenchido())
                {
                    numeroPaginaQueryString = "1";
                    numeroRegistrosQueryString = "10";
                }

                var numeroPagina = numeroPaginaQueryString.ConverterParaInteiro();
                var numeroRegistros = numeroRegistrosQueryString.ConverterParaInteiro();
                var ordenacao = ordenacaoQueryString.ConverterParaInteiro();

                return new Paginacao(numeroPagina, numeroRegistros == 0 ? 10 : numeroRegistros, ordenacao);
            }
        }

        public long[] ObterTiposAcervosPermitidosDoPerfilLogado()
        {
            var perfilLogado = new Guid(contextoAplicacao.PerfilUsuario);

            var tiposAcervosDisponiveis = ObterTodosTipos().Select(s => s.Id);

            return perfilLogado switch
            {
                _ when perfilLogado.EhPerfilAdminGeral() || perfilLogado.EhPerfilBasico()
                    => [.. tiposAcervosDisponiveis],

                _ when perfilLogado.EhPerfilAdminBiblioteca()
                    => [.. tiposAcervosDisponiveis.Where(w => w == (long)TipoAcervo.Bibliografico)],

                _ when perfilLogado.EhPerfilAdminMemoria()
                    => [.. tiposAcervosDisponiveis.Where(w => w == (long)TipoAcervo.DocumentacaoTextual)],

                _ when perfilLogado.EhPerfilAdminMemorial()
                    => [.. tiposAcervosDisponiveis
                    .Where(w => w == (long)TipoAcervo.ArtesGraficas
                                 || w == (long)TipoAcervo.Fotografico
                                 || w == (long)TipoAcervo.Tridimensional
                                 || w == (long)TipoAcervo.Audiovisual)],

                _ => []
            };
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

        public async Task<IEnumerable<string>> ObterAutocompletacaoTituloAcervosBaixadosAsync(string termoPesquisado) =>
            termoPesquisado.Trim().Length >= 3 
            ? await repositorioAcervo.ObterTituloAcervosBaixadosAsync(termoPesquisado.TrimStart())
            : [];
    }
}