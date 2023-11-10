using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoBibliograficoLinhaDTO: AcervoLinhaDTO
{
    public LinhaConteudoAjustarDTO Titulo { get; set; }
    public LinhaConteudoAjustarDTO SubTitulo { get; set; }
    public LinhaConteudoAjustarDTO Material { get; set; }
    public LinhaConteudoAjustarDTO Autor { get; set; }
    public LinhaConteudoAjustarDTO CoAutor { get; set; }
    public LinhaConteudoAjustarDTO TipoAutoria { get; set; }
    public LinhaConteudoAjustarDTO Editora { get; set; }
    public LinhaConteudoAjustarDTO Assunto { get; set; }
    public LinhaConteudoAjustarDTO Ano { get; set; }
    public LinhaConteudoAjustarDTO Edicao { get; set; }
    public LinhaConteudoAjustarDTO NumeroPaginas { get; set; }
    public LinhaConteudoAjustarDTO Altura { get; set; }
    public LinhaConteudoAjustarDTO Largura { get; set; }
    public LinhaConteudoAjustarDTO SerieColecao { get; set; }
    public LinhaConteudoAjustarDTO Volume { get; set; }
    public LinhaConteudoAjustarDTO Idioma { get; set; }
    public LinhaConteudoAjustarDTO LocalizacaoCDD { get; set; }
    public LinhaConteudoAjustarDTO LocalizacaoPHA { get; set; }
    public LinhaConteudoAjustarDTO NotasGerais { get; set; }
    public LinhaConteudoAjustarDTO Isbn { get; set; }
    public LinhaConteudoAjustarDTO Tombo { get; set; }

    public void DefinirLinhaComoSucesso()
    {
        PossuiErros = false;
        Mensagem = string.Empty;
        Status = ImportacaoStatus.Sucesso;

        Titulo.DefinirComoSucesso();
        SubTitulo.DefinirComoSucesso();
        Material.DefinirComoSucesso();
        Autor.DefinirComoSucesso();
        CoAutor.DefinirComoSucesso();
        TipoAutoria.DefinirComoSucesso();
        Editora.DefinirComoSucesso();
        Assunto.DefinirComoSucesso();
        Ano.DefinirComoSucesso();
        Edicao.DefinirComoSucesso();
        NumeroPaginas.DefinirComoSucesso();
        Altura.DefinirComoSucesso();
        Largura.DefinirComoSucesso();
        SerieColecao.DefinirComoSucesso();
        Volume.DefinirComoSucesso();
        Idioma.DefinirComoSucesso();
        LocalizacaoCDD.DefinirComoSucesso();
        LocalizacaoPHA.DefinirComoSucesso();
        NotasGerais.DefinirComoSucesso();
        Isbn.DefinirComoSucesso();
        Tombo.DefinirComoSucesso();
    }
}