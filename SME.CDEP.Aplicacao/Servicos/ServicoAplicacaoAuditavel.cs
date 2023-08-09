﻿using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAplicacaoAuditavel<E,D> : IServicoAplicacao where E : EntidadeBaseAuditavel where D : IdNomeExcluidoAuditavelDTO
    {
        private readonly IRepositorioBase<E> repositorio;
        private readonly IMapper mapper;
        private readonly IContextoAplicacao contextoAplicacao;
        
        public ServicoAplicacaoAuditavel(IRepositorioBase<E> repositorio, IMapper mapper,IContextoAplicacao contextoAplicacao) 
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public Task<long> Inserir(D entidadeDto)
        {
            var entidade = mapper.Map<E>(entidadeDto);
            entidade.CriadoEm = DateTimeExtension.HorarioBrasilia();
            entidade.CriadoPor = contextoAplicacao.NomeUsuario;
            entidade.CriadoLogin = contextoAplicacao.UsuarioLogado;
            return repositorio.Inserir(entidade);
        }
        
        public async Task<IList<D>> ObterTodos()
        {
            return (await repositorio.ObterTodos()).Where(w=> !w.Excluido).Select(s=> mapper.Map<D>(s)).ToList();
        }

        public async Task<PaginacaoResultadoDTO<D>> ObterPaginado()
        {
            var registros = await ObterTodos();
            var totalRegistros = registros.Count;
            var paginacao = Paginacao;
            var registrosOrdenados = OrdenarRegistros(paginacao, registros);
            
            var retornoPaginado = new PaginacaoResultadoDTO<D>()
            {
                Items = registrosOrdenados.ToList().Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
                
            return retornoPaginado;
        }

        private IOrderedEnumerable<D> OrdenarRegistros(Paginacao paginacao, IList<D> registros)
        {
            return paginacao.Ordenacao switch
            {
                TipoOrdenacao.DATA => registros.OrderByDescending(o => o.AlteradoEm.HasValue ? o.AlteradoEm.Value : o.CriadoEm),
                TipoOrdenacao.AZ => registros.OrderBy(o => o.Nome),
                TipoOrdenacao.ZA => registros.OrderByDescending(o => o.Nome),
            };
        }

        public async Task<D> Alterar(D entidadeDto)
        {
            var entidadeExistente = await repositorio.ObterPorId(entidadeDto.Id);
            
            var entidade = mapper.Map<E>(entidadeDto);
            entidade.CriadoEm = entidadeExistente.CriadoEm;
            entidade.CriadoLogin = entidadeExistente.CriadoLogin;
            entidade.CriadoPor = entidadeExistente.CriadoPor;
            entidade.AlteradoEm = DateTimeExtension.HorarioBrasilia();
            entidade.AlteradoLogin = contextoAplicacao.UsuarioLogado;
            entidade.AlteradoPor = contextoAplicacao.NomeUsuario;
            
            return mapper.Map<D>(await repositorio.Atualizar(entidade));
        }

        public async Task<D> ObterPorId(long entidadeId)
        {
            return mapper.Map<D>(await repositorio.ObterPorId(entidadeId));
        }

        public async Task<bool> Excluir(long entidaId)
        {
            var entidade = await ObterPorId(entidaId);
            entidade.Excluido = true;
            await Alterar(entidade);
            return true;
        }
        public async Task<IList<D>> PesquisarPorNome(string nome)
        {
            return mapper.Map<IList<D>>(await repositorio.PesquisarPorNome(nome));
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