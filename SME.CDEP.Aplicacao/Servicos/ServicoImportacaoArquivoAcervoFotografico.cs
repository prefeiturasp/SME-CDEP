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

        public async Task<ImportacaoArquivoRetornoDTO<AcervoFotograficoLinhaRetornoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Fotografico);
        
            return ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(arquivoImportado.Conteudo));
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoFotograficoLinhaRetornoDTO>> ImportarArquivo(IFormFile file)
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
        
            return ObterRetornoImportacaoAcervo(arquivoImportado, acervosFotograficoLinhas);
        }
        
        private ImportacaoArquivoRetornoDTO<AcervoFotograficoLinhaRetornoDTO> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoFotograficoLinhaDTO> acervosFotograficoLinhas)
        {
            var acervoFotograficoRetorno = new ImportacaoArquivoRetornoDTO<AcervoFotograficoLinhaRetornoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosFotograficoLinhas
                        .Where(w => w.PossuiErros)
                        .Select(ObterAcervoFotograficoLinhaRetornoDto),
                Sucesso = acervosFotograficoLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(ObterAcervoFotograficoLinhaRetornoDto)
            };
            return acervoFotograficoRetorno;
        }
        
        private static AcervoFotograficoLinhaRetornoDTO ObterAcervoFotograficoLinhaRetornoDto(AcervoFotograficoLinhaDTO s)
        {
            return new AcervoFotograficoLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Tombo = ObterConteudoMensagemStatus(s.Tombo),
                Credito = ObterConteudoMensagemStatus(s.Credito),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                Data = ObterConteudoMensagemStatus(s.Data),
                CopiaDigital = ObterConteudoMensagemStatus(s.CopiaDigital),
                AutorizacaoUsoDeImagem = ObterConteudoMensagemStatus(s.AutorizacaoUsoDeImagem),
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
            };
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
                        Codigo = acervoFotograficoLinha.Tombo.Conteudo,
                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoFotograficoLinha.Credito.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoFotograficoLinha.Localizacao.Conteudo,
                        Procedencia = acervoFotograficoLinha.Procedencia.Conteudo,
                        DataAcervo = acervoFotograficoLinha.Data.Conteudo,
                        CopiaDigital = ObterCopiaDigitalPorValorDoCampo(acervoFotograficoLinha.CopiaDigital.Conteudo),
                        PermiteUsoImagem = ObterAutorizaUsoDeImagemPorValorDoCampo(acervoFotograficoLinha.AutorizacaoUsoDeImagem.Conteudo),
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoFotograficoLinha.EstadoConservacao.Conteudo),
                        Descricao = acervoFotograficoLinha.Descricao.Conteudo,
                        Quantidade = long.Parse(acervoFotograficoLinha.Quantidade.Conteudo),
                        Largura = long.Parse(acervoFotograficoLinha.Largura.Conteudo),
                        Altura = long.Parse(acervoFotograficoLinha.Altura.Conteudo),
                        SuporteId = ObterSuporteImagemIdPorValorDoCampo(acervoFotograficoLinha.Suporte.Conteudo),
                        FormatoId = ObterFormatoImagemIdPorValorDoCampo(acervoFotograficoLinha.FormatoImagem.Conteudo),
                        TamanhoArquivo = acervoFotograficoLinha.TamanhoArquivo.Conteudo,
                        CromiaId = ObterCromiaIdPorValorDoCampo(acervoFotograficoLinha.Cromia.Conteudo),
                        Resolucao = acervoFotograficoLinha.Resolucao.Conteudo,
                    };
                    await servicoAcervoFotografico.Inserir(acervoFotografico);
        
                    acervoFotograficoLinha.Status = ImportacaoStatus.Sucesso;
                    acervoFotograficoLinha.Mensagem = string.Empty;
                    acervoFotograficoLinha.PossuiErros = false;
                }
                catch (Exception ex)
                {
                    acervoFotograficoLinha.PossuiErros = true;
                    acervoFotograficoLinha.Status = ImportacaoStatus.Erros;
                    acervoFotograficoLinha.Mensagem = ex.Message;
                }
            }
        } 
        
        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoFotograficoLinhaDTO> linhas)
        {
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Tombo, Constantes.TOMBO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Credito, Constantes.CREDITO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Localizacao,Constantes.LOCALIZACAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Procedencia,Constantes.PROCEDENCIA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Data,Constantes.DATA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.CopiaDigital,Constantes.COPIA_DIGITAL, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.AutorizacaoUsoDeImagem,Constantes.AUTORIZACAO_USO_DE_IMAGEM, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Quantidade,Constantes.QUANTIDADE, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Suporte,Constantes.SUPORTE, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.FormatoImagem,Constantes.FORMATO_IMAGEM, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.TamanhoArquivo,Constantes.TAMANHO_ARQUIVO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Cromia,Constantes.CROMIA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Resolucao,Constantes.RESOLUCAO, linha.NumeroLinha);
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.PossuiErros = true;
                    linha.Status = ImportacaoStatus.Erros;
                    linha.Mensagem = string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, linha.NumeroLinha, e.Message);
                }
            }
        }
        
        private bool PossuiErro(AcervoFotograficoLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.Tombo.PossuiErro 
                   || linha.Credito.PossuiErro
                   || linha.Localizacao.PossuiErro 
                   || linha.Procedencia.PossuiErro
                   || linha.Data.PossuiErro
                   || linha.CopiaDigital.PossuiErro 
                   || linha.AutorizacaoUsoDeImagem.PossuiErro 
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
                await ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(linhasComsucesso.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct(), TipoCreditoAutoria.Autoria);
                
                await ValidarOuInserirCromia(linhasComsucesso.Select(s => s.Cromia.Conteudo).Distinct());
                
                await ValidarOuInserirSuporte(linhasComsucesso.Select(s => s.Suporte.Conteudo).Distinct(), TipoSuporte.IMAGEM);
                
                await ValidarOuInserirConservacao(linhasComsucesso.Select(s => s.EstadoConservacao.Conteudo).Distinct());
                
                await ValidarOuInserirFormato(linhasComsucesso.Select(s => s.FormatoImagem.Conteudo).Distinct(), TipoFormato.ACERVO_FOTOS);
                
            }
            catch (Exception e)
            {
                foreach (var linha in linhas)
                {
                    linha.PossuiErros = true;
                    linha.Status = ImportacaoStatus.Erros;
                    linha.Mensagem = string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X, e.Message);
                }
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
                        Tombo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TOMBO),
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
                        },
                        AutorizacaoUsoDeImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_AUTORIZACAO_USO_DE_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3,
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
    }
}