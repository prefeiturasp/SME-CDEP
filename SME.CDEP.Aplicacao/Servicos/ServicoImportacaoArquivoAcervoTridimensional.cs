﻿using ClosedXML.Excel;
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
    public class ServicoImportacaoArquivoAcervoTridimensional : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoTridimensional 
    {
        private readonly IServicoAcervoTridimensional servicoAcervoTridimensional;
        
        public ServicoImportacaoArquivoAcervoTridimensional(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte, IServicoFormato servicoFormato,IServicoAcervoTridimensional servicoAcervoTridimensional)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato)
        {
            this.servicoAcervoTridimensional = servicoAcervoTridimensional ?? throw new ArgumentNullException(nameof(servicoAcervoTridimensional));
        }

        public void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores)
        {
            CreditosAutores = creditosAutores;
        }
        
        public async Task<bool> RemoverLinhaDoArquivo(long id, int linhaDoArquivo)
        {
            return await RemoverLinhaDoArquivo<AcervoTridimensionalLinhaDTO>(id, linhaDoArquivo, TipoAcervo.Tridimensional);
        }

        public async Task<bool> AtualizarLinhaParaSucesso(long id, int linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoTridimensionalLinhaDTO>(id, linhaDoArquivo, TipoAcervo.Tridimensional);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Tridimensional);
        
            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(arquivoImportado.Conteudo), false);
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosTridimensionalLinhas = await LerPlanilha(file);
        
            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.Tridimensional, JsonConvert.SerializeObject(acervosTridimensionalLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosTridimensionalLinhas);
            
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosTridimensionalLinhas),ImportacaoStatus.ValidadoPreenchimentoValorFormatoQtdeCaracteres);
            
            await ValidacaoObterOuInserirDominios(acervosTridimensionalLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosTridimensionalLinhas),ImportacaoStatus.ValidacaoDominios);
            
            await PersistenciaAcervo(acervosTridimensionalLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosTridimensionalLinhas), acervosTridimensionalLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
        
            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);
        
            return await ObterRetornoImportacaoAcervo(arquivoImportado, acervosTridimensionalLinhas);
        }
        
        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoTridimensionalLinhaDTO> acervosTridimensionalLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
                await ObterConservacoes(acervosTridimensionalLinhas.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
            
            var acervoTridimensionalRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosTridimensionalLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s,arquivoImportado.TipoAcervo)),
                Sucesso = acervosTridimensionalLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s=> ObterLinhasComSucesso(s.Titulo.Conteudo, s.Tombo.Conteudo, s.NumeroLinha)),
            };
            return acervoTridimensionalRetorno;
        }
        
         private AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoTridimensionalLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterConteudoTexto(linha.Tombo),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoTridimensionalDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoTridimensionalDTO ObterAcervoTridimensionalDto(AcervoTridimensionalLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoTridimensionalDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                TipoAcervoId = (int)tipoAcervo,
                Codigo = ObterConteudoTexto(linha.Tombo),
                Procedencia = ObterConteudoTexto(linha.Procedencia),
                DataAcervo = ObterConteudoTexto(linha.Data),
                ConservacaoId = ObterConservacaoIdPorValorDoCampo(linha.EstadoConservacao.Conteudo, false),
                Quantidade = ObterConteudoLongoOuNulo(linha.Quantidade),
                Descricao = ObterConteudoTexto(linha.Descricao),
                Largura = ObterConteudoDoubleOuNulo(linha.Largura),
                Altura = ObterConteudoDoubleOuNulo(linha.Altura),
                Profundidade = ObterConteudoDoubleOuNulo(linha.Profundidade),
                Diametro = ObterConteudoDoubleOuNulo(linha.Diametro),
            };
        }
        
        private static AcervoTridimensionalLinhaRetornoDTO ObterLinhasComErros(AcervoTridimensionalLinhaDTO s)
        {
            return new AcervoTridimensionalLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Tombo = ObterConteudoMensagemStatus(s.Tombo),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                Data = ObterConteudoMensagemStatus(s.Data),
                EstadoConservacao = ObterConteudoMensagemStatus(s.EstadoConservacao),
                Quantidade = ObterConteudoMensagemStatus(s.Quantidade),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                Profundidade = ObterConteudoMensagemStatus(s.Profundidade),
                Diametro = ObterConteudoMensagemStatus(s.Diametro),
                NumeroLinha = s.NumeroLinha,
                Status = s.Status,
                Mensagem = s.Mensagem
            };
        }
        
        public async Task PersistenciaAcervo(IEnumerable<AcervoTridimensionalLinhaDTO> acervosTridimensionalsLinhas)
        {
            foreach (var acervoTridimensionalLinha in acervosTridimensionalsLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoTridimensional = new AcervoTridimensionalCadastroDTO()
                    {
                        Titulo = acervoTridimensionalLinha.Titulo.Conteudo,
                        Codigo = acervoTridimensionalLinha.Tombo.Conteudo,
                        Procedencia = acervoTridimensionalLinha.Procedencia.Conteudo,
                        DataAcervo = acervoTridimensionalLinha.Data.Conteudo,
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoTridimensionalLinha.EstadoConservacao.Conteudo),
                        Quantidade = acervoTridimensionalLinha.Quantidade.Conteudo.ObterLongoPorValorDoCampo(),
                        Descricao = acervoTridimensionalLinha.Descricao.Conteudo,
                        Largura = acervoTridimensionalLinha.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Altura = acervoTridimensionalLinha.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Profundidade = acervoTridimensionalLinha.Profundidade.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Diametro = acervoTridimensionalLinha.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                    };
                    await servicoAcervoTridimensional.Inserir(acervoTridimensional);
        
                    acervoTridimensionalLinha.DefinirLinhaComoSucesso();
                }
                catch (Exception ex)
                {
                    acervoTridimensionalLinha.DefinirLinhaComoErro(ex.Message);
                }
            }
        }

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoTridimensionalLinhaDTO> linhas)
        {
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Tombo, Constantes.TOMBO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Procedencia,Constantes.PROCEDENCIA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Data,Constantes.DATA);
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Quantidade,Constantes.QUANTIDADE);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Profundidade,Constantes.PROFUNDIDADE);
                    ValidarPreenchimentoLimiteCaracteres(linha.Diametro,Constantes.DIAMETRO);
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, linha.NumeroLinha, e.Message));
                }
            }
        }
        
        private bool PossuiErro(AcervoTridimensionalLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.Tombo.PossuiErro 
                   || linha.Procedencia.PossuiErro
                   || linha.Data.PossuiErro
                   || linha.EstadoConservacao.PossuiErro
                   || linha.Quantidade.PossuiErro
                   || linha.Descricao.PossuiErro
                   || linha.Largura.PossuiErro
                   || linha.Altura.PossuiErro
                   || linha.Profundidade.PossuiErro
                   || linha.Diametro.PossuiErro;
        }
        
        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoTridimensionalLinhaDTO> linhas)
        {
            var linhasComsucesso = linhas.Where(w => !w.PossuiErros);
        
            try
            {
                await ValidarOuInserirConservacao(linhasComsucesso.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                
            }
            catch (Exception e)
            {
                foreach (var linha in linhas)
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X, e.Message));
            }
        }
        
        private async Task<IEnumerable<AcervoTridimensionalLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoTridimensionalLinhaDTO>();
        
            var stream = file.OpenReadStream();
        
            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                if (planilha == null)
                    throw new NegocioException(Constantes.NAO_FOI_POSSIVEL_LER_A_PLANILHA);
        
                var totalLinhas = planilha.Rows().Count();
        
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);
                
                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoTridimensionalLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Tombo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_TOMBO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Procedencia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_PROCEDENCIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                            EhCampoObrigatorio = true
                        },
                        Data = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DATA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                            EhCampoObrigatorio = true
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Quantidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_QUANTIDADE),
                            FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO,
                            EhCampoObrigatorio = true
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DESCRICAO),
                            EhCampoObrigatorio = true
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_LARGURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ALTURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Profundidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_PROFUNDIDADE),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        },
                        Diametro = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DIAMETRO),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        }
                    });
                }
            }

            return linhas;
        }

        private static void ValidarOrdemColunas(IXLWorksheet planilha, int numeroLinha)
        {
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TITULO, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_TITULO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TOMBO, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_TOMBO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_PROCEDENCIA, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_PROCEDENCIA, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DATA, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DATA, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ESTADO_CONSERVACAO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_QUANTIDADE,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_QUANTIDADE, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DESCRICAO,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DESCRICAO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_LARGURA, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ALTURA, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_PROFUNDIDADE,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_PROFUNDIDADE, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_DIAMETRO,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DIAMETRO, Constantes.TRIDIMENSIONAL);
        }
    }
}