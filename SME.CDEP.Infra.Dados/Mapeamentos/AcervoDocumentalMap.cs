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
            Map(c => c.IdiomaId).ToColumn("idioma_id");
            Map(c => c.NumeroPagina).ToColumn("numero_pagina");
            Map(c => c.Volume).ToColumn("volume");
            Map(c => c.TipoAnexo).ToColumn("tipo_anexo");
            Map(c => c.Largura).ToColumn("largura");
            Map(c => c.Altura).ToColumn("altura");
            Map(c => c.TamanhoArquivo).ToColumn("tamanho_arquivo");
            Map(c => c.Localizacao).ToColumn("localizacao");
            Map(c => c.CopiaDigital).ToColumn("copia_digital");
            Map(c => c.ConservacaoId).ToColumn("conservacao_id");
            Map(c => c.Excluido).Ignore();
        }
    }
}