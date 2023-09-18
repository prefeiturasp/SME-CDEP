using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

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
        private readonly IServicoAcervo servicoAcervoCreditoAutor;
        private readonly ITransacao transacao;
        private readonly IServicoMoverArquivoTemporario servicoMoverArquivoTemporario;
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ServicoAcervoFotografico(IRepositorioAcervoFotografico repositorioAcervoFotografico, 
            IMapper mapper, ITransacao transacao,IServicoAcervo servicoAcervo,IRepositorioArquivo repositorioArquivo,
            IRepositorioAcervoFotograficoArquivo repositorioAcervoFotograficoArquivo,
            IRepositorioAcervo repositorioAcervo,IServicoMoverArquivoTemporario servicoMoverArquivoTemporario,IServicoArmazenamento servicoArmazenamento)
        {
            this.repositorioAcervoFotografico = repositorioAcervoFotografico ?? throw new ArgumentNullException(nameof(repositorioAcervoFotografico));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.repositorioAcervoFotograficoArquivo = repositorioAcervoFotograficoArquivo ?? throw new ArgumentNullException(nameof(repositorioAcervoFotograficoArquivo));
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.servicoMoverArquivoTemporario = servicoMoverArquivoTemporario ?? throw new ArgumentNullException(nameof(servicoMoverArquivoTemporario));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }

        public async Task<long> Inserir(AcervoFotograficoCadastroDTO acervoFotograficoCadastroDto)
        {
            var arquivosCompletos =  Enumerable.Empty<Arquivo>();

            if (acervoFotograficoCadastroDto.Arquivos != null)
                arquivosCompletos = await repositorioArquivo.ObterPorIds(acervoFotograficoCadastroDto.Arquivos);
            
            var acervo = mapper.Map<Acervo>(acervoFotograficoCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.Fotografico;
            acervo.Codigo = acervo.Codigo.ContemSigla() 
                ? acervo.Codigo
                : $"{acervo.Codigo}{Constantes.SIGLA_ACERVO_FOTOGRAFICO}";
            
            var acervoFotografico = mapper.Map<AcervoFotografico>(acervoFotograficoCadastroDto);
            
            var tran = transacao.Iniciar();
            try
            {
                var retornoAcervo = await servicoAcervo.Inserir(acervo);
                acervoFotografico.AcervoId = retornoAcervo;
                
                await repositorioAcervoFotografico.Inserir(acervoFotografico);
                
                foreach (var arquivo in arquivosCompletos)
                {
                    await repositorioAcervoFotograficoArquivo.Inserir(new AcervoFotograficoArquivo()
                    {
                        ArquivoId = arquivo.Id, 
                        AcervoFotograficoId = acervoFotografico.Id
                    });
                }
                
                tran.Commit();
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

            foreach (var arquivo in arquivosCompletos)
                await servicoMoverArquivoTemporario.Mover(TipoArquivo.AcervoFotografico, arquivo);

            return acervoFotografico.AcervoId;
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
            acervo.CreditosAutoresIds = acervoFotograficoAlteracaoDto.CreditosAutoresIds;
            
            var acervoFotografico = mapper.Map<AcervoFotografico>(acervoFotograficoAlteracaoDto);
            acervo.Codigo = acervo.Codigo.ContemSigla() 
                ? acervo.Codigo
                : $"{acervo.Codigo}{Constantes.SIGLA_ACERVO_FOTOGRAFICO}";
            
            var arquivosExistentes = await repositorioAcervoFotograficoArquivo.ObterPorAcervoFotograficoId(acervoFotograficoAlteracaoDto.Id);
            
            var arquivosAlteradosCompletos = await repositorioArquivo.ObterPorIds(arquivosAlterados);
            var arquivosExistentesCompletos = await repositorioArquivo.ObterPorIds(arquivosExistentes.Select(s=> s.ArquivoId).ToArray());
            arquivosIdsInserir = arquivosAlteradosCompletos.Select(a => a.Id).Except(arquivosExistentes.Select(b => b.ArquivoId));
            arquivosIdsExcluir = arquivosExistentes.Select(b => b.ArquivoId).Except(arquivosAlteradosCompletos.Select(a => a.Id));
            var arquivosMover = arquivosAlteradosCompletos.Where(w => arquivosIdsInserir.Contains(w.Id)).Select(s => s);
            var arquivosExcluir = arquivosExistentesCompletos.Where(w => arquivosIdsExcluir.Contains(w.Id)).Select(s => s);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervo);
                
                await repositorioAcervoFotografico.Atualizar(acervoFotografico);
                
                foreach (var arquivo in arquivosIdsInserir)
                {
                    await repositorioAcervoFotograficoArquivo.Inserir(new AcervoFotograficoArquivo()
                    {
                        ArquivoId = arquivo, 
                        AcervoFotograficoId = acervoFotografico.Id
                    });
                }

                await repositorioAcervoFotograficoArquivo.Excluir(arquivosIdsExcluir.ToArray(), acervoFotografico.Id);
                
                tran.Commit();
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

            foreach (var arquivoAMover in arquivosMover)
                await servicoMoverArquivoTemporario.Mover(TipoArquivo.AcervoFotografico, arquivoAMover);

            foreach (var arquivoAExcluir in arquivosExcluir)
            {
                var extensao = Path.GetExtension(arquivoAExcluir.Nome);
                var nomeArquivoBucket= $"{arquivoAExcluir.Codigo.ToString()}{extensao}";
                await servicoArmazenamento.Excluir(nomeArquivoBucket);
            }

            return await ObterPorId(acervoFotograficoAlteracaoDto.AcervoId);
        }

        private static long[] TratamentoArquivosAlterados(AcervoFotograficoAlteracaoDTO acervoFotograficoAlteracaoDto)
        {
            return acervoFotograficoAlteracaoDto.Arquivos != null ? acervoFotograficoAlteracaoDto.Arquivos : Array.Empty<long>();
        }

        public async Task<AcervoFotograficoDTO> ObterPorId(long id)
        {
            var acervoFotograficoSimples = await repositorioAcervoFotografico.ObterPorId(id);
            var acervoFotograficoDto = mapper.Map<AcervoFotograficoDTO>(acervoFotograficoSimples);
            acervoFotograficoDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoFotograficoSimples);
            return acervoFotograficoDto;
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }
    }
}