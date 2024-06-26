﻿using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoArteGrafica : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoArteGrafica 
    {
        private readonly IServicoMensageria servicoMensageria;
        
        public ServicoImportacaoArquivoAcervoArteGrafica(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoMensageria servicoMensageria, IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.ArtesGraficas);

            if (arquivoImportado.EhNulo())
                return default;

            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoArteGraficaLinhaDTO>>(arquivoImportado.Conteudo), false);
        }
        
        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id)
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterImportacaoPorId(id);

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
           
            await servicoMensageria.Publicar(RotasRabbit.ExecutarImportacaoArquivoAcervoArteGrafica, importacaoArquivoId, Guid.NewGuid());

            return new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = importacaoArquivoId,
                Nome = importacaoArquivo.Nome,
                TipoAcervo = importacaoArquivo.TipoAcervo,
                DataImportacao = DateTimeExtension.HorarioBrasilia(),
                Status = ImportacaoStatus.Pendente
            };
        }

        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoArteGraficaLinhaDTO> acervosArtesGraficasLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
                await CarregarTodosOsDominios();
            
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Credito);
            
            await ObterSuportesPorTipo(TipoSuporte.IMAGEM);
            
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
                        .Select(s=> ObterLinhasComSucessoSufixo(s.Titulo.Conteudo, s.Codigo.Conteudo, s.NumeroLinha,Constantes.SIGLA_ACERVO_ARTE_GRAFICA)),
            };
            return acervoArteGraficaRetorno;
        }

        private AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoArteGraficaLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterSufixo(ObterConteudoTexto(linha.Codigo),Constantes.SIGLA_ACERVO_ARTE_GRAFICA),
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
                Codigo = ObterSufixo(ObterConteudoTexto(linha.Codigo),Constantes.SIGLA_ACERVO_ARTE_GRAFICA),
                Localizacao = ObterConteudoTexto(linha.Localizacao),
                Procedencia = ObterConteudoTexto(linha.Procedencia),
                Ano = ObterConteudoTexto(linha.Ano),
                CopiaDigital = ObterConteudoSimNao(linha.CopiaDigital),
                PermiteUsoImagem = ObterConteudoSimNao(linha.PermiteUsoImagem),
                ConservacaoId = ObterConservacaoIdOuNuloPorValorDoCampo(linha.EstadoConservacao.Conteudo),
                CromiaId = ObterCromiaIdOuNuloPorValorDoCampo(linha.Cromia.Conteudo),
                Largura = linha.Largura.Conteudo,
                Altura = linha.Altura.Conteudo,
                Diametro = linha.Diametro.Conteudo,
                Tecnica = ObterConteudoTexto(linha.Tecnica),
                SuporteId = ObterSuporteImagemIdOuNuloPorValorDoCampo(linha.Suporte.Conteudo),
                Quantidade = ObterConteudoLongoOuNulo(linha.Quantidade),
                Descricao = ObterConteudoTexto(linha.Descricao),
                CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(linha.Credito.Conteudo, TipoCreditoAutoria.Credito, false),
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
                Ano = ObterConteudoMensagemStatus(s.Ano),
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

        private string[] ObterMensagemErroLinha(AcervoArteGraficaLinhaDTO acervoArteGraficaLinhaDto)
        {
            var mensagemErro = new List<string>();

            if (acervoArteGraficaLinhaDto.Titulo.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Titulo.Mensagem);
            
            if (acervoArteGraficaLinhaDto.Codigo.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Codigo.Mensagem);
            
            if (acervoArteGraficaLinhaDto.Credito.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Credito.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Localizacao.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Localizacao.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Procedencia.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Procedencia.Mensagem);
            
            if (acervoArteGraficaLinhaDto.Ano.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Ano.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.CopiaDigital.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.CopiaDigital.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.PermiteUsoImagem.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.PermiteUsoImagem.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.EstadoConservacao.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.EstadoConservacao.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Cromia.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Cromia.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Largura.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Largura.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Altura.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Altura.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Diametro.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Diametro.Mensagem);
            
            if (acervoArteGraficaLinhaDto.Tecnica.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Tecnica.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Suporte.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Suporte.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Quantidade.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Quantidade.Mensagem);
                    
            if (acervoArteGraficaLinhaDto.Descricao.PossuiErro)
                mensagemErro.Add(acervoArteGraficaLinhaDto.Descricao.Mensagem);

            return mensagemErro.ToArray();
        }

        private async Task<IEnumerable<AcervoArteGraficaLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoArteGraficaLinhaDTO>();

            var stream = file.OpenReadStream();

            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                var totalLinhas = planilha.Rows().Count();

                await ValidarQtdeLinhasImportadas(totalLinhas);

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
                            PermiteNovoRegistro = true
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
                        Ano = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_ANO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_7,
                            EhCampoObrigatorio = true,
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
                            EhCampoObrigatorio = true,
                        },
                        Cromia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_CROMIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true,
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_LARGURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA)
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_ALTURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA)
                        },
                        Diametro = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_DIAMETRO).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA)
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
                            EhCampoObrigatorio = true,
                            PermiteNovoRegistro = true
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
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ANO, 
                Constantes.ACERVO_ARTE_GRAFICA_CAMPO_ANO, Constantes.ARTE_GRAFICA);
            
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