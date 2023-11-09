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
    public class ServicoImportacaoArquivoAcervoAudiovisual : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoAudiovisual 
    {
        private readonly IServicoAcervoAudiovisual servicoAcervoAudiovisual;
        
        public ServicoImportacaoArquivoAcervoAudiovisual(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoAudiovisual servicoAcervoAudiovisual)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte, servicoFormato)
        {
            this.servicoAcervoAudiovisual = servicoAcervoAudiovisual ?? throw new ArgumentNullException(nameof(servicoAcervoAudiovisual));
        }

        public void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores)
        {
            CreditosAutores = creditosAutores;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoAudiovisualLinhaRetornoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Audiovisual);
        
            return ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivoImportado.Conteudo));
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoAudiovisualLinhaRetornoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosAudiovisualLinhas = await LerPlanilha(file);
        
            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.Audiovisual, JsonConvert.SerializeObject(acervosAudiovisualLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosAudiovisualLinhas);
            
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosAudiovisualLinhas),ImportacaoStatus.ValidadoPreenchimentoValorFormatoQtdeCaracteres);
            
            await ValidacaoObterOuInserirDominios(acervosAudiovisualLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosAudiovisualLinhas),ImportacaoStatus.ValidacaoDominios);
            
            await PersistenciaAcervo(acervosAudiovisualLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosAudiovisualLinhas), acervosAudiovisualLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
        
            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);
        
            return ObterRetornoImportacaoAcervo(arquivoImportado, acervosAudiovisualLinhas);
        }
        
        private ImportacaoArquivoRetornoDTO<AcervoAudiovisualLinhaRetornoDTO> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoAudiovisualLinhaDTO> acervosAudiovisualLinhas)
        {
            var acervoAudiovisualRetorno = new ImportacaoArquivoRetornoDTO<AcervoAudiovisualLinhaRetornoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosAudiovisualLinhas
                        .Where(w => w.PossuiErros)
                        .Select(ObterAcervoAudiovisualLinhaRetornoDto),
                Sucesso = acervosAudiovisualLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(ObterAcervoAudiovisualLinhaRetornoDto)
            };
            return acervoAudiovisualRetorno;
        }
        
        private static AcervoAudiovisualLinhaRetornoDTO ObterAcervoAudiovisualLinhaRetornoDto(AcervoAudiovisualLinhaDTO s)
        {
            return new AcervoAudiovisualLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Tombo = ObterConteudoMensagemStatus(s.Tombo),
                Credito = ObterConteudoMensagemStatus(s.Credito),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                Data = ObterConteudoMensagemStatus(s.Data),
                Copia = ObterConteudoMensagemStatus(s.Copia),
                AutorizacaoUsoDeImagem = ObterConteudoMensagemStatus(s.AutorizacaoUsoDeImagem),
                EstadoConservacao = ObterConteudoMensagemStatus(s.EstadoConservacao),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                Suporte = ObterConteudoMensagemStatus(s.Suporte),
                Duracao = ObterConteudoMensagemStatus(s.Duracao),
                Cromia = ObterConteudoMensagemStatus(s.Cromia),
                TamanhoArquivo = ObterConteudoMensagemStatus(s.TamanhoArquivo),
                Acessibilidade = ObterConteudoMensagemStatus(s.Acessibilidade),
                Disponibilizacao = ObterConteudoMensagemStatus(s.Disponibilizacao)
            };
        }
        
        public async Task PersistenciaAcervo(IEnumerable<AcervoAudiovisualLinhaDTO> acervosAudiovisualLinhas)
        {
            foreach (var acervoAudiovisualLinha in acervosAudiovisualLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoAudiovisual = new AcervoAudiovisualCadastroDTO()
                    {
                        Titulo = acervoAudiovisualLinha.Titulo.Conteudo,
                        Codigo = acervoAudiovisualLinha.Tombo.Conteudo,
                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoAudiovisualLinha.Credito.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoAudiovisualLinha.Localizacao.Conteudo,
                        Procedencia = acervoAudiovisualLinha.Procedencia.Conteudo,
                        DataAcervo = acervoAudiovisualLinha.Data.Conteudo,
                        Copia = acervoAudiovisualLinha.Copia.Conteudo,
                        PermiteUsoImagem = acervoAudiovisualLinha.AutorizacaoUsoDeImagem.Conteudo.EstaPreenchido() ? acervoAudiovisualLinha.AutorizacaoUsoDeImagem.Conteudo.ToLower().Equals(Constantes.OPCAO_SIM) : false,
                        ConservacaoId = Conservacoes.FirstOrDefault(f => f.Nome.Equals(acervoAudiovisualLinha.EstadoConservacao.Conteudo)).Id,
                        Descricao = acervoAudiovisualLinha.Descricao.Conteudo,
                        SuporteId = Suportes.FirstOrDefault(f => f.Nome.Equals(acervoAudiovisualLinha.Suporte.Conteudo)).Id,
                        Duracao = acervoAudiovisualLinha.Duracao.Conteudo,
                        CromiaId = Cromias.FirstOrDefault(f => f.Nome.Equals(acervoAudiovisualLinha.Cromia.Conteudo)).Id,
                        TamanhoArquivo = acervoAudiovisualLinha.TamanhoArquivo.Conteudo,
                        Acessibilidade = acervoAudiovisualLinha.Acessibilidade.Conteudo,
                        Disponibilizacao = acervoAudiovisualLinha.Disponibilizacao.Conteudo,
                    };
                    await servicoAcervoAudiovisual.Inserir(acervoAudiovisual);
        
                    acervoAudiovisualLinha.Status = ImportacaoStatus.Sucesso;
                    acervoAudiovisualLinha.Mensagem = string.Empty;
                    acervoAudiovisualLinha.PossuiErros = false;
                }
                catch (Exception ex)
                {
                    acervoAudiovisualLinha.PossuiErros = true;
                    acervoAudiovisualLinha.Status = ImportacaoStatus.Erros;
                    acervoAudiovisualLinha.Mensagem = ex.Message;
                }
            }
        } 
        
        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoAudiovisualLinhaDTO> linhas)
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
                    ValidarPreenchimentoLimiteCaracteres(linha.Copia,Constantes.COPIA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.AutorizacaoUsoDeImagem,Constantes.AUTORIZACAO_USO_DE_IMAGEM, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Suporte,Constantes.SUPORTE, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Duracao,Constantes.DURACAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Cromia,Constantes.CROMIA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.TamanhoArquivo,Constantes.TAMANHO_ARQUIVO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Acessibilidade,Constantes.ACESSIBILIDADE, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Disponibilizacao,Constantes.DISPONIBILIDADE, linha.NumeroLinha);
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
        
        private bool PossuiErro(AcervoAudiovisualLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.Tombo.PossuiErro 
                   || linha.Credito.PossuiErro
                   || linha.Localizacao.PossuiErro 
                   || linha.Procedencia.PossuiErro
                   || linha.Data.PossuiErro
                   || linha.Copia.PossuiErro 
                   || linha.AutorizacaoUsoDeImagem.PossuiErro 
                   || linha.EstadoConservacao.PossuiErro
                   || linha.Descricao.PossuiErro
                   || linha.Suporte.PossuiErro
                   || linha.Duracao.PossuiErro
                   || linha.Cromia.PossuiErro 
                   || linha.TamanhoArquivo.PossuiErro 
                   || linha.Acessibilidade.PossuiErro
                   || linha.Disponibilizacao.PossuiErro;
        }
        
        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoAudiovisualLinhaDTO> linhas)
        {
            var linhasComsucesso = linhas.Where(w => !w.PossuiErros);
        
            try
            {
                await ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(linhasComsucesso.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct(), TipoCreditoAutoria.Autoria);
                
                await ValidarOuInserirCromia(linhasComsucesso.Select(s => s.Cromia.Conteudo).Distinct());
                
                await ValidarOuInserirSuporte(linhasComsucesso.Select(s => s.Suporte.Conteudo).Distinct(), TipoSuporte.IMAGEM);
                
                await ValidarOuInserirConservacao(linhasComsucesso.Select(s => s.EstadoConservacao.Conteudo).Distinct());
                
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
        
        private async Task<IEnumerable<AcervoAudiovisualLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoAudiovisualLinhaDTO>();
        
            var stream = file.OpenReadStream();
        
            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();
        
                var totalLinhas = planilha.Rows().Count();
        
                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoAudiovisualLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Tombo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_TOMBO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Credito = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_CREDITO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Localizacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_LOCALIZACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        Procedencia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_PROCEDENCIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Data = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_DATA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                            EhCampoObrigatorio = true
                        },
                        Copia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_COPIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100, 
                        },
                        AutorizacaoUsoDeImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_AUTORIZACAO_USO_DE_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3,
                            EhCampoObrigatorio = true
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_DESCRICAO),
                            EhCampoObrigatorio = true
                        },
                        Suporte = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_SUPORTE),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Duracao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_DURACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        Cromia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_CROMIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        },
                        TamanhoArquivo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_TAMANHO_ARQUIVO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        Acessibilidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_ACESSIBILIDADE),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        Disponibilizacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_DISPONIBILIZACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        }
                    });
                }
            }

            return linhas;
        }
    }
}