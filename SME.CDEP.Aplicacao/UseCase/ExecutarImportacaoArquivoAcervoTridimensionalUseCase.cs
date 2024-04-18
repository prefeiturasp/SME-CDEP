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
    public class ExecutarImportacaoArquivoAcervoTridimensionalUseCase : ServicoImportacaoArquivoBase, IExecutarImportacaoArquivoAcervoTridimensionalUseCase
    {
        private readonly IServicoAcervoTridimensional servicoAcervoTridimensional;
        private readonly IMapper mapper;
        
        public ExecutarImportacaoArquivoAcervoTridimensionalUseCase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoTridimensional servicoAcervoTridimensional,IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte, servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoAcervoTridimensional = servicoAcervoTridimensional ?? throw new ArgumentNullException(nameof(servicoAcervoTridimensional));
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
            
            await CarregarTodosOsDominios();

            var acervoTridimensionalLinhaDtos = JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(importacaoArquivo.Conteudo);
            
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoTridimensionalLinhaDtos);
            
            await PersistenciaAcervo(acervoTridimensionalLinhaDtos);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervoTridimensionalLinhaDtos), acervoTridimensionalLinhaDtos.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            return true;
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
                        Codigo = acervoTridimensionalLinha.Codigo.Conteudo,
                        Procedencia = acervoTridimensionalLinha.Procedencia.Conteudo,
                        Ano = acervoTridimensionalLinha.Ano.Conteudo,
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoTridimensionalLinha.EstadoConservacao.Conteudo),
                        Quantidade = acervoTridimensionalLinha.Quantidade.Conteudo.ConverterParaInteiro(),
                        Descricao = acervoTridimensionalLinha.Descricao.Conteudo,
                        Largura = acervoTridimensionalLinha.Largura.Conteudo,
                        Altura = acervoTridimensionalLinha.Altura.Conteudo,
                        Profundidade = acervoTridimensionalLinha.Profundidade.Conteudo,
                        Diametro = acervoTridimensionalLinha.Diametro.Conteudo,
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
                    ValidarPreenchimentoLimiteCaracteres(linha.Codigo, Constantes.TOMBO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Procedencia,Constantes.PROCEDENCIA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Ano,Constantes.ANO);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);
                    ValidarConteudoCampoComDominio(linha.EstadoConservacao, Conservacoes, Constantes.ESTADO_CONSERVACAO);
                    
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
                   || linha.Codigo.PossuiErro 
                   || linha.Procedencia.PossuiErro
                   || linha.Ano.PossuiErro
                   || linha.EstadoConservacao.PossuiErro
                   || linha.Quantidade.PossuiErro
                   || linha.Descricao.PossuiErro
                   || linha.Largura.PossuiErro
                   || linha.Altura.PossuiErro
                   || linha.Profundidade.PossuiErro
                   || linha.Diametro.PossuiErro;
        }
    }
}