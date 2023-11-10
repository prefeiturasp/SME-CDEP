using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoLinhaDTO
{
    public ImportacaoStatus Status { get; set; }
    public string Mensagem { get; set; }
    public int NumeroLinha { get; set; }
    public bool PossuiErros { get; set; }
    
    public void DefinirLinhaComoErro(string mensagem)
    {
        PossuiErros = true;
        Status = ImportacaoStatus.Erros;
        Mensagem = mensagem;
    }
}