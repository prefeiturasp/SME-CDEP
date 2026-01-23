using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Extensions;
using SME.CDEP.Aplicacao.Servicos.Fachadas;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoSolicitacao(        
        IRepositorioUsuario repositorioUsuario, 
        IRepositorioParametroSistema repositorioParametroSistema,
        ContextoDadosAcervoSolicitacao dados,
        ContextoInfraAcervoSolicitacao infra,
        ContextoRegrasAcervoSolicitacao regras) : IServicoAcervoSolicitacao
    {

        public async Task<bool> Remover(long acervoSolicitacaoId)
        {
            await dados.RepositorioSolicitacao.Remover(acervoSolicitacaoId);
            return true;
        }

        public async Task<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>> ObterItensAcervoPorAcervosIds(long[] acervosIds)
        {
            var acervos = await dados.RepositorioSolicitacao.ObterItensDoAcervoPorAcervosIds(acervosIds);

            if (acervos.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
            
            return infra.Mapper.Map<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>>(acervos);
        }
        
        public async Task<AcervoSolicitacaoRetornoCadastroDTO> ObterPorId(long acervoSolicitacaoId)
        {
            var tiposAcervosPermitidos = regras.ServicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();

            return await regras.ServicoAcervo.ObterAcervosSolicitacoesPorIdTiposPermitidosAsync(acervoSolicitacaoId, tiposAcervosPermitidos);
        }

        public async Task<AcervoSolicitacaoRetornoCadastroDTO> ObterMinhaSolicitacaoPorId(long acervoSolicitacaoId)
        {
            var tiposAcervosPermitidos = Enum.GetValues<TipoAcervo>().Select(v => (long)v).ToArray();
                
            return await regras.ServicoAcervo.ObterAcervosSolicitacoesPorIdTiposPermitidosAsync(acervoSolicitacaoId, tiposAcervosPermitidos);
        }
        
        public async Task<bool> Excluir(long acervoSolicitacaoId)
        {
            await dados.RepositorioSolicitacao.Excluir(acervoSolicitacaoId);
            return true;
        }

        public async Task<PaginacaoResultadoDTO<MinhaSolicitacaoDTO>> ObterMinhasSolicitacoes()
        {
            var usuario = await infra.ServicoUsuario.ObterUsuarioLogado();

            var acervoSolicitacaoItemsDTOs = infra.Mapper.Map<IEnumerable<MinhaSolicitacaoDTO>>(await dados.RepositorioItem.ObterMinhasSolicitacoes(usuario.Id));

            var totalRegistros = acervoSolicitacaoItemsDTOs.Count();
            var paginacao = Paginacao;
            
            return new PaginacaoResultadoDTO<MinhaSolicitacaoDTO>()
            {
                Items = acervoSolicitacaoItemsDTOs.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
        }

        public Paginacao Paginacao
        {
            get
            {
                var numeroPaginaQueryString = regras.ContextoAplicacao.ObterVariavel<string>("NumeroPagina");
                var numeroRegistrosQueryString = regras.ContextoAplicacao.ObterVariavel<string>("NumeroRegistros");
                var ordenacaoQueryString = regras.ContextoAplicacao.ObterVariavel<string>("Ordenacao");

                if (numeroPaginaQueryString.NaoEstaPreenchido() || numeroRegistrosQueryString.NaoEstaPreenchido()|| ordenacaoQueryString.NaoEstaPreenchido())
                    return new Paginacao(0, 0,0);

                var numeroPagina = numeroPaginaQueryString.ConverterParaInteiro();
                var numeroRegistros = numeroRegistrosQueryString.ConverterParaInteiro();
                var ordenacao = ordenacaoQueryString.ConverterParaInteiro();

                return new Paginacao(numeroPagina, numeroRegistros == 0 ? 10 : numeroRegistros,ordenacao);
            }
        }

        public Task<IEnumerable<SituacaoItemDTO>> ObterSituacoesAtendimentosItem()
        {
            var lista = Enum.GetValues<SituacaoSolicitacaoItem>()
                .OrderBy(O=> O)
                .Select(v => new SituacaoItemDTO
                {
                    Id = (short)v,
                    Nome = v.Descricao()
                });

            return Task.FromResult(lista);
        }

        public async Task<PaginacaoResultadoDTO<SolicitacaoDTO>> ObterAtendimentoSolicitacoesPorFiltro(FiltroSolicitacaoDTO filtroSolicitacaoDto)
        {
            var tiposAcervosPermitidos = regras.ServicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();

            var solicitacoes = infra.Mapper.Map<IEnumerable<SolicitacaoDTO>>(await dados.RepositorioItem
                .ObterSolicitacoesPorFiltro(filtroSolicitacaoDto.AcervoSolicitacaoId, filtroSolicitacaoDto.TipoAcervo,
                    filtroSolicitacaoDto.DataSolicitacaoInicio, filtroSolicitacaoDto.DataSolicitacaoFim, filtroSolicitacaoDto.Responsavel, filtroSolicitacaoDto.SituacaoItem,
                    filtroSolicitacaoDto.DataVisitaInicio, filtroSolicitacaoDto.DataVisitaFim, filtroSolicitacaoDto.SolicitanteRf, filtroSolicitacaoDto.SituacaoEmprestimo, tiposAcervosPermitidos));

            var totalRegistros = solicitacoes.Count();
            var paginacao = Paginacao;
            
            return new PaginacaoResultadoDTO<SolicitacaoDTO>()
            {
                Items = solicitacoes.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
        }

        public async Task<AcervoSolicitacaoDetalheDTO> ObterDetalhesParaAtendimentoSolicitadoesPorId(long acervoSolicitacaoId)
        {
            var perfilLogado = new Guid(regras.ContextoAplicacao.PerfilUsuario);

            var tiposAcervosPermitidos = regras.ServicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();

            var acervoSolicitacao = infra.Mapper.Map<AcervoSolicitacaoDetalheDTO>(await dados.RepositorioSolicitacao.ObterDetalhesPorIdTiposPermitidos(acervoSolicitacaoId, tiposAcervosPermitidos));

            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);

            acervoSolicitacao.DadosSolicitante = infra.Mapper.Map<DadosSolicitanteDTO>(await infra.ServicoUsuario.ObterDadosSolicitantePorUsuarioId(acervoSolicitacao.UsuarioId));
            acervoSolicitacao.PodeFinalizar = PodeFinalizar(perfilLogado, acervoSolicitacao);
            
            acervoSolicitacao.PodeCancelar = perfilLogado.EhPerfilAdminGeral() && acervoSolicitacao.SituacaoId.NaoEstaFinalizadoAtendimentoOuCancelado()
                                             && !acervoSolicitacao.Itens.Any(a=> 
                                                 a.SituacaoId == SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE 
                                                 || a.SituacaoId == SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);

            if (acervoSolicitacao.Itens.Any(a=> a.TipoAcervoId.EhAcervoBibliografico()))
            {
                var limiteDiasEmprestimoAcervo = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteDiasEmprestimoAcervo,DateTimeExtension.HorarioBrasilia().Year);
                acervoSolicitacao.LimiteDiasEmprestimoAcervo = int.Parse(limiteDiasEmprestimoAcervo.Valor);
            }

            return acervoSolicitacao;
        }

        public bool PodeFinalizar(Guid perfilLogado, AcervoSolicitacaoDetalheDTO acervoSolicitacao)
        {
            return perfilLogado.EhPerfilAdminGeral() 
                   && acervoSolicitacao.SituacaoId.NaoEstaFinalizadoAtendimentoOuCancelado()
                   && !acervoSolicitacao.Itens.Any(a =>
                       a.SituacaoId.EstaAguardandoAtendimento()
                       || (a.SituacaoId.EstaAguardandoVisita() 
                           && a.DataVisita.HasValue 
                           && a.DataVisita.EhDataFutura())
                       || (a.TipoAcervoId.EhAcervoBibliografico() && a.SituacaoId.EstaEmSituacaoAguardandoVisitaEAguardandoAtendimento())
                       );
        }

        public IEnumerable<IdNomeDTO> ObterTiposDeAtendimentos()
        {
            return Enum.GetValues<TipoAtendimento>()
                    .Select(v => new IdNomeDTO
                    {
                        Id = (int)v,
                        Nome = v.ObterAtributo<DisplayAttribute>().Description ?? string.Empty,
                    });
        }
        
        public async Task<bool> ConfirmarAtendimento(AcervoSolicitacaoConfirmarDto acervoSolicitacaoConfirmar)
        {
            return await regras.ServicoConfirmacao.Executar(acervoSolicitacaoConfirmar);
        }

        public async Task<bool> FinalizarAtendimento(long acervoSolicitacaoId)
        {
            var acervoSolicitacao = await dados.RepositorioSolicitacao.ObterPorId(acervoSolicitacaoId);
            
            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
            
            if (acervoSolicitacao.Situacao.FoiAtendidaParcialmente())
                throw new NegocioException(MensagemNegocio.CANCELAR_SOLICITACAO_NAO_PERMITIDO_QUANDO_ITENS_ATENDIDOS_PARCIALMENTE);

            if (await dados.RepositorioItem.PossuiItensEmSituacaoAguardandoAtendimentoOuAguardandoVisitaComDataFutura(acervoSolicitacaoId))
                throw new NegocioException(MensagemNegocio.NÃO_PODE_FINALIZAR_QUANDO_AGUARDANDO_VISITA_DATA_FUTURA_OU_AGUARDANDO_ATENDIMENTO);

            var itens = await dados.RepositorioItem.ObterItensEmSituacaoAguardandoVisitaPorSolicitacaoId(acervoSolicitacaoId);

            var tran = infra.Transacao.Iniciar();
            try
            {
                acervoSolicitacao.Situacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
                await dados.RepositorioSolicitacao.Atualizar(acervoSolicitacao);

                foreach (var item in itens)
                {
                    item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
                    await dados.RepositorioItem.Atualizar(item);
                }
                
                tran.Commit();
                return true;
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
        
        public async Task<bool> FinalizarAtendimentoItem(long acervoSolicitacaoItemId)
        {
            var acervoSolicitacaoItem = await dados.RepositorioItem.ObterPorId(acervoSolicitacaoItemId);
            
            if (acervoSolicitacaoItem.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);

            var acervo = await dados.RepositorioAcervo.ObterPorId(acervoSolicitacaoItem.AcervoId);
            
            if (acervo.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
            
            var podeFinalizarItem = acervoSolicitacaoItem.TipoAtendimento.EhAtendimentoPresencial()
                                    && acervoSolicitacaoItem.DataVisita.HasValue
                                    && acervo.TipoAcervoId.NaoEhAcervoBibliografico()
                                    && acervoSolicitacaoItem.DataVisita.NaoEhDataFutura();
            
            if (!podeFinalizarItem)
                throw new NegocioException(MensagemNegocio.PERMITIDO_FINALIZAR_ATENDIMENTO_AGUARDANDO_VISITA_ATE_O_DIA_DE_HOJE);
            
            acervoSolicitacaoItem.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
            await dados.RepositorioItem.Atualizar(acervoSolicitacaoItem);

           var itens = await dados.RepositorioItem.ObterItensPorSolicitacaoId(acervoSolicitacaoItem.AcervoSolicitacaoId);
           
           if (!itens.Any(a=> a.Situacao.EstaEmSituacaoAguardandoVisitaEAguardandoAtendimento()))
           {
               var acervoSolicitacao = await dados.RepositorioSolicitacao.ObterPorId(acervoSolicitacaoItem.AcervoSolicitacaoId);
               acervoSolicitacao.Situacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
               await dados.RepositorioSolicitacao.Atualizar(acervoSolicitacao);
           }
           
           return true;
        }

        public async Task<bool> CancelarAtendimento(long acervoSolicitacaoId)
        {
            var acervoSolicitacao = await dados.RepositorioSolicitacao.ObterPorId(acervoSolicitacaoId);
            
            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
            
            if (await dados.RepositorioItem.PossuiItensFinalizadosAutomaticamente(acervoSolicitacaoId))
                throw new NegocioException(MensagemNegocio.NAO_PODE_CANCELAR_ATENDIMENTO_COM_ITEM_FINALIZADO_AUTOMATICAMENTE_MANUALMENTE);
            
            var itens = await dados.RepositorioItem.ObterItensPorSolicitacaoId(acervoSolicitacaoId);

            var acervos = await dados.RepositorioAcervo.ObterAcervosPorIds(itens.Select(s => s.AcervoId).ToArray());
                
            var tran = infra.Transacao.Iniciar();
            try
            {
                acervoSolicitacao.Situacao = SituacaoSolicitacao.CANCELADO;
                await dados.RepositorioSolicitacao.Atualizar(acervoSolicitacao);

                foreach (var item in itens)
                {
                    item.Situacao = SituacaoSolicitacaoItem.CANCELADO;
                    await dados.RepositorioItem.Atualizar(item);

                    if (item.TipoAtendimento is not null && item.TipoAtendimento.EhAtendimentoPresencial())
                    {
                        await regras.ServicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(item.Id);

                        var ehAcervoBibliografico = acervos.Any(f => f.Id == item.AcervoId && f.TipoAcervoId.EhAcervoBibliografico());
                        
                        if (ehAcervoBibliografico)
                            await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL,item.AcervoId);
                    }
                }
                
                tran.Commit();
                await infra.ServicoMensageria.Publicar(RotasRabbit.NotificarViaEmailCancelamentoAtendimento, acervoSolicitacaoId);
                return true;
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

        public async Task<bool> CancelarItemAtendimento(long acervoSolicitacaoItemId)
        {
            var acervoSolicitacaoItem = await dados.RepositorioItem.ObterPorId(acervoSolicitacaoItemId);
            
            if (acervoSolicitacaoItem.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);
            
            var itens = await dados.RepositorioItem.ObterItensPorSolicitacaoId(acervoSolicitacaoItem.AcervoSolicitacaoId);

            if (itens.Any(a=> a.Situacao.EstaEmSituacaoFinalizadoAutomaticamenteOuCancelado() && a.Id == acervoSolicitacaoItemId)) 
                throw new NegocioException(MensagemNegocio.NAO_PODE_CANCELAR_ATENDIMENTO_COM_ITEM_FINALIZADO_AUTOMATICAMENTE_MANUALMENTE);
            
            var acervoSolicitacao = await dados.RepositorioSolicitacao.ObterPorId(acervoSolicitacaoItem.AcervoSolicitacaoId);
            var todosItensEstaoCancelados = itens.Where(w => w.Id != acervoSolicitacaoItemId).All(a => a.Situacao.EstaCancelado());

            var acervos = await dados.RepositorioAcervo.ObterAcervosPorIds([acervoSolicitacaoItem.AcervoId]);

            var tran = infra.Transacao.Iniciar();
            try
            {
                acervoSolicitacaoItem.Situacao = SituacaoSolicitacaoItem.CANCELADO;
                await dados.RepositorioItem.Atualizar(acervoSolicitacaoItem);

                if (acervoSolicitacaoItem.TipoAtendimento.EhAtendimentoPresencial())
                    await regras.ServicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(acervoSolicitacaoItem.Id);

                if (acervos.Any(a=> a.TipoAcervoId.EhAcervoBibliografico()))
                    await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL,acervoSolicitacaoItem.AcervoId);
                
                await regras.ServicoProcessamentoSituacao.AtualizarSituacaoGeralSolicitacaoAsync(acervoSolicitacao,todosItensEstaoCancelados);

                await infra.ServicoMensageria.Publicar(RotasRabbit.NotificarViaEmailCancelamentoAtendimentoItem, acervoSolicitacaoItemId, Guid.NewGuid(), null);

                tran.Commit();

                return true;
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

        public async Task<bool> AlterarDataVisitaDoItemAtendimento(AlterarDataVisitaAcervoSolicitacaoItemDTO alterarDataVisitaAcervoSolicitacaoItemDto)
        {
            if (alterarDataVisitaAcervoSolicitacaoItemDto.DataVisita < DateTimeExtension.HorarioBrasilia().Date)
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_PRESENCIAL_NAO_DEVEM_TER_DATA_ACERVO_PASSADAS);
            
            var acervoSolicitacaoItem = await dados.RepositorioItem.ObterPorId(alterarDataVisitaAcervoSolicitacaoItemDto.Id);
            
            if (acervoSolicitacaoItem.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);
            
            if (await dados.RepositorioItem.AtendimentoPossuiItemSituacaoFinalizadoAutomaticamenteOuCancelado(alterarDataVisitaAcervoSolicitacaoItemDto.Id))
                throw new NegocioException(MensagemNegocio.ATENDIMENTO_NAO_ESTA_AGUARDANDO_VISITA);

            acervoSolicitacaoItem.DataVisita = alterarDataVisitaAcervoSolicitacaoItemDto.DataVisita;
            await dados.RepositorioItem.Atualizar(acervoSolicitacaoItem);
            
            await regras.ServicoEvento.AtualizarEventoVisita(acervoSolicitacaoItem.DataVisita.Value, acervoSolicitacaoItem.Id);
            
            return true;
        }

        public async Task<long> Inserir(AcervoSolicitacaoItemCadastroDTO[] acervosSolicitacaoItensCadastroDTO)
        {
            var usuarioLogado = await infra.ServicoUsuario.ObterUsuarioLogado();

            var usuarioSolicitante = await repositorioUsuario.ObterPorId(usuarioLogado.Id);
            if (usuarioSolicitante.EhNulo())
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);

            var arquivosEncontrados = await dados.RepositorioAcervo.ObterArquivosPorAcervoId([.. acervosSolicitacaoItensCadastroDTO.Select(s => s.AcervoId)]);

            var tran = infra.Transacao.Iniciar();
            try
            {
                var acervoSolicitacao = new AcervoSolicitacao()
                {
                    UsuarioId = usuarioLogado.Id,
                    DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date,
                    Situacao = acervosSolicitacaoItensCadastroDTO
                        .Select(s => s.AcervoId)
                        .Except(arquivosEncontrados.Select(s => s.AcervoId))
                        .Any() ? SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO : SituacaoSolicitacao.FINALIZADO_ATENDIMENTO
                };

                acervoSolicitacao.Id = await dados.RepositorioSolicitacao.Inserir(acervoSolicitacao);

                foreach (var item in acervosSolicitacaoItensCadastroDTO)
                {
                    var acervoSolicitacaoItem = infra.Mapper.Map<AcervoSolicitacaoItem>(item);

                    acervoSolicitacaoItem.AcervoSolicitacaoId = acervoSolicitacao.Id;

                    acervoSolicitacaoItem.Situacao = arquivosEncontrados.Any(a => a.AcervoId == item.AcervoId)
                        ? SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE
                        : SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;

                    await dados.RepositorioItem.Inserir(acervoSolicitacaoItem);

                    await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO, item.AcervoId);
                }
                tran.Commit();

                return acervoSolicitacao.Id;
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
        public async Task<long> Inserir(AcervoSolicitacaoManualDTO acervoSolicitacaoManualDto)
        {
            var usuario = await infra.ServicoUsuario.ObterPorId(acervoSolicitacaoManualDto.UsuarioId);
            
            if (usuario.EhNulo())
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);
            
            ValidarCamposDataVisitaEmprestimoDevolucao(acervoSolicitacaoManualDto);
            
            var datasDasVisitas = acervoSolicitacaoManualDto.Itens
                .Where(w => w.TipoAtendimento.EhAtendimentoPresencial())
                .Select(s => s.DataVisita!.Value);

            await regras.ServicoEvento.ValidarConflitosAsync(datasDasVisitas);
            
            var acervoSolicitacao = infra.Mapper.Map<AcervoSolicitacao>(acervoSolicitacaoManualDto);
            
            acervoSolicitacao.Origem = Origem.Manual;

            var usuarioLogado = await infra.ServicoUsuario.ObterUsuarioLogado();
            
            acervoSolicitacao.Situacao = acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhAtendimentoPresencial() && !a.DataEmprestimo.HasValue && !a.DataDevolucao.HasValue) 
                ? SituacaoSolicitacao.AGUARDANDO_VISITA 
                : SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
            
            var tran = infra.Transacao.Iniciar();
            try
            {
                acervoSolicitacao.Id =  await dados.RepositorioSolicitacao.Inserir(acervoSolicitacao);

                foreach (var item in acervoSolicitacao.Itens)
                {
                    item.AcervoSolicitacaoId = acervoSolicitacao.Id;
                    item.ResponsavelId = usuarioLogado.Id;
                    
                    var acervoSolicitacaoItemManualDto = acervoSolicitacaoManualDto.Itens.FirstOrDefault(f => f.AcervoId == item.AcervoId);

                    var possuiInformacoesEmprestimo = acervoSolicitacaoItemManualDto!.DataEmprestimo.HasValue && acervoSolicitacaoItemManualDto.DataDevolucao.HasValue;

                    item.Situacao = item.TipoAtendimento.EhAtendimentoViaEmail() || possuiInformacoesEmprestimo
                        ? SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                        : SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                    
                    await dados.RepositorioItem.Inserir(item);

                    if (item.TipoAtendimento.EhAtendimentoPresencial())
                    {
                        await regras.ServicoEvento.InserirEventoVisita(item.DataVisita!.Value, item.Id);

                        if (acervoSolicitacaoItemManualDto.TipoAcervo.EhAcervoBibliografico())
                        {
                            if (possuiInformacoesEmprestimo)
                            {
                                await dados.RepositorioAcervoEmprestimo.Inserir(new ()
                                {
                                    AcervoSolicitacaoItemId = item.Id,
                                    DataEmprestimo = acervoSolicitacaoItemManualDto.DataEmprestimo!.Value,
                                    DataDevolucao = acervoSolicitacaoItemManualDto.DataDevolucao!.Value,
                                    Situacao = SituacaoEmprestimo.EMPRESTADO
                                });
                                await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO,item.AcervoId);
                            }
                            else
                                await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO,item.AcervoId);
                        }
                    }
                }
                tran.Commit();

                return acervoSolicitacao.Id;
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

        public async Task<long> Alterar(AcervoSolicitacaoManualDTO acervoSolicitacaoManualDto)
        {
           var usuario = await infra.ServicoUsuario.ObterPorId(acervoSolicitacaoManualDto.UsuarioId);
            
            if (usuario.EhNulo())
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);

            var acervoSolicitacao = await dados.RepositorioSolicitacao.ObterPorId(acervoSolicitacaoManualDto.Id);
            
            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
            
            ValidarCamposDataVisitaEmprestimoDevolucao(acervoSolicitacaoManualDto);

            var datasDasVisitas = acervoSolicitacaoManualDto.Itens
                .Where(w => w.TipoAtendimento.EhAtendimentoPresencial())
                .Select(s => s.DataVisita!.Value);

            await regras.ServicoEvento.ValidarConflitosAsync(datasDasVisitas);
            
            acervoSolicitacao.Origem = Origem.Manual;
            
            acervoSolicitacao.DataSolicitacao = acervoSolicitacaoManualDto.DataSolicitacao;

            var usuarioLogado = await infra.ServicoUsuario.ObterUsuarioLogado();
            
            acervoSolicitacao.Situacao = acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhAtendimentoPresencial() && !a.DataEmprestimo.HasValue && !a.DataDevolucao.HasValue) 
                ? SituacaoSolicitacao.AGUARDANDO_VISITA 
                : SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
            
            var itensAtuais = await dados.RepositorioItem.ObterItensPorSolicitacaoId(acervoSolicitacao.Id);
            
            var itensEmprestados = await dados.RepositorioAcervoEmprestimo.ObterUltimoEmprestimoPorAcervoSolicitacaoItemIds([.. acervoSolicitacaoManualDto.Itens.Where(w=> w.Id.HasValue).Select(s => s.Id!.Value)]);

            var tran = infra.Transacao.Iniciar();
            try
            {
                await dados.RepositorioSolicitacao.Atualizar(acervoSolicitacao);
                
                foreach (var itemProposto in acervoSolicitacaoManualDto.Itens)
                {
                    var item = infra.Mapper.Map<AcervoSolicitacaoItem>(itemProposto);
                    
                    var possuiInformacoesEmprestimo = itemProposto.DataEmprestimo.HasValue && itemProposto.DataDevolucao.HasValue;
                    
                    item.Situacao =  item.TipoAtendimento.EhAtendimentoViaEmail()|| possuiInformacoesEmprestimo
                                     ? SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                                     : SituacaoSolicitacaoItem.AGUARDANDO_VISITA;

                    item.ResponsavelId = usuarioLogado.Id;
                    
                    if (item.Id.EhMaiorQueZero())
                    {
                        var itemAtual = itensAtuais.FirstOrDefault(f=> f.Id == item.Id);

                        var eraPresencial = itemAtual!.TipoAtendimento.EhAtendimentoPresencial();
                        
                        itemAtual.TipoAtendimento = item.TipoAtendimento;
                        
                        itemAtual.DataVisita = item.DataVisita;
                        
                        itemAtual.Situacao = item.Situacao;
                        
                        await dados.RepositorioItem.Atualizar(itemAtual);

                        var itemEmprestado = itensEmprestados.FirstOrDefault(f => f.AcervoSolicitacaoItemId == itemAtual.Id);
                        var naoTemEmprestimo = itemEmprestado!.EhNulo();
                        
                        if (eraPresencial && item.TipoAtendimento.EhAtendimentoViaEmail())
                        {
                            await regras.ServicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(item.Id);
                            await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL,item.AcervoId);
                        }

                        if (item.TipoAtendimento.EhAtendimentoPresencial())
                        {
                            await regras.ServicoEvento.AtualizarEventoVisita(item.DataVisita!.Value, item.Id);
                            
                            if (itemProposto.TipoAcervo.EhAcervoBibliografico())
                            {
                                if (naoTemEmprestimo)
                                {
                                    if (possuiInformacoesEmprestimo)
                                    {
                                        await dados.RepositorioAcervoEmprestimo.Inserir(new ()
                                        {
                                            AcervoSolicitacaoItemId = item.Id,
                                            DataEmprestimo = itemProposto.DataEmprestimo!.Value,
                                            DataDevolucao = itemProposto.DataDevolucao!.Value,
                                            Situacao = SituacaoEmprestimo.EMPRESTADO
                                        });
                                        await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO,item.AcervoId);
                                    }
                                    else
                                        await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO,item.AcervoId);
                                }
                                else
                                {
                                    itemEmprestado!.DataEmprestimo = itemProposto.DataEmprestimo!.Value;
                                    itemEmprestado.DataDevolucao = itemProposto.DataDevolucao!.Value;
                                    itemEmprestado.Situacao = SituacaoEmprestimo.EMPRESTADO;
                                    await dados.RepositorioAcervoEmprestimo.Atualizar(itemEmprestado);
                                }
                            }
                        }
                    }
                    else
                    {
                        item.AcervoSolicitacaoId = acervoSolicitacao.Id;
                    
                        await dados.RepositorioItem.Inserir(item);

                        var temInformacoesDeEmprestimo = itemProposto.DataEmprestimo.HasValue &&
                                                         itemProposto.DataDevolucao.HasValue &&
                                                         itemProposto.TipoAcervo.EhAcervoBibliografico();

                        if (item.TipoAtendimento.EhAtendimentoPresencial())
                        {
                            await regras.ServicoEvento.InserirEventoVisita(item.DataVisita!.Value, item.Id);

                            if (itemProposto.TipoAcervo.EhAcervoBibliografico())
                            {
                                if (temInformacoesDeEmprestimo)
                                {
                                    await dados.RepositorioAcervoEmprestimo.Inserir(new ()
                                    {
                                        AcervoSolicitacaoItemId = item.Id,
                                        DataEmprestimo = itemProposto.DataEmprestimo!.Value,
                                        DataDevolucao = itemProposto.DataDevolucao!.Value,
                                        Situacao = SituacaoEmprestimo.EMPRESTADO
                                    });
                                    await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO,item.AcervoId);
                                }
                                else
                                    await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO,item.AcervoId);
                            }
                        }
                    }
                }
                tran.Commit();

                return acervoSolicitacao.Id;
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

        private static void ValidarCamposDataVisitaEmprestimoDevolucao(AcervoSolicitacaoManualDTO acervoSolicitacaoManualDto)
        {
            if (acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhInvalido()))
                throw new NegocioException(MensagemNegocio.TIPO_ATENDIMENTO_INVALIDO);
            
            if (acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhAtendimentoViaEmail() && a.DataVisita.HasValue))
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_EMAIL_NAO_DEVEM_TER_DATA_ACERVO);
            
            if (acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAcervo.EhAcervoBibliografico()))
            {
                if (acervoSolicitacaoManualDto.Itens.Any(a=> a.DataEmprestimo.HasValue && a.DataEmprestimo.Value.Date > DateTimeExtension.HorarioBrasilia().Date))
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_NAO_PODE_SER_FUTURA);
                
                if (acervoSolicitacaoManualDto.Itens.Any(a=> a.DataEmprestimo.HasValue && a.DataVisita.HasValue && a.DataEmprestimo.Value.Date < a.DataVisita.Value.Date))
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_MENOR_QUE_DATA_VISITA);
                
                if (acervoSolicitacaoManualDto.Itens.Any(a=> a.DataDevolucao.HasValue && a.DataEmprestimo.HasValue && a.DataDevolucao.Value.Date < a.DataEmprestimo.Value.Date))
                    throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DO_EMPRESTIMO);                
            }
            else
            {
                if (acervoSolicitacaoManualDto.Itens.Any(a=> a.DataEmprestimo.HasValue || a.DataDevolucao.HasValue))
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_E_DEVOLUCAO_EXCLUSIVO_PARA_ACERVOS_BIBLIOGRAFICOS);
            }
        }
    }
}
