﻿using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoTridimensional
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo);
        Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo);
        Task<long> AtualizarImportacao(long id, string conteudo, ImportacaoStatus? status = null);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id);
    }
}
