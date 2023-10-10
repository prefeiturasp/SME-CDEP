using System.ComponentModel.DataAnnotations;
using AutoMapper;
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
        
        public ServicoAcervoAuditavel(IRepositorioAcervo repositorioAcervo, IMapper mapper, IContextoAplicacao contextoAplicacao,
            IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor)
        {
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.repositorioAcervoCreditoAutor = repositorioAcervoCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioAcervoCreditoAutor));
        }
        
        public async Task<long> Inserir(Acervo acervo)
        {
            ValidarCodigoTomboCodigoNovo(acervo);
            
            await ValidarTituloDuplicado(acervo.Titulo, acervo.Id);
            
            await ValidarCodigoTomboCodigoNovoDuplicado(acervo.Codigo, acervo.Id);
            
            if (!string.IsNullOrEmpty(acervo.CodigoNovo))
                await ValidarCodigoTomboCodigoNovoDuplicado(acervo.CodigoNovo, acervo.Id, "Código Novo");
            
            if (!string.IsNullOrEmpty(acervo.CodigoNovo) && acervo.TipoAcervoId != (long)TipoAcervo.DocumentacaoHistorica)
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

            return acervoId;
        }

        private static void ValidarCodigoTomboCodigoNovo(Acervo acervo)
        {
            var codigoNaoPreenchido = string.IsNullOrEmpty(acervo.Codigo);
            var codigoNovoNaoPreenchido = string.IsNullOrEmpty(acervo.CodigoNovo);
            
            if (codigoNaoPreenchido && codigoNovoNaoPreenchido)
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_NAO_INFORMADO, "Código/Tombo/Código Novo"));
            
            if ((!codigoNaoPreenchido && !codigoNovoNaoPreenchido) && acervo.Codigo.Equals(acervo.CodigoNovo))
                throw new NegocioException(string.Format(MensagemNegocio.REGISTRO_X_DUPLICADO, "Código"));
        }

        public async Task ValidarCodigoTomboCodigoNovoDuplicado(string codigo, long id, string nomeCampo = "codigo")
        {
            if (codigo.EstaPreenchido() && await repositorioAcervo.ExisteCodigo(codigo, id))
                throw new NegocioException(string.Format(MensagemNegocio.REGISTRO_X_DUPLICADO,nomeCampo));
        }
        
        public async Task ValidarTituloDuplicado(string titulo, long id)
        {
            if (await repositorioAcervo.ExisteTitulo(titulo, id))
                throw new NegocioException(string.Format(MensagemNegocio.REGISTRO_X_DUPLICADO, "Título"));
        }

        public async Task<IEnumerable<AcervoDTO>> ObterTodos()
        {
            return (await repositorioAcervo.ObterTodos()).Where(w=> !w.Excluido).Select(s=> mapper.Map<AcervoDTO>(s));
        }

        public async Task<AcervoDTO> Alterar(Acervo acervo)
        {
            ValidarCodigoTomboCodigoNovo(acervo);
            
            await ValidarTituloDuplicado(acervo.Titulo, acervo.Id);
            
            await ValidarCodigoTomboCodigoNovoDuplicado(acervo.Codigo, acervo.Id);

            if (!string.IsNullOrEmpty(acervo.CodigoNovo))
                await ValidarCodigoTomboCodigoNovoDuplicado(acervo.CodigoNovo, acervo.Id, "Código Novo");
            
            acervo.AlteradoEm = DateTimeExtension.HorarioBrasilia();
            acervo.AlteradoLogin = contextoAplicacao.UsuarioLogado;
            acervo.AlteradoPor = contextoAplicacao.NomeUsuario;
            
            if (!string.IsNullOrEmpty(acervo.CodigoNovo) && acervo.TipoAcervoId != (long)TipoAcervo.DocumentacaoHistorica)
                throw new NegocioException(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO);
            
            var acervoAlterado = mapper.Map<AcervoDTO>(await repositorioAcervo.Atualizar(acervo));

            var creditosAutoresPropostos = acervo.CreditosAutoresIds != null ? acervo.CreditosAutoresIds.ToList() : Enumerable.Empty<long>();
            var acervoCreditoAutorAtuais = await repositorioAcervoCreditoAutor.ObterPorAcervoId(acervoAlterado.Id);
            var acervoCreditoAutorAInserir = creditosAutoresPropostos.Select(a => a)
                                         .Except(acervoCreditoAutorAtuais.Select(b => b.CreditoAutorId));
            var arquivosIdsExcluir = acervoCreditoAutorAtuais.Select(a => a.CreditoAutorId)
                                 .Except(creditosAutoresPropostos.Select(b => b)).ToArray();

            foreach (var creditoAutorId in acervoCreditoAutorAInserir)
                await repositorioAcervoCreditoAutor.Inserir(new AcervoCreditoAutor()
                {
                    AcervoId = acervo.Id,
                    CreditoAutorId = creditoAutorId
                });
            
            await repositorioAcervoCreditoAutor.Excluir(arquivosIdsExcluir,acervo.Id);

            return acervoAlterado;
        }

        public async Task<AcervoDTO> Alterar(long id, string titulo, string codigo, long[] creditosAutoresIds, string codigoNovo = "")
        {
            var acervo = await repositorioAcervo.ObterPorId(id);

            if (!string.IsNullOrEmpty(codigoNovo) && acervo.TipoAcervoId != (long)TipoAcervo.DocumentacaoHistorica)
                throw new NegocioException(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO);
            
            acervo.Titulo = titulo;
            acervo.Codigo = codigo;
            acervo.CreditosAutoresIds = creditosAutoresIds;
            acervo.CodigoNovo = codigoNovo;
            return await Alterar(acervo);
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

        public async Task<PaginacaoResultadoDTO<IdTipoTituloCreditoAutoriaCodigoAcervoDTO>> ObterPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo)
        {
            var paginacao = Paginacao;
            
            var registros = await repositorioAcervo.PesquisarPorFiltro(tipoAcervo, titulo, creditoAutorId, codigo);
            
            var registrosOrdenados = OrdenarRegistros(paginacao, registros);
            
            var acervosAgrupandoCreditoAutor = registrosOrdenados
                .GroupBy(g => new { g.Id, g.Titulo, g.Codigo, g.TipoAcervoId })
                .Select(s => new IdTipoTituloCreditoAutoriaCodigoAcervoDTO
                {
                    Titulo = s.Key.Titulo,
                    AcervoId = s.Key.Id,
                    Codigo = s.Key.Codigo,
                    CreditoAutoria = s.Any(w=> w.CreditoAutor != null ) ? string.Join(", ", s.Select(ca=> ca.CreditoAutor.Nome)) : string.Empty,
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

                if (string.IsNullOrWhiteSpace(numeroPaginaQueryString) || string.IsNullOrWhiteSpace(numeroRegistrosQueryString)|| string.IsNullOrWhiteSpace(ordenacaoQueryString))
                    return new Paginacao(0, 0,0);

                var numeroPagina = int.Parse(numeroPaginaQueryString);
                var numeroRegistros = int.Parse(numeroRegistrosQueryString);
                var ordenacao = int.Parse(ordenacaoQueryString);

                return new Paginacao(numeroPagina, numeroRegistros == 0 ? 10 : numeroRegistros,ordenacao);
            }
        }
    }
}  