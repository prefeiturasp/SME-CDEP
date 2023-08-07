﻿using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoTipoAnexo : ServicoAplicacao<TipoAnexo, IdNomeExcluidoDTO>,IServicoTipoAnexo
    {
        public ServicoTipoAnexo(IRepositorioTipoAnexo repositorio, IMapper mapper) : base(repositorio, mapper)
        {}
    }
}