using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAplicacaoAuditavel<E,D> : IServicoAplicacao where E : EntidadeBaseAuditavel where D : BaseAuditavelDTO
    {
        private readonly IRepositorioBase<E> repositorio;
        private readonly IMapper mapper;
        private readonly IContextoAplicacao contextoAplicacao;
        
        public ServicoAplicacaoAuditavel(IRepositorioBase<E> repositorioAssunto, IMapper mapper,IContextoAplicacao contextoAplicacao) 
        {
            this.repositorio = repositorioAssunto ?? throw new ArgumentNullException(nameof(repositorioAssunto));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public async Task<long> Inserir(D entidadeDto)
        {
            var entidade = mapper.Map<E>(entidadeDto);
            entidade.CriadoEm = DateTimeExtension.HorarioBrasilia();
            entidade.CriadoPor = contextoAplicacao.NomeUsuario;
            entidade.CriadoLogin = contextoAplicacao.UsuarioLogado;
            return await repositorio.Inserir(entidade);
        }

        public async Task<IEnumerable<D>> ObterTodos()
        {
            return (await repositorio.ObterTodos()).Where(w=> !w.Excluido).Select(s=> mapper.Map<D>(s)).ToList();
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
            var retorno = await repositorio.ObterPorId(entidadeId);
            return mapper.Map<D>(retorno.Excluido ? default : retorno);
        }

        public async Task<bool> Excluir(long entidaId)
        {
            var entidade = await ObterPorId(entidaId);
            entidade.Excluido = true;
            await Alterar(entidade);
            return true;
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

                var numeroPagina = int.Parse(numeroPaginaQueryString);
                var numeroRegistros = int.Parse(numeroRegistrosQueryString);
                var ordenacao = int.Parse(ordenacaoQueryString);

                return new Paginacao(numeroPagina, numeroRegistros == 0 ? 10 : numeroRegistros,ordenacao);
            }
        }
    }
}