﻿
namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoArteGraficaLinhaDTO: AcervoLinhaDTO
{
    public LinhaConteudoAjustarDTO Titulo { get; set; }
    public LinhaConteudoAjustarDTO Tombo { get; set; }
    public LinhaConteudoAjustarDTO Credito { get; set; }
    public LinhaConteudoAjustarDTO Localizacao { get; set; }
    public LinhaConteudoAjustarDTO Procedencia { get; set; }
    public LinhaConteudoAjustarDTO Data { get; set; }
    public LinhaConteudoAjustarDTO CopiaDigital { get; set; }
    public LinhaConteudoAjustarDTO AutorizacaoUsoDeImagem { get; set; }
    public LinhaConteudoAjustarDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarDTO Cromia { get; set; }
    public LinhaConteudoAjustarDTO Largura { get; set; }
    public LinhaConteudoAjustarDTO Altura { get; set; }
    public LinhaConteudoAjustarDTO Diametro { get; set; }
    public LinhaConteudoAjustarDTO Tecnica { get; set; }
    public LinhaConteudoAjustarDTO Suporte { get; set; }
    public LinhaConteudoAjustarDTO Quantidade { get; set; }
    public LinhaConteudoAjustarDTO Descricao { get; set; }
}