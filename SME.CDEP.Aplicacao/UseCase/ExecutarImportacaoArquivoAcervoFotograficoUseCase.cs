using AutoMapper;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ExecutarImportacaoArquivoAcervoFotograficoUseCase : ServicoImportacaoArquivoBase, IExecutarImportacaoArquivoAcervoFotograficoUseCase,IImportacaoArquivoAcervoFotograficoAuxiliar
    {
        private readonly IServicoAcervoFotografico servicoAcervoFotografico;
        private readonly IMapper mapper;
        
        public ExecutarImportacaoArquivoAcervoFotograficoUseCase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoFotografico servicoAcervoFotografico,IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte, servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoAcervoFotografico = servicoAcervoFotografico ?? throw new ArgumentNullException(nameof(servicoAcervoFotografico));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (param.Mensagem.EhNulo())
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            long importacaoArquivoId = 0;
            
            if (!long.TryParse(param.Mensagem.ToString(),out importacaoArquivoId))
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);
            
            var importacaoArquivo = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);
            
            if (importacaoArquivo.EhNulo())
                throw new NegocioException(MensagemNegocio.IMPORTACAO_NAO_LOCALIZADA);
            
            await CarregarDominiosFotograficos();

            var acervoFotograficoLinhaDtos = JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(importacaoArquivo.Conteudo);
            
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoFotograficoLinhaDtos);
            
            await PersistenciaAcervo(acervoFotograficoLinhaDtos);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervoFotograficoLinhaDtos), acervoFotograficoLinhaDtos.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            return true;
        }

        public async Task CarregarDominiosFotograficos()
        {
            await CarregarTodosOsDominios();
            
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Credito);
                
            await ObterFormatosPorTipo(TipoFormato.ACERVO_FOTOS);
            
            await ObterSuportesPorTipo(TipoSuporte.IMAGEM);
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
                            .Where(f => acervoFotograficoLinha.Credito.Conteudo.RemoverAcentuacaoFormatarMinusculo().FormatarTextoEmArray().Contains(f.Nome.RemoverAcentuacaoFormatarMinusculo()))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoFotograficoLinha.Localizacao.Conteudo,
                        Procedencia = acervoFotograficoLinha.Procedencia.Conteudo,
                        Ano = acervoFotograficoLinha.Ano.Conteudo,
                        DataAcervo = acervoFotograficoLinha.Data.Conteudo,
                        CopiaDigital = ObterCopiaDigitalPorValorDoCampo(acervoFotograficoLinha.CopiaDigital.Conteudo),
                        PermiteUsoImagem = ObterAutorizaUsoDeImagemPorValorDoCampo(acervoFotograficoLinha.PermiteUsoImagem.Conteudo),
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoFotograficoLinha.EstadoConservacao.Conteudo),
                        Descricao = acervoFotograficoLinha.Descricao.Conteudo,
                        Quantidade = acervoFotograficoLinha.Quantidade.Conteudo.ConverterParaInteiro(),
                        Largura = acervoFotograficoLinha.Largura.Conteudo,
                        Altura = acervoFotograficoLinha.Altura.Conteudo,
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
            var creditos = CreditosAutores.Where(w => w.Tipo == (int)TipoCreditoAutoria.Credito).Select(s=> mapper.Map<IdNomeDTO>(s));
            
            var formatosFotos = Formatos.Where(w => w.Tipo == (int)TipoFormato.ACERVO_FOTOS).Select(s=> mapper.Map<IdNomeDTO>(s));

            var suporteImagens = Suportes.Where(w => w.Tipo == (int)TipoSuporte.IMAGEM).Select(s=> mapper.Map<IdNomeDTO>(s));
            
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Codigo, Constantes.TOMBO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Credito, Constantes.CREDITO);
                    ValidarConteudoCampoListaComDominio(linha.Credito, creditos, Constantes.CREDITO);

                    ValidarPreenchimentoLimiteCaracteres(linha.Localizacao,Constantes.LOCALIZACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Procedencia,Constantes.PROCEDENCIA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Ano,Constantes.ANO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Data,Constantes.DATA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.CopiaDigital,Constantes.COPIA_DIGITAL);
                    ValidarPreenchimentoLimiteCaracteres(linha.PermiteUsoImagem,Constantes.AUTORIZACAO_USO_DE_IMAGEM);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);
                    ValidarConteudoCampoComDominio(linha.EstadoConservacao, Conservacoes, Constantes.ESTADO_CONSERVACAO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Quantidade,Constantes.QUANTIDADE);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Suporte,Constantes.SUPORTE);
                    ValidarConteudoCampoComDominio(linha.Suporte, suporteImagens, Constantes.SUPORTE);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.FormatoImagem,Constantes.FORMATO_IMAGEM);
                    ValidarConteudoCampoComDominio(linha.FormatoImagem, formatosFotos, Constantes.FORMATO_IMAGEM);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.TamanhoArquivo,Constantes.TAMANHO_ARQUIVO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Cromia,Constantes.CROMIA);
                    ValidarConteudoCampoComDominio(linha.Cromia, Cromias, Constantes.CROMIA);
                    
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
                   || linha.Ano.PossuiErro
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
    }
}