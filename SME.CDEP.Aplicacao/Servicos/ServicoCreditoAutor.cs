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
    public class ServicoCreditoAutor : ServicoAplicacaoAuditavel<CreditoAutor, IdNomeTipoExcluidoAuditavelDTO>,IServicoCreditoAutor
    {
        private readonly IRepositorioCreditoAutor repositorioCreditoAutor;
        private readonly IMapper mapper;

        public ServicoCreditoAutor(IRepositorioCreditoAutor repositorioCreditoAutor, IMapper mapper,IContextoAplicacao contextoAplicacao) : base(repositorioCreditoAutor, mapper, contextoAplicacao)
        {
            this.repositorioCreditoAutor = repositorioCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioCreditoAutor));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PaginacaoResultadoDTO<IdNomeTipoExcluidoAuditavelDTO>> ObterPaginado(NomeTipoCreditoAutoriaDTO nomeTipoDto)
        {
            var registros = string.IsNullOrEmpty(nomeTipoDto.Nome) 
                ? mapper.Map<IEnumerable<IdNomeTipoExcluidoAuditavelDTO>>(await repositorioCreditoAutor.ObterTodosPorTipo(nomeTipoDto.Tipo)) 
                : mapper.Map<IEnumerable<IdNomeTipoExcluidoAuditavelDTO>>(await repositorioCreditoAutor.PesquisarPorNomeTipo(nomeTipoDto.Nome, nomeTipoDto.Tipo));
            
            var totalRegistros = registros.Count();
            var paginacao = Paginacao;
            var registrosOrdenados = OrdenarRegistros(paginacao, registros.ToList());
            
            var retornoPaginado = new PaginacaoResultadoDTO<IdNomeTipoExcluidoAuditavelDTO>()
            {
                Items = registrosOrdenados.ToList().Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
                
            return retornoPaginado;
        }

        private IOrderedEnumerable<IdNomeTipoExcluidoAuditavelDTO> OrdenarRegistros(Paginacao paginacao, IList<IdNomeTipoExcluidoAuditavelDTO> registros)
        {
            return paginacao.Ordenacao switch
            {
                TipoOrdenacao.DATA => registros.OrderByDescending(o => o.AlteradoEm.HasValue ? o.AlteradoEm.Value : o.CriadoEm),
                TipoOrdenacao.AZ => registros.OrderBy(o => o.Nome),
                TipoOrdenacao.ZA => registros.OrderByDescending(o => o.Nome),
            };
        }
    }
}
