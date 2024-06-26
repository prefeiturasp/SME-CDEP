﻿using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acesso_documento : TesteBase
    {
        public Ao_fazer_manutencao_acesso_documento(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acesso Documento - Inserir")]
        public async Task Inserir()
        {
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            var acessoDocumento = await servicoAcessoDocumento.Inserir(new IdNomeExcluidoDTO(){Nome = ConstantesTestes.DIGITAL_E_FISICO});
            acessoDocumento.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<AcessoDocumento>();
            obterTodos.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Acesso Documento - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirAcessoDocumentos();
            
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            await servicoAcessoDocumento.Inserir(new IdNomeExcluidoDTO(){Nome = ConstantesTestes.DIGITAL_E_FISICO}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Acesso Documento - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirAcessoDocumentos();
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            var acessoDocumentoDtos = await servicoAcessoDocumento.ObterTodos();
            acessoDocumentoDtos.ShouldNotBeNull();
            acessoDocumentoDtos.Count().ShouldBe(3);
        }

        [Fact(DisplayName = "Acesso Documento - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirAcessoDocumentos();
            
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            var acessoDocumentoDto = await servicoAcessoDocumento.ObterPorId(1);
            acessoDocumentoDto.ShouldNotBeNull();
            acessoDocumentoDto.Id.ShouldBe(1);
            acessoDocumentoDto.Nome.ShouldBe(ConstantesTestes.DIGITAL);
        }

        [Fact(DisplayName = "Acesso Documento - Atualizar")]
        public async Task Atualizar()
        {
            await InserirAcessoDocumentos();
            
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            var acessoDocumentoDto = await servicoAcessoDocumento.ObterPorId(3);
            acessoDocumentoDto.Nome = ConstantesTestes.VDF;
            
            var acessosDocumentosDto = await servicoAcessoDocumento.Alterar(acessoDocumentoDto);
            
            acessosDocumentosDto.ShouldNotBeNull();
            acessosDocumentosDto.Nome = ConstantesTestes.VDF;
        }
        
        [Fact(DisplayName = "Acesso Documento - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirAcessoDocumentos();
            
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            var acessoDocumentoDto = await servicoAcessoDocumento.ObterPorId(3);
            acessoDocumentoDto.Nome = ConstantesTestes.DIGITAL;
            
            await servicoAcessoDocumento.Alterar(acessoDocumentoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acesso Documento - Excluir")]
        public async Task Excluir()
        {
            await InserirAcessoDocumentos();
            
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            await servicoAcessoDocumento.Excluir(3);

            var acessosDocumentos = ObterTodos<AcessoDocumento>();
            acessosDocumentos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            acessosDocumentos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(2);
        }

        private async Task InserirAcessoDocumentos()
        {
            await InserirNaBase(new AcessoDocumento() { Nome = ConstantesTestes.DIGITAL });
            await InserirNaBase(new AcessoDocumento() { Nome = ConstantesTestes.FISICO });
            await InserirNaBase(new AcessoDocumento() { Nome = ConstantesTestes.DIGITAL_E_FISICO });
        }
    }
}