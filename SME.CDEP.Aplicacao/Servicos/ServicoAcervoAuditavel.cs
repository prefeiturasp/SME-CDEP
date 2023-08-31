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
        private readonly IMapper mapper;
        private readonly IContextoAplicacao contextoAplicacao;
        
        public ServicoAcervoAuditavel(IRepositorioAcervo repositorioAcervo, IMapper mapper, IContextoAplicacao contextoAplicacao)
        {
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }
        
        public async Task<long> Inserir(AcervoDTO acervoDto)
        {
            ValidarTitulo(acervoDto.Titulo);
            
            await ValidarDuplicado(acervoDto.Titulo, acervoDto.Id);
                
            var entidade = mapper.Map<Acervo>(acervoDto);
            entidade.CriadoEm = DateTimeExtension.HorarioBrasilia();
            entidade.CriadoPor = contextoAplicacao.NomeUsuario;
            entidade.CriadoLogin = contextoAplicacao.UsuarioLogado;
            return await repositorioAcervo.Inserir(entidade);
        }
        
        private static void ValidarTitulo(string titulo)
        {
            if (titulo is null || titulo.Trim().Length == 0)
                throw new NegocioException(MensagemNegocio.TITULO_NAO_INFORMADO);
        }
        
        public async Task ValidarDuplicado(string nome, long id)
        {
            if ((await repositorioAcervo.PesquisarPorNome(nome, "titulo")).ToList().Any(a => a.Id != id))
                throw new NegocioException(MensagemNegocio.REGISTRO_DUPLICADO);
        }

        public async Task<IList<AcervoDTO>> ObterTodos()
        {
            return (await repositorioAcervo.ObterTodos()).Where(w=> !w.Excluido).Select(s=> mapper.Map<AcervoDTO>(s)).ToList();
        }

        public async Task<AcervoDTO> Alterar(AcervoDTO acervoDto)
        {
           ValidarTitulo(acervoDto.Titulo);
            
            await ValidarDuplicado(acervoDto.Titulo, acervoDto.Id);
            
            var entidadeExistente = mapper.Map<Acervo>(acervoDto);
            entidadeExistente.AlteradoEm = DateTimeExtension.HorarioBrasilia();
            entidadeExistente.AlteradoLogin = contextoAplicacao.UsuarioLogado;
            entidadeExistente.AlteradoPor = contextoAplicacao.NomeUsuario;
            
            return mapper.Map<AcervoDTO>(await repositorioAcervo.Atualizar(entidadeExistente));
        }

        public async Task<AcervoDTO> ObterPorId(long acervoId)
        {
            return mapper.Map<AcervoDTO>(await repositorioAcervo.ObterPorId(acervoId));
        }

        public IList<IdNomeDTO> ObterTodosTipos()
        {
            return Enum.GetValues(typeof(TipoAcervo))
                .Cast<TipoAcervo>()
                .Select(v => new IdNomeDTO
                {
                    Id = (int)v,
                    Nome = v.ObterAtributo<DisplayAttribute>().Name,
                })
                .ToList();
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
            var registros = await repositorioAcervo.PesquisarPorFiltro(tipoAcervo, titulo, creditoAutorId, codigo);
            var totalRegistros = registros.Count;
            var paginacao = Paginacao;
            var registrosOrdenados = OrdenarRegistros(paginacao, registros);
            
            var retornoPaginado = new PaginacaoResultadoDTO<IdTipoTituloCreditoAutoriaCodigoAcervoDTO>()
            {
                Items = registrosOrdenados.Select(s=> new IdTipoTituloCreditoAutoriaCodigoAcervoDTO
                {
                    Titulo = s.Titulo,
                    AcervoId = s.Id,
                    Codigo = s.Codigo,
                    CreditoAutoria = s.CreditoAutor.Nome,
                    TipoAcervo = ((TipoAcervo)s.TipoAcervoId).Nome(),
                }).ToList().Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
                
            return retornoPaginado;
        }

        private IOrderedEnumerable<Acervo> OrdenarRegistros(Paginacao paginacao, IList<Acervo> registros)
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