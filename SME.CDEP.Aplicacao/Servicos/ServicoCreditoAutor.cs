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
    public class ServicoCreditoAutor : ServicoAplicacaoAuditavel<CreditoAutor, IdNomeTipoExcluidoAuditavelDTO>,IServicoCreditoAutor
    {
        private readonly IRepositorioCreditoAutor repositorioCreditoAutor;
        private readonly IMapper mapper;

        public ServicoCreditoAutor(IRepositorioCreditoAutor repositorioCreditoAutor, IMapper mapper,IContextoAplicacao contextoAplicacao) : base(repositorioCreditoAutor, mapper, contextoAplicacao)
        {
            this.repositorioCreditoAutor = repositorioCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioCreditoAutor));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(IdNomeTipoExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO)
        {
            ValidarNome(idNomeExcluidoAuditavelDTO);
            await ValidarDuplicado(idNomeExcluidoAuditavelDTO.Nome, idNomeExcluidoAuditavelDTO.Id,idNomeExcluidoAuditavelDTO.Tipo);
            return await base.Inserir(idNomeExcluidoAuditavelDTO);
        }
        
        public async Task<IdNomeTipoExcluidoAuditavelDTO> Alterar(IdNomeTipoExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO)
        {
            ValidarNome(idNomeExcluidoAuditavelDTO);
            await ValidarDuplicado(idNomeExcluidoAuditavelDTO.Nome, idNomeExcluidoAuditavelDTO.Id,idNomeExcluidoAuditavelDTO.Tipo);
            return await base.Alterar(idNomeExcluidoAuditavelDTO);
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

        public async Task<IEnumerable<IdNomeTipoExcluidoAuditavelDTO>> ObterTodos(TipoCreditoAutoria? tipo)
        {
            return tipo.HasValue 
                ? mapper.Map<IEnumerable<IdNomeTipoExcluidoAuditavelDTO>>(await repositorioCreditoAutor.ObterTodosPorTipo(tipo.Value)) 
                : mapper.Map<IEnumerable<IdNomeTipoExcluidoAuditavelDTO>>(await repositorioCreditoAutor.ObterTodos());
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
        
        private async Task ValidarDuplicado(string nome, long id, int tipo)
        {
            if (await repositorioCreditoAutor.Existe(nome, id, tipo))
                throw new NegocioException(MensagemNegocio.REGISTRO_DUPLICADO);
        }
        
        private void ValidarNome(IdNomeExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO)
        {
            if (idNomeExcluidoAuditavelDTO.Nome is null || idNomeExcluidoAuditavelDTO.Nome.Trim().Length == 0)
                throw new NegocioException(MensagemNegocio.NOME_NAO_INFORMADO);
        }
    }
}
