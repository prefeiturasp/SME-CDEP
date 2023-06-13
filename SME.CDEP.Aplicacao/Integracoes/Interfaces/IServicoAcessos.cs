﻿using SME.CDEP.Aplicacao.DTOS;

namespace SSME.CDEP.Aplicacao.Integracoes.Interfaces;

public interface IServicoAcessos
{
    Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha);
    Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login);
}