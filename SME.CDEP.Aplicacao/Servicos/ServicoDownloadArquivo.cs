﻿using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoDownloadArquivo : IServicoDownloadArquivo
    {
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ServicoDownloadArquivo(IRepositorioArquivo repositorioArquivo,IServicoArmazenamento servicoArmazenamento)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }
       
        public async Task<(byte[], string, string)> Download(Guid codigoArquivo)
        {
            return await ObterArquivo(await repositorioArquivo.ObterPorCodigo(codigoArquivo));
        }

        public async Task<(byte[], string, string)> DownloadPorTipoAcervo(TipoAcervo tipoAcervo)
        {
            return await ObterArquivo(await repositorioArquivo.ObterArquivoPorNomeTipoArquivo(tipoAcervo.ObterPlanilhaModelo(), TipoArquivo.Sistema));
        }

        private async Task<(byte[], string, string)> ObterArquivo(Arquivo arquivo)
        {
            var extensao = Path.GetExtension(arquivo.Nome);

            var nomeArquivoComExtensao = $"{arquivo.Codigo}{extensao}";

            var enderecoArquivo = await servicoArmazenamento.Obter(nomeArquivoComExtensao, arquivo.Tipo == TipoArquivo.Temp);

            var arquivoFisico = Array.Empty<byte>();

            if (enderecoArquivo.EstaPreenchido())
            {
                var response = await new HttpClient().GetAsync(enderecoArquivo);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    arquivoFisico = await response.Content.ReadAsByteArrayAsync();
            }
            else
                throw new NegocioException(MensagemNegocio.ARQUIVO_NAO_ENCONTRADO);

            return (arquivoFisico, arquivo.TipoConteudo, arquivo.Nome);
        }
    }
}  