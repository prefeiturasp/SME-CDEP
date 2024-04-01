using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_pesquisar_acervo_por_codigo_tombo : TesteBase
    {
        private readonly IServicoAcervo _servicoAcervo;

        public Ao_pesquisar_acervo_por_codigo_tombo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _servicoAcervo = GetServicoAcervo();
        }
       
        [Fact(DisplayName = "Acervo - Deve pesquisar qualquer tipo de acervo com o perfil Adm Geral")]
        public async Task Deve_pesquisar_qualquer_tipo_deAcervo_com_perfil_admin_geral()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervos();

            var acervos = ObterTodos<Acervo>();

            var filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoDocumental()).Codigo };
            await DeveRetornarCodigoTombo(filtro);

            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoTridimensional()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoBibliografico()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoArteGrafica()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoFotografico()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoAudiovisual()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
        }

        [Fact(DisplayName = "Acervo - Deve pesquisar qualquer tipo de acervo com o perfil Básico")]
        public async Task Deve_retornar_qualquer_tipo_deAcervo_com_perfil_basico()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_BASICO_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervos();

            var acervos = ObterTodos<Acervo>();

            var filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoDocumental()).Codigo };
            await DeveRetornarCodigoTombo(filtro);

            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoTridimensional()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoBibliografico()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoArteGrafica()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoFotografico()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoAudiovisual()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
        }
        
        [Fact(DisplayName = "Acervo - Deve pesquisar todos os tipos de acervo exceto bibliográfico e documental com perfil Admin Memorial")]
        public async Task Deve_retornar_todos_os_tipos_de_acervo_exceto_bibliografico_e_documental_com_perfil_admin_memorial()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervos();

            var acervos = ObterTodos<Acervo>();

            var filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoDocumental()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);

            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoTridimensional()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoBibliografico()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoArteGrafica()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoFotografico()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoAudiovisual()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
        }
        
        [Fact(DisplayName = "Acervo - Deve pesquisar somente o tipo de acervo bibliográfico com perfil Admin Bibliográfico")]
        public async Task Deve_retornar_somente_o_tipo_de_acervo_bibliografico_com_perfil_admin_biblioteca()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_BIBLIOTECA_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervos();

            var acervos = ObterTodos<Acervo>();

            var filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoDocumental()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);

            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoTridimensional()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoBibliografico()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoArteGrafica()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoFotografico()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoAudiovisual()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
        }
        
        [Fact(DisplayName = "Acervo - Deve pesquisar somente o tipo de acervo documental com perfil Admin Memoria")]
        public async Task Deve_retornar_somente_o_tipo_de_acervo_documental_com_perfil_admin_memoria()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIA_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervos();

            var acervos = ObterTodos<Acervo>();

            var filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoDocumental()).Codigo };
            await DeveRetornarCodigoTombo(filtro);

            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoTridimensional()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoBibliografico()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoArteGrafica()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoFotografico()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoAudiovisual()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
        }
        
        [Fact(DisplayName = "Acervo - Não deve pesquisar nenhum tipo de acervo com perfil Externo")]
        public async Task Nao_deve_pesquisar_nenhum_tipo_de_acervo_com_perfil_externo()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_EXTERNO_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervos();

            var acervos = ObterTodos<Acervo>();

            var filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoDocumental()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);

            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoTridimensional()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoBibliografico()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoArteGrafica()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoFotografico()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoAudiovisual()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
        }
        
        [Theory(DisplayName = "Acervo - Não deve retornar acervo disponível com perfil Admin Geral")]
        [InlineData(SituacaoSaldo.RESERVADO)]
        [InlineData(SituacaoSaldo.EMPRESTADO)]
        public async Task Nao_deve_retornar_acervo_disponivel_com_perfil_admin_geral(SituacaoSaldo situacaoSaldo)
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervos(situacaoSaldo);

            var acervos = ObterTodos<Acervo>();

            var filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoDocumental()).Codigo };
            await DeveRetornarCodigoTombo(filtro);

            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoTridimensional()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoBibliografico()).Codigo };
            await NaoDeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoArteGrafica()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoFotografico()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
            
            filtro = new FiltroCodigoTomboDTO() { CodigoTombo = acervos.FirstOrDefault(f => f.TipoAcervoId.EhAcervoAudiovisual()).Codigo };
            await DeveRetornarCodigoTombo(filtro);
        }
        
        private async Task DeveRetornarCodigoTombo(FiltroCodigoTomboDTO filtro)
        {
            var retorno = await _servicoAcervo.PesquisarAcervoPorCodigoTombo(filtro);
            retorno.ShouldNotBeNull();
            retorno.Codigo.ShouldBe(filtro.CodigoTombo);
        }

        private async Task NaoDeveRetornarCodigoTombo(FiltroCodigoTomboDTO filtro)
        {
            var excecao = await Should.ThrowAsync<NegocioException>(() => _servicoAcervo.PesquisarAcervoPorCodigoTombo(filtro));
            excecao.Message.Equals(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
        }
        
        private async Task NaoDeveRetornarAcervoDisponivel(FiltroCodigoTomboDTO filtro)
        {
            var excecao = await Should.ThrowAsync<NegocioException>(() => _servicoAcervo.PesquisarAcervoPorCodigoTombo(filtro));
            excecao.Message.Equals(MensagemNegocio.ACERVO_INDISPONIVEL);
        }
    }
}