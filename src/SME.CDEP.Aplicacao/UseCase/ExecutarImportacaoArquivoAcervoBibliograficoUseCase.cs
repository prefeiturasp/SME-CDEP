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
    public class ExecutarImportacaoArquivoAcervoBibliograficoUseCase : ServicoImportacaoArquivoBase, IExecutarImportacaoArquivoAcervoBibliograficoUseCase, IImportacaoArquivoAcervoBibliograficoAuxiliar 
    {
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico;
        private readonly IMapper mapper;
        
        public ExecutarImportacaoArquivoAcervoBibliograficoUseCase(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoBibliografico servicoAcervoBibliografico,IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte, servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoAcervoBibliografico = servicoAcervoBibliografico ?? throw new ArgumentNullException(nameof(servicoAcervoBibliografico));
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
            
            await CarregarDominiosBibliograficos();

            var acervosBibliograficosLinhas = JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(importacaoArquivo.Conteudo);
            
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosBibliograficosLinhas);
            
            await PersistenciaAcervo(acervosBibliograficosLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosBibliograficosLinhas), acervosBibliograficosLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            return true;
        }

        public async Task CarregarDominiosBibliograficos()
        {
            await CarregarTodosOsDominios();
            
            await ObterMateriaisPorTipo(TipoMaterial.BIBLIOGRAFICO);
                
            await ObterCoAutores(TipoCreditoAutoria.Coautor);
            
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Autoria);
        }

        public async Task PersistenciaAcervo(IEnumerable<AcervoBibliograficoLinhaDTO> acervosBibliograficosLinhas)
        {
            foreach (var acervoBibliograficoLinha in acervosBibliograficosLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoBibliografico = new AcervoBibliograficoCadastroDTO()
                    {
                        Titulo = acervoBibliograficoLinha.Titulo.Conteudo,

                        SubTitulo = acervoBibliograficoLinha.SubTitulo.Conteudo,

                        MaterialId = ObterMaterialBibliograficoIdPorValorDoCampo(acervoBibliograficoLinha.Material.Conteudo),

                        CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(acervoBibliograficoLinha.Autor.Conteudo, TipoCreditoAutoria.Autoria),

                        CoAutores = ObterCoAutoresTipoAutoria(acervoBibliograficoLinha.CoAutor.Conteudo,acervoBibliograficoLinha.TipoAutoria.Conteudo),

                        EditoraId = ObterEditoraIdOuNuloPorValorDoCampo(acervoBibliograficoLinha.Editora.Conteudo),

                        AssuntosIds = ObterAssuntosIdsPorValorDoCampo(acervoBibliograficoLinha.Assunto.Conteudo),

                        Ano = acervoBibliograficoLinha.Ano.Conteudo,
                        Edicao = acervoBibliograficoLinha.Edicao.Conteudo,
                        NumeroPagina = acervoBibliograficoLinha.NumeroPaginas.Conteudo.ObterInteiroOuNuloPorValorDoCampo(),
                        Largura = acervoBibliograficoLinha.Largura.Conteudo,
                        Altura = acervoBibliograficoLinha.Altura.Conteudo,

                        SerieColecaoId = ObterSerieColecaoIdOuNuloPorValorDoCampo(acervoBibliograficoLinha.SerieColecao.Conteudo),

                        Volume = acervoBibliograficoLinha.Volume.Conteudo,

                        IdiomaId = ObterIdiomaIdPorValorDoCampo(acervoBibliograficoLinha.Idioma.Conteudo),

                        LocalizacaoCDD = acervoBibliograficoLinha.LocalizacaoCDD.Conteudo,
                        LocalizacaoPHA = acervoBibliograficoLinha.LocalizacaoPHA.Conteudo,
                        NotasGerais = acervoBibliograficoLinha.NotasGerais.Conteudo,
                        Isbn = acervoBibliograficoLinha.Isbn.Conteudo,
                        Codigo = acervoBibliograficoLinha.Codigo.Conteudo
                    };
                    await servicoAcervoBibliografico.Inserir(acervoBibliografico);

                    acervoBibliograficoLinha.DefinirLinhaComoSucesso();
                }
                catch (Exception ex)
                {
                    acervoBibliograficoLinha.DefinirLinhaComoErro(ex.Message);
                }
            }
        }

        public CoAutorDTO[] ObterCoAutoresTipoAutoria(string coautores, string tiposAutoria)
        {
            if (coautores.NaoEstaPreenchido())
                return null;
            
            var coAutoresEmTexto = coautores.FormatarTextoEmArray();
            
            var coAutoresEmTextoAutoNumerados = coAutoresEmTexto.Select((coautoresEmTexto, indice) => new IdNomeDTO { Id = indice + 1, Nome = coautoresEmTexto }).ToList();

            var tiposAutoriaEmTextoAutoNumerados = Enumerable.Empty<IdNomeDTO>();

            if (tiposAutoria.EstaPreenchido())
            {
                var tiposAutoriaEmTexto = tiposAutoria.FormatarTextoEmArraySemDistinct();
            
                tiposAutoriaEmTextoAutoNumerados = tiposAutoriaEmTexto.Select((tipoAutoria, indice) => new IdNomeDTO() { Id = indice + 1, Nome = tipoAutoria });    
            }
            
            var coAutoresCompletos = coAutoresEmTextoAutoNumerados.Select(coAutorAutoNumerado => new CoAutorDTO
            {
                CreditoAutorId = CoAutores.FirstOrDefault(f=> f.Nome.RemoverAcentuacao().SaoIguais(coAutorAutoNumerado.Nome.RemoverAcentuacao()))?.Id,
                TipoAutoria = tiposAutoriaEmTextoAutoNumerados.FirstOrDefault(f => f.Id.SaoIguais(coAutorAutoNumerado.Id))?.Nome,
                CreditoAutorNome = CoAutores.FirstOrDefault(f=> f.Nome.RemoverAcentuacao().SaoIguais(coAutorAutoNumerado.Nome.RemoverAcentuacao()))?.Nome,
            }).ToArray();

            return coAutoresCompletos;
        }

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoBibliograficoLinhaDTO> linhas)
        {
            var autores = CreditosAutores.Select(s=> mapper.Map<IdNomeDTO>(s));
            var coAutores = CoAutores.Select(s=> mapper.Map<IdNomeDTO>(s));
            var materiaisBibliograficos = Materiais.Where(w => w.Tipo == (int)TipoMaterial.BIBLIOGRAFICO).Select(s=> mapper.Map<IdNomeDTO>(s));
            
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO);
                    ValidarPreenchimentoLimiteCaracteres(linha.SubTitulo, Constantes.SUB_TITULO);

                    ValidarPreenchimentoLimiteCaracteres(linha.Material,Constantes.MATERIAL);
                    ValidarConteudoCampoComDominio(linha.Material, materiaisBibliograficos, Constantes.MATERIAL);

                    ValidarPreenchimentoLimiteCaracteres(linha.Autor,Constantes.AUTOR);
                    ValidarConteudoCampoListaComDominio(linha.Autor, autores, Constantes.AUTOR);

                    ValidarPreenchimentoLimiteCaracteres(linha.CoAutor,Constantes.CO_AUTOR);
                    ValidarConteudoCampoListaComDominio(linha.CoAutor, coAutores, Constantes.CO_AUTOR);

                    ValidarPreenchimentoLimiteCaracteres(linha.TipoAutoria,Constantes.TIPO_AUTORIA);
                    
                    if (linha.TipoAutoria.Conteudo.EstaPreenchido() && linha.CoAutor.Conteudo.NaoEstaPreenchido())
                        DefinirMensagemErro(linha.TipoAutoria, Constantes.CAMPO_COAUTOR_SEM_PREENCHIMENTO_E_TIPO_AUTORIA_PREENCHIDO);

                    if (linha.TipoAutoria.Conteudo.SplitPipe().Count() > linha.CoAutor.Conteudo.SplitPipe().Count())
                        DefinirMensagemErro(linha.TipoAutoria, Constantes.TEMOS_MAIS_TIPO_AUTORIA_QUE_COAUTORES);

                    ValidarPreenchimentoLimiteCaracteres(linha.Editora,Constantes.EDITORA);
                    ValidarConteudoCampoComDominio(linha.Editora, Editoras, Constantes.EDITORA);

                    ValidarPreenchimentoLimiteCaracteres(linha.Assunto,Constantes.ASSUNTO);
                    ValidarConteudoCampoListaComDominio(linha.Assunto, Assuntos, Constantes.ASSUNTO);

                    ValidarPreenchimentoLimiteCaracteres(linha.Ano,Constantes.ANO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Edicao,Constantes.EDICAO);

                    ValidarPreenchimentoLimiteCaracteres(linha.NumeroPaginas,Constantes.NUMERO_PAGINAS);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);

                    ValidarPreenchimentoLimiteCaracteres(linha.SerieColecao,Constantes.SERIE_COLECAO);
                    ValidarConteudoCampoListaComDominio(linha.SerieColecao, SeriesColecoes, Constantes.SERIE_COLECAO);

                    ValidarPreenchimentoLimiteCaracteres(linha.Volume,Constantes.VOLUME);

                    ValidarPreenchimentoLimiteCaracteres(linha.Idioma,Constantes.IDIOMA);
                    ValidarConteudoCampoComDominio(linha.Idioma, Idiomas, Constantes.IDIOMA);

                    ValidarPreenchimentoLimiteCaracteres(linha.LocalizacaoCDD,Constantes.LOCALIZACAO_CDD);
                    ValidarPreenchimentoLimiteCaracteres(linha.LocalizacaoPHA,Constantes.LOCALIZACAO_PHA);

                    ValidarPreenchimentoLimiteCaracteres(linha.NotasGerais,Constantes.NOTAS_GERAIS);
                    ValidarPreenchimentoLimiteCaracteres(linha.Isbn,Constantes.ISBN);

                    ValidarPreenchimentoLimiteCaracteres(linha.Codigo,Constantes.TOMBO);
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, linha.NumeroLinha, e.Message));
                }
            }
        }

        private bool PossuiErro(AcervoBibliograficoLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.SubTitulo.PossuiErro 
                   || linha.Material.PossuiErro 
                   || linha.Autor.PossuiErro 
                   || linha.CoAutor.PossuiErro 
                   || linha.TipoAutoria.PossuiErro 
                   || linha.Editora.PossuiErro 
                   || linha.Edicao.PossuiErro 
                   || linha.Assunto.PossuiErro 
                   || linha.Ano.PossuiErro 
                   || linha.NumeroPaginas.PossuiErro
                   || linha.Largura.PossuiErro 
                   || linha.Altura.PossuiErro 
                   || linha.SerieColecao.PossuiErro 
                   || linha.Volume.PossuiErro 
                   || linha.Idioma.PossuiErro
                   || linha.LocalizacaoCDD.PossuiErro
                   || linha.LocalizacaoPHA.PossuiErro
                   || linha.NotasGerais.PossuiErro
                   || linha.Isbn.PossuiErro
                   || linha.Codigo.PossuiErro;
        }
    }
}