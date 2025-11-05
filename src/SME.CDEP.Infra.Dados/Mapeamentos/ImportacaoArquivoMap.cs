using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class ImportacaoArquivoMap : BaseMapAuditavel<ImportacaoArquivo>
    {
        public ImportacaoArquivoMap()
        {
            ToTable("importacao_arquivo");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.TipoAcervo).ToColumn("tipo_acervo");
            Map(c => c.Status).ToColumn("status");
            Map(c => c.Conteudo).ToColumn("conteudo");
        }
    }
}