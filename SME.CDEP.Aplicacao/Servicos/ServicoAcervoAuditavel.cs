using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

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
        
        public ServicoAcervoAuditavel(IRepositorioAcervo repositorioAcervo, IMapper mapper, IContextoAplicacao contextoAplicacao,
            IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor,IRepositorioArquivo repositorioArquivo, IConfiguration configuration)
        {
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.repositorioAcervoCreditoAutor = repositorioAcervoCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioAcervoCreditoAutor));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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

        private static void ValidarCodigoTomboCodigoNovoAno(Acervo acervo)
        {
            var codigoNaoPreenchido = acervo.Codigo.NaoEstaPreenchido();
            var codigoNovoNaoPreenchido = acervo.CodigoNovo.NaoEstaPreenchido();
            
            if (codigoNaoPreenchido && codigoNovoNaoPreenchido)
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_NAO_INFORMADO, ObterCodigoOuTomboPorTipoAcervo((TipoAcervo)acervo.TipoAcervoId)));
            
            if ((!codigoNaoPreenchido && !codigoNovoNaoPreenchido) && acervo.Codigo.Equals(acervo.CodigoNovo))
                throw new NegocioException(string.Format(MensagemNegocio.REGISTRO_X_DUPLICADO, ObterCodigoOuTomboPorTipoAcervo((TipoAcervo)acervo.TipoAcervoId)));
            
            if (acervo.Ano > DateTimeExtension.HorarioBrasilia().Year)
                throw new NegocioException(MensagemNegocio.NAO_PERMITIDO_ANO_FUTURO);
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

        public IEnumerable<IdNomeDTO> ObterTodosTipos()
        {
            return Enum.GetValues(typeof(TipoAcervo))
                .Cast<TipoAcervo>()
                .Select(v => new IdNomeDTO
                {
                    Id = (int)v,
                    Nome = v.ObterAtributo<DisplayAttribute>().Name,
                });
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
                var acervosIds = acervos.Where(w=> w.Tipo.EhAcervoArteGraficaOuFotografico()).Select(s => s.AcervoId).Distinct().ToArray();

                var acervosCodigoNomeResumidos = await repositorioArquivo.ObterAcervoCodigoNomeArquivoPorAcervoId(acervosIds);

                var hostAplicacao = configuration["UrlFrontEnd"];
            
                var acervosAgrupandoCreditoAutor = acervos
                    .GroupBy(g => new { g.AcervoId,g.Codigo, g.Titulo, g.Tipo, g.Descricao, g.TipoAcervoTag, g.DataAcervo })
                    .Select(s => new PesquisaAcervoDTO
                    {
                        Codigo = s.Key.Codigo,
                        Tipo = s.Key.Tipo,
                        Titulo = s.Key.Titulo,
                        Descricao = s.Key.Descricao.RemoverTagsHtml(),
                        DataAcervo = s.Key.DataAcervo.RemoverTagsHtml(),
                        TipoAcervoTag = s.Key.TipoAcervoTag,
                        CreditoAutoria = s.Any(w=> w.CreditoAutoria.NaoEhNulo() ) ? string.Join(", ", s.Select(ca=> ca.CreditoAutoria).Distinct()) : string.Empty,
                        Assunto = s.Any(w=> w.Assunto.NaoEhNulo() ) ? string.Join(", ", s.Select(ca=> ca.Assunto).Distinct()) : string.Empty,
                        EnderecoImagem = acervosCodigoNomeResumidos.Any(f=> f.AcervoId == s.Key.AcervoId) 
                            ? $"{hostAplicacao}{Constantes.BUCKET_CDEP}/{acervosCodigoNomeResumidos.FirstOrDefault(f=> f.AcervoId == s.Key.AcervoId).NomeArquivo}"
                            : string.Empty
                    });
            
                var totalRegistros = acervosAgrupandoCreditoAutor.Count();
            
                return new PaginacaoResultadoDTO<PesquisaAcervoDTO>()
                {
                    Items = acervosAgrupandoCreditoAutor.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                    TotalRegistros = totalRegistros,
                    TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
                };
            }
            return default;
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
                    TipoAcervo = ((TipoAcervo)s.Key.TipoAcervoId).Nome(),
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
    }
}  