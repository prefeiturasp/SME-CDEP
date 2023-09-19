using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
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
            await ValidarTituloDuplicado(acervo.Titulo, acervo.Id);
            
            await ValidarTomboDuplicado(acervo.Codigo, acervo.Id);
                
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
        
        public async Task ValidarTomboDuplicado(string codigo, long id)
        {
            if (await repositorioAcervo.ExisteCodigo(codigo, id))
                throw new NegocioException(string.Format(MensagemNegocio.REGISTRO_X_DUPLICADO,"Codigo"));
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
            await ValidarTituloDuplicado(acervo.Titulo, acervo.Id);
            
            acervo.AlteradoEm = DateTimeExtension.HorarioBrasilia();
            acervo.AlteradoLogin = contextoAplicacao.UsuarioLogado;
            acervo.AlteradoPor = contextoAplicacao.NomeUsuario;
            
            var acervoAlterado = mapper.Map<AcervoDTO>(await repositorioAcervo.Atualizar(acervo));

            var creditosAutoresPropostos = acervo.CreditosAutoresIds.ToList();
            var acervoCreditoAutorAtuais = await repositorioAcervoCreditoAutor.ObterPorAcervoId(acervoAlterado.Id);
            var acervoCreditoAutorAInserir = creditosAutoresPropostos.Select(a => a).Except(acervoCreditoAutorAtuais.Select(b => b.CreditoAutorId));
            var arquivosIdsExcluir = acervoCreditoAutorAtuais.Select(a => a.CreditoAutorId).Except(creditosAutoresPropostos.Select(b => b)).ToArray();
                
            foreach (var creditoAutorId in acervoCreditoAutorAInserir)
                await repositorioAcervoCreditoAutor.Inserir(new AcervoCreditoAutor()
                {
                    AcervoId = acervo.Id,
                    CreditoAutorId = creditoAutorId
                });
            
            await repositorioAcervoCreditoAutor.Excluir(arquivosIdsExcluir,acervo.Id);

            return acervoAlterado;
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
            var entidade = await repositorioAcervo.ObterPorId(entidaId);
            entidade.Excluido = true;
            entidade.AlteradoEm = DateTimeExtension.HorarioBrasilia();
            entidade.AlteradoLogin = contextoAplicacao.UsuarioLogado;
            entidade.AlteradoPor = contextoAplicacao.NomeUsuario;
            await repositorioAcervo.Atualizar(entidade);
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
                    CreditoAutoria = string.Join(", ", s.Select(ca=> ca.CreditoAutor.Nome)),
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