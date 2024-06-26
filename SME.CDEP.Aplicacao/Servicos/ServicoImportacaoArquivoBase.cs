using System.Text.RegularExpressions;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoBase : IServicoImportacaoArquivoBase
    {
        private readonly IRepositorioParametroSistema repositorioParametroSistema;
        private readonly IServicoMaterial servicoMaterial;
        private readonly IServicoEditora servicoEditora;
        private readonly IServicoSerieColecao servicoSerieColecao;
        private readonly IServicoIdioma servicoIdioma;
        private readonly IServicoAssunto servicoAssunto;
        private readonly IServicoCreditoAutor servicoCreditoAutor;
        private readonly IServicoConservacao servicoConservacao;
        private readonly IServicoAcessoDocumento servicoAcessoDocumento;
        private readonly IServicoCromia servicoCromia;
        private readonly IServicoSuporte servicoSuporte;
        private readonly IServicoFormato servicoFormato;
        private readonly IMapper mapper;
        
        protected readonly IRepositorioImportacaoArquivo repositorioImportacaoArquivo;
        protected List<IdNomeTipoDTO> Materiais;
        protected List<IdNomeDTO> Editoras;
        protected List<IdNomeDTO> SeriesColecoes;
        protected List<IdNomeDTO> Idiomas;
        protected List<IdNomeDTO> Assuntos;
        protected List<IdNomeDTO> Conservacoes;
        protected List<IdNomeDTO> AcessoDocumentos;
        protected List<IdNomeDTO> Cromias;
        protected List<IdNomeTipoDTO> Suportes;
        protected List<IdNomeTipoDTO> Formatos;
        protected long LimiteAcervosImportadosViaPanilha;

        protected List<IdNomeTipoDTO> CreditosAutores { get; set; }
        protected List<IdNomeTipoDTO> CoAutores { get; set; }

        public ServicoImportacaoArquivoBase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato, IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
        {
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
            this.repositorioImportacaoArquivo = repositorioImportacaoArquivo ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
            this.servicoMaterial = servicoMaterial ?? throw new ArgumentNullException(nameof(servicoMaterial));
            this.servicoEditora = servicoEditora ?? throw new ArgumentNullException(nameof(servicoEditora));
            this.servicoSerieColecao = servicoSerieColecao ?? throw new ArgumentNullException(nameof(servicoSerieColecao));
            this.servicoIdioma = servicoIdioma ?? throw new ArgumentNullException(nameof(servicoIdioma));
            this.servicoAssunto = servicoAssunto ?? throw new ArgumentNullException(nameof(servicoAssunto));
            this.servicoCreditoAutor = servicoCreditoAutor ?? throw new ArgumentNullException(nameof(servicoCreditoAutor));
            this.servicoConservacao = servicoConservacao ?? throw new ArgumentNullException(nameof(servicoConservacao));
            this.servicoAcessoDocumento = servicoAcessoDocumento ?? throw new ArgumentNullException(nameof(servicoAcessoDocumento));
            this.servicoCromia = servicoCromia ?? throw new ArgumentNullException(nameof(servicoCromia));
            this.servicoSuporte = servicoSuporte ?? throw new ArgumentNullException(nameof(servicoSuporte));
            this.servicoFormato = servicoFormato ?? throw new ArgumentNullException(nameof(servicoFormato));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Materiais = new List<IdNomeTipoDTO>();
            Editoras = new List<IdNomeDTO>();
            SeriesColecoes = new List<IdNomeDTO>();
            Idiomas = new List<IdNomeDTO>();
            Assuntos = new List<IdNomeDTO>();
            CreditosAutores = new List<IdNomeTipoDTO>();
            CoAutores = new List<IdNomeTipoDTO>();
            AcessoDocumentos = new List<IdNomeDTO>();
            Formatos = new List<IdNomeTipoDTO>();
            Suportes = new List<IdNomeTipoDTO>();
            Cromias = new List<IdNomeDTO>();
            Conservacoes = new List<IdNomeDTO>();
        }

        protected async Task CarregarTodosOsDominios()
        {
            LimiteAcervosImportadosViaPanilha = long.Parse((await repositorioParametroSistema
                .ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, DateTimeExtension.HorarioBrasilia().Year)).Valor);
            
            Suportes = (await servicoSuporte.ObterTodos()).Select(s=> mapper.Map<IdNomeTipoDTO>(s)).ToList();
            Cromias = (await servicoCromia.ObterTodos()).Select(s => mapper.Map<IdNomeDTO>(s)).ToList();
            Conservacoes = (await servicoConservacao.ObterTodos()).Select(s=> mapper.Map<IdNomeDTO>(s)).ToList();
            AcessoDocumentos = (await servicoAcessoDocumento.ObterTodos()).Select(s=> mapper.Map<IdNomeDTO>(s)).ToList();
            CreditosAutores = (await servicoCreditoAutor.ObterTodos()).Select(s=> mapper.Map<IdNomeTipoDTO>(s)).ToList();
            Materiais = (await servicoMaterial.ObterTodos()).Select(s=> mapper.Map<IdNomeTipoDTO>(s)).ToList();
            Editoras = (await servicoEditora.ObterTodos()).Select(s=> mapper.Map<IdNomeDTO>(s)).ToList();
            SeriesColecoes = (await servicoSerieColecao.ObterTodos()).Select(s=> mapper.Map<IdNomeDTO>(s)).ToList();
            Idiomas = (await servicoIdioma.ObterTodos()).Select(s=> mapper.Map<IdNomeDTO>(s)).ToList();
            Assuntos = (await servicoAssunto.ObterTodos()).Select(s=> mapper.Map<IdNomeDTO>(s)).ToList();
            Formatos = (await servicoFormato.ObterTodos()).Select(s=> mapper.Map<IdNomeTipoDTO>(s)).ToList();
        }

        public void ValidarArquivo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new NegocioException(MensagemNegocio.ARQUIVO_VAZIO);
            
            if (file.ContentType.NaoEhArquivoXlsx())
                throw new NegocioException(MensagemNegocio.SOMENTE_ARQUIVO_XLSX_SUPORTADO);
        }

        public async Task<long> PersistirImportacao(ImportacaoArquivo importacaoArquivo)
        {
            return await repositorioImportacaoArquivo.Salvar(importacaoArquivo);
        }

        protected ImportacaoArquivo ObterImportacaoArquivoParaSalvar(string nomeDoArquivo, TipoAcervo tipoAcervo, string conteudo)
        {
            return new ImportacaoArquivo()
            {
                Nome = nomeDoArquivo,
                TipoAcervo = tipoAcervo,
                Status = ImportacaoStatus.Pendente,
                Conteudo = conteudo
            };
        }

        public async Task<long> AtualizarImportacao(long id, string conteudo,ImportacaoStatus? status = null)
        {
            var importacaoArquivo = await repositorioImportacaoArquivo.ObterPorId(id);
            if (status.HasValue)
                importacaoArquivo.Status = status.Value;
            
            importacaoArquivo.Conteudo = conteudo;
            
            return await repositorioImportacaoArquivo.Salvar(importacaoArquivo);
        }

        public void ValidarPreenchimentoLimiteCaracteres(LinhaConteudoAjustarDTO campo, string nomeCampo)
        {
            var conteudoCampo = campo.Conteudo.Trim();
            
            if (conteudoCampo.EstaPreenchido())
            {
                if (campo.FormatoTipoDeCampo.EhFormatoString())
                {
                    var conteudoCampoArray = conteudoCampo.FormatarTextoEmArray().ToList();
                    foreach (var item in conteudoCampoArray)
                    {
                        if (campo.ValoresPermitidos.NaoEhNulo())
                        {
                            if (!campo.ValoresPermitidos.Contains(campo.Conteudo.ToLower()))
                            {
                                DefinirMensagemErro(campo, string.Format(Constantes.VALOR_X_DO_CAMPO_X_NAO_PERMITIDO_ESPERADO_X, item, nomeCampo, string.Join(", ", campo.ValoresPermitidos)));
                                break;
                            }
                        }
                        if(campo.Conteudo.EstaPreenchido() && campo.ValidarComExpressaoRegular.EstaPreenchido() && !Regex.IsMatch(campo.Conteudo,campo.ValidarComExpressaoRegular))
                        {
                            DefinirMensagemErro(campo, campo.MensagemValidacao);
                            break;
                        }
                        
                        if (item.ValidarLimiteDeCaracteres(campo.LimiteCaracteres))
                            DefinirCampoValidado(campo);
                        else
                        {
                            DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_ATINGIU_LIMITE_CARACTERES, nomeCampo));
                            break;
                        }
                    }
                }
                else if (campo.FormatoTipoDeCampo.EhFormatoDouble())
                {
                    if (double.TryParse(conteudoCampo, out double formatoDouble))
                        DefinirCampoValidado(campo);
                    else
                        DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, nomeCampo));  
                }
                else if (campo.FormatoTipoDeCampo.EhFormatoInteiro())
                {
                    if (int.TryParse(conteudoCampo, out int formatoInteiro))
                        DefinirCampoValidado(campo);
                    else
                        DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, nomeCampo));  
                }
                else if (campo.FormatoTipoDeCampo.EhFormatoLongo())
                {
                    if (long.TryParse(conteudoCampo, out long formatoInteiro))
                        DefinirCampoValidado(campo);
                    else
                        DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, nomeCampo));  
                }
            }
            else
            {
                if (campo.EhCampoObrigatorio)
                    DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_NAO_PREENCHIDO, nomeCampo));
                else
                    DefinirCampoValidado(campo);
            }
        }

        protected void DefinirMensagemErro(LinhaConteudoAjustarDTO campo, string mensagemErro)
        {
            campo.PossuiErro = true;
            campo.Mensagem = mensagemErro;
        }
        
        private void DefinirCampoValidado(LinhaConteudoAjustarDTO campo)
        {
            campo.PossuiErro = false;
            campo.Mensagem = string.Empty;
        }
        
        protected static LinhaConteudoAjustarRetornoDTO ObterConteudoMensagemStatus(LinhaConteudoAjustarDTO linha)
        {
            return new LinhaConteudoAjustarRetornoDTO()
            {
                Conteudo = linha.Conteudo, 
                PossuiErro = linha.PossuiErro, 
                Mensagem = linha.Mensagem,
            };
        }
        
        protected static string ObterConteudoTexto(LinhaConteudoAjustarDTO linha)
        {
            return linha.Conteudo;
        }
        
        protected static bool? ObterConteudoSimNao(LinhaConteudoAjustarDTO linha)
        {
            var valoresPermitidos = new List<string>() { Constantes.OPCAO_SIM.RemoverAcentuacao(), Constantes.OPCAO_NAO.RemoverAcentuacao() };

            if (linha.Conteudo.EhNulo())
                return default;
            
            if (valoresPermitidos.Contains(linha.Conteudo.RemoverAcentuacaoFormatarMinusculo()))
                return linha.Conteudo.EhOpcaoSim();
            
            return default;
        } 
        
        protected static long? ObterConteudoLongoOuNulo(LinhaConteudoAjustarDTO linha)
        {
            return linha.PossuiErro ? null : linha.Conteudo.EstaPreenchido() ? long.Parse(linha.Conteudo) : null;
        }
        
        protected static int? ObterConteudoInteiroOuNulo(LinhaConteudoAjustarDTO linha)
        {
            return linha.PossuiErro ? null : linha.Conteudo.EstaPreenchido() ? linha.Conteudo.ConverterParaInteiro() : null;
        }
        
        protected long? ObterEditoraIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Editoras);
        }
        
        protected long? ObterSerieColecaoIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, SeriesColecoes);
        }
        
        protected long[] ObterCreditoAutoresIdsPorValorDoCampo(string valorDoCampo, TipoCreditoAutoria tipoCreditoAutoria, bool gerarExcecao = true)
        {
            if (valorDoCampo.NaoEstaPreenchido())
            {
                if (gerarExcecao)
                    throw new NegocioException(string.Format(Constantes.CAMPO_X_NAO_PREENCHIDO, valorDoCampo));
                
                return null;
            }

            var retorno = new List<long>();
            
            var conteudoCampoArray = valorDoCampo.FormatarTextoEmArray().ToList();
            foreach (var item in conteudoCampoArray)
            {
                if (item.EstaPreenchido())
                {
                    var itemSemAcentuacao = item.RemoverAcentuacao();
                    
                    var creditoAutor = CreditosAutores.FirstOrDefault(f => f.Nome.RemoverAcentuacao().SaoIguais(itemSemAcentuacao));
                    if (creditoAutor.EhNulo())
                    {
                        if (gerarExcecao)
                            throw new NegocioException(string.Format(MensagemNegocio.O_ITEM_X_DO_DOMINIO_X_NAO_ENCONTRADO, item, Constantes.CREDITOS_AUTORES));
                    }
                    else 
                        retorno.Add(creditoAutor.Id);    
                }
            }
            return retorno.Any() ? retorno.ToArray() : null;
        }
        
        protected long[] ObterAssuntosIdsPorValorDoCampo(string valorDoCampo, bool gerarExcecao = true)
        {
            if (valorDoCampo.NaoEstaPreenchido())
            {
                if (gerarExcecao)
                    throw new NegocioException(string.Format(Constantes.CAMPO_X_NAO_PREENCHIDO, valorDoCampo));
                
                return null;
            }

            var retorno = new List<long>();
            
            var conteudoCampoArray = valorDoCampo.FormatarTextoEmArray().ToList();
            foreach (var item in conteudoCampoArray)
            {
                if (item.EstaPreenchido())
                {
                    var itemSemAcentuacao = item.RemoverAcentuacao();
                    
                    var assunto = Assuntos.FirstOrDefault(f => f.Nome.RemoverAcentuacao().SaoIguais(itemSemAcentuacao));
                    if (assunto.EhNulo())
                    {
                        if (gerarExcecao)
                            throw new NegocioException(string.Format(MensagemNegocio.O_ITEM_X_DO_DOMINIO_X_NAO_ENCONTRADO, item, Constantes.ASSUNTOS));
                    }
                    else
                        retorno.Add(assunto.Id);
                }
            }
            
            return retorno.Any() ? retorno.ToArray() : null;
        }
        
        protected long[] ObterAcessoDocumentosIdsPorValorDoCampo(string valorDoCampo, bool gerarExcecao = true)
        {
            if (valorDoCampo.NaoEstaPreenchido())
            {
                if (gerarExcecao)
                    throw new NegocioException(string.Format(Constantes.CAMPO_X_NAO_PREENCHIDO, valorDoCampo));
                
                return null;
            }

            var retorno = new List<long>();
            
            var conteudoCampoArray = valorDoCampo.FormatarTextoEmArray().ToList();
            foreach (var item in conteudoCampoArray)
            {
                if (item.EstaPreenchido())
                {
                    var itemSemAcentuacao = item.RemoverAcentuacao();
                    
                    var acessoDocumento = AcessoDocumentos.FirstOrDefault(f => f.Nome.RemoverAcentuacao().SaoIguais(itemSemAcentuacao));
                    if (acessoDocumento.EhNulo())
                    {
                        if (gerarExcecao)
                            throw new NegocioException(string.Format(Constantes.O_VALOR_X_DO_CAMPO_X_NAO_FOI_LOCALIZADO, item, Constantes.ACESSO_DOCUMENTO));
                    }
                    else
                        retorno.Add(acessoDocumento.Id);
                }
            }
            return retorno.Any() ? retorno.ToArray() : null;
        }
        
        protected long ObterIdiomaIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Idiomas, Constantes.IDIOMA);
        }
        
        protected long? ObterIdiomaIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Idiomas);
        }
        
        protected long? ObterMaterialDocumentalIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Materiais, (int)TipoMaterial.DOCUMENTAL);
        }
        
        protected long ObterMaterialBibliograficoIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Materiais, Constantes.MATERIAL, (int)TipoMaterial.BIBLIOGRAFICO);
        }
        
        protected long? ObterMaterialBibliograficoIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Materiais, (int)TipoMaterial.BIBLIOGRAFICO);
        }
        
        protected long ObterConservacaoIdPorValorDoCampo(string valorDoCampo)
        {
           return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Conservacoes, Constantes.ESTADO_CONSERVACAO);
        }
        
        protected long? ObterConservacaoIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Conservacoes);
        }
        
        protected long ObterCromiaIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Cromias, Constantes.CROMIA);
        }
        
        protected long? ObterCromiaIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Cromias);
        }
        
        protected long ObterFormatoImagemIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Formatos, Constantes.FORMATO_IMAGEM, (int)TipoFormato.ACERVO_FOTOS);
        }
        
        protected long? ObterFormatoImagemIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Formatos, (int)TipoFormato.ACERVO_FOTOS);
        }
        
        protected long ObterSuporteImagemIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Suportes, Constantes.SUPORTE, (int)TipoSuporte.IMAGEM);
        }
        
        protected long? ObterSuporteImagemIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Suportes, (int)TipoSuporte.IMAGEM);
        }
        
        protected long ObterSuporteVideoIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Suportes, Constantes.SUPORTE, (int)TipoSuporte.VIDEO);
        }
        
        protected long? ObterSuporteVideoIdOuNuloPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdOuNuloPorValorDoCampo(valorDoCampo, Suportes, (int)TipoSuporte.VIDEO);
        }
        
        private long ObterIdentificadorIdPorValorDoCampo(string valorDoCampo, List<IdNomeTipoDTO> dominios, string nomeDoCampo, int tipoFormato)
        {
            var valorDoCampoSemAcentuacao = valorDoCampo.RemoverAcentuacao();
                
            var dominioEncontrado = dominios.FirstOrDefault(f => f.Nome.RemoverAcentuacao().SaoIguais(valorDoCampoSemAcentuacao) && f.Tipo == tipoFormato);

            if (dominioEncontrado.EhNulo())
                throw new NegocioException(string.Format(Constantes.O_VALOR_X_DO_CAMPO_X_NAO_FOI_LOCALIZADO, valorDoCampo, nomeDoCampo));
            
            return dominioEncontrado.Id;
        }
        
        private long? ObterIdentificadorIdOuNuloPorValorDoCampo(string valorDoCampo, List<IdNomeTipoDTO> dominios, int tipoFormato)
        {
            var valorDoCampoSemAcentuacao = valorDoCampo.RemoverAcentuacao();
            
            var dominioEncontrado = dominios.FirstOrDefault(f => f.Nome.RemoverAcentuacao().SaoIguais(valorDoCampoSemAcentuacao) && f.Tipo == tipoFormato);

            if (dominioEncontrado.NaoEhNulo())
                return dominioEncontrado.Id;    
                
            return null;
        }
        
        private long ObterIdentificadorIdPorValorDoCampo(string valorDoCampo, List<IdNomeDTO> dominios, string nomeDoCampo)
        {
            var valorDoCampoSemAcentuacao = valorDoCampo.RemoverAcentuacao();
            
            var dominioEncontrado = dominios.FirstOrDefault(f => f.Nome.RemoverAcentuacao().SaoIguais(valorDoCampoSemAcentuacao));

            if (dominioEncontrado.EhNulo())
                throw new NegocioException(string.Format(Constantes.O_VALOR_X_DO_CAMPO_X_NAO_FOI_LOCALIZADO, valorDoCampo, nomeDoCampo));
            
            return dominioEncontrado.Id;
        }
        
        private long? ObterIdentificadorIdOuNuloPorValorDoCampo(string valorDoCampo, List<IdNomeDTO> dominios)
        {
            var valorDoCampoSemAcento = valorDoCampo.RemoverAcentuacao();
            
            var dominioEncontrado = dominios.FirstOrDefault(f => f.Nome.RemoverAcentuacao().SaoIguais(valorDoCampoSemAcento));

            if (dominioEncontrado.NaoEhNulo())
                return dominioEncontrado.Id;    
                
            return null;
        }
        
        protected bool ObterCopiaDigitalPorValorDoCampo(string valorDoCampo)
        {
            return ObterValorBoleanoPorValorCampo(valorDoCampo,Constantes.COPIA_DIGITAL);
        }
        
        protected bool ObterAutorizaUsoDeImagemPorValorDoCampo(string valorDoCampo)
        {
            return ObterValorBoleanoPorValorCampo(valorDoCampo,Constantes.AUTORIZACAO_USO_DE_IMAGEM);
        }

        private bool ObterValorBoleanoPorValorCampo(string valorDoCampo, string nomeDoCampo)
        {
            if (valorDoCampo.EstaPreenchido())
            {
                var valoresPermitidos = new List<string>() { Constantes.OPCAO_SIM.RemoverAcentuacaoFormatarMinusculo(), Constantes.OPCAO_NAO.RemoverAcentuacaoFormatarMinusculo() };
                
                if (!valoresPermitidos.Contains(valorDoCampo.RemoverAcentuacaoFormatarMinusculo()))
                    throw new NegocioException(string.Format(Constantes.VALOR_X_DO_CAMPO_X_NAO_PERMITIDO_ESPERADO_X,valorDoCampo, nomeDoCampo));

                return valorDoCampo.RemoverAcentuacao().SaoIguais(Constantes.OPCAO_SIM.RemoverAcentuacao());
            }
            return false;
        }
        
        private async Task<ImportacaoArquivo> ObterImportacaoArquivoPorIdComValidacoes(long id,TipoAcervo tipoAcervoEsperado)
        {
            var arquivo = await repositorioImportacaoArquivo.ObterPorId(id);

            if (arquivo.EhNulo())
                throw new NegocioException(Constantes.ARQUIVO_NAO_ENCONTRADO);

            if (arquivo.Conteudo.EhNulo() || arquivo.Conteudo.NaoEstaPreenchido())
                throw new NegocioException(Constantes.CONTEUDO_DO_ARQUIVO_INVALIDO);

            if (arquivo.TipoAcervo.NaoSaoIguais(tipoAcervoEsperado))
                throw new NegocioException(string.Format(Constantes.ESSE_ARQUIVO_NAO_EH_ACERVO_X, tipoAcervoEsperado.Descricao()));

            return arquivo;
        }
        
        public async Task<bool> RemoverLinhaDoArquivo<T>(long id, LinhaDTO linhaDto, TipoAcervo tipoAcervoEsperado) where T: AcervoLinhaDTO
        {
            var conteudo = await ValidacoesImportacaoArquivo<T>(id, linhaDto.NumeroLinha, tipoAcervoEsperado);
            
            var novoConteudo = conteudo.Where(w => w.NumeroLinha.SaoDiferentes(linhaDto.NumeroLinha));

            if (!novoConteudo.Any())
                throw new NegocioException(Constantes.NAO_EH_POSSIVEL_EXCLUIR_A_UNICA_LINHA_DO_ARQUIVO);

            await AtualizarImportacao(id, JsonConvert.SerializeObject(novoConteudo));

            return true;
        }

        public async Task<bool> Remover(long id)
        {
            await repositorioImportacaoArquivo.Remover(id);
            return true;
        }

        public async Task<IEnumerable<T>> ValidacoesImportacaoArquivo<T>(long id, int linhaDoArquivo, TipoAcervo tipoAcervoEsperado) where T : AcervoLinhaDTO
        {
            var arquivo = await ObterImportacaoArquivoPorIdComValidacoes(id, tipoAcervoEsperado);

            var conteudo = JsonConvert.DeserializeObject<IEnumerable<T>>(arquivo.Conteudo);

            var existeLinha = conteudo?.Any(w => w.NumeroLinha.SaoIguais(linhaDoArquivo)) ?? false;

            if (!existeLinha)
                throw new NegocioException(Constantes.A_LINHA_INFORMADA_NAO_EXISTE_NO_ARQUIVO);
            
            return conteudo;
        }
        
        protected static void ValidarTituloDaColuna(IXLWorksheet planilha, int numeroLinha, string nomeDaColuna, int numeroDaColuna, string nomeDoAcervo)
        {
            if (planilha.ObterValorDaCelula(numeroLinha, numeroDaColuna).RemoverAcentuacao().SaoDiferentes(nomeDaColuna.RemoverAcentuacao()))
                throw new NegocioException(string.Format(Constantes.A_PLANLHA_DE_ACERVO_X_NAO_TEM_O_NOME_DA_COLUNA_Y_NA_COLUNA_Z, nomeDoAcervo,nomeDaColuna,numeroDaColuna));
        }
        
        protected AcervoLinhaRetornoSucessoDTO ObterLinhasComSucesso(string titulo, string tombo, int numeroLinha)
        {
            return new AcervoLinhaRetornoSucessoDTO()
            {
                Titulo = titulo,
                Tombo = tombo,
                NumeroLinha = numeroLinha,
            };
        }
        
        protected AcervoLinhaRetornoSucessoDTO ObterLinhasComSucessoSufixo(string titulo, string tombo, int numeroLinha, string sufixo)
        {
            return new AcervoLinhaRetornoSucessoDTO()
            {
                Titulo = titulo,
                Tombo = ObterSufixo(tombo,sufixo),
                NumeroLinha = numeroLinha,
            };
        }
        

        protected async Task ObterSuportesPorTipo(TipoSuporte tipoSuporte)
        {
            Suportes = Suportes.Where(w=> w.Tipo == (int)tipoSuporte).ToList();
        }
        
        protected async Task ObterMateriaisPorTipo(TipoMaterial tipoMaterial)
        {
            Materiais = Materiais.Where(w=> w.Tipo == (int)tipoMaterial).ToList();
        }
        
        protected async Task ObterCreditosAutoresPorTipo(TipoCreditoAutoria tipoCreditoAutoria)
        {
            CreditosAutores = CreditosAutores.Where(w=> w.Tipo == (int)tipoCreditoAutoria).ToList();
        }
        
        protected async Task ObterCoAutores(TipoCreditoAutoria tipoCreditoAutoria)
        {
            CoAutores = CreditosAutores.Where(w=> w.Tipo == (int)tipoCreditoAutoria).ToList();
        }
        
        protected async Task ObterFormatosPorTipo(TipoFormato tipoFormato)
        {
            Formatos = Formatos.Where(w=> w.Tipo == (int)tipoFormato).ToList();
        }
        protected string ObterSufixo(string codigo, string sufixo)
        {
            if (codigo.NaoEstaPreenchido())
                return default;
            
            return codigo.Contains(sufixo) ? codigo : $"{codigo}{sufixo}";
        }
        
        protected void ValidarConteudoCampoListaComDominio(LinhaConteudoAjustarDTO conteudoCampo, IEnumerable<IdNomeDTO> dominio, string nomeCampo)
        {
            var itensAAvaliar = conteudoCampo.Conteudo.FormatarTextoEmArray().UnificarPipe().SplitPipe().Distinct().ToList();
            
            var itensInexistentes = string.Empty;
            
            foreach (var itemAvaliar in itensAAvaliar.Where(linha => !dominio.Any(a=> a.Nome.RemoverAcentuacao().SaoIguais(linha.RemoverAcentuacao()))))
                itensInexistentes += itensInexistentes.NaoEstaPreenchido() ? itemAvaliar : $" | {itemAvaliar}";
                    
            if (itensInexistentes.EstaPreenchido())
                DefinirMensagemErro(conteudoCampo, string.Format(MensagemNegocio.O_ITEM_X_DO_DOMINIO_X_NAO_ENCONTRADO, itensInexistentes, nomeCampo));
        }

        protected void ValidarConteudoCampoComDominio(LinhaConteudoAjustarDTO campo, IEnumerable<IdNomeDTO> dominio,string nomeCampo)
        {
            if (campo.Conteudo.EstaPreenchido())
            {
                if (!dominio.Any(a=> a.Nome.RemoverAcentuacao().SaoIguais(campo.Conteudo.RemoverAcentuacao())))
                    DefinirMensagemErro(campo, string.Format(MensagemNegocio.O_ITEM_X_DO_DOMINIO_X_NAO_ENCONTRADO, campo.Conteudo, nomeCampo));    
            }
        }
        
        protected async Task ValidarQtdeLinhasImportadas(int totalLinhas)
        {
            LimiteAcervosImportadosViaPanilha = long.Parse((await repositorioParametroSistema
                .ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, DateTimeExtension.HorarioBrasilia().Year)).Valor);
            
            if (totalLinhas <= Constantes.INICIO_LINHA_TITULO)
                throw new NegocioException(MensagemNegocio.PLANILHA_VAZIA);
            
            var qtdeLinhasComAcervos = totalLinhas - 1;
            if (qtdeLinhasComAcervos > LimiteAcervosImportadosViaPanilha)
                throw new NegocioException(string.Format(MensagemNegocio.LIMITE_ACERVOS_IMPORTADOS_VIA_PLANILHA,LimiteAcervosImportadosViaPanilha));
        }
    }
}