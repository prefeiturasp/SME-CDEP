using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoFotografico : ServicoAcervoBase,IServicoAcervoFotografico
    {
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioAcervoFotograficoArquivo repositorioAcervoFotograficoArquivo;
        private readonly IRepositorioAcervoFotografico repositorioAcervoFotografico;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        private readonly IServicoMoverArquivoTemporario servicoMoverArquivoTemporario;
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ServicoAcervoFotografico(
            IRepositorioAcervo repositorioAcervo,
            IRepositorioAcervoFotografico repositorioAcervoFotografico, 
            IMapper mapper,
            ITransacao transacao,
            IServicoAcervo servicoAcervo,
            IRepositorioArquivo repositorioArquivo,
            IRepositorioAcervoFotograficoArquivo repositorioAcervoFotograficoArquivo,
            IServicoMoverArquivoTemporario servicoMoverArquivoTemporario,
            IServicoArmazenamento servicoArmazenamento) : 
            base(repositorioAcervo,
                repositorioArquivo,
                servicoMoverArquivoTemporario,
                servicoArmazenamento)
        {
            this.repositorioAcervoFotografico = repositorioAcervoFotografico ?? throw new ArgumentNullException(nameof(repositorioAcervoFotografico));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioAcervoFotograficoArquivo = repositorioAcervoFotograficoArquivo ?? throw new ArgumentNullException(nameof(repositorioAcervoFotograficoArquivo));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
        }

        public async Task<long> Inserir(AcervoFotograficoCadastroDTO acervoFotograficoCadastroDto)
        {
            var arquivosCompletos =  acervoFotograficoCadastroDto.Arquivos != null
                ? await ObterArquivosPorIds(acervoFotograficoCadastroDto.Arquivos) 
                : Enumerable.Empty<Arquivo>();
            
            var acervo = mapper.Map<Acervo>(acervoFotograficoCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.Fotografico;
            acervo.Codigo = ObterCodigoAcervo(acervo.Codigo);
            
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

            await MoverArquivosTemporarios(TipoArquivo.AcervoFotografico,arquivosCompletos);
          
            return acervoFotografico.AcervoId;
        }

        private string ObterCodigoAcervo(string codigo)
        {
            return codigo.ContemSigla() 
                ? codigo
                : $"{codigo}{Constantes.SIGLA_ACERVO_FOTOGRAFICO}";
        }

        public async Task<IEnumerable<AcervoFotograficoDTO>> ObterTodos()
        {
            return (await repositorioAcervoFotografico.ObterTodos()).Select(s=> mapper.Map<AcervoFotograficoDTO>(s));
        }

        public async Task<AcervoFotograficoDTO> Alterar(AcervoFotograficoAlteracaoDTO acervoFotograficoAlteracaoDto)
        {
            var arquivosIdsInserir =  Enumerable.Empty<long>();
            var arquivosIdsExcluir =  Enumerable.Empty<long>();
            
            var acervoFotografico = mapper.Map<AcervoFotografico>(acervoFotograficoAlteracaoDto);
            var codigo = ObterCodigoAcervo(acervoFotograficoAlteracaoDto.Codigo);
            
            var arquivosExistentes = (await repositorioAcervoFotograficoArquivo.ObterPorAcervoFotograficoId(acervoFotograficoAlteracaoDto.Id)).Select(s => s.ArquivoId).ToArray();
            (arquivosIdsInserir, arquivosIdsExcluir) = await ObterArquivosInseridosExcluidosMovidos(acervoFotograficoAlteracaoDto.Arquivos, arquivosExistentes);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervoFotograficoAlteracaoDto.AcervoId,
                    acervoFotograficoAlteracaoDto.Titulo, 
                    codigo, 
                    acervoFotograficoAlteracaoDto.CreditosAutoresIds);
                
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

            await MoverArquivosTemporarios(TipoArquivo.AcervoFotografico);

            await ExcluirArquivosArmazenamento();

            return await ObterPorId(acervoFotograficoAlteracaoDto.AcervoId);
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
            return await base.Excluir(id);
        }
    }
}