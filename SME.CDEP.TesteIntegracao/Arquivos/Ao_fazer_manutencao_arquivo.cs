﻿using Bogus;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Arquivos.Mock;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_arquivo : TesteBase
    {
        public Ao_fazer_manutencao_arquivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Arquivo - Excluir")]
        public async Task Excluir()
        {
            await InserirArquivos();
            var servicoExcluirArquivo = GetServicoExcluirArquivo();

            var arquivos = ObterTodos<Arquivo>();
            await servicoExcluirArquivo.Excluir(arquivos.Take(30).Select(s => s.Id).ToArray());
            
            arquivos = ObterTodos<Arquivo>();
            arquivos.Count().ShouldBe(20);
        }

        [Fact(DisplayName = "Acervo - Mover")]
        public async Task Mover()
        {
            await InserirArquivos();
            
            var servicoMoverArquivoTemporario = GetServicoMoverArquivoTemporario();

            var arquivos = ObterTodos<Arquivo>();
            var arquivoAMover = arquivos.First();
            
            await servicoMoverArquivoTemporario.Mover(TipoArquivo.AcervoFotografico, arquivoAMover);
            
            arquivos = ObterTodos<Arquivo>();
            arquivos
                .Any(a => a.Id == arquivoAMover.Id
                    && a.Tipo == TipoArquivo.AcervoFotografico)
                .ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Upload")]
        public async Task Upload()
        {
            await InserirArquivos();
            
            var servicoUploadArquivo = GetServicoUploadArquivo();

            var nomeArquivo = $"{faker.Lorem.Slug()}.jpeg";
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns(nomeArquivo);
            fileMock.Setup(_ => _.ContentType).Returns("image/jpeg");
                
            await servicoUploadArquivo.Upload(fileMock.Object,TipoArquivo.AcervoFotografico);
            
            var arquivos = ObterTodos<Arquivo>();
            arquivos.Count().ShouldBe(51);
            arquivos.Any(w=> w.Nome.Equals(nomeArquivo)).ShouldBeTrue();
        }
        private async Task InserirArquivos(TipoArquivo tipoArquivo = TipoArquivo.Temp)
        {
            for (int j = 1; j <= 50; j++)
                await InserirNaBase(ArquivoSalvarMock.GerarArquivoValido(tipoArquivo));
        }
    }
}