using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoBibliografico : IServicoAcervoBibliografico
    {
        private readonly IRepositorioAcervoBibliograficoAssunto repositorioAcervoBibliograficoAssunto;
        private readonly IRepositorioAssunto repositorioAssunto;
        private readonly IRepositorioAcervoBibliografico repositorioAcervoBibliografico;
        private readonly IMapper mapper;
        private readonly IServicoAcervo servicoAcervo;
        private readonly ITransacao transacao;
        
        public ServicoAcervoBibliografico(
            IRepositorioAcervoBibliograficoAssunto repositorioAcervoBibliograficoAssunto, 
            IMapper mapper,
            ITransacao transacao,
            IRepositorioAcervoBibliografico repositorioAcervoBibliografico,
            IRepositorioAssunto repositorioAssunto,
            IServicoAcervo servicoAcervo)
        {
            this.repositorioAcervoBibliograficoAssunto = repositorioAcervoBibliograficoAssunto ?? throw new ArgumentNullException(nameof(repositorioAcervoBibliograficoAssunto));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioAcervoBibliografico = repositorioAcervoBibliografico ?? throw new ArgumentNullException(nameof(repositorioAcervoBibliografico));
            this.repositorioAssunto = repositorioAssunto ?? throw new ArgumentNullException(nameof(repositorioAssunto));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
        }

        public async Task<long> Inserir(AcervoBibliograficoCadastroDTO acervoBibliograficoCadastroDto)
        {
            ValidarPreenchimentoAcervoBibliografico(acervoBibliograficoCadastroDto.Altura, acervoBibliograficoCadastroDto.Largura);
            
            var assuntosSelecionados =  await repositorioAssunto.ObterPorIds(acervoBibliograficoCadastroDto.AssuntosIds);
            
            var acervo = mapper.Map<Acervo>(acervoBibliograficoCadastroDto);
            acervo.TipoAcervoId = (int)TipoAcervo.Bibliografico;
            
            var acervoBibliografico = mapper.Map<AcervoBibliografico>(acervoBibliograficoCadastroDto);
            
            var tran = transacao.Iniciar();
            try
            {
                var retornoAcervo = await servicoAcervo.Inserir(acervo);
                acervoBibliografico.AcervoId = retornoAcervo;
                
                await repositorioAcervoBibliografico.Inserir(acervoBibliografico);
                
                foreach (var assunto in assuntosSelecionados)
                {
                    await repositorioAcervoBibliograficoAssunto.Inserir(new AcervoBibliograficoAssunto()
                    {
                        AssuntoId = assunto.Id, 
                        AcervoBibliograficoId= acervoBibliografico.Id
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

            return acervoBibliografico.AcervoId;
        }
        
        private void ValidarPreenchimentoAcervoBibliografico(string? altura, string? largura)
        {
            if (largura.EstaPreenchido() && largura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));

            if (altura.EstaPreenchido() && altura.NaoEhNumericoComCasasDecimais())
                throw new NegocioException(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA));
        }

        public async Task<IEnumerable<AcervoBibliograficoDTO>> ObterTodos()
        {
            return (await repositorioAcervoBibliografico.ObterTodos()).Select(s=> mapper.Map<AcervoBibliograficoDTO>(s));
        }

        public async Task<AcervoBibliograficoDTO> Alterar(AcervoBibliograficoAlteracaoDTO acervoBibliograficoAlteracaoDto)
        {
            ValidarPreenchimentoAcervoBibliografico(acervoBibliograficoAlteracaoDto.Altura, acervoBibliograficoAlteracaoDto.Largura);
            
            var assuntosIdsInserir =  Enumerable.Empty<long>();
            var assuntosIdsExcluir =  Enumerable.Empty<long>();
            
            var acervoBibliografico = mapper.Map<AcervoBibliografico>(acervoBibliograficoAlteracaoDto);

            var acervoDTO = mapper.Map<AcervoDTO>(acervoBibliograficoAlteracaoDto);
            
            var assuntosExistentes = (await repositorioAcervoBibliograficoAssunto.ObterPorAcervoBibliograficoId(acervoBibliograficoAlteracaoDto.Id)).Select(s => s.AssuntoId).ToArray();
            (assuntosIdsInserir, assuntosIdsExcluir) = await ObterAssuntoInseridosExcluidos(acervoBibliograficoAlteracaoDto.AssuntosIds, assuntosExistentes);
            
            var tran = transacao.Iniciar();
            try
            {
                await servicoAcervo.Alterar(acervoDTO);
                
                await repositorioAcervoBibliografico.Atualizar(acervoBibliografico);
                
                foreach (var assunto in assuntosIdsInserir)
                {
                    await repositorioAcervoBibliograficoAssunto.Inserir(new AcervoBibliograficoAssunto()
                    {
                        AssuntoId = assunto, 
                        AcervoBibliograficoId= acervoBibliografico.Id
                    });
                }

                await repositorioAcervoBibliograficoAssunto.Excluir(assuntosIdsExcluir.ToArray(), acervoBibliografico.Id);
                
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

            return await ObterPorId(acervoBibliograficoAlteracaoDto.AcervoId);
        }

        private async Task<(IEnumerable<long>, IEnumerable<long>)> ObterAssuntoInseridosExcluidos(long[] assuntosAlterados, long[] assuntosExistentes)
        {
            var assuntosIdsInserir = assuntosAlterados.Except(assuntosExistentes);
            var assuntosIdsExcluir = assuntosExistentes.Except(assuntosAlterados);
            
            return (assuntosIdsInserir,assuntosIdsExcluir);
        }

        public async Task<AcervoBibliograficoDTO> ObterPorId(long id)
        {
            var acervoBibliograficoCompleto = await repositorioAcervoBibliografico.ObterAcervoBibliograficoCompletoPorId(id);
            if (acervoBibliograficoCompleto.NaoEhNulo())
            {
                var acervoBibliograficoDto = mapper.Map<AcervoBibliograficoDTO>(acervoBibliograficoCompleto);
                acervoBibliograficoDto.Auditoria = mapper.Map<AuditoriaDTO>(acervoBibliograficoCompleto);
                return acervoBibliograficoDto;
            }

            return default;
        }

        public async Task<bool> Excluir(long id)
        {
            return await servicoAcervo.Excluir(id);
        }
        
        public async Task<bool> AlterarSituacaoSaldo(SituacaoSaldo situacaoSaldo, long id)
        {
            var acervoBibliografico = await repositorioAcervoBibliografico.ObterPorAcervoId(id);
            if (acervoBibliografico.EhNulo()) return false;
            
            acervoBibliografico.DefinirSituacaoSaldo(situacaoSaldo);
            await repositorioAcervoBibliografico.Atualizar(acervoBibliografico);
            return true;
        }
    }
}