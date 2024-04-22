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
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoAudiovisual : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoAudiovisual 
    {
        private readonly IServicoMensageria servicoMensageria;
        
        public ServicoImportacaoArquivoAcervoAudiovisual(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoMensageria servicoMensageria, IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte, servicoFormato, mapper, repositorioParametroSistema)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }
        
        public async Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo)
        {
            return await RemoverLinhaDoArquivo<AcervoAudiovisualLinhaDTO>(id, linhaDoArquivo, TipoAcervo.Audiovisual);
        }
        
        public async Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoAudiovisualLinhaDTO>(id, linhaDoArquivo.NumeroLinha, TipoAcervo.Audiovisual);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo.NumeroLinha));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Audiovisual);
            
            if (arquivoImportado.EhNulo())
                return default;
        
            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivoImportado.Conteudo), false);
        }
        
        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id)
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterImportacaoPorId(id);
            
            if (arquivoImportado.EhNulo())
                return default;
        
            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivoImportado.Conteudo), false);
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosAudiovisualLinhas = await LerPlanilha(file);
        
            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.Audiovisual, JsonConvert.SerializeObject(acervosAudiovisualLinhas));

            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            await servicoMensageria.Publicar(RotasRabbit.ExecutarImportacaoArquivoAcervoAudiovisual, importacaoArquivoId, Guid.NewGuid());

            return new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = importacaoArquivoId,
                Nome = importacaoArquivo.Nome,
                TipoAcervo = importacaoArquivo.TipoAcervo,
                DataImportacao = DateTimeExtension.HorarioBrasilia(),
                Status = ImportacaoStatus.Pendente
            };
        }
        
        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoAudiovisualLinhaDTO> acervosAudiovisualLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
                await CarregarTodosOsDominios();
            
            await ObterSuportesPorTipo(TipoSuporte.VIDEO);
            
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Credito);
            
            var acervoAudiovisualRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Status = arquivoImportado.Status,
                Erros =  acervosAudiovisualLinhas
                    .Where(w => w.PossuiErros)
                    .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s, arquivoImportado.TipoAcervo)),
                Sucesso = acervosAudiovisualLinhas
                    .Where(w => !w.PossuiErros)
                    .Select(s=> ObterLinhasComSucessoSufixo(s.Titulo.Conteudo, s.Codigo.Conteudo, s.NumeroLinha,Constantes.SIGLA_ACERVO_AUDIOVISUAL)),
            };
            return acervoAudiovisualRetorno;
        }
        
        private AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoAudiovisualLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterSufixo(ObterConteudoTexto(linha.Codigo),Constantes.SIGLA_ACERVO_AUDIOVISUAL),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoAudiovisualDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoAudiovisualDTO ObterAcervoAudiovisualDto(AcervoAudiovisualLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoAudiovisualDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                TipoAcervoId = (int)tipoAcervo,
                Codigo = ObterSufixo(ObterConteudoTexto(linha.Codigo),Constantes.SIGLA_ACERVO_AUDIOVISUAL),
                Localizacao = ObterConteudoTexto(linha.Localizacao),
                Procedencia = ObterConteudoTexto(linha.Procedencia),
                Ano = ObterConteudoTexto(linha.Ano),
                Copia = ObterConteudoTexto(linha.Copia),
                PermiteUsoImagem = ObterConteudoSimNao(linha.PermiteUsoImagem),
                ConservacaoId = ObterConservacaoIdOuNuloPorValorDoCampo(linha.EstadoConservacao.Conteudo),
                Descricao = ObterConteudoTexto(linha.Descricao),
                SuporteId = ObterSuporteVideoIdOuNuloPorValorDoCampo(linha.Suporte.Conteudo),
                Duracao = ObterConteudoTexto(linha.Duracao),
                CromiaId = ObterCromiaIdOuNuloPorValorDoCampo(linha.Cromia.Conteudo),
                TamanhoArquivo = ObterConteudoTexto(linha.TamanhoArquivo),
                Acessibilidade = ObterConteudoTexto(linha.Acessibilidade),
                Disponibilizacao = ObterConteudoTexto(linha.Disponibilizacao),
                CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(linha.Credito.Conteudo, TipoCreditoAutoria.Credito, false),
            };
        }
        
        private AcervoAudiovisualLinhaRetornoDTO ObterLinhasComErros(AcervoAudiovisualLinhaDTO s)
        {
            return new AcervoAudiovisualLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Codigo = ObterConteudoMensagemStatus(s.Codigo),
                CreditosAutoresIds = ObterConteudoMensagemStatus(s.Credito),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                Ano = ObterConteudoMensagemStatus(s.Ano),
                Copia = ObterConteudoMensagemStatus(s.Copia),
                PermiteUsoImagem = ObterConteudoMensagemStatus(s.PermiteUsoImagem),
                ConservacaoId = ObterConteudoMensagemStatus(s.EstadoConservacao),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                SuporteId = ObterConteudoMensagemStatus(s.Suporte),
                Duracao = ObterConteudoMensagemStatus(s.Duracao),
                CromiaId = ObterConteudoMensagemStatus(s.Cromia),
                TamanhoArquivo = ObterConteudoMensagemStatus(s.TamanhoArquivo),
                Acessibilidade = ObterConteudoMensagemStatus(s.Acessibilidade),
                Disponibilizacao = ObterConteudoMensagemStatus(s.Disponibilizacao),
                NumeroLinha = s.NumeroLinha,
                Status = ImportacaoStatus.Erros,
                Mensagem = s.Mensagem,
                ErrosCampos = ObterMensagemErroLinha(s),
            };
        }
        
        private string[] ObterMensagemErroLinha(AcervoAudiovisualLinhaDTO acervoAudiovisualLinhaDTO)
        {
            var mensagemErro = new List<string>();

            if (acervoAudiovisualLinhaDTO.Titulo.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Titulo.Mensagem);
            
            if (acervoAudiovisualLinhaDTO.Codigo.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Codigo.Mensagem);
            
            if (acervoAudiovisualLinhaDTO.Credito.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Credito.Mensagem);
                    
            if (acervoAudiovisualLinhaDTO.Localizacao.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Localizacao.Mensagem);
                    
            if (acervoAudiovisualLinhaDTO.Procedencia.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Procedencia.Mensagem);
            
            if (acervoAudiovisualLinhaDTO.Ano.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Ano.Mensagem);
            
            if (acervoAudiovisualLinhaDTO.Copia.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Copia.Mensagem);
                    
            if (acervoAudiovisualLinhaDTO.PermiteUsoImagem.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.PermiteUsoImagem.Mensagem);
                    
            if (acervoAudiovisualLinhaDTO.EstadoConservacao.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.EstadoConservacao.Mensagem);
            
            if (acervoAudiovisualLinhaDTO.Descricao.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Descricao.Mensagem);
            
            if (acervoAudiovisualLinhaDTO.Suporte.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Suporte.Mensagem);
            
            if (acervoAudiovisualLinhaDTO.Duracao.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Suporte.Mensagem);
                    
            if (acervoAudiovisualLinhaDTO.Cromia.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Cromia.Mensagem);
                    
            if (acervoAudiovisualLinhaDTO.TamanhoArquivo.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.TamanhoArquivo.Mensagem);
                    
            if (acervoAudiovisualLinhaDTO.Acessibilidade.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Acessibilidade.Mensagem);
                    
            if (acervoAudiovisualLinhaDTO.Disponibilizacao.PossuiErro)
                mensagemErro.Add(acervoAudiovisualLinhaDTO.Disponibilizacao.Mensagem);

            return mensagemErro.ToArray();
        }
        
        private async Task<IEnumerable<AcervoAudiovisualLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoAudiovisualLinhaDTO>();
        
            var stream = file.OpenReadStream();
        
            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();
        
                var totalLinhas = planilha.Rows().Count();
                
                await ValidarQtdeLinhasImportadas(totalLinhas);
                
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);
        
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
                        Codigo = new LinhaConteudoAjustarDTO()
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
                        Ano = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_ANO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_7,
                            EhCampoObrigatorio = true
                        },
                        Copia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_COPIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100, 
                        },
                        PermiteUsoImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_AUDIOVISUAL_CAMPO_AUTORIZACAO_USO_DE_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3,
                            EhCampoObrigatorio = true,
                            ValoresPermitidos  = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO }
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
        
         private void ValidarOrdemColunas(IXLWorksheet planilha, int numeroLinha)
        {
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TITULO, 
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_TITULO, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CREDITO, 
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_CREDITO, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_LOCALIZACAO, 
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_LOCALIZACAO, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_PROCEDENCIA, 
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_PROCEDENCIA, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ANO, 
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_ANO, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_COPIA, 
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_COPIA, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_AUTORIZACAO_USO_DE_IMAGEM,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_AUTORIZACAO_USO_DE_IMAGEM, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_ESTADO_CONSERVACAO, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DESCRICAO,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_DESCRICAO, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_SUPORTE,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_SUPORTE, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DURACAO,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_DURACAO, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CROMIA,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_CROMIA, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TAMANHO_DO_ARQUIVO,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_TAMANHO_ARQUIVO, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ACESSIBILIDADE,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_ACESSIBILIDADE, Constantes.AUDIOVISUAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DISPONIBILIZACAO,
                Constantes.ACERVO_AUDIOVISUAL_CAMPO_DISPONIBILIZACAO, Constantes.AUDIOVISUAL);
        }
    }
}