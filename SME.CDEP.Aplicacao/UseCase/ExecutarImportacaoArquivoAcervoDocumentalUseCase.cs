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
    public class ExecutarImportacaoArquivoAcervoDocumentalUseCase : ServicoImportacaoArquivoBase, IExecutarImportacaoArquivoAcervoDocumentalUseCase, IImportacaoArquivoAcervoDocumentalAuxiliar 
    {
        private readonly IServicoAcervoDocumental servicoAcervoDocumental;
        private readonly IMapper mapper;
        
        public ExecutarImportacaoArquivoAcervoDocumentalUseCase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoDocumental servicoAcervoDocumental,IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte, servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoAcervoDocumental = servicoAcervoDocumental ?? throw new ArgumentNullException(nameof(servicoAcervoDocumental));
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
            
            await CarregarDominiosDocumentais();

            var acervosDocumentaisLinhas = JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(importacaoArquivo.Conteudo);
            
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosDocumentaisLinhas);
            
            await PersistenciaAcervo(acervosDocumentaisLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosDocumentaisLinhas), acervosDocumentaisLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            return true;
        }

        public async Task CarregarDominiosDocumentais()
        {
            await CarregarTodosOsDominios();
            
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Autoria);
                
            await ObterMateriaisPorTipo(TipoMaterial.DOCUMENTAL);
        }

        public async Task PersistenciaAcervo(IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas)
        {
            foreach (var acervoDocumentalLinha in acervosDocumentalLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoDocumental = new AcervoDocumentalCadastroDTO()
                    {
                        Titulo = acervoDocumentalLinha.Titulo.Conteudo,
                        Codigo = acervoDocumentalLinha.Codigo.Conteudo,
                        CodigoNovo = acervoDocumentalLinha.CodigoNovo.Conteudo,
                        MaterialId = ObterMaterialDocumentalIdOuNuloPorValorDoCampo(acervoDocumentalLinha.Material.Conteudo),
                        IdiomaId = ObterIdiomaIdPorValorDoCampo(acervoDocumentalLinha.Idioma.Conteudo),
                        CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(acervoDocumentalLinha.Autor.Conteudo, TipoCreditoAutoria.Autoria, false),
                        Ano = acervoDocumentalLinha.Ano.Conteudo,
                        NumeroPagina = acervoDocumentalLinha.NumeroPaginas.Conteudo.ConverterParaInteiro(),
                        Volume = acervoDocumentalLinha.Volume.Conteudo,
                        Descricao = acervoDocumentalLinha.Descricao.Conteudo,
                        TipoAnexo = acervoDocumentalLinha.TipoAnexo.Conteudo,
                        Largura = acervoDocumentalLinha.Largura.Conteudo,
                        Altura = acervoDocumentalLinha.Altura.Conteudo,
                        TamanhoArquivo = acervoDocumentalLinha.TamanhoArquivo.Conteudo,
                        AcessoDocumentosIds = ObterAcessoDocumentosIdsPorValorDoCampo(acervoDocumentalLinha.AcessoDocumento.Conteudo),
                        Localizacao = acervoDocumentalLinha.Localizacao.Conteudo,
                        CopiaDigital = acervoDocumentalLinha.CopiaDigital.Conteudo.EhOpcaoSim(),
                        ConservacaoId = acervoDocumentalLinha.EstadoConservacao.Conteudo.EstaPreenchido() ? ObterConservacaoIdPorValorDoCampo(acervoDocumentalLinha.EstadoConservacao.Conteudo) : null
                    };
                    await servicoAcervoDocumental.Inserir(acervoDocumental);

                    acervoDocumentalLinha.DefinirLinhaComoSucesso();
                }
                catch (Exception ex)
                {
                    acervoDocumentalLinha.DefinirLinhaComoErro(ex.Message);
                }
            }
        } 

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoDocumentalLinhaDTO> linhas)
        {
            var autores = CreditosAutores.Where(w => w.Tipo == (int)TipoCreditoAutoria.Autoria).Select(s=> mapper.Map<IdNomeDTO>(s));
            var materiaisDocumentais = Materiais.Where(w => w.Tipo == (int)TipoMaterial.DOCUMENTAL).Select(s=> mapper.Map<IdNomeDTO>(s));
            
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Codigo, Constantes.CODIGO_ANTIGO);
                    ValidarPreenchimentoLimiteCaracteres(linha.CodigoNovo, Constantes.CODIGO_NOVO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Material,Constantes.MATERIAL);
                    ValidarConteudoCampoComDominio(linha.Material, materiaisDocumentais, Constantes.MATERIAL);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Idioma,Constantes.IDIOMA);
                    ValidarConteudoCampoComDominio(linha.Idioma, Idiomas, Constantes.IDIOMA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Autor,Constantes.AUTOR);
                    ValidarConteudoCampoListaComDominio(linha.Autor, autores, Constantes.AUTOR);

                    ValidarPreenchimentoLimiteCaracteres(linha.Ano,Constantes.ANO);
                    ValidarPreenchimentoLimiteCaracteres(linha.NumeroPaginas,Constantes.NUMERO_PAGINAS);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Volume,Constantes.VOLUME);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.TipoAnexo,Constantes.TIPO_ANEXO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.TamanhoArquivo,Constantes.TAMANHO_ARQUIVO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.AcessoDocumento,Constantes.ACESSO_DOCUMENTO);
                    ValidarConteudoCampoListaComDominio(linha.AcessoDocumento, AcessoDocumentos, Constantes.ACESSO_DOCUMENTO);

                    ValidarPreenchimentoLimiteCaracteres(linha.Localizacao,Constantes.LOCALIZACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.CopiaDigital,Constantes.COPIA_DIGITAL);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);
                    ValidarConteudoCampoComDominio(linha.EstadoConservacao, Conservacoes, Constantes.ESTADO_CONSERVACAO);
                    
                    if (linha.Codigo.Conteudo.NaoEstaPreenchido() && linha.CodigoNovo.Conteudo.NaoEstaPreenchido())
                    {
                        DefinirMensagemErro(linha.Codigo, string.Format(Constantes.CAMPO_X_NAO_PREENCHIDO,Constantes.CODIGO_ANTIGO));
                        DefinirMensagemErro(linha.CodigoNovo, string.Format(Constantes.CAMPO_X_NAO_PREENCHIDO,Constantes.CODIGO_NOVO));
                    }
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, linha.NumeroLinha, e.Message));
                }
            }
        }

        private bool PossuiErro(AcervoDocumentalLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.Codigo.PossuiErro 
                   || linha.CodigoNovo.PossuiErro 
                   || linha.Material.PossuiErro
                   || linha.Idioma.PossuiErro
                   || linha.Autor.PossuiErro 
                   || linha.Ano.PossuiErro 
                   || linha.NumeroPaginas.PossuiErro
                   || linha.Volume.PossuiErro
                   || linha.Descricao.PossuiErro
                   || linha.TipoAnexo.PossuiErro
                   || linha.Largura.PossuiErro 
                   || linha.Altura.PossuiErro
                   || linha.TamanhoArquivo.PossuiErro 
                   || linha.AcessoDocumento.PossuiErro 
                   || linha.Localizacao.PossuiErro 
                   || linha.CopiaDigital.PossuiErro 
                   || linha.EstadoConservacao.PossuiErro;
        }
    }
}