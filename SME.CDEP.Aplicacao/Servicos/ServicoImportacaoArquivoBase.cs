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
        
        protected List<IdNomeTipoDTO> CreditosAutores { get; set; }

        public ServicoImportacaoArquivoBase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato)
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
            Materiais = new List<IdNomeTipoDTO>();
            Editoras = new List<IdNomeDTO>();
            SeriesColecoes = new List<IdNomeDTO>();
            Idiomas = new List<IdNomeDTO>();
            Assuntos = new List<IdNomeDTO>();
            CreditosAutores = new List<IdNomeTipoDTO>();
            Conservacoes = new List<IdNomeDTO>();
            AcessoDocumentos = new List<IdNomeDTO>();
            Suportes = new List<IdNomeTipoDTO>();
            Cromias = new List<IdNomeDTO>();
            Formatos = new List<IdNomeTipoDTO>();
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

        public void ValidarPreenchimentoLimiteCaracteres(LinhaConteudoAjustarDTO campo, string nomeCampo, int numeroLinha)
        {
            var conteudoCampo = campo.Conteudo.Trim();
            
            if (conteudoCampo.EstaPreenchido())
            {
                if (campo.FormatoTipoDeCampo.EhFormatoString())
                {
                    var conteudoCampoArray = conteudoCampo.FormatarTextoEmArray().ToList();
                    foreach (var item in conteudoCampoArray)
                    {
                        if (item.ValidarLimiteDeCaracteres(campo.LimiteCaracteres))
                            DefinirCampoValidado(campo);
                        else
                        {
                            DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_ATINGIU_LIMITE_CARACTERES, nomeCampo),numeroLinha);
                            break;
                        }
                    }
                }
                else if (campo.FormatoTipoDeCampo.EhFormatoDouble())
                {
                    if (double.TryParse(conteudoCampo, out double formatoDouble))
                        DefinirCampoValidado(campo);
                    else
                        DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, nomeCampo),numeroLinha);  
                }
                else if (campo.FormatoTipoDeCampo.EhFormatoInteiro())
                {
                    if (int.TryParse(conteudoCampo, out int formatoInteiro))
                        DefinirCampoValidado(campo);
                    else
                        DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, nomeCampo),numeroLinha);  
                }
                else if (campo.FormatoTipoDeCampo.EhFormatoLongo())
                {
                    if (long.TryParse(conteudoCampo, out long formatoInteiro))
                        DefinirCampoValidado(campo);
                    else
                        DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, nomeCampo),numeroLinha);  
                }
            }
            else
            {
                if (campo.EhCampoObrigatorio)
                    DefinirMensagemErro(campo, string.Format(Constantes.CAMPO_X_NAO_PREENCHIDO, nomeCampo),numeroLinha);
                else
                    DefinirCampoValidado(campo);
            }
        }

        protected void DefinirMensagemErro(LinhaConteudoAjustarDTO campo, string mensagemErro, int numeroLinha)
        {
            campo.PossuiErro = true;
            campo.Mensagem = mensagemErro;
        }

        private void DefinirCampoValidado(LinhaConteudoAjustarDTO campo)
        {
            campo.PossuiErro = false;
            campo.Mensagem = string.Empty;
        }
        
        public async Task ValidarOuInserirConservacao(IEnumerable<string> conservacoes)
        {
            foreach (var nome in conservacoes)
            {
                if (!await ExisteConservacaoPorNome(nome))
                {
                    var id = await servicoConservacao.Inserir(new IdNomeExcluidoDTO() { Nome = nome });
                    CachearConservacoes(nome, id);
                }
            }
        }
        
        private async Task<bool> ExisteConservacaoPorNome(string nome)
        {
            var id = await servicoConservacao.ObterPorNome(nome);
            
            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearConservacoes(nome, id);
            
            return existeRegistro;
        }
        
        private void CachearConservacoes(string nome, long id)
        {
            Conservacoes.Add(new IdNomeDTO() { Id = id, Nome = nome });
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
        
        public async Task ValidarOuInserirSuporte(IEnumerable<string> suportes, TipoSuporte tipoSuporte)
        {
            foreach (var nome in suportes)
            {
                if (!await ExisteSuportePorNomeETipo(nome,(int)tipoSuporte))
                {
                    var id = await servicoSuporte.Inserir(new IdNomeTipoExcluidoDTO() { Nome = nome, Tipo = (int)tipoSuporte});
                    CachearSuporte(nome, id, (int)tipoSuporte);
                }
            }
        }
        
        private async Task<bool> ExisteSuportePorNomeETipo(string nome, int tipoSuporte)
        {
            var id = await servicoSuporte.ObterPorNomeETipo(nome,tipoSuporte);
            
            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearSuporte(nome, id,tipoSuporte);
            
            return existeRegistro;
        }
        
        private void CachearSuporte(string nome, long id, int tipoSuporte)
        {
            Suportes.Add(new IdNomeTipoDTO() { Id = id, Nome = nome, Tipo = tipoSuporte});
        }
       
        public async Task ValidarOuInserirCromia(IEnumerable<string> cromias)
        {
            foreach (var nome in cromias)
            {
                if (!await ExisteCromiaPorNome(nome))
                {
                    var id = await servicoCromia.Inserir(new IdNomeExcluidoDTO() { Nome = nome });
                    CachearCromia(nome, id);
                }
            }
        }
        
        private async Task<bool> ExisteCromiaPorNome(string nome)
        {
            var id = await servicoCromia.ObterPorNome(nome);
            
            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearCromia(nome, id);
            
            return existeRegistro;
        }
        
        private void CachearCromia(string nome, long id)
        {
            Cromias.Add(new IdNomeDTO() { Id = id, Nome = nome });
        }
        
        protected static LinhaConteudoAjustarRetornoDTO ObterConteudoMensagemStatus(LinhaConteudoAjustarDTO linha)
        {
            return new LinhaConteudoAjustarRetornoDTO()
            {
                Conteudo = linha.Conteudo, 
                Validado = linha.PossuiErro, 
                Mensagem = linha.Mensagem
            };
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
        
        protected long ObterEditoraIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Editoras, Constantes.EDITORA);
        }
        protected long ObterSerieColecaoIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, SeriesColecoes, Constantes.SERIE_COLECAO);
        }
        
        protected long ObterIdiomaIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Idiomas, Constantes.IDIOMA);
        }
        
        protected long ObterMaterialDocumentalIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Materiais, Constantes.MATERIAL, (int)TipoMaterial.DOCUMENTAL);
        }
        
        protected long ObterMaterialBibliograficoIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Materiais, Constantes.MATERIAL, (int)TipoMaterial.BIBLIOGRAFICO);
        }
        
        protected long ObterConservacaoIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Conservacoes, Constantes.ESTADO_CONSERVACAO);
        }
        
        protected long ObterCromiaIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Cromias, Constantes.CROMIA);
        }
        
        protected long ObterFormatoImagemIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Formatos, Constantes.FORMATO_IMAGEM, (int)TipoFormato.ACERVO_FOTOS);
        }
        
        protected long ObterSuporteImagemIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Suportes, Constantes.SUPORTE, (int)TipoSuporte.IMAGEM);
        }
        
        protected long ObterSuporteVideoIdPorValorDoCampo(string valorDoCampo)
        {
            return ObterIdentificadorIdPorValorDoCampo(valorDoCampo, Suportes, Constantes.SUPORTE, (int)TipoSuporte.VIDEO);
        }
        
        private long ObterIdentificadorIdPorValorDoCampo(string valorDoCampo, List<IdNomeTipoDTO> dominios, string nomeDoCampo, int tipoFormato)
        {
            var possuiNome = dominios.Any(f => f.Nome.Equals(valorDoCampo) && f.Tipo == tipoFormato);

            if (!possuiNome)
                throw new NegocioException(string.Format(Constantes.O_VALOR_DO_CAMPO_X_NAO_FOI_LOCALIZADO, nomeDoCampo));

            return dominios.FirstOrDefault(f => f.Nome.Equals(valorDoCampo) && f.Tipo == tipoFormato).Id;
        }
        
        private long ObterIdentificadorIdPorValorDoCampo(string valorDoCampo, List<IdNomeDTO> dominios, string nomeDoCampo)
        {
            var possuiNome = dominios.Any(f => f.Nome.Equals(valorDoCampo));

            if (!possuiNome)
                throw new NegocioException(string.Format(Constantes.O_VALOR_DO_CAMPO_X_NAO_FOI_LOCALIZADO, nomeDoCampo));

            return dominios.FirstOrDefault(f => f.Nome.Equals(valorDoCampo)).Id;
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
                    throw new NegocioException(string.Format(Constantes.VALOR_DO_CAMPO_X_NAO_PERMITIDO_ESPERADO_SIM_OU_NAO,nomeDoCampo));

                return valorDoCampo.ToLower().Equals(Constantes.OPCAO_SIM);
            }
            return false;
        }
    }
}