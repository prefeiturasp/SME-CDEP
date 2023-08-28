using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoFotografico : IServicoAcervoFotografico
    {
        private readonly IRepositorioAcervoFotografico repositorioAcervoFotografico;
        private readonly IMapper mapper;
        private readonly IContextoAplicacao contextoAplicacao;
        private readonly IServicoAcervo servicoAcervo;
        
        public ServicoAcervoFotografico(IRepositorioAcervoFotografico repositorioAcervoFotografico, IMapper mapper, IContextoAplicacao contextoAplicacao,IServicoAcervo servicoAcervo)
        {
            this.repositorioAcervoFotografico = repositorioAcervoFotografico ?? throw new ArgumentNullException(nameof(repositorioAcervoFotografico));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
        }

        public async Task<long> Inserir(AcervoFotograficoDTO acervoFotograficoDto)
        {
            var retornoAcervo = await servicoAcervo.Inserir(new AcervoDTO()
            {
                Titulo = acervoFotograficoDto.Titulo,
                Codigo = acervoFotograficoDto.Codigo,
                CreditoAutorId = acervoFotograficoDto.CreditoAutorId,
                TipoAcervoId = (int)TipoAcervo.Fotografico
            });

            if (retornoAcervo > 0)
            {
                var entidade = mapper.Map<AcervoFotografico>(acervoFotograficoDto);
                entidade.AcervoId = retornoAcervo;
                return await repositorioAcervoFotografico.Inserir(entidade);
            }
            return default;
        } 

        public async Task<IList<AcervoFotograficoDTO>> ObterTodos()
        {
            return (await repositorioAcervoFotografico.ObterTodos()).Select(s=> mapper.Map<AcervoFotograficoDTO>(s)).ToList();
        }

        public async Task<AcervoFotograficoDTO> Alterar(AcervoFotograficoDTO acervoFotograficoDto)
        {
            var retornoAcervo = await servicoAcervo.Alterar(new AcervoDTO()
            {
                Titulo = acervoFotograficoDto.Titulo,
                Codigo = acervoFotograficoDto.Codigo,
                CreditoAutorId = acervoFotograficoDto.CreditoAutorId,
                TipoAcervoId = (int)TipoAcervo.Fotografico
            });

            if (retornoAcervo != null)
            {
                var entidadeExistente = await repositorioAcervoFotografico.ObterPorId(acervoFotograficoDto.Id);

                entidadeExistente.Localizacao = acervoFotograficoDto.Localizacao;
                entidadeExistente.Procedencia = acervoFotograficoDto.Procedencia;
                entidadeExistente.DataAcervo = acervoFotograficoDto.DataAcervo;
                entidadeExistente.CopiaDigital = acervoFotograficoDto.CopiaDigital;
                entidadeExistente.PermiteUsoImagem = acervoFotograficoDto.PermiteUsoImagem;
                entidadeExistente.ConservacaoId = acervoFotograficoDto.ConservacaoId;
                entidadeExistente.Descricao = acervoFotograficoDto.Descricao;
                entidadeExistente.Quantidade = acervoFotograficoDto.Quantidade;
                entidadeExistente.Largura = acervoFotograficoDto.Largura;
                entidadeExistente.Altura = acervoFotograficoDto.Altura;
                entidadeExistente.SuporteId = acervoFotograficoDto.SuporteId;
                entidadeExistente.FormatoId = acervoFotograficoDto.FormatoId;
                entidadeExistente.CromiaId = acervoFotograficoDto.CromiaId;
                entidadeExistente.Resolucao = acervoFotograficoDto.Resolucao;
                entidadeExistente.TamanhoArquivo = acervoFotograficoDto.TamanhoArquivo;
                
                return mapper.Map<AcervoFotograficoDTO>(await repositorioAcervoFotografico.Atualizar(entidadeExistente));
            }
            return default;
        }

        public async Task<AcervoFotograficoDTO> ObterPorId(long id)
        {
           return mapper.Map<AcervoFotograficoDTO>(await repositorioAcervoFotografico.ObterPorId(id));
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }
    }
}