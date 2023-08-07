using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAssunto : ServicoAplicacao<Assunto, IdNomeExcluidoAuditavelDTO>,IServicoAssunto
    {
        public ServicoAssunto(IRepositorioAssunto repositorio, IMapper mapper,IContextoAplicacao contextoAplicacao) : base(repositorio, mapper,contextoAplicacao)
        {}
    }
}
