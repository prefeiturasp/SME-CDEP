using System.Threading.Tasks;

namespace SME.CDEP.Aplicacao.Integracoes.Interfaces
{
    public interface IServicoGithub
    {
        Task<string> RecuperarUltimaVersao();

   }
}