﻿using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoFotograficoMap : BaseMapSomenteId<AcervoFotografico>
    {
        public AcervoFotograficoMap()
        {
            ToTable("acervo_fotografico");
            Map(c => c.AcervoId).ToColumn("acervo_id");
            Map(c => c.Localizacao).ToColumn("localizacao");
            Map(c => c.Procedencia).ToColumn("procedencia");
            Map(c => c.CopiaDigital).ToColumn("copia_digital");
            Map(c => c.PermiteUsoImagem).ToColumn("permite_uso_imagem");
            Map(c => c.ConservacaoId).ToColumn("conservacao_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.Largura).ToColumn("largura");
            Map(c => c.Altura).ToColumn("altura");
            Map(c => c.SuporteId).ToColumn("suporte_id");
            Map(c => c.FormatoId).ToColumn("formato_id");
            Map(c => c.CromiaId).ToColumn("cromia_id");
            Map(c => c.Resolucao).ToColumn("resolucao");
            Map(c => c.TamanhoArquivo).ToColumn("tamanho_arquivo");
            Map(c => c.Excluido).Ignore();
        }
    }
}