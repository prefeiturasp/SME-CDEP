using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoBibliograficoMap : BaseMapSomenteId<AcervoBibliografico>
    {
        public AcervoBibliograficoMap()
        {
            ToTable("acervo_bibliografico");
            Map(c => c.AcervoId).ToColumn("acervo_id");
            Map(c => c.MaterialId).ToColumn("material_id");
            Map(c => c.EditoraId).ToColumn("editora_id");
            Map(c => c.Edicao).ToColumn("edicao");
            Map(c => c.NumeroPagina).ToColumn("numero_pagina");
            Map(c => c.Largura).ToColumn("largura");
            Map(c => c.Altura).ToColumn("altura");
            Map(c => c.SerieColecaoId).ToColumn("serie_colecao_id");
            Map(c => c.Volume).ToColumn("volume");
            Map(c => c.IdiomaId).ToColumn("idioma_id");
            Map(c => c.LocalizacaoCDD).ToColumn("localizacao_cdd");
            Map(c => c.LocalizacaoPHA).ToColumn("localizacao_pha");
            Map(c => c.NotasGerais).ToColumn("notas_gerais");
            Map(c => c.Isbn).ToColumn("isbn");
            Map(c => c.Excluido).Ignore();
            Map(c => c.SituacaoSaldo).ToColumn("situacao_saldo");
        }
    }
}