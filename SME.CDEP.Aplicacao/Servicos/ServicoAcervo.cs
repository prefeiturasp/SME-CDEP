using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervo : ServicoAcervoAuditavel<Acervo, AcervoDTO>,IServicoAcervo
    {
        private readonly IRepositorioAcervo repositorioAcervo;
        
        public ServicoAcervo(IRepositorioAcervo repositorioAcervo, IMapper mapper, IContextoAplicacao contextoAplicacao) : base(repositorioAcervo, mapper, contextoAplicacao)
        {
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
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
                    AcervoId = s.TipoAcervoId,
                    Codigo = s.Codigo,
                    CreditoAutoria = s.CreditoAutoria.Nome,
                    TipoAcervo = ((TipoAcervo)s.TipoAcervoId).Name(),
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
    }
}  