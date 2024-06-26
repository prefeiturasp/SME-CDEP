﻿using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalLinhaDTO: AcervoLinhaDTO
{
    public LinhaConteudoAjustarDTO Titulo { get; set; }
    public LinhaConteudoAjustarDTO Codigo { get; set; }
    public LinhaConteudoAjustarDTO Procedencia { get; set; }
    public LinhaConteudoAjustarDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarDTO Quantidade { get; set; }
    public LinhaConteudoAjustarDTO Descricao { get; set; }
    public LinhaConteudoAjustarDTO Largura { get; set; }
    public LinhaConteudoAjustarDTO Altura { get; set; }
    public LinhaConteudoAjustarDTO Profundidade { get; set; }
    public LinhaConteudoAjustarDTO Diametro { get; set; }
    public LinhaConteudoAjustarDTO Ano { get; set; }

    public void DefinirLinhaComoSucesso()
    {
        PossuiErros = false;
        Mensagem = string.Empty;
        Status = ImportacaoStatus.Sucesso;

        Titulo.DefinirComoSucesso();
        Codigo.DefinirComoSucesso();
        Procedencia.DefinirComoSucesso();
        EstadoConservacao.DefinirComoSucesso();
        Descricao.DefinirComoSucesso();
        Quantidade.DefinirComoSucesso();
        Altura.DefinirComoSucesso();
        Largura.DefinirComoSucesso();
        Profundidade.DefinirComoSucesso();
        Diametro.DefinirComoSucesso();
    }
}