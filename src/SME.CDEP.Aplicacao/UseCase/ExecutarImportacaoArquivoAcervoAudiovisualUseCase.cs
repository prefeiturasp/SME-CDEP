using AutoMapper;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ExecutarImportacaoArquivoAcervoAudiovisualUseCase : ServicoImportacaoArquivoBase, IExecutarImportacaoArquivoAcervoAudiovisualUseCase,IImportacaoArquivoAcervoAudiovisualAuxiliar
    {
        private readonly IServicoAcervoAudiovisual servicoAcervoAudiovisual;
        private readonly IMapper mapper;
        
        public ExecutarImportacaoArquivoAcervoAudiovisualUseCase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoAudiovisual servicoAcervoAudiovisual,IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte, servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoAcervoAudiovisual = servicoAcervoAudiovisual ?? throw new ArgumentNullException(nameof(servicoAcervoAudiovisual));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (param.Mensagem.EhNulo())
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            long importacaoArquivoId = 0;
            
            if (!long.TryParse(param.Mensagem.ToString(),out importacaoArquivoId))
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);
            
            var importacaoArquivo = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);
            
            if (importacaoArquivo.EhNulo())
                throw new NegocioException(MensagemNegocio.IMPORTACAO_NAO_LOCALIZADA);
            
            await CarregarDominiosAudiovisuais();

            var acervosAudiovisuaisLinhas = JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(importacaoArquivo.Conteudo);
            
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosAudiovisuaisLinhas);
            
            await PersistenciaAcervo(acervosAudiovisuaisLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosAudiovisuaisLinhas), acervosAudiovisuaisLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            return true;
        }

        public async Task CarregarDominiosAudiovisuais()
        {
            await CarregarTodosOsDominios();
            
            await ObterSuportesPorTipo(TipoSuporte.VIDEO);
            
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Credito);
        }

        public async Task PersistenciaAcervo(IEnumerable<AcervoAudiovisualLinhaDTO> acervosAudiovisualLinhas)
        {
            foreach (var acervoAudiovisualLinha in acervosAudiovisualLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoAudiovisual = new AcervoAudiovisualCadastroDTO()
                    {
                        Titulo = acervoAudiovisualLinha.Titulo.Conteudo,
                        Codigo = acervoAudiovisualLinha.Codigo.Conteudo,
                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoAudiovisualLinha.Credito.Conteudo.RemoverAcentuacaoFormatarMinusculo().FormatarTextoEmArray().Contains(f.Nome.RemoverAcentuacaoFormatarMinusculo()))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoAudiovisualLinha.Localizacao.Conteudo,
                        Procedencia = acervoAudiovisualLinha.Procedencia.Conteudo,
                        Ano = acervoAudiovisualLinha.Ano.Conteudo,
                        Copia = acervoAudiovisualLinha.Copia.Conteudo,
                        PermiteUsoImagem = ObterAutorizaUsoDeImagemPorValorDoCampo(acervoAudiovisualLinha.PermiteUsoImagem.Conteudo),
                        ConservacaoId = acervoAudiovisualLinha.EstadoConservacao.Conteudo.EstaPreenchido() ? ObterConservacaoIdPorValorDoCampo(acervoAudiovisualLinha.EstadoConservacao.Conteudo) : null,
                        Descricao = acervoAudiovisualLinha.Descricao.Conteudo,
                        SuporteId = ObterSuporteVideoIdPorValorDoCampo(acervoAudiovisualLinha.Suporte.Conteudo),
                        Duracao = acervoAudiovisualLinha.Duracao.Conteudo,
                        CromiaId = acervoAudiovisualLinha.Cromia.Conteudo.EstaPreenchido() ? ObterCromiaIdPorValorDoCampo(acervoAudiovisualLinha.Cromia.Conteudo) : null,
                        TamanhoArquivo = acervoAudiovisualLinha.TamanhoArquivo.Conteudo,
                        Acessibilidade = acervoAudiovisualLinha.Acessibilidade.Conteudo,
                        Disponibilizacao = acervoAudiovisualLinha.Disponibilizacao.Conteudo,
                    };
                    await servicoAcervoAudiovisual.Inserir(acervoAudiovisual);
        
                    acervoAudiovisualLinha.DefinirLinhaComoSucesso();
                }
                catch (Exception ex)
                {
                    acervoAudiovisualLinha.DefinirLinhaComoErro(ex.Message);
                }
            }
        } 
        
        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoAudiovisualLinhaDTO> linhas)
        {
            var creditos = CreditosAutores.Where(w => w.Tipo == (int)TipoCreditoAutoria.Credito).Select(s=> mapper.Map<IdNomeDTO>(s));
            var suportesDeVideo = Suportes.Where(w => w.Tipo == (int)TipoSuporte.VIDEO).Select(s=> mapper.Map<IdNomeDTO>(s));
            
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Codigo, Constantes.TOMBO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Credito, Constantes.CREDITO);
                    ValidarConteudoCampoListaComDominio(linha.Credito, creditos, Constantes.CREDITO);	

                    ValidarPreenchimentoLimiteCaracteres(linha.Localizacao,Constantes.LOCALIZACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Procedencia,Constantes.PROCEDENCIA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Ano,Constantes.ANO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Copia,Constantes.COPIA);
                    ValidarPreenchimentoLimiteCaracteres(linha.PermiteUsoImagem,Constantes.AUTORIZACAO_USO_DE_IMAGEM);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);
                    ValidarConteudoCampoComDominio(linha.EstadoConservacao, Conservacoes, Constantes.ESTADO_CONSERVACAO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Suporte,Constantes.SUPORTE);
                    ValidarConteudoCampoComDominio(linha.Suporte, suportesDeVideo, Constantes.SUPORTE);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Duracao,Constantes.DURACAO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Cromia,Constantes.CROMIA);
                    ValidarConteudoCampoComDominio(linha.Cromia, Cromias, Constantes.CROMIA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.TamanhoArquivo,Constantes.TAMANHO_ARQUIVO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Acessibilidade,Constantes.ACESSIBILIDADE);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Disponibilizacao,Constantes.DISPONIBILIDADE);
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, linha.NumeroLinha, e.Message));
                }
            }
        }

        private bool PossuiErro(AcervoAudiovisualLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.Codigo.PossuiErro 
                   || linha.Credito.PossuiErro
                   || linha.Localizacao.PossuiErro 
                   || linha.Procedencia.PossuiErro
                   || linha.Ano.PossuiErro
                   || linha.Copia.PossuiErro 
                   || linha.PermiteUsoImagem.PossuiErro 
                   || linha.EstadoConservacao.PossuiErro
                   || linha.Descricao.PossuiErro
                   || linha.Suporte.PossuiErro
                   || linha.Duracao.PossuiErro
                   || linha.Cromia.PossuiErro 
                   || linha.TamanhoArquivo.PossuiErro 
                   || linha.Acessibilidade.PossuiErro
                   || linha.Disponibilizacao.PossuiErro;
        }
    }
}