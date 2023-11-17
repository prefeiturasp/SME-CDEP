using System.Text;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoDocumental : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoDocumental 
    {
        private readonly IServicoAcervoDocumental servicoAcervoDocumental;
        
        public ServicoImportacaoArquivoAcervoDocumental(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoDocumental servicoAcervoDocumental)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato)
        {
            this.servicoAcervoDocumental = servicoAcervoDocumental ?? throw new ArgumentNullException(nameof(servicoAcervoDocumental));
        }

        public void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores)
        {
            CreditosAutores = creditosAutores;
        }

        public async Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo)
        {
            return await RemoverLinhaDoArquivo<AcervoDocumentalLinhaDTO>(id, linhaDoArquivo, TipoAcervo.DocumentacaoHistorica);
        }
        
        public async Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoDocumentalLinhaDTO>(id, linhaDoArquivo.NumeroLinha, TipoAcervo.DocumentacaoHistorica);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo.NumeroLinha));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }
        
        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.DocumentacaoHistorica);

            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivoImportado.Conteudo));
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosDocumentalLinhas = await LerPlanilha(file);

            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.DocumentacaoHistorica, JsonConvert.SerializeObject(acervosDocumentalLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosDocumentalLinhas);
            
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosDocumentalLinhas),ImportacaoStatus.ValidadoPreenchimentoValorFormatoQtdeCaracteres);
            
            await ValidacaoObterOuInserirDominios(acervosDocumentalLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosDocumentalLinhas),ImportacaoStatus.ValidacaoDominios);
            
            await PersistenciaAcervo(acervosDocumentalLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosDocumentalLinhas), acervosDocumentalLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);

            return await ObterRetornoImportacaoAcervo(arquivoImportado, acervosDocumentalLinhas);
        }

        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
            {
                await ObterMateriais(acervosDocumentalLinhas.Select(s => s.Material.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoMaterial.BIBLIOGRAFICO);
                await ObterIdiomas(acervosDocumentalLinhas.Select(s => s.Idioma.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                await ObterConservacoes(acervosDocumentalLinhas.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                await ObterAcessoDocumentos(acervosDocumentalLinhas.Select(s => s.AcessoDocumento.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()));
                await ObterCreditosAutoresTipoAutoria(acervosDocumentalLinhas.Select(s => s.Autor.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()), TipoCreditoAutoria.Autoria);
            }
            
            var acervoDocumentalRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosDocumentalLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s,arquivoImportado.TipoAcervo)),
                Sucesso = acervosDocumentalLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s=> ObterLinhasComSucesso(s.Titulo.Conteudo, ObterCodigo(s), s.NumeroLinha)),
            };
            return acervoDocumentalRetorno;
        }

        private string ObterCodigo(AcervoDocumentalLinhaDTO s)
        {
            if (s.CodigoAntigo.Conteudo.EstaPreenchido() && s.CodigoNovo.Conteudo.EstaPreenchido())
                return $"{s.CodigoAntigo.Conteudo}/{s.CodigoNovo.Conteudo}";
            
            if (s.CodigoAntigo.Conteudo.EstaPreenchido())
                return s.CodigoAntigo.Conteudo;
            
            return s.CodigoNovo.Conteudo.EstaPreenchido() ? s.CodigoNovo.Conteudo : default;
        }
        
        private AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoDocumentalLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterCodigo(linha),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoDocumentalDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoDocumentalDTO ObterAcervoDocumentalDto(AcervoDocumentalLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoDocumentalDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Codigo = ObterConteudoTexto(linha.CodigoAntigo),
                CodigoNovo = ObterConteudoTexto(linha.CodigoNovo),
                TipoAcervoId = (int)tipoAcervo,
                MaterialId = ObterMaterialBibliograficoIdPorValorDoCampo(linha.Material.Conteudo,false),
                IdiomaId = ObterIdiomaIdPorValorDoCampo(linha.Idioma.Conteudo,false),
                Ano = ObterConteudoTexto(linha.Ano),
                NumeroPagina = ObterConteudoTexto(linha.NumeroPaginas),
                Volume = ObterConteudoTexto(linha.Volume),
                Descricao = ObterConteudoTexto(linha.Descricao),
                TipoAnexo = ObterConteudoTexto(linha.TipoAnexo),
                Largura = ObterConteudoDoubleOuNulo(linha.Largura),
                Altura = ObterConteudoDoubleOuNulo(linha.Altura),
                TamanhoArquivo = ObterConteudoTexto(linha.TamanhoArquivo),
                Localizacao = ObterConteudoTexto(linha.Localizacao),
                CopiaDigital = ObterConteudoBooleano(linha.CopiaDigital),
                ConservacaoId = ObterConservacaoIdPorValorDoCampo(linha.EstadoConservacao.Conteudo,false),
                AcessoDocumentosIds = ObterAcessoDocumentosIdsPorValorDoCampo(linha.AcessoDocumento.Conteudo,false),
                CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(linha.Autor.Conteudo, TipoCreditoAutoria.Autoria),
            };
        }

        private AcervoDocumentalLinhaRetornoDTO ObterLinhasComErros(AcervoDocumentalLinhaDTO s)
        {
            return new AcervoDocumentalLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                CodigoAntigo = ObterConteudoMensagemStatus(s.CodigoAntigo),
                CodigoNovo = ObterConteudoMensagemStatus(s.CodigoNovo),
                Material = ObterConteudoMensagemStatus(s.Material),
                Idioma = ObterConteudoMensagemStatus(s.Idioma),
                Autor = ObterConteudoMensagemStatus(s.Autor),
                Ano = ObterConteudoMensagemStatus(s.Ano),
                NumeroPaginas = ObterConteudoMensagemStatus(s.NumeroPaginas),
                Volume = ObterConteudoMensagemStatus(s.Volume),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                TipoAnexo = ObterConteudoMensagemStatus(s.TipoAnexo),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                TamanhoArquivo = ObterConteudoMensagemStatus(s.TamanhoArquivo),
                AcessoDocumento = ObterConteudoMensagemStatus(s.AcessoDocumento),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                CopiaDigital = ObterConteudoMensagemStatus(s.CopiaDigital),
                EstadoConservacao = ObterConteudoMensagemStatus(s.EstadoConservacao),
                NumeroLinha = s.NumeroLinha,
                Status = ImportacaoStatus.Erros,
                Mensagem = s.Mensagem.NaoEstaPreenchido() ? ObterMensagemErroLinha(s) : s.Mensagem,
            };
        }
        
        private string ObterMensagemErroLinha(AcervoDocumentalLinhaDTO acervoDocumentalLinhaDTO)
        {
	        var mensagemErro = new StringBuilder();

	        if (acervoDocumentalLinhaDTO.Titulo.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Titulo.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.CodigoAntigo.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.CodigoAntigo.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.CodigoNovo.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.CodigoNovo.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Material.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Material.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Idioma.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Idioma.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Autor.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Autor.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Ano.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Ano.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.NumeroPaginas.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.NumeroPaginas.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Volume.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Volume.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.Descricao.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Descricao.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.TipoAnexo.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.TipoAnexo.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.Largura.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Largura.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Altura.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Altura.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.TamanhoArquivo.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.TamanhoArquivo.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.AcessoDocumento.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.AcessoDocumento.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Localizacao.PossuiErro)
		        mensagemErro.AppendLine(acervoDocumentalLinhaDTO.Localizacao.Mensagem);
            
            if (acervoDocumentalLinhaDTO.CopiaDigital.PossuiErro)
                mensagemErro.AppendLine(acervoDocumentalLinhaDTO.CopiaDigital.Mensagem);
            
            if (acervoDocumentalLinhaDTO.EstadoConservacao.PossuiErro)
                mensagemErro.AppendLine(acervoDocumentalLinhaDTO.EstadoConservacao.Mensagem);

	        return mensagemErro.ToString();
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
                        Codigo = acervoDocumentalLinha.CodigoAntigo.Conteudo,
                        CodigoNovo = acervoDocumentalLinha.CodigoNovo.Conteudo,
                        MaterialId = ObterMaterialDocumentalIdOuNuloPorValorDoCampo(acervoDocumentalLinha.Material.Conteudo,false),
                        IdiomaId = ObterIdiomaIdPorValorDoCampo(acervoDocumentalLinha.Idioma.Conteudo,false),
                        CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(acervoDocumentalLinha.Autor.Conteudo, TipoCreditoAutoria.Autoria),
                        Ano = acervoDocumentalLinha.Ano.Conteudo,
                        NumeroPagina = acervoDocumentalLinha.NumeroPaginas.Conteudo,
                        Volume = acervoDocumentalLinha.Volume.Conteudo,
                        Descricao = acervoDocumentalLinha.Descricao.Conteudo,
                        TipoAnexo = acervoDocumentalLinha.TipoAnexo.Conteudo,
                        Largura = acervoDocumentalLinha.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Altura = acervoDocumentalLinha.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        TamanhoArquivo = acervoDocumentalLinha.TamanhoArquivo.Conteudo,
                        AcessoDocumentosIds = ObterAcessoDocumentosIdsPorValorDoCampo(acervoDocumentalLinha.AcessoDocumento.Conteudo),
                        Localizacao = acervoDocumentalLinha.Localizacao.Conteudo,
                        CopiaDigital = acervoDocumentalLinha.CopiaDigital.Conteudo.EhOpcaoSim(),
                        ConservacaoId = ObterConservacaoIdOuNuloPorValorDoCampo(acervoDocumentalLinha.EstadoConservacao.Conteudo, false)
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
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO);
                    ValidarPreenchimentoLimiteCaracteres(linha.CodigoAntigo, Constantes.CODIGO_ANTIGO);
                    ValidarPreenchimentoLimiteCaracteres(linha.CodigoNovo, Constantes.CODIGO_NOVO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Material,Constantes.MATERIAL);
                    ValidarPreenchimentoLimiteCaracteres(linha.Idioma,Constantes.IDIOMA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Autor,Constantes.AUTOR);
                    ValidarPreenchimentoLimiteCaracteres(linha.Ano,Constantes.ANO);
                    ValidarPreenchimentoLimiteCaracteres(linha.NumeroPaginas,Constantes.NUMERO_PAGINAS);
                    ValidarPreenchimentoLimiteCaracteres(linha.Volume,Constantes.VOLUME);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.TipoAnexo,Constantes.TIPO_ANEXO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.TamanhoArquivo,Constantes.TAMANHO_ARQUIVO);
                    ValidarPreenchimentoLimiteCaracteres(linha.AcessoDocumento,Constantes.ACESSO_DOCUMENTO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Localizacao,Constantes.LOCALIZACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.CopiaDigital,Constantes.COPIA_DIGITAL);
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);

                    if (linha.CodigoAntigo.Conteudo.NaoEstaPreenchido() && linha.CodigoNovo.Conteudo.NaoEstaPreenchido())
                    {
                        DefinirMensagemErro(linha.CodigoAntigo, Constantes.CAMPO_CODIGO_ANTIGO_OU_CODIGO_NOVO_DEVE_SER_PREENCHIDO);
                        DefinirMensagemErro(linha.CodigoNovo, Constantes.CAMPO_CODIGO_ANTIGO_OU_CODIGO_NOVO_DEVE_SER_PREENCHIDO);
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
                   || linha.CodigoAntigo.PossuiErro 
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

        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoDocumentalLinhaDTO> linhas)
        {
            var linhasComsucesso = linhas.Where(w => !w.PossuiErros);

            try
            {
                await ValidarOuInserirMateriais(linhasComsucesso.Select(s => s.Material.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoMaterial.DOCUMENTAL);

                await ValidarOuInserirIdiomas(linhasComsucesso.Select(s => s.Idioma.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                
                await ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(linhasComsucesso.Select(s => s.Autor.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()), TipoCreditoAutoria.Autoria);
                
                await ValidarOuInserirAcessoDocumento(linhasComsucesso.Select(s => s.AcessoDocumento.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()));
                
                await ValidarOuInserirConservacao(linhasComsucesso.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                
            }
            catch (Exception e)
            {
                foreach (var linha in linhas)
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X, e.Message));
            }
        }

        private async Task<IEnumerable<AcervoDocumentalLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoDocumentalLinhaDTO>();

            var stream = file.OpenReadStream();

            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                var totalLinhas = planilha.Rows().Count();
                
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);

                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoDocumentalLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        CodigoAntigo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_ANTIGO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        CodigoNovo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_NOVO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        Material = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_MATERIAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        },
                        Idioma = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_IDIOMA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Autor = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_AUTOR),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Ano = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_ANO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        NumeroPaginas = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_NUMERO_PAGINAS),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_4,
                            EhCampoObrigatorio = true
                        },
                        Volume = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_VOLUME),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_DESCRICAO),
                            EhCampoObrigatorio = true
                        },
                        TipoAnexo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_TIPO_ANEXO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_LARGURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_ALTURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        TamanhoArquivo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_TAMANHO_ARQUIVO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15
                        },
                        AcessoDocumento = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_ACESSO_DOCUMENTO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Localizacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_LOCALIZACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        CopiaDigital = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_COPIA_DIGITAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3, //aqui nas planilhas, não parece um campo booleano
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        }
                    });
                }
            }

            return linhas;
        }
        
        private void ValidarOrdemColunas(IXLWorksheet planilha, int numeroLinha)
        {
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TITULO, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_TITULO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CODIGO_ANTIGO, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_ANTIGO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CODIGO_NOVO, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_NOVO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_MATERIAL, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_MATERIAL, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_IDIOMA, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_IDIOMA, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_AUTOR, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_AUTOR, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ANO, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_ANO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_NUMERO_PAGINAS,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_NUMERO_PAGINAS, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_VOLUME,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_VOLUME, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DESCRICAO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_DESCRICAO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TIPO_DE_ANEXO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_TIPO_ANEXO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_LARGURA, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_ALTURA, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TAMANHO_DO_ARQUIVO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_TAMANHO_ARQUIVO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ACESSO_DO_DOCUMENTO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_ACESSO_DOCUMENTO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_LOCALIZACAO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_LOCALIZACAO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_COPIA_DIGITAL,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_COPIA_DIGITAL, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_ESTADO_CONSERVACAO, Constantes.DOCUMENTAL);
        }
    }
}