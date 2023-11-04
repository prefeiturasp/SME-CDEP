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
        private readonly IRepositorioImportacaoArquivo repositorioImportacaoArquivo;
        private readonly IRepositorioMaterial repositorioMaterial;
        private readonly IRepositorioEditora repositorioEditora;
        private readonly IRepositorioSerieColecao repositorioSerieColecao;
        private readonly IRepositorioIdioma repositorioIdioma;
        private readonly IRepositorioAssunto repositorioAssunto;
        private readonly IRepositorioCreditoAutor repositorioCreditoAutor;
        private readonly IMapper mapper;
        protected List<IdNomeTipoDTO> Materiais;
        protected List<IdNomeDTO> Editoras;
        protected List<IdNomeDTO> SeriesColecoes;
        protected List<IdNomeDTO> Idiomas;
        protected List<IdNomeDTO> Assuntos;
        protected List<IdNomeTipoDTO> CreditosAutores { get; set; }

        public ServicoImportacaoArquivoBase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IMapper mapper,
            IRepositorioMaterial repositorioMaterial,IRepositorioEditora repositorioEditora,IRepositorioSerieColecao repositorioSerieColecao,
            IRepositorioIdioma repositorioIdioma,IRepositorioAssunto repositorioAssunto,IRepositorioCreditoAutor repositorioCreditoAutor)
        {
            this.repositorioImportacaoArquivo = repositorioImportacaoArquivo ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
            this.repositorioMaterial = repositorioMaterial ?? throw new ArgumentNullException(nameof(repositorioMaterial));
            this.repositorioEditora = repositorioEditora ?? throw new ArgumentNullException(nameof(repositorioEditora));
            this.repositorioSerieColecao = repositorioSerieColecao ?? throw new ArgumentNullException(nameof(repositorioSerieColecao));
            this.repositorioIdioma = repositorioIdioma ?? throw new ArgumentNullException(nameof(repositorioIdioma));
            this.repositorioAssunto = repositorioAssunto ?? throw new ArgumentNullException(nameof(repositorioAssunto));
            this.repositorioCreditoAutor = repositorioCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioCreditoAutor));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Materiais = new List<IdNomeTipoDTO>();
            Editoras = new List<IdNomeDTO>();
            SeriesColecoes = new List<IdNomeDTO>();
            Idiomas = new List<IdNomeDTO>();
            Assuntos = new List<IdNomeDTO>();
            CreditosAutores = new List<IdNomeTipoDTO>();
        }

        public void ValidarArquivo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new NegocioException(MensagemNegocio.ARQUIVO_VAZIO);
            
            if (file.ContentType.NaoEhArquivoXlsx())
                throw new NegocioException(MensagemNegocio.SOMENTE_ARQUIVO_XLSX_SUPORTADO);
        }

        public async Task<long> PersistirImportacao(string nomeDoArquivo, TipoAcervo tipoAcervo, string conteudo)
        {
            var importacaoArquivo = new ImportacaoArquivo()
            {
                Nome = nomeDoArquivo,
                TipoAcervo = tipoAcervo,
                Status = ImportacaoStatus.Pendente,
                Conteudo = conteudo
            };
            
            return await repositorioImportacaoArquivo.Salvar(importacaoArquivo);
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
                if (!await ExisteMaterialPorNomeTipo(tipoMaterial, nome))
                {
                    var id = await repositorioMaterial.Inserir(new Material() { Nome = nome, Tipo = tipoMaterial });
                    CachearMateriais(tipoMaterial, nome, id);
                }
            }
        }
        
        private async Task<bool> ExisteMaterialPorNomeTipo(TipoMaterial tipo, string nome)
        {
            var id = await repositorioMaterial.ObterPorNomeTipo(nome, tipo);
            
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
                    var id = await repositorioEditora.Inserir(new Editora() { Nome = nome });
                    CachearEditora(nome, id);
                }
            }
        }

        private async Task<bool> ExisteEditoraPorNome(string nome)
        {
            var id = await repositorioEditora.ObterPorNome(nome);
            
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
                    var id  = await repositorioSerieColecao.Inserir(new SerieColecao() { Nome = nome });
                    CachearSerieColecao(nome, id);
                }
            }
        }

        private async Task<bool> ExisteSerieColecaoPorNome(string nome)
        {
            var id = await repositorioSerieColecao.ObterPorNome(nome);

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
                    var id  = await repositorioIdioma.Inserir(new Idioma() { Nome = nome });
                    CachearIdioma(nome, id);
                }
            }
        }

        private async Task<bool> ExisteIdiomaPorNome(string nome)
        {
            var id = await repositorioIdioma.ObterPorNome(nome);

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
                    var id  = await repositorioAssunto.Inserir(new Assunto() { Nome = nome });
                    CachearAssunto(nome, id);
                }
            }
        }

        private async Task<bool> ExisteAssuntoPorNome(string nome)
        {
            var id = await repositorioAssunto.ObterPorNome(nome);

            var existeRegistro = id.EhMaiorQueZero();
            
            if (existeRegistro)
                CachearAssunto(nome, id);

            return existeRegistro;
        }

        private void CachearAssunto(string nome, long id)
        {
            Assuntos.Add(new IdNomeDTO() { Id = id, Nome = nome });
        }

        public async Task ValidarOuInserirCreditoAutoresCoAutores(IEnumerable<string> creditosAutoresCoautores, TipoCreditoAutoria tipoCreditoAutoria)
        {
            foreach (var nome in creditosAutoresCoautores)
            {
                if (!await ExisteCreditoAutorCoAutorPorNome(nome, tipoCreditoAutoria))
                {
                    var id  = await repositorioCreditoAutor.Inserir(new CreditoAutor() { Nome = nome, Tipo = tipoCreditoAutoria});
                    CachearCreditoAutor(nome, id, tipoCreditoAutoria);
                }
            }
        }

        private async Task<bool> ExisteCreditoAutorCoAutorPorNome(string nome, TipoCreditoAutoria tipoCreditoAutoria)
        {
            var id = await repositorioCreditoAutor.ObterPorNomeTipo(nome, tipoCreditoAutoria);

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
            campo.Validado = false;
            campo.Mensagem = mensagemErro;
        }

        private void DefinirCampoValidado(LinhaConteudoAjustarDTO campo)
        {
            campo.Validado = true;
            campo.Mensagem = string.Empty;
        }
    }
}