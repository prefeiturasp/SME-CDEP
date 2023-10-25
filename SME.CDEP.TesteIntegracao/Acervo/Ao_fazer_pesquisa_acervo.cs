using System.Security.Cryptography;
using System.Text;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;
using Xunit.Sdk;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_pesquisa_acervo : TesteBase
    {
        public Ao_fazer_pesquisa_acervo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Acervo - Obter paginado")]
        public async Task Obter_paginado()
        {
            
        }
    }
}