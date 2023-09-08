using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoFotografico : IServicoAcervoFotografico
    {
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioAcervoFotograficoArquivo repositorioAcervoFotograficoArquivo;
        private readonly IRepositorioAcervoFotografico repositorioAcervoFotografico;
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        
        public ServicoAcervoFotografico(IRepositorioAcervoFotografico repositorioAcervoFotografico, 
            IMapper mapper, ITransacao transacao,IServicoAcervo servicoAcervo,IRepositorioArquivo repositorioArquivo,
            IRepositorioAcervoFotograficoArquivo repositorioAcervoFotograficoArquivo,
            IRepositorioAcervo repositorioAcervo)
        {
            this.repositorioAcervoFotografico = repositorioAcervoFotografico ?? throw new ArgumentNullException(nameof(repositorioAcervoFotografico));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.repositorioAcervoFotograficoArquivo = repositorioAcervoFotograficoArquivo ?? throw new ArgumentNullException(nameof(repositorioAcervoFotograficoArquivo));
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
        }

        public async Task<long> Inserir(AcervoFotograficoCadastroDTO acervoFotograficoCadastroDto)
        {
            var arquivosCompletos =  Enumerable.Empty<Arquivo>();

            if (acervoFotograficoCadastroDto.Arquivos != null)
                arquivosCompletos = await repositorioArquivo.ObterPorCodigos(acervoFotograficoCadastroDto.Arquivos);
            
            var tran = transacao.Iniciar();
            try
            {
                var acervo = mapper.Map<Acervo>(acervoFotograficoCadastroDto);
                acervo.TipoAcervoId = (int)TipoAcervo.Fotografico;
                acervo.Codigo = $"{acervo.Codigo}FT";
                
                var retornoAcervo = await servicoAcervo.Inserir(acervo);

                var acervoFotografico = mapper.Map<AcervoFotografico>(acervoFotograficoCadastroDto);
                acervoFotografico.AcervoId = retornoAcervo;
                
                var retorno = await repositorioAcervoFotografico.Inserir(acervoFotografico);
                
                foreach (var arquivo in arquivosCompletos)
                {
                    await repositorioAcervoFotograficoArquivo.Inserir(new AcervoFotograficoArquivo()
                    {
                        ArquivoId = arquivo.Id, 
                        AcervoFotograficoId = acervoFotografico.Id
                    });
                }
                
                tran.Commit();

                return retorno;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        } 

        public async Task<IEnumerable<AcervoFotograficoDTO>> ObterTodos()
        {
            return (await repositorioAcervoFotografico.ObterTodos()).Select(s=> mapper.Map<AcervoFotograficoDTO>(s));
        }

        public async Task<AcervoFotograficoDTO> Alterar(AcervoFotograficoAlteracaoDTO acervoFotograficoAlteracaoDto)
        {
            var arquivosIdsInserir =  Enumerable.Empty<long>();
            var arquivosIdsExcluir =  Enumerable.Empty<long>();
            var arquivosAlterados = TratamentoArquivosAlterados(acervoFotograficoAlteracaoDto);
            
            var acervo = await repositorioAcervo.ObterPorId(acervoFotograficoAlteracaoDto.AcervoId);
            acervo.Titulo = acervoFotograficoAlteracaoDto.Titulo;
            acervo.Codigo = acervoFotograficoAlteracaoDto.Codigo;
            acervo.CreditoAutorId = acervoFotograficoAlteracaoDto.CreditoAutorId;
            
            var acervoFotografico = mapper.Map<AcervoFotografico>(acervoFotograficoAlteracaoDto);
            
            var arquivosExistentes = await repositorioAcervoFotograficoArquivo.ObterPorAcervoFotograficoId(acervoFotograficoAlteracaoDto.Id);
            
            var arquivosAlteradosCompletos = await repositorioArquivo.ObterPorCodigos(arquivosAlterados);
            arquivosIdsInserir = arquivosAlteradosCompletos.Select(a => a.Id).Except(arquivosExistentes.Select(b => b.ArquivoId));
            arquivosIdsExcluir = arquivosExistentes.Select(b => b.ArquivoId).Except(arquivosAlteradosCompletos.Select(a => a.Id));
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervo);
                
                var acervoFotograficoAtualizado = await repositorioAcervoFotografico.Atualizar(acervoFotografico);
                
                foreach (var arquivo in arquivosIdsInserir)
                {
                    await repositorioAcervoFotograficoArquivo.Inserir(new AcervoFotograficoArquivo()
                    {
                        ArquivoId = arquivo, 
                        AcervoFotograficoId = acervoFotografico.Id
                    });
                }

                await repositorioAcervoFotograficoArquivo.Excluir(arquivosIdsExcluir.ToArray(),acervoFotografico.Id);
                
                tran.Commit();

                return await ObterPorId(acervoFotograficoAlteracaoDto.Id);
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        }

        private static string[] TratamentoArquivosAlterados(AcervoFotograficoAlteracaoDTO acervoFotograficoAlteracaoDto)
        {
            return acervoFotograficoAlteracaoDto.Arquivos != null ? acervoFotograficoAlteracaoDto.Arquivos : Array.Empty<string>();
        }

        public async Task<AcervoFotograficoDTO> ObterPorId(long id)
        {
            var acervoFotograficoSimples = mapper.Map<AcervoFotograficoDTO>(await repositorioAcervoFotografico.ObterPorId(id));
            return acervoFotograficoSimples;
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }
    }
}