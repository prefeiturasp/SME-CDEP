using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDocumentalLinhaDTO: AcervoLinhaDTO
{
    public LinhaConteudoAjustarDTO Titulo { get; set; }
    public LinhaConteudoAjustarDTO Codigo { get; set; }
    public LinhaConteudoAjustarDTO CodigoNovo { get; set; }
    public LinhaConteudoAjustarDTO Material { get; set; }
    public LinhaConteudoAjustarDTO Idioma { get; set; }
    public LinhaConteudoAjustarDTO Autor { get; set; }
    public LinhaConteudoAjustarDTO Ano { get; set; }
    public LinhaConteudoAjustarDTO NumeroPaginas { get; set; }
    public LinhaConteudoAjustarDTO Volume { get; set; }
    public LinhaConteudoAjustarDTO Descricao { get; set; }
    public LinhaConteudoAjustarDTO TipoAnexo { get; set; }
    public LinhaConteudoAjustarDTO Altura { get; set; }
    public LinhaConteudoAjustarDTO Largura { get; set; }
    public LinhaConteudoAjustarDTO TamanhoArquivo { get; set; }
    public LinhaConteudoAjustarDTO AcessoDocumento { get; set; }
    public LinhaConteudoAjustarDTO Localizacao { get; set; }
    public LinhaConteudoAjustarDTO CopiaDigital { get; set; }
    public LinhaConteudoAjustarDTO EstadoConservacao { get; set; }

    public void DefinirLinhaComoSucesso()
    {
        PossuiErros = false;
        Mensagem = string.Empty;
        Status = ImportacaoStatus.Sucesso;

        Titulo.DefinirComoSucesso();
        Codigo.DefinirComoSucesso();
        CodigoNovo.DefinirComoSucesso();
        Material.DefinirComoSucesso();
        Idioma.DefinirComoSucesso();
        Autor.DefinirComoSucesso();
        Ano.DefinirComoSucesso();
        NumeroPaginas.DefinirComoSucesso();
        Volume.DefinirComoSucesso();
        Descricao.DefinirComoSucesso();
        Altura.DefinirComoSucesso();
        Largura.DefinirComoSucesso();
        TipoAnexo.DefinirComoSucesso();
        TamanhoArquivo.DefinirComoSucesso();
        AcessoDocumento.DefinirComoSucesso();
        Localizacao.DefinirComoSucesso();
        CopiaDigital.DefinirComoSucesso();
        EstadoConservacao.DefinirComoSucesso();
    }
}