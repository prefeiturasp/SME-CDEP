using System.Text;
using AutoMapper;
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
    public class ServicoImportacaoArquivoAcervoFotografico : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoFotografico 
    {
        private readonly IServicoAcervoFotografico servicoAcervoFotografico;
        
        public ServicoImportacaoArquivoAcervoFotografico(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte, IServicoFormato servicoFormato,IServicoAcervoFotografico servicoAcervoFotografico)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato)
        {
            this.servicoAcervoFotografico = servicoAcervoFotografico ?? throw new ArgumentNullException(nameof(servicoAcervoFotografico));
        }

        public void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores)
        {
            CreditosAutores = creditosAutores;
        }
        
        public async Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo)
        {
            return await RemoverLinhaDoArquivo<AcervoFotograficoLinhaDTO>(id, linhaDoArquivo, TipoAcervo.Fotografico);
        }
        
        public async Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoFotograficoLinhaDTO>(id, linhaDoArquivo.NumeroLinha, TipoAcervo.Fotografico);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo.NumeroLinha));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Fotografico);
            
            if (arquivoImportado.EhNulo())
                return default;
        
            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(arquivoImportado.Conteudo), false);
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosFotograficoLinhas = await LerPlanilha(file);
        
            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.Fotografico, JsonConvert.SerializeObject(acervosFotograficoLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosFotograficoLinhas);
            
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosFotograficoLinhas),ImportacaoStatus.ValidadoPreenchimentoValorFormatoQtdeCaracteres);
            
            await ValidacaoObterOuInserirDominios(acervosFotograficoLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosFotograficoLinhas),ImportacaoStatus.ValidacaoDominios);
            
            await PersistenciaAcervo(acervosFotograficoLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosFotograficoLinhas), acervosFotograficoLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
        
            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);
        
            return await ObterRetornoImportacaoAcervo(arquivoImportado, acervosFotograficoLinhas);
        }
        
        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoFotograficoLinhaDTO> acervosFotograficoLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
            {
                await ObterConservacoes(acervosFotograficoLinhas.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                await ObterSuportes(acervosFotograficoLinhas.Select(s => s.Suporte.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoSuporte.IMAGEM);
                await ObterFormatos(acervosFotograficoLinhas.Select(s => s.FormatoImagem.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoFormato.ACERVO_FOTOS);
                await ObterCromias(acervosFotograficoLinhas.Select(s => s.Cromia.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                await ObterCreditosAutoresTipoAutoria(acervosFotograficoLinhas.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()), TipoCreditoAutoria.Credito);
            }
            
            var acervoFotograficoRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosFotograficoLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s,arquivoImportado.TipoAcervo)),
                Sucesso = acervosFotograficoLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s=> ObterLinhasComSucesso(s.Titulo.Conteudo, s.Codigo.Conteudo, s.NumeroLinha)),
            };
            return acervoFotograficoRetorno;
        }
        
        private AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoFotograficoLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterConteudoTexto(linha.Codigo),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoFotograficoDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoFotograficoDTO ObterAcervoFotograficoDto(AcervoFotograficoLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoFotograficoDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                TipoAcervoId = (int)tipoAcervo,
                Codigo = ObterConteudoTexto(linha.Codigo),
                Localizacao = ObterConteudoTexto(linha.Localizacao),
                Procedencia = ObterConteudoTexto(linha.Procedencia),
                DataAcervo = ObterConteudoTexto(linha.Data),
                CopiaDigital = ObterConteudoSimNao(linha.CopiaDigital),
                PermiteUsoImagem = ObterConteudoSimNao(linha.PermiteUsoImagem),
                ConservacaoId = ObterConservacaoIdPorValorDoCampo(linha.EstadoConservacao.Conteudo,false),
                Descricao = ObterConteudoTexto(linha.Descricao),
                Quantidade = ObterConteudoLongoOuNulo(linha.Quantidade),
                Largura = ObterConteudoDoubleOuNulo(linha.Largura),
                Altura = ObterConteudoDoubleOuNulo(linha.Altura),
                SuporteId = ObterSuporteImagemIdPorValorDoCampo(linha.Suporte.Conteudo, false),
                FormatoId = ObterSuporteImagemIdPorValorDoCampo(linha.FormatoImagem.Conteudo, false),
                CromiaId = ObterSuporteImagemIdPorValorDoCampo(linha.Cromia.Conteudo, false),
                Resolucao = ObterConteudoTexto(linha.Resolucao),
                TamanhoArquivo = ObterConteudoTexto(linha.TamanhoArquivo),
                CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(linha.Credito.Conteudo, TipoCreditoAutoria.Credito),
            };
        }
        
        private AcervoFotograficoLinhaRetornoDTO ObterLinhasComErros(AcervoFotograficoLinhaDTO s)
        {
            return new AcervoFotograficoLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Codigo = ObterConteudoMensagemStatus(s.Codigo),
                Credito = ObterConteudoMensagemStatus(s.Credito),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                Data = ObterConteudoMensagemStatus(s.Data),
                CopiaDigital = ObterConteudoMensagemStatus(s.CopiaDigital),
                PermiteUsoImagem = ObterConteudoMensagemStatus(s.PermiteUsoImagem),
                EstadoConservacao = ObterConteudoMensagemStatus(s.EstadoConservacao),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                Quantidade = ObterConteudoMensagemStatus(s.Quantidade),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                Suporte = ObterConteudoMensagemStatus(s.Suporte),
                FormatoImagem = ObterConteudoMensagemStatus(s.FormatoImagem),
                TamanhoArquivo = ObterConteudoMensagemStatus(s.TamanhoArquivo),
                Cromia = ObterConteudoMensagemStatus(s.Cromia),
                Resolucao = ObterConteudoMensagemStatus(s.Resolucao),
                NumeroLinha = s.NumeroLinha,
                Status = ImportacaoStatus.Erros,
                Mensagem = s.Mensagem,
                ErrosCampos = ObterMensagemErroLinha(s),
            };
        }
				
        private string ObterMensagemErroLinha(AcervoFotograficoLinhaDTO acervoFotograficoLinhaDTO)
        {
	        var mensagemErro = new StringBuilder();

	        if (acervoFotograficoLinhaDTO.Titulo.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Titulo.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Codigo.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Codigo.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Credito.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Credito.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Localizacao.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Localizacao.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Procedencia.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Procedencia.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Data.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Data.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.CopiaDigital.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.CopiaDigital.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.PermiteUsoImagem.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.PermiteUsoImagem.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.EstadoConservacao.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.EstadoConservacao.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Descricao.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Descricao.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Suporte.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Suporte.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Quantidade.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Quantidade.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Cromia.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Cromia.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.TamanhoArquivo.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.TamanhoArquivo.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Largura.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Largura.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Altura.PossuiErro)
		        mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Altura.Mensagem);
            
            if (acervoFotograficoLinhaDTO.FormatoImagem.PossuiErro)
                mensagemErro.AppendLine(acervoFotograficoLinhaDTO.FormatoImagem.Mensagem);
            
            if (acervoFotograficoLinhaDTO.Resolucao.PossuiErro)
                mensagemErro.AppendLine(acervoFotograficoLinhaDTO.Resolucao.Mensagem);

	        return mensagemErro.ToString();
        }
        
        public async Task PersistenciaAcervo(IEnumerable<AcervoFotograficoLinhaDTO> acervosFotograficosLinhas)
        {
            foreach (var acervoFotograficoLinha in acervosFotograficosLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoFotografico = new AcervoFotograficoCadastroDTO()
                    {
                        Titulo = acervoFotograficoLinha.Titulo.Conteudo,
                        Codigo = acervoFotograficoLinha.Codigo.Conteudo,
                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoFotograficoLinha.Credito.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoFotograficoLinha.Localizacao.Conteudo,
                        Procedencia = acervoFotograficoLinha.Procedencia.Conteudo,
                        DataAcervo = acervoFotograficoLinha.Data.Conteudo,
                        CopiaDigital = ObterCopiaDigitalPorValorDoCampo(acervoFotograficoLinha.CopiaDigital.Conteudo),
                        PermiteUsoImagem = ObterAutorizaUsoDeImagemPorValorDoCampo(acervoFotograficoLinha.PermiteUsoImagem.Conteudo),
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoFotograficoLinha.EstadoConservacao.Conteudo),
                        Descricao = acervoFotograficoLinha.Descricao.Conteudo,
                        Quantidade = acervoFotograficoLinha.Quantidade.Conteudo.ObterLongoPorValorDoCampo(),
                        Largura = acervoFotograficoLinha.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Altura = acervoFotograficoLinha.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        SuporteId = ObterSuporteImagemIdPorValorDoCampo(acervoFotograficoLinha.Suporte.Conteudo),
                        FormatoId = ObterFormatoImagemIdPorValorDoCampo(acervoFotograficoLinha.FormatoImagem.Conteudo),
                        TamanhoArquivo = acervoFotograficoLinha.TamanhoArquivo.Conteudo,
                        CromiaId = ObterCromiaIdPorValorDoCampo(acervoFotograficoLinha.Cromia.Conteudo),
                        Resolucao = acervoFotograficoLinha.Resolucao.Conteudo,
                    };
                    await servicoAcervoFotografico.Inserir(acervoFotografico);

                    acervoFotograficoLinha.DefinirLinhaComoSucesso();
                }
                catch (Exception ex)
                {
                    acervoFotograficoLinha.DefinirLinhaComoErro(ex.Message);
                }
            }
        } 
        
        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoFotograficoLinhaDTO> linhas)
        {
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Codigo, Constantes.TOMBO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Credito, Constantes.CREDITO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Localizacao,Constantes.LOCALIZACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Procedencia,Constantes.PROCEDENCIA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Data,Constantes.DATA);
                    ValidarPreenchimentoLimiteCaracteres(linha.CopiaDigital,Constantes.COPIA_DIGITAL);
                    ValidarPreenchimentoLimiteCaracteres(linha.PermiteUsoImagem,Constantes.AUTORIZACAO_USO_DE_IMAGEM);
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Quantidade,Constantes.QUANTIDADE);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Suporte,Constantes.SUPORTE);
                    ValidarPreenchimentoLimiteCaracteres(linha.FormatoImagem,Constantes.FORMATO_IMAGEM);
                    ValidarPreenchimentoLimiteCaracteres(linha.TamanhoArquivo,Constantes.TAMANHO_ARQUIVO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Cromia,Constantes.CROMIA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Resolucao,Constantes.RESOLUCAO);
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, e.Message));
                }
            }
        }
        
        private bool PossuiErro(AcervoFotograficoLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.Codigo.PossuiErro 
                   || linha.Credito.PossuiErro
                   || linha.Localizacao.PossuiErro 
                   || linha.Procedencia.PossuiErro
                   || linha.Data.PossuiErro
                   || linha.CopiaDigital.PossuiErro 
                   || linha.PermiteUsoImagem.PossuiErro 
                   || linha.EstadoConservacao.PossuiErro
                   || linha.Descricao.PossuiErro
                   || linha.Quantidade.PossuiErro
                   || linha.Largura.PossuiErro
                   || linha.Altura.PossuiErro
                   || linha.Suporte.PossuiErro
                   || linha.FormatoImagem.PossuiErro
                   || linha.TamanhoArquivo.PossuiErro
                   || linha.Cromia.PossuiErro 
                   || linha.Resolucao.PossuiErro;
        }
        
        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoFotograficoLinhaDTO> linhas)
        {
            var linhasComsucesso = linhas.Where(w => !w.PossuiErros);
        
            try
            {
                await ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(linhasComsucesso.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()), TipoCreditoAutoria.Autoria);
                
                await ValidarOuInserirCromia(linhasComsucesso.Select(s => s.Cromia.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                
                await ValidarOuInserirSuporte(linhasComsucesso.Select(s => s.Suporte.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoSuporte.IMAGEM);
                
                await ValidarOuInserirConservacao(linhasComsucesso.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                
                await ValidarOuInserirFormato(linhasComsucesso.Select(s => s.FormatoImagem.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoFormato.ACERVO_FOTOS);
                
            }
            catch (Exception e)
            {
                foreach (var linha in linhas)
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X, e.Message));
            }
        }
        
        private async Task<IEnumerable<AcervoFotograficoLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoFotograficoLinhaDTO>();
        
            var stream = file.OpenReadStream();
        
            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();
        
                var totalLinhas = planilha.Rows().Count();
                
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);
        
                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoFotograficoLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Codigo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CODIGO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Credito = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CREDITO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                            EhCampoObrigatorio = true
                        },
                        Localizacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_LOCALIZACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        Procedencia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_PROCEDENCIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                            EhCampoObrigatorio = true
                        },
                        Data = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_DATA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                            EhCampoObrigatorio = true
                        },
                        CopiaDigital = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_COPIA_DIGITAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3, 
                            ValoresPermitidos  = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO }
                        },
                        PermiteUsoImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_AUTORIZACAO_USO_DE_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3,
                            ValoresPermitidos  = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO }
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_DESCRICAO),
                            EhCampoObrigatorio = true
                        },
                        Quantidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_QUANTIDADE),
                            FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO,
                            EhCampoObrigatorio = true
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_LARGURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ALTURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Suporte = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_SUPORTE),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        FormatoImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_FORMATO_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        TamanhoArquivo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TAMANHO_ARQUIVO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Cromia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CROMIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Resolucao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_RESOLUCAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        }
                    });
                }
            }

            return linhas;
        }
        
        private void ValidarOrdemColunas(IXLWorksheet planilha, int numeroLinha)
        {
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TITULO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_TITULO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TOMBO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_CODIGO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CREDITO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_CREDITO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_LOCALIZACAO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_LOCALIZACAO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_PROCEDENCIA, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_PROCEDENCIA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DATA, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_DATA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_COPIA_DIGITAL,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_COPIA_DIGITAL, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_AUTORIZACAO_USO_DE_IMAGEM,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_AUTORIZACAO_USO_DE_IMAGEM, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_ESTADO_CONSERVACAO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DESCRICAO,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_DESCRICAO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_QUANTIDADE,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_QUANTIDADE, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_LARGURA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_ALTURA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_SUPORTE,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_SUPORTE, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_FORMATO_DA_IMAGEM,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_FORMATO_IMAGEM, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TAMANHO_DO_ARQUIVO,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_TAMANHO_ARQUIVO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CROMIA,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_CROMIA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_RESOLUCAO,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_RESOLUCAO, Constantes.FOTOGRAFICO);
        }
    }
}