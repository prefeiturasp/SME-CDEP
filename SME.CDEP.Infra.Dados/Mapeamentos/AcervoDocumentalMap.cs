using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoDocumentalMap : BaseMapSomenteId<AcervoDocumental>
    {
        public AcervoDocumentalMap()
        {
            ToTable("acervo_documental");
            Map(c => c.AcervoId).ToColumn("acervo_id");
            Map(c => c.MaterialId).ToColumn("material_id");
            Map(c => c.IdiomalId).ToColumn("idioma_id");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.NumeroPagina).ToColumn("numero_pagina");
            Map(c => c.Volume).ToColumn("volume");
            Map(c => c.TipoAnexo).ToColumn("tipo_anexo");
            Map(c => c.Largura).ToColumn("largura");
            Map(c => c.Altura).ToColumn("altura");
            Map(c => c.TamanhoArquivo).ToColumn("tamanho_arquivo");
            Map(c => c.Localizacao).ToColumn("localizacao");
            Map(c => c.Digitalizado).ToColumn("digitalizado");
            Map(c => c.ConservacaoId).ToColumn("conservacao_id");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Excluido).Ignore();
        }
    }
}