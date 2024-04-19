using Bogus;
using Bogus.Extensions.Brazil;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;

namespace SME.CDEP.TesteIntegracao
{
    public abstract class BaseMock
    {
        public static long GerarIdAleatorio()
        {
            return new Faker().Random.Long(1);
        }
    }
}