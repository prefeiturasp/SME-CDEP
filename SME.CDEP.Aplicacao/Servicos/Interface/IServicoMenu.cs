using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoMenu
    {
        Task<IEnumerable<MenuRetornoDTO>> ObterMenu();
    }
}