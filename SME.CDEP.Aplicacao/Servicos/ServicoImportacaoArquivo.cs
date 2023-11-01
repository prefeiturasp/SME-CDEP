using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivo : IServicoImportacaoArquivo
    {
        private readonly IRepositorioImportacaoArquivo repositorioImportacaoArquivo;
        private readonly IMapper mapper;
        
        public ServicoImportacaoArquivo(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IMapper mapper)
        {
            this.repositorioImportacaoArquivo = repositorioImportacaoArquivo ?? throw new ArgumentNullException(nameof(repositorioImportacaoArquivo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<long> Inserir(ImportacaoArquivo importacaoArquivo)
        {
            return await repositorioImportacaoArquivo.Inserir(importacaoArquivo);
        }

        public async Task<ImportacaoArquivoDTO> Alterar(ImportacaoArquivo importacaoArquivo)
        {
            return mapper.Map<ImportacaoArquivoDTO>(await repositorioImportacaoArquivo.Atualizar(importacaoArquivo));
        }

        public async Task<bool> Excluir(long importacaoArquivoId)
        {
            await repositorioImportacaoArquivo.Remover(importacaoArquivoId);
            return true;
        }

        public async Task<ImportacaoArquivoCompleto> ObterUltimaImportacao()
        {
            return await repositorioImportacaoArquivo.ObterUltimaImportacao();
        }
    }
}  