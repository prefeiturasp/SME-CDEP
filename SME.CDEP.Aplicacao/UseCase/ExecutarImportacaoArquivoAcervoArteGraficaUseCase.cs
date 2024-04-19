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
    public class ExecutarImportacaoArquivoAcervoArteGraficaUseCase : ServicoImportacaoArquivoBase, IExecutarImportacaoArquivoAcervoArteGraficaUseCase, IImportacaoArquivoAcervoArteGraficaAuxiliar 
    {
        private readonly IServicoAcervoArteGrafica servicoAcervoArteGrafica;
        private readonly IMapper mapper;
        
        public ExecutarImportacaoArquivoAcervoArteGraficaUseCase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoArteGrafica servicoAcervoArteGrafica,IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte, servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoAcervoArteGrafica = servicoAcervoArteGrafica ?? throw new ArgumentNullException(nameof(servicoAcervoArteGrafica));
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
            
            await CarregarDominiosArteGrafica();

            var acervosArtesGraficasLinhas = JsonConvert.DeserializeObject<IEnumerable<AcervoArteGraficaLinhaDTO>>(importacaoArquivo.Conteudo);
            
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosArtesGraficasLinhas);
            
            await PersistenciaAcervo(acervosArtesGraficasLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosArtesGraficasLinhas), acervosArtesGraficasLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            return true;
        }

        public async Task CarregarDominiosArteGrafica()
        {
            await CarregarTodosOsDominios();
            
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Credito);
            
            await ObterSuportesPorTipo(TipoSuporte.IMAGEM);
        }

        public async Task PersistenciaAcervo(IEnumerable<AcervoArteGraficaLinhaDTO> acervosArtesGraficasLinhas)
        {
            foreach (var acervoArteGraficaLinha in acervosArtesGraficasLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoArteGrafica = new AcervoArteGraficaCadastroDTO()
                    {
                        Titulo = acervoArteGraficaLinha.Titulo.Conteudo,
                        Codigo = acervoArteGraficaLinha.Codigo.Conteudo,
                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoArteGraficaLinha.Credito.Conteudo.RemoverAcentuacaoFormatarMinusculo().FormatarTextoEmArray().Contains(f.Nome.RemoverAcentuacaoFormatarMinusculo()))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoArteGraficaLinha.Localizacao.Conteudo,
                        Procedencia = acervoArteGraficaLinha.Procedencia.Conteudo,
                        Ano = acervoArteGraficaLinha.Ano.Conteudo,
                        CopiaDigital = ObterCopiaDigitalPorValorDoCampo(acervoArteGraficaLinha.CopiaDigital.Conteudo),
                        PermiteUsoImagem = ObterAutorizaUsoDeImagemPorValorDoCampo(acervoArteGraficaLinha.PermiteUsoImagem.Conteudo),
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoArteGraficaLinha.EstadoConservacao.Conteudo),
                        CromiaId = ObterCromiaIdPorValorDoCampo(acervoArteGraficaLinha.Cromia.Conteudo),
                        Largura = acervoArteGraficaLinha.Largura.Conteudo,
                        Altura = acervoArteGraficaLinha.Altura.Conteudo,
                        Diametro = acervoArteGraficaLinha.Diametro.Conteudo,
                        Tecnica = acervoArteGraficaLinha.Tecnica.Conteudo,
                        SuporteId = ObterSuporteImagemIdPorValorDoCampo(acervoArteGraficaLinha.Suporte.Conteudo),
                        Quantidade = acervoArteGraficaLinha.Quantidade.Conteudo.ObterLongoPorValorDoCampo(),
                        Descricao = acervoArteGraficaLinha.Descricao.Conteudo,
                    };
                    await servicoAcervoArteGrafica.Inserir(acervoArteGrafica);

                    acervoArteGraficaLinha.DefinirLinhaComoSucesso();
                }
                catch (Exception ex)
                {
                    acervoArteGraficaLinha.DefinirLinhaComoErro(ex.Message);
                }
            }
        } 

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoArteGraficaLinhaDTO> linhas)
        {
            var creditos = CreditosAutores.Where(w => w.Tipo == (int)TipoCreditoAutoria.Credito).Select(s=> mapper.Map<IdNomeDTO>(s));
            var suportesDeImagem = Suportes.Where(w => w.Tipo == (int)TipoSuporte.IMAGEM).Select(s=> mapper.Map<IdNomeDTO>(s));
            
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
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.CopiaDigital,Constantes.COPIA_DIGITAL);
                    ValidarPreenchimentoLimiteCaracteres(linha.PermiteUsoImagem,Constantes.AUTORIZACAO_USO_DE_IMAGEM);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);
                    ValidarConteudoCampoComDominio(linha.EstadoConservacao, Conservacoes, Constantes.ESTADO_CONSERVACAO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Cromia,Constantes.CROMIA);
                    ValidarConteudoCampoComDominio(linha.Cromia, Cromias, Constantes.CROMIA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Diametro,Constantes.DIAMETRO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Tecnica,Constantes.TECNICA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Suporte,Constantes.SUPORTE);
                    ValidarConteudoCampoComDominio(linha.Suporte, suportesDeImagem, Constantes.SUPORTE);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Quantidade,Constantes.QUANTIDADE);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO);
                    
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, linha.NumeroLinha, e.Message));
                }
            }
        }

        private bool PossuiErro(AcervoArteGraficaLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.Codigo.PossuiErro 
                   || linha.Credito.PossuiErro
                   || linha.Localizacao.PossuiErro 
                   || linha.Procedencia.PossuiErro
                   || linha.Ano.PossuiErro
                   || linha.CopiaDigital.PossuiErro 
                   || linha.PermiteUsoImagem.PossuiErro 
                   || linha.EstadoConservacao.PossuiErro
                   || linha.Cromia.PossuiErro 
                   || linha.Largura.PossuiErro 
                   || linha.Altura.PossuiErro
                   || linha.Diametro.PossuiErro 
                   || linha.Tecnica.PossuiErro
                   || linha.Suporte.PossuiErro
                   || linha.Quantidade.PossuiErro
                   || linha.Descricao.PossuiErro;
        }
    }
}