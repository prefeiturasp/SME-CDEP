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
            await RemoverLinhaDoArquivo<AcervoTridimensionalLinhaDTO>(id, linhaDoArquivo, TipoAcervo.Tridimensional);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoTridimensionalLinhaRetornoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Tridimensional);
        
            return ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(arquivoImportado.Conteudo));
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoTridimensionalLinhaRetornoDTO>> ImportarArquivo(IFormFile file)
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
        
            return ObterRetornoImportacaoAcervo(arquivoImportado, acervosTridimensionalLinhas);
        }
        
        private ImportacaoArquivoRetornoDTO<AcervoTridimensionalLinhaRetornoDTO> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoTridimensionalLinhaDTO> acervosTridimensionalLinhas)
        {
            var acervoTridimensionalRetorno = new ImportacaoArquivoRetornoDTO<AcervoTridimensionalLinhaRetornoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosTridimensionalLinhas
                        .Where(w => w.PossuiErros)
                        .Select(ObterAcervoTridimensionalLinhaRetornoDto),
                Sucesso = acervosTridimensionalLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(ObterAcervoTridimensionalLinhaRetornoDto)
            };
            return acervoTridimensionalRetorno;
        }
        
        private static AcervoTridimensionalLinhaRetornoDTO ObterAcervoTridimensionalLinhaRetornoDto(AcervoTridimensionalLinhaDTO s)
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
                Diametro = ObterConteudoMensagemStatus(s.Diametro)
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
                        Profundidade = acervoTridimensionalLinha.Profundidade.Conteudo.ObterLongoOuNuloPorValorDoCampo(),
                        Diametro = acervoTridimensionalLinha.Diametro.Conteudo.ObterLongoOuNuloPorValorDoCampo(),
                    };
                    await servicoAcervoTridimensional.Inserir(acervoTridimensional);
        
                    acervoTridimensionalLinha.Status = ImportacaoStatus.Sucesso;
                    acervoTridimensionalLinha.Mensagem = string.Empty;
                    acervoTridimensionalLinha.PossuiErros = false;
                }
                catch (Exception ex)
                {
                    acervoTridimensionalLinha.PossuiErros = true;
                    acervoTridimensionalLinha.Status = ImportacaoStatus.Erros;
                    acervoTridimensionalLinha.Mensagem = ex.Message;
                }
            }
        }

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoTridimensionalLinhaDTO> linhas)
        {
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Tombo, Constantes.TOMBO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Procedencia,Constantes.PROCEDENCIA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Data,Constantes.DATA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Quantidade,Constantes.QUANTIDADE, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Profundidade,Constantes.PROFUNDIDADE, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Diametro,Constantes.DIAMETRO, linha.NumeroLinha);
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
                            EhCampoObrigatorio = true
                        },
                        Diametro = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DIAMETRO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        }
                    });
                }
            }

            return linhas;
        }
    }
}