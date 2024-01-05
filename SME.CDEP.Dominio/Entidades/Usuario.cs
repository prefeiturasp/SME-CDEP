using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Usuario : EntidadeBaseAuditavel
    {
        public DateTime? ExpiracaoRecuperacaoSenha { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public Guid? TokenRecuperacaoSenha { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public string Bairro { get; set; }

        public bool EhCadastroExterno()
        {
            return TipoUsuario is TipoUsuario.SERVIDOR_PUBLICO or TipoUsuario.ESTUDANTE or TipoUsuario.PROFESSOR or TipoUsuario.POPULACAO_GERAL;
        }

        public void AtualizarUltimoLogin()
        {
            UltimoLogin = DateTimeExtension.HorarioBrasilia();
        }
    }
}
