using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoAuditavel <E,D> : IServicoAplicacao where E : Acervo where D : AcervoDTO
    {
        private readonly IRepositorioBase<E> repositorio;
        private readonly IMapper mapper;
        private readonly IContextoAplicacao contextoAplicacao;
        
        public ServicoAcervoAuditavel(IRepositorioBase<E> repositorio, IMapper mapper,IContextoAplicacao contextoAplicacao) 
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public async Task<long> Inserir(D entidadeDto)
        {
            ValidarTitulo(entidadeDto);
            
            await ValidarDuplicado(entidadeDto.Titulo, entidadeDto.Id);
                
            var entidade = mapper.Map<E>(entidadeDto);
            entidade.CriadoEm = DateTimeExtension.HorarioBrasilia();
            entidade.CriadoPor = contextoAplicacao.NomeUsuario;
            entidade.CriadoLogin = contextoAplicacao.UsuarioLogado;
            return await repositorio.Inserir(entidade);
        }
        
        private static void ValidarTitulo(D entidadeDto)
        {
            if (entidadeDto.Titulo is null || entidadeDto.Titulo.Trim().Length == 0)
                throw new NegocioException(MensagemNegocio.NOME_NAO_INFORMADO);
        }
        
        public async Task<IList<D>> ObterTodos()
        {
            return (await repositorio.ObterTodos()).Where(w=> !w.Excluido).Select(s=> mapper.Map<D>(s)).ToList();
        }

        public async Task<D> Alterar(D entidadeDto)
        {
            ValidarTitulo(entidadeDto);
            
            await ValidarDuplicado(entidadeDto.Titulo, entidadeDto.Id);
            
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
        
        public async Task<IList<D>> PesquisarPorNome(string nome)
        {
            return mapper.Map<IList<D>>(await repositorio.PesquisarPorNome(nome));
        }
        
        public async Task ValidarDuplicado(string nome, long id)
        {
            if ((await PesquisarPorNome(nome)).ToList().Any(a => a.Id != id))
                throw new NegocioException(MensagemNegocio.REGISTRO_DUPLICADO);
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

  
        
       
        
       