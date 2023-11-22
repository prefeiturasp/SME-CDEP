﻿using System.Text;
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
    public class ServicoImportacaoArquivoAcervoArteGrafica : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoArteGrafica 
    {
        private readonly IServicoAcervoArteGrafica servicoAcervoArteGrafica;
        
        public ServicoImportacaoArquivoAcervoArteGrafica(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoArteGrafica servicoAcervoArteGrafica)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato)
        {
            this.servicoAcervoArteGrafica = servicoAcervoArteGrafica ?? throw new ArgumentNullException(nameof(servicoAcervoArteGrafica));
        }

        public void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores)
        {
            CreditosAutores = creditosAutores;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.ArtesGraficas);

            if (arquivoImportado.EhNulo())
                return default;

            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoArteGraficaLinhaDTO>>(arquivoImportado.Conteudo), false);
        }

        public async Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo)
        {
            await RemoverLinhaDoArquivo<AcervoArteGraficaLinhaDTO>(id, linhaDoArquivo, TipoAcervo.ArtesGraficas);

            return true;
        }
        
        public async Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linha)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoArteGraficaLinhaDTO>(id, linha.NumeroLinha, TipoAcervo.ArtesGraficas);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linha.NumeroLinha));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosArtesGraficasLinhas = await LerPlanilha(file);

            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.ArtesGraficas, JsonConvert.SerializeObject(acervosArtesGraficasLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosArtesGraficasLinhas);
            
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosArtesGraficasLinhas),ImportacaoStatus.ValidadoPreenchimentoValorFormatoQtdeCaracteres);
            
            await ValidacaoObterOuInserirDominios(acervosArtesGraficasLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosArtesGraficasLinhas),ImportacaoStatus.ValidacaoDominios);
            
            await PersistenciaAcervo(acervosArtesGraficasLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosArtesGraficasLinhas), acervosArtesGraficasLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);

            return await ObterRetornoImportacaoAcervo(arquivoImportado, acervosArtesGraficasLinhas);
        }

        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoArteGraficaLinhaDTO> acervosArtesGraficasLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
            {
                await ObterConservacoes(acervosArtesGraficasLinhas.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                await ObterCromias(acervosArtesGraficasLinhas.Select(s => s.Cromia.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                await ObterSuportes(acervosArtesGraficasLinhas.Select(s => s.Suporte.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoSuporte.IMAGEM);
                await ObterCreditosAutoresTipoAutoria(acervosArtesGraficasLinhas.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()), TipoCreditoAutoria.Credito);
            }
            
            var acervoArteGraficaRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Status = arquivoImportado.Status,
                Erros =  acervosArtesGraficasLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s,arquivoImportado.TipoAcervo)),
                Sucesso = acervosArtesGraficasLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s=> ObterLinhasComSucesso(s.Titulo.Conteudo, s.Codigo.Conteudo, s.NumeroLinha)),
            };
            return acervoArteGraficaRetorno;
        }

        private AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoArteGraficaLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterConteudoTexto(linha.Codigo),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoArteGraficaDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoArteGraficaDTO ObterAcervoArteGraficaDto(AcervoArteGraficaLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoArteGraficaDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                TipoAcervoId = (int)tipoAcervo,
                Codigo = ObterConteudoTexto(linha.Codigo),
                Localizacao = ObterConteudoTexto(linha.Localizacao),
                Procedencia = ObterConteudoTexto(linha.Procedencia),
                DataAcervo = ObterConteudoTexto(linha.Data),
                CopiaDigital = ObterConteudoSimNao(linha.CopiaDigital),
                PermiteUsoImagem = ObterConteudoSimNao(linha.PermiteUsoImagem),
                ConservacaoId = ObterConservacaoIdOuNuloPorValorDoCampo(linha.EstadoConservacao.Conteudo),
                CromiaId = ObterCromiaIdOuNuloPorValorDoCampo(linha.Cromia.Conteudo),
                Largura = ObterConteudoDoubleOuNulo(linha.Largura),
                Altura = ObterConteudoDoubleOuNulo(linha.Altura),
                Diametro = ObterConteudoDoubleOuNulo(linha.Diametro),
                Tecnica = ObterConteudoTexto(linha.Tecnica),
                SuporteId = ObterSuporteImagemIdOuNuloPorValorDoCampo(linha.Suporte.Conteudo),
                Quantidade = ObterConteudoLongoOuNulo(linha.Quantidade),
                Descricao = ObterConteudoTexto(linha.Descricao),
                CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(linha.Credito.Conteudo, TipoCreditoAutoria.Credito),
            };
        }
        
        private AcervoArteGraficaLinhaRetornoDTO ObterLinhasComErros(AcervoArteGraficaLinhaDTO s)
        {
            return new AcervoArteGraficaLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Codigo = ObterConteudoMensagemStatus(s.Codigo),
                CreditosAutoresIds = ObterConteudoMensagemStatus(s.Credito),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                DataAcervo = ObterConteudoMensagemStatus(s.Data),
                CopiaDigital = ObterConteudoMensagemStatus(s.CopiaDigital),
                PermiteUsoImagem = ObterConteudoMensagemStatus(s.PermiteUsoImagem),
                ConservacaoId = ObterConteudoMensagemStatus(s.EstadoConservacao),
                CromiaId = ObterConteudoMensagemStatus(s.Cromia),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                Diametro = ObterConteudoMensagemStatus(s.Diametro),
                Tecnica = ObterConteudoMensagemStatus(s.Tecnica),
                SuporteId = ObterConteudoMensagemStatus(s.Suporte),
                Quantidade = ObterConteudoMensagemStatus(s.Quantidade),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                NumeroLinha = s.NumeroLinha,
                Status = ImportacaoStatus.Erros,
                Mensagem = s.Mensagem,
                ErrosCampos = ObterMensagemErroLinha(s),
            };
        }

        private string ObterMensagemErroLinha(AcervoArteGraficaLinhaDTO acervoArteGraficaLinhaDto)
        {
            var mensagemErro = new StringBuilder();

            if (acervoArteGraficaLinhaDto.Titulo.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Titulo.Mensagem);
            
            if (acervoArteGraficaLinhaDto.Codigo.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Codigo.Mensagem);
            
            if (acervoArteGraficaLinhaDto.Credito.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Credito.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Localizacao.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Localizacao.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Procedencia.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Procedencia.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Data.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Data.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.CopiaDigital.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.CopiaDigital.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.PermiteUsoImagem.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.PermiteUsoImagem.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.EstadoConservacao.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.EstadoConservacao.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Cromia.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Cromia.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Largura.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Largura.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Altura.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Altura.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Diametro.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Diametro.Mensagem);
            
            if (acervoArteGraficaLinhaDto.Tecnica.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Tecnica.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Suporte.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Suporte.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Quantidade.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Quantidade.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Descricao.PossuiErro)
                mensagemErro.AppendLine(acervoArteGraficaLinhaDto.Descricao.Mensagem);

            return mensagemErro.ToString();
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
                            .Where(f => acervoArteGraficaLinha.Credito.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoArteGraficaLinha.Localizacao.Conteudo,
                        Procedencia = acervoArteGraficaLinha.Procedencia.Conteudo,
                        DataAcervo = acervoArteGraficaLinha.Data.Conteudo,
                        CopiaDigital = ObterCopiaDigitalPorValorDoCampo(acervoArteGraficaLinha.CopiaDigital.Conteudo),
                        PermiteUsoImagem = ObterAutorizaUsoDeImagemPorValorDoCampo(acervoArteGraficaLinha.PermiteUsoImagem.Conteudo),
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoArteGraficaLinha.EstadoConservacao.Conteudo),
                        CromiaId = ObterCromiaIdPorValorDoCampo(acervoArteGraficaLinha.Cromia.Conteudo),
                        Largura = acervoArteGraficaLinha.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Altura = acervoArteGraficaLinha.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Diametro = acervoArteGraficaLinha.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
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
                    ValidarPreenchimentoLimiteCaracteres(linha.Cromia,Constantes.CROMIA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Diametro,Constantes.DIAMETRO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Tecnica,Constantes.TECNICA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Suporte,Constantes.SUPORTE);
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
                   || linha.Data.PossuiErro
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

        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoArteGraficaLinhaDTO> linhas)
        {
            var linhasComsucesso = linhas.Where(w => !w.PossuiErros);

            try
            {
                await ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(linhasComsucesso.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()), TipoCreditoAutoria.Autoria);
                
                await ValidarOuInserirCromia(linhasComsucesso.Select(s => s.Cromia.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                
                await ValidarOuInserirSuporte(linhasComsucesso.Select(s => s.Suporte.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoSuporte.IMAGEM);
                
                await ValidarOuInserirConservacao(linhasComsucesso.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                
            }
            catch (Exception e)
            {
                foreach (var linha in linhas)
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X, e.Message));
            }
        }

        private async Task<IEnumerable<AcervoArteGraficaLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoArteGraficaLinhaDTO>();

            var stream = file.OpenReadStream();

            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                var totalLinhas = planilha.Rows().Count();

                if (totalLinhas <= Constantes.INICIO_LINHA_TITULO)
                    throw new NegocioException(MensagemNegocio.PLANILHA_VAZIA);
                
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);

                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoArteGraficaLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Codigo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TOMBO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Credito = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_CREDITO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Localizacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_LOCALIZACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        Procedencia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_PROCEDENCIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Data = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DATA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                            EhCampoObrigatorio = true
                        },
                        CopiaDigital = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_COPIA_DIGITAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3, 
                            EhCampoObrigatorio = true,
                            ValoresPermitidos  = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO }
                        },
                        PermiteUsoImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_AUTORIZACAO_USO_DE_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3,
                            EhCampoObrigatorio = true,
                            ValoresPermitidos  = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO }
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Cromia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_CROMIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_LARGURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_ALTURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Diametro = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_DIAMETRO),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Tecnica = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TECNICA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        Suporte = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_SUPORTE),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Quantidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_QUANTIDADE),
                            FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO,
                            EhCampoObrigatorio = true
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DESCRICAO),
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
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TITULO, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TOMBO, 
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TOMBO, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CREDITO, 
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_CREDITO, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_LOCALIZACAO, 
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_LOCALIZACAO, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_PROCEDENCIA, 
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_PROCEDENCIA, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DATA, 
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DATA, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_COPIA_DIGITAL,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_COPIA_DIGITAL, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_AUTORIZACAO_USO_DE_IMAGEM,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_AUTORIZACAO_USO_DE_IMAGEM, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_ESTADO_CONSERVACAO, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CROMIA,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_CROMIA, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_LARGURA, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_ALTURA, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_LARGURA, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_ALTURA, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_DIAMETRO,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_DIAMETRO, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TECNICA,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TECNICA, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_SUPORTE,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_SUPORTE, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_QUANTIDADE,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_QUANTIDADE, Constantes.ARTE_GRAFICA);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DESCRICAO,
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DESCRICAO, Constantes.ARTE_GRAFICA);
        }
    }
}