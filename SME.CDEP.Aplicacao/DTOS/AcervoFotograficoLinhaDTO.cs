﻿using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoFotograficoLinhaDTO: AcervoLinhaDTO
{
    public LinhaConteudoAjustarDTO Titulo { get; set; }
    public LinhaConteudoAjustarDTO Codigo { get; set; }
    public LinhaConteudoAjustarDTO Credito { get; set; }
    public LinhaConteudoAjustarDTO Localizacao { get; set; }
    public LinhaConteudoAjustarDTO Procedencia { get; set; }
    public LinhaConteudoAjustarDTO Data { get; set; }
    public LinhaConteudoAjustarDTO CopiaDigital { get; set; }
    public LinhaConteudoAjustarDTO PermiteUsoImagem { get; set; }
    public LinhaConteudoAjustarDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarDTO Descricao { get; set; }
    public LinhaConteudoAjustarDTO Quantidade { get; set; }
    public LinhaConteudoAjustarDTO Largura { get; set; }
    public LinhaConteudoAjustarDTO Altura { get; set; }
    public LinhaConteudoAjustarDTO Suporte { get; set; }
    public LinhaConteudoAjustarDTO FormatoImagem { get; set; }
    public LinhaConteudoAjustarDTO TamanhoArquivo { get; set; }
    public LinhaConteudoAjustarDTO Cromia { get; set; }
    public LinhaConteudoAjustarDTO Resolucao { get; set; }
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
        Data.DefinirComoSucesso();
        CopiaDigital.DefinirComoSucesso();
        PermiteUsoImagem.DefinirComoSucesso();
        EstadoConservacao.DefinirComoSucesso();
        Descricao.DefinirComoSucesso();
        Quantidade.DefinirComoSucesso();
        Altura.DefinirComoSucesso();
        Largura.DefinirComoSucesso();
        Suporte.DefinirComoSucesso();
        FormatoImagem.DefinirComoSucesso();
        TamanhoArquivo.DefinirComoSucesso();
        Cromia.DefinirComoSucesso();
        Resolucao.DefinirComoSucesso();
    }
}