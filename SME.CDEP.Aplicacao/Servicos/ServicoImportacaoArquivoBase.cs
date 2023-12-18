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
        protected readonly IRepositorioImportacaoArquivo repositorioImportacaoArquivo;
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
        private readonly IMapper mapper;

        protected List<IdNomeTipoDTO> CreditosAutores { get; set; }

        public ServicoImportacaoArquivoBase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato, IMapper mapper)
        {
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
            AcessoDocumentos = new List<IdNomeDTO>();
            Formatos = new List<IdNomeTipoDTO>();
            Suportes = new List<IdNomeTipoDTO>();
            Cromias = new List<IdNomeDTO>();
            Conservacoes = new List<IdNomeDTO>();
        }

        protected async Task ObterDominiosImutaveis()
        {
            Suportes = (await servicoSuporte.ObterTodos()).Select(s=> mapper.Map<IdNomeTipoDTO>(s)).ToList();
            Cromias = (await servicoCromia.ObterTodos()).Select(s => mapper.Map<IdNomeDTO>(s)).ToList();
            Conservacoes = (await servicoConservacao.ObterTodos()).Select(s=> mapper.Map<IdNomeDTO>(s)).ToList();
            AcessoDocumentos = (await servicoAcessoDocumento.ObterTodos()).Select(s=> mapper.Map<IdNomeDTO>(s)).ToList();
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

        public async Task ValidarOuInserirMateriais(IEnumerable<string> materiais, TipoMaterial tipoMaterial)
        {
            foreach (var nome in materiais)
            {
                if (!await ExisteMaterialPorNomeETipo(tipoMaterial, nome))
                {
                    var id = await servicoMaterial.Inserir(new IdNomeTipoExcluidoDTO() { Nome = nome, Tipo = (int)tipoMaterial });
                    CachearMateriais(tipoMaterial, nome, id);
                }
            }
        }
        
        private async Task<bool> ExisteMaterialPorNomeETipo(TipoMaterial tipo, string nome)
        {
            var id = await servicoMaterial.ObterPorNomeETipo(nome, tipo);
            
            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearMateriais(tipo, nome, id);
            
            return existeRegistro;
        }

        private void CachearMateriais(TipoMaterial tipo, string nome, long id)
        {
            Materiais.Add(new IdNomeTipoDTO() { Id = id, Nome = nome, Tipo = (int)tipo });
        }

        public async Task ValidarOuInserirEditoras(IEnumerable<string> editoras)
        {
            foreach (var nome in editoras)
            {
                if (!await ExisteEditoraPorNome(nome))
                {
                    var id = await servicoEditora.Inserir(new IdNomeExcluidoAuditavelDTO() { Nome = nome});
                    CachearEditora(nome, id);
                }
            }
        }

        private async Task<bool> ExisteEditoraPorNome(string nome)
        {
            var id = await servicoEditora.ObterPorNome(nome);
            
            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearEditora(nome, id);
            
            return existeRegistro;
        }

        private void CachearEditora(string nome, long id)
        {
            Editoras.Add(new IdNomeDTO() { Id = id, Nome = nome });
        }

        public async Task ValidarOuInserirSeriesColecoes(IEnumerable<string> seriesColecoes)
        {
            foreach (var nome in seriesColecoes)
            {
                if (!await ExisteSerieColecaoPorNome(nome))
                {
                    var id  = await servicoSerieColecao.Inserir(new IdNomeExcluidoAuditavelDTO() { Nome = nome });
                    CachearSerieColecao(nome, id);
                }
            }
        }

        private async Task<bool> ExisteSerieColecaoPorNome(string nome)
        {
            var id = await servicoSerieColecao.ObterPorNome(nome);

            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearSerieColecao(nome, id);

            return existeRegistro;
        }

        private void CachearSerieColecao(string nome, long id)
        {
            SeriesColecoes.Add(new IdNomeDTO() { Id = id, Nome = nome });
        }
        
        public async Task ValidarOuInserirIdiomas(IEnumerable<string> idiomas)
        {
            foreach (var nome in idiomas)
            {
                if (!await ExisteIdiomaPorNome(nome))
                {
                    var id  = await servicoIdioma.Inserir(new IdNomeExcluidoDTO() { Nome = nome });
                    CachearIdioma(nome, id);
                }
            }
        }

        private async Task<bool> ExisteIdiomaPorNome(string nome)
        {
            var id = await servicoIdioma.ObterPorNome(nome);

            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearIdioma(nome, id);

            return existeRegistro;
        }

        private void CachearIdioma(string nome, long id)
        {
            Idiomas.Add(new IdNomeDTO() { Id = id, Nome = nome });
        }
        
        public async Task ValidarOuInserirAssuntos(IEnumerable<string> assuntos)
        {
            foreach (var nome in assuntos)
            {
                if (!await ExisteAssuntoPorNome(nome))
                {
                    var id  = await servicoAssunto.Inserir(new IdNomeExcluidoAuditavelDTO() { Nome = nome });
                    CachearAssunto(nome, id);
                }
            }
        }

        private async Task<bool> ExisteAssuntoPorNome(string nome)
        {
            var id = await servicoAssunto.ObterPorNome(nome);

            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearAssunto(nome, id);

            return existeRegistro;
        }

        private void CachearAssunto(string nome, long id)
        {
            Assuntos.Add(new IdNomeDTO() { Id = id, Nome = nome });
        }

        public async Task ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(IEnumerable<string> creditosAutoresCoautores, TipoCreditoAutoria tipoCreditoAutoria)
        {
            foreach (var nome in creditosAutoresCoautores)
            {
                if (!await ExisteCreditoAutorCoAutorPorNomeETipoAutoria(nome, tipoCreditoAutoria))
                {
                    var id  = await servicoCreditoAutor.Inserir(new IdNomeTipoExcluidoAuditavelDTO() { Nome = nome, Tipo = (int)tipoCreditoAutoria});
                    CachearCreditoAutor(nome, id, tipoCreditoAutoria);
                }
            }
        }

        private async Task<bool> ExisteCreditoAutorCoAutorPorNomeETipoAutoria(string nome, TipoCreditoAutoria tipoCreditoAutoria)
        {
            var id = await servicoCreditoAutor.ObterPorNomeETipo(nome, tipoCreditoAutoria);

            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearCreditoAutor(nome, id, tipoCreditoAutoria);

            return existeRegistro;
        }

        private void CachearCreditoAutor(string nome, long id, TipoCreditoAutoria tipoCreditoAutoria)
        {
            CreditosAutores.Add(new IdNomeTipoDTO() { Id = id, Nome = nome, Tipo = (int)tipoCreditoAutoria});
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
                                DefinirMensagemErro(campo, string.Format(Constantes.VALOR_X_DO_CAMPO_X_NAO_PERMITIDO_ESPERADO_X, item, nomeCampo, string.Join(", ", campo.ValoresPermitidos)));
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
        
        public async Task ValidarOuInserirAcessoDocumento(IEnumerable<string> acessoDocumentos)
        {
            foreach (var nome in acessoDocumentos)
            {
                if (!await ExisteAcessoDocumentoPorNome(nome))
                {
                    var id  = await servicoAcessoDocumento.Inserir(new IdNomeExcluidoDTO() { Nome = nome});
                    CachearAcessoDocumento(nome, id);
                }
            }
        }

        private async Task<bool> ExisteAcessoDocumentoPorNome(string nome)
        {
            var id = await servicoAcessoDocumento.ObterPorNome(nome);

            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearAcessoDocumento(nome, id);

            return existeRegistro;
        }

        private void CachearAcessoDocumento(string nome, long id)
        {
            AcessoDocumentos.Add(new IdNomeDTO() { Id = id, Nome = nome});
        }
        
        protected static LinhaConteudoAjustarRetornoDTO ObterConteudoMensagemStatus(LinhaConteudoAjustarDTO linha)
        {
            return new LinhaConteudoAjustarRetornoDTO()
            {
                Conteudo = linha.Conteudo, 
                PossuiErro = linha.PossuiErro, 
                Mensagem = linha.Mensagem
            };
        }
        
        protected static string ObterConteudoTexto(LinhaConteudoAjustarDTO linha)
        {
            return linha.Conteudo;
        }
        
        protected static bool? ObterConteudoSimNao(LinhaConteudoAjustarDTO linha)
        {
            var valoresPermitidos = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO };

            if (linha.Conteudo.EhNulo())
                return default;
            
            if (valoresPermitidos.Contains(linha.Conteudo.ToLower()))
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
        
        protected static double? ObterConteudoDoubleOuNulo(LinhaConteudoAjustarDTO linha)
        {
            return linha.PossuiErro ? null : linha.Conteudo.EstaPreenchido() ? double.Parse(linha.Conteudo) : null;
        }
        
        public async Task ValidarOuInserirFormato(IEnumerable<string> formatos, TipoFormato tipoFormato)
        {
            foreach (var nome in formatos)
            {
                if (!await ExisteFormatoPorNomeETipo(nome, tipoFormato))
                {
                    var id = await servicoFormato.Inserir(new IdNomeTipoExcluidoDTO() { Nome = nome, Tipo = (int)tipoFormato});
                    CachearFormatos(nome, id, tipoFormato);
                }
            }
        }
        
        private async Task<bool> ExisteFormatoPorNomeETipo(string nome, TipoFormato tipoFormato)
        {
            var id = await servicoFormato.ObterPorNomeETipo(nome, (int)tipoFormato);
            
            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearFormatos(nome, id, tipoFormato);
            
            return existeRegistro;
        }
        
        private void CachearFormatos(string nome, long id, TipoFormato tipoFormato)
        {
            Formatos.Add(new IdNomeTipoDTO() { Id = id, Nome = nome, Tipo = (int)tipoFormato });
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
                    var possuiNome = CreditosAutores.Any(f => f.Nome.Equals(item));
                    if (!possuiNome)
                    {
                        if (gerarExcecao)
                            throw new NegocioException(string.Format(MensagemNegocio.O_ITEM_X_DO_DOMINIO_X_NAO_ENCONTRADO, item, Constantes.CREDITOS_AUTORES));
                    }
                    else
                        retorno.Add(CreditosAutores.FirstOrDefault(f => f.Nome.SaoIguais(item) && f.Tipo.SaoIguais((int)tipoCreditoAutoria)).Id);    
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
                    var possuiNome = Assuntos.Any(f => f.Nome.Equals(item));
                    if (!possuiNome)
                    {
                        if (gerarExcecao)
                            throw new NegocioException(string.Format(MensagemNegocio.O_ITEM_X_DO_DOMINIO_X_NAO_ENCONTRADO, item, Constantes.ASSUNTOS));
                    }
                    else
                        retorno.Add(Assuntos.FirstOrDefault(f => f.Nome.SaoIguais(item)).Id);
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
                    var possuiNome = AcessoDocumentos.Any(f => f.Nome.Equals(item));
                    if (!possuiNome)
                    {
                        if (gerarExcecao)
                            throw new NegocioException(string.Format(Constantes.O_VALOR_X_DO_CAMPO_X_NAO_FOI_LOCALIZADO, item, Constantes.ACESSO_DOCUMENTO));
                    }
                    else
                        retorno.Add(AcessoDocumentos.FirstOrDefault(f => f.Nome.SaoIguais(item)).Id);
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
            var possuiNome = dominios.Any(f => f.Nome.SaoIguais(valorDoCampo) && f.Tipo == tipoFormato);

            if (!possuiNome)
                throw new NegocioException(string.Format(Constantes.O_VALOR_X_DO_CAMPO_X_NAO_FOI_LOCALIZADO, valorDoCampo, nomeDoCampo));
            
            return dominios.FirstOrDefault(f => f.Nome.SaoIguais(valorDoCampo) && f.Tipo == tipoFormato).Id;
        }
        
        private long? ObterIdentificadorIdOuNuloPorValorDoCampo(string valorDoCampo, List<IdNomeTipoDTO> dominios, int tipoFormato)
        {
            var possuiNome = dominios.Any(f => f.Nome.SaoIguais(valorDoCampo) && f.Tipo == tipoFormato);

            if (possuiNome)
                return dominios.FirstOrDefault(f => f.Nome.SaoIguais(valorDoCampo) && f.Tipo == tipoFormato).Id;    
                
            return null;
        }
        
        private long ObterIdentificadorIdPorValorDoCampo(string valorDoCampo, List<IdNomeDTO> dominios, string nomeDoCampo)
        {
            var possuiNome = dominios.Any(f => f.Nome.SaoIguais(valorDoCampo));

            if (!possuiNome)
                throw new NegocioException(string.Format(Constantes.O_VALOR_X_DO_CAMPO_X_NAO_FOI_LOCALIZADO, valorDoCampo, nomeDoCampo));
            
            return dominios.FirstOrDefault(f => f.Nome.SaoIguais(valorDoCampo)).Id;
        }
        
        private long? ObterIdentificadorIdOuNuloPorValorDoCampo(string valorDoCampo, List<IdNomeDTO> dominios)
        {
            var possuiNome = dominios.Any(f => f.Nome.SaoIguais(valorDoCampo));

            if (possuiNome)
                return dominios.FirstOrDefault(f => f.Nome.SaoIguais(valorDoCampo)).Id;    
                
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
                var valoresPermitidos = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO };
                
                if (!valoresPermitidos.Contains(valorDoCampo.ToLower()))
                    throw new NegocioException(string.Format(Constantes.VALOR_X_DO_CAMPO_X_NAO_PERMITIDO_ESPERADO_X,valorDoCampo, nomeDoCampo));

                return valorDoCampo.SaoIguais(Constantes.OPCAO_SIM);
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
                throw new NegocioException(string.Format(Constantes.ESSE_ARQUIVO_NAO_EH_ACERVO_X, tipoAcervoEsperado.Nome()));

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
            if (planilha.ObterValorDaCelula(numeroLinha, numeroDaColuna).ToLower().SaoDiferentes(nomeDaColuna.ToLower()))
                throw new NegocioException(string.Format(Constantes.A_PLANLHA_DE_ACERVO_X_NAO_TEM_O_NOME_DA_COLUNA_Y_NA_COLUNA_Z, nomeDoAcervo,nomeDaColuna,numeroDaColuna));
        }
        
        protected static AcervoLinhaRetornoSucessoDTO ObterLinhasComSucesso(string titulo, string tombo, int numeroLinha)
        {
            return new AcervoLinhaRetornoSucessoDTO()
            {
                Titulo = titulo,
                Tombo = tombo,
                NumeroLinha = numeroLinha,
            };
        }     
        

        protected async Task ObterSuportesPorTipo(TipoSuporte tipoSuporte)
        {
            Suportes = Suportes.Where(w=> w.Tipo == (int)tipoSuporte).ToList();
        }

        protected async Task ObterCreditosAutoresTipoAutoria(IEnumerable<string> creditosAutores, TipoCreditoAutoria tipoAutoria)
        {
            foreach (var nome in creditosAutores)
                await ExisteCreditoAutorCoAutorPorNomeETipoAutoria(nome, tipoAutoria);
        }

        protected async Task ObterMateriais(IEnumerable<string> materiais, TipoMaterial tipoMaterial)
        {
            foreach (var nome in materiais)
                await ExisteMaterialPorNomeETipo(tipoMaterial, nome);
        }
        
        protected async Task ObterEditoras(IEnumerable<string> editoras)
        {
            foreach (var nome in editoras)
                await ExisteEditoraPorNome(nome);
        }
        
        protected async Task ObterAssuntos(IEnumerable<string> assuntos)
        {
            foreach (var nome in assuntos)
                await ExisteAssuntoPorNome(nome);
        }
        
        protected async Task ObterSeriesColecoes(IEnumerable<string> seriesColecoes)
        {
            foreach (var nome in seriesColecoes)
                await ExisteSerieColecaoPorNome(nome);
        }
        
        protected async Task ObterIdiomas(IEnumerable<string> idiomas)
        {
            foreach (var nome in idiomas)
                await ExisteIdiomaPorNome(nome);
        }
        
        protected async Task ObterFormatos(IEnumerable<string> formatos, TipoFormato tipoFormato)
        {
            foreach (var nome in formatos)
                await ExisteFormatoPorNomeETipo(nome,tipoFormato);
        }
        protected string ObterSufixo(string codigo, string sufixo)
        {
            return codigo.Contains(sufixo) ? codigo : $"{codigo}{sufixo}";
        }
    }
}