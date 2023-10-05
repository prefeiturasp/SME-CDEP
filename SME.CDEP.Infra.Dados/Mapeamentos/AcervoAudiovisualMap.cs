using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoAudiovisualMap : BaseMapSomenteId<AcervoAudiovisual>
    {
        public AcervoAudiovisualMap()
        {
            ToTable("acervo_audiovisual");
            Map(c => c.AcervoId).ToColumn("acervo_id");
            Map(c => c.Localizacao).ToColumn("localizacao");
            Map(c => c.Procedencia).ToColumn("procedencia");
            Map(c => c.DataAcervo).ToColumn("data_acervo");
            Map(c => c.Copia).ToColumn("copia");
            Map(c => c.PermiteUsoImagem).ToColumn("permite_uso_imagem");
            Map(c => c.ConservacaoId).ToColumn("conservacao_id");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.SuporteId).ToColumn("suporte_id");
            Map(c => c.Duracao).ToColumn("duracao");
            Map(c => c.CromiaId).ToColumn("cromia_id");
            Map(c => c.TamanhoArquivo).ToColumn("tamanho_arquivo");
            Map(c => c.Acessibilidade ).ToColumn("acessibilidade");
            Map(c => c.Disponibilizacao ).ToColumn("disponibilizacao");
            Map(c => c.Excluido).Ignore();
        }
    }
}