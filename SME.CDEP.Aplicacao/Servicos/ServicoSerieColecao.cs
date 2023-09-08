using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoSerieColecao : ServicoAplicacaoAuditavel<SerieColecao, IdNomeExcluidoAuditavelDTO>,IServicoSerieColecao
    {
        private readonly IRepositorioSerieColecao repositorioSerieColecao;
        private readonly IMapper mapper;

        public ServicoSerieColecao(IRepositorioSerieColecao repositorioSerieColecao, IMapper mapper,IContextoAplicacao contextoAplicacao) : base(repositorioSerieColecao, mapper,contextoAplicacao)
        {
            this.repositorioSerieColecao = repositorioSerieColecao ?? throw new ArgumentNullException(nameof(repositorioSerieColecao));   
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));   
        }

        public async Task<long> Inserir(IdNomeExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO)
        {
            ValidarNome(idNomeExcluidoAuditavelDTO);
            await ValidarDuplicado(idNomeExcluidoAuditavelDTO.Nome, idNomeExcluidoAuditavelDTO.Id);
            return await base.Inserir(idNomeExcluidoAuditavelDTO);
        }
        
        public async Task<IdNomeExcluidoAuditavelDTO> Alterar(IdNomeExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO)
        {
            ValidarNome(idNomeExcluidoAuditavelDTO);
            await ValidarDuplicado(idNomeExcluidoAuditavelDTO.Nome, idNomeExcluidoAuditavelDTO.Id);
            return await base.Alterar(idNomeExcluidoAuditavelDTO);
        }
        
        public async Task<PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>> ObterPaginado(string? nome = null)
        {
            var registros = string.IsNullOrEmpty(nome) 
                ?  await ObterTodos() 
                : mapper.Map<IEnumerable<IdNomeExcluidoAuditavelDTO>>(await repositorioSerieColecao.PesquisarPorNome(nome));
            
            var totalRegistros = registros.Count();
            var paginacao = Paginacao;
            var registrosOrdenados = OrdenarRegistros(paginacao, registros);
            
            var retornoPaginado = new PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>()
            {
                Items = registrosOrdenados.ToList().Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
                
            return retornoPaginado;
        }
        
        private IOrderedEnumerable<IdNomeExcluidoAuditavelDTO> OrdenarRegistros(Paginacao paginacao, IEnumerable<IdNomeExcluidoAuditavelDTO> registros)
        {
            return paginacao.Ordenacao switch
            {
                TipoOrdenacao.DATA => registros.OrderByDescending(o => o.AlteradoEm.HasValue ? o.AlteradoEm.Value : o.CriadoEm),
                TipoOrdenacao.AZ => registros.OrderBy(o => o.Nome),
                TipoOrdenacao.ZA => registros.OrderByDescending(o => o.Nome),
            };
        }
        
        private async Task ValidarDuplicado(string nome, long id)
        {
            if (await repositorioSerieColecao.Existe(nome, id))
                throw new NegocioException(MensagemNegocio.REGISTRO_DUPLICADO);
        }
        
        private void ValidarNome(IdNomeExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO)
        {
            if (idNomeExcluidoAuditavelDTO.Nome is null || idNomeExcluidoAuditavelDTO.Nome.Trim().Length == 0)
                throw new NegocioException(MensagemNegocio.NOME_NAO_INFORMADO);
        }
    }
}
