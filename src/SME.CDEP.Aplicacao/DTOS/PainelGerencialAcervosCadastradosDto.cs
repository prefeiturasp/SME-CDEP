using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class PainelGerencialAcervosCadastradosDto
    {
        public TipoAcervo Id { get; set; }
        public string Nome { get; set; } = null!;
        public int Valor { get; set; }
    }
}