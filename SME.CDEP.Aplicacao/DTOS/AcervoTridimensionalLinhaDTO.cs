using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalLinhaDTO: AcervoLinhaDTO
{
    public LinhaConteudoAjustarDTO Titulo { get; set; }
    public LinhaConteudoAjustarDTO Tombo { get; set; }
    public LinhaConteudoAjustarDTO Procedencia { get; set; }
    public LinhaConteudoAjustarDTO Data { get; set; }
    public LinhaConteudoAjustarDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarDTO Quantidade { get; set; }
    public LinhaConteudoAjustarDTO Descricao { get; set; }
    public LinhaConteudoAjustarDTO Largura { get; set; }
    public LinhaConteudoAjustarDTO Altura { get; set; }
    public LinhaConteudoAjustarDTO Profundidade { get; set; }
    public LinhaConteudoAjustarDTO Diametro { get; set; }

    public void DefinirLinhaComoSucesso()
    {
        PossuiErros = false;
        Mensagem = string.Empty;
        Status = ImportacaoStatus.Sucesso;

        Titulo.DefinirComoSucesso();
        Tombo.DefinirComoSucesso();
        Procedencia.DefinirComoSucesso();
        Data.DefinirComoSucesso();
        EstadoConservacao.DefinirComoSucesso();
        Descricao.DefinirComoSucesso();
        Quantidade.DefinirComoSucesso();
        Altura.DefinirComoSucesso();
        Largura.DefinirComoSucesso();
        Profundidade.DefinirComoSucesso();
        Diametro.DefinirComoSucesso();
    }
}