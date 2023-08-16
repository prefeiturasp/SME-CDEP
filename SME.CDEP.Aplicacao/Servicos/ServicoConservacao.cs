﻿using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoConservacao : ServicoAplicacao<Conservacao, IdNomeExcluidoDTO>,IServicoConservacao
    {
        public ServicoConservacao(IRepositorioConservacao repositorio, IMapper mapper) : base(repositorio, mapper)
        {}
    }
}