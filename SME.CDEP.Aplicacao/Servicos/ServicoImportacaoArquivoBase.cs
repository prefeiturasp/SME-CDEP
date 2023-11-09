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
        protected List<IdNomeTipoDTO> Materiais;
        protected List<IdNomeDTO> Editoras;
        protected List<IdNomeDTO> SeriesColecoes;
        protected List<IdNomeDTO> Idiomas;
        protected List<IdNomeDTO> Assuntos;
        protected List<IdNomeDTO> Conservacoes;
        protected List<IdNomeDTO> AcessoDocumentos;
        protected List<IdNomeDTO> Cromias;
        protected List<IdNomeTipoDTO> Suportes;
        
        protected List<IdNomeTipoDTO> CreditosAutores { get; set; }

        public ServicoImportacaoArquivoBase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte)
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
                if (!await ExisteMaterialPorNomeTipo(tipoMaterial, nome))
                {
                    var id = await servicoMaterial.Inserir(new IdNomeTipoExcluidoDTO() { Nome = nome, Tipo = (int)tipoMaterial });
                    CachearMateriais(tipoMaterial, nome, id);
                }
            }
        }
        
        private async Task<bool> ExisteMaterialPorNomeTipo(TipoMaterial tipo, string nome)
        {
            var id = await servicoMaterial.ObterPorNomeTipo(nome, tipo);
            
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

        public async Task ValidarOuInserirCreditoAutoresCoAutores(IEnumerable<string> creditosAutoresCoautores, TipoCreditoAutoria tipoCreditoAutoria)
        {
            foreach (var nome in creditosAutoresCoautores)
            {
                if (!await ExisteCreditoAutorCoAutorPorNome(nome, tipoCreditoAutoria))
                {
                    var id  = await servicoCreditoAutor.Inserir(new IdNomeTipoExcluidoAuditavelDTO() { Nome = nome, Tipo = (int)tipoCreditoAutoria});
                    CachearCreditoAutor(nome, id, tipoCreditoAutoria);
                }
            }
        }

        private async Task<bool> ExisteCreditoAutorCoAutorPorNome(string nome, TipoCreditoAutoria tipoCreditoAutoria)
        {
            var id = await servicoCreditoAutor.ObterPorNomeTipo(nome, tipoCreditoAutoria);

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
                if (!await ExisteSuportePorNomeTipo(nome,(int)tipoSuporte))
                {
                    var id = await servicoSuporte.Inserir(new IdNomeTipoExcluidoDTO() { Nome = nome, Tipo = (int)tipoSuporte});
                    CachearSuporte(nome, id, (int)tipoSuporte);
                }
            }
        }
        
        private async Task<bool> ExisteSuportePorNomeTipo(string nome, int tipoSuporte)
        {
            var id = await servicoSuporte.ObterPorNomePorTipo(nome,tipoSuporte);
            
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
    }
}