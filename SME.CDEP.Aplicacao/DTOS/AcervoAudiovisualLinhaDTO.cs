using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAudiovisualLinhaDTO: AcervoLinhaDTO
{
    public LinhaConteudoAjustarDTO Titulo { get; set; }
    public LinhaConteudoAjustarDTO Codigo { get; set; }
    public LinhaConteudoAjustarDTO Credito { get; set; }
    public LinhaConteudoAjustarDTO Localizacao { get; set; }
    public LinhaConteudoAjustarDTO Procedencia { get; set; }
    public LinhaConteudoAjustarDTO Copia { get; set; }
    public LinhaConteudoAjustarDTO PermiteUsoImagem { get; set; }
    public LinhaConteudoAjustarDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarDTO Descricao { get; set; }
    public LinhaConteudoAjustarDTO Suporte { get; set; }
    public LinhaConteudoAjustarDTO Duracao { get; set; }
    public LinhaConteudoAjustarDTO Cromia { get; set; }
    public LinhaConteudoAjustarDTO TamanhoArquivo { get; set; }
    public LinhaConteudoAjustarDTO Acessibilidade { get; set; }
    public LinhaConteudoAjustarDTO Disponibilizacao { get; set; }
    public LinhaConteudoAjustarDTO Ano { get; set; }

    public void DefinirLinhaComoSucesso()
    {
        PossuiErros = false;
        Mensagem = string.Empty;
        Status = ImportacaoStatus.Sucesso;

        Titulo.DefinirComoSucesso();
        Codigo.DefinirComoSucesso();
        Credito.DefinirComoSucesso();
        Localizacao.DefinirComoSucesso();
        Procedencia.DefinirComoSucesso();
        Copia.DefinirComoSucesso();
        PermiteUsoImagem.DefinirComoSucesso();
        EstadoConservacao.DefinirComoSucesso();
        Descricao.DefinirComoSucesso();
        Suporte.DefinirComoSucesso();
        Cromia.DefinirComoSucesso();
        TamanhoArquivo.DefinirComoSucesso();
        Acessibilidade.DefinirComoSucesso();
        Disponibilizacao.DefinirComoSucesso();
    }
}