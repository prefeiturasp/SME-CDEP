using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_finalizar : TesteBase
    {
        public Ao_finalizar(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Solicitação - Não pode finalizar: aguardando visita futura e bibliográfico")]
        public async Task Nao_pode_finalizar_atendimento_aguardando_visita_futura_e_bibliografico()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(1),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                }
            };
            
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não pode finalizar: aguardando visita não futura e bibliográfico")]
        public async Task Nao_pode_finalizar_atendimento_aguardando_visita_nao_futura_e_bibliografico()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                }
            };
            
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não pode finalizar: aguardando atendimento e bibliográfico")]
        public async Task Nao_pode_finalizar_aguardando_atendimento_e_bibliografico()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                }
            };
            
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não pode finalizar: itens finalizados manualmente e bibliográfico")]
        public async Task Nao_pode_finalizar_itens_finalizados_manualmente_e_bibliografico()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                }
            };
            
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não pode finalizar: aguardando visita futura e tridimensional")]
        public async Task Nao_pode_finalizar_atendimento_aguardando_visita_futura_e_tridimensional()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Tridimensional,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(1),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Tridimensional,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                }
            };
	
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pode finalizar: aguardando visita não futura e tridimensional")]
        public async Task Pode_finalizar_atendimento_aguardando_visita_nao_futura_e_tridimensional()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Tridimensional,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Tridimensional,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                }
            };
	
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pode finalizar: aguardando visita não futura com acervo tridimensional e arte gráfica")]
        public async Task Pode_finalizar_atendimento_aguardando_visita_nao_futura_com_acervo_tridimensional_e_arte_grafica()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Tridimensional,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.ArtesGraficas,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                }
            };
	
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pode finalizar: aguardando visita não futura com acervo tridimensional, arte gráfica e bibliográfico")]
        public async Task Pode_finalizar_atendimento_aguardando_visita_nao_futura_com_acervo_tridimensional_arte_grafica_e_bibliografico()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Tridimensional,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.ArtesGraficas,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                }
            };
	
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não pode finalizar: tem itens aguardando atendimento")]
        public async Task Nao_pode_finalizar_atendimento_com_itens_aguardando_atendimento()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Tridimensional,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.ArtesGraficas,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO
                    },
                }
            };
	
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não pode finalizar: tem itens bibliográficos aguardando visita não futura")]
        public async Task Nao_pode_finalizar_atendimento_com_itens_bibliograficos_aguardando_visita_nao_futura()
        {
            var perfilAdmGeral = new Guid(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            var acervoSolicitacaoDetalheDTO = new AcervoSolicitacaoDetalheDTO()
            {
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
                {
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Tridimensional,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.ArtesGraficas,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    },
                    new ()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    },
                }
            };
	
            GetServicoAcervoSolicitacao().PodeFinalizar(perfilAdmGeral,acervoSolicitacaoDetalheDTO).ShouldBeFalse();
        }
    }
}