using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoIdioma : ServicoAplicacao<Idioma, BaseComNomeDTO>,IServicoIdioma
    {
        public ServicoIdioma(IRepositorioIdioma repositorio, IMapper mapper) : base(repositorio, mapper)
        {}
    }
}
