using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemRetornoCadastroDTOTeste
    {
        private readonly Faker faker;

        public AcervoSolicitacaoItemRetornoCadastroDTOTeste()
        {
            faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "DTO - Deve criar instância com todos os parâmetros")]
        public void Deve_criar_instancia_com_todos_parametros()
        {
            var id = faker.Random.Long(1, 1000);
            var tipoAcervo = faker.Lorem.Word();
            var acervoId = faker.Random.Long(1, 1000);
            var titulo = faker.Lorem.Sentence();
            var autoresCreditos = faker.Make(3, () => faker.Name.FullName()).ToArray();
            var situacao = faker.Lorem.Word();
            var situacaoId = faker.Random.Int(1, 10);
            var tipoAtendimento = faker.Lorem.Word();
            var dataVisita = faker.Date.Future();
            var arquivo1 = new ArquivoCodigoNomeDTO { Codigo = Guid.NewGuid(), Nome = faker.System.FileName() };
            var arquivo2 = new ArquivoCodigoNomeDTO { Codigo = Guid.NewGuid(), Nome = faker.System.FileName() };
            var arquivos = new List<ArquivoCodigoNomeDTO> { arquivo1, arquivo2 };
            var alteraDataVisita = faker.Random.Bool();
            var situacaoEmprestimo = SituacaoEmprestimo.EMPRESTADO;
            var situacaoSaldo = SituacaoSaldo.RESERVADO;
            var acervoSolicitacaoId = faker.Random.Long(1, 1000);
            var temControleDisponibilidade = faker.Random.Bool();
            var situacaoDisponibilidade = faker.Lorem.Word();
            var estaDisponivel = faker.Random.Bool();

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Id = id,
                TipoAcervo = tipoAcervo,
                AcervoId = acervoId,
                Titulo = titulo,
                AutoresCreditos = autoresCreditos,
                Situacao = situacao,
                SituacaoId = situacaoId,
                TipoAtendimento = tipoAtendimento,
                DataVisita = dataVisita,
                Arquivos = arquivos,
                AlteraDataVisita = alteraDataVisita,
                SituacaoEmprestimo = situacaoEmprestimo,
                SituacaoSaldo = situacaoSaldo,
                acervoSolicitacaoId = acervoSolicitacaoId,
                TemControleDisponibilidade = temControleDisponibilidade,
                SituacaoDisponibilidade = situacaoDisponibilidade,
                EstaDisponivel = estaDisponivel
            };

            dto.Id.Should().Be(id);
            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.AcervoId.Should().Be(acervoId);
            dto.Titulo.Should().Be(titulo);
            dto.AutoresCreditos.Should().Equal(autoresCreditos);
            dto.Situacao.Should().Be(situacao);
            dto.SituacaoId.Should().Be(situacaoId);
            dto.TipoAtendimento.Should().Be(tipoAtendimento);
            dto.DataVisita.Should().Be(dataVisita);
            dto.Arquivos.Should().HaveCount(2);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
            dto.AlteraDataVisita.Should().Be(alteraDataVisita);
            dto.SituacaoEmprestimo.Should().Be(situacaoEmprestimo);
            dto.SituacaoSaldo.Should().Be(situacaoSaldo);
            dto.acervoSolicitacaoId.Should().Be(acervoSolicitacaoId);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidade);
            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidade);
            dto.EstaDisponivel.Should().Be(estaDisponivel);
        }

        [Fact(DisplayName = "DTO - Deve criar instância com propriedades padrão")]
        public void Deve_criar_instancia_com_propriedades_padrao()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO();

            dto.Id.Should().Be(0);
            dto.TipoAcervo.Should().BeNull();
            dto.AcervoId.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.AutoresCreditos.Should().BeNull();
            dto.Situacao.Should().BeNull();
            dto.SituacaoId.Should().Be(0);
            dto.TipoAtendimento.Should().BeNull();
            dto.DataVisita.Should().BeNull();
            dto.Arquivos.Should().BeNull();
            dto.AlteraDataVisita.Should().BeFalse();
            dto.SituacaoEmprestimo.Should().BeNull();
            dto.SituacaoSaldo.Should().Be(default(SituacaoSaldo));
            dto.acervoSolicitacaoId.Should().Be(0);
            dto.TemControleDisponibilidade.Should().BeFalse();
            dto.SituacaoDisponibilidade.Should().BeNull();
            dto.EstaDisponivel.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de propriedades após criação")]
        public void Deve_permitir_modificacao_propriedades_apos_criacao()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO();
            var novoId = faker.Random.Long(1, 1000);
            var novoTipoAcervo = faker.Lorem.Word();
            var novoAcervoId = faker.Random.Long(1, 1000);
            var novoTitulo = faker.Lorem.Sentence();

            dto.Id = novoId;
            dto.TipoAcervo = novoTipoAcervo;
            dto.AcervoId = novoAcervoId;
            dto.Titulo = novoTitulo;

            dto.Id.Should().Be(novoId);
            dto.TipoAcervo.Should().Be(novoTipoAcervo);
            dto.AcervoId.Should().Be(novoAcervoId);
            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact(DisplayName = "DTO - Deve suportar array vazio de autores e créditos")]
        public void Deve_suportar_array_vazio_autores_creditos()
        {
            var autoresVazio = Array.Empty<string>();

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                AutoresCreditos = autoresVazio
            };

            dto.AutoresCreditos.Should().NotBeNull();
            dto.AutoresCreditos.Should().HaveCount(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar array com múltiplos autores e créditos")]
        public void Deve_suportar_array_multiplos_autores_creditos()
        {
            var autoresCreditos = faker.Make(5, () => faker.Name.FullName()).ToArray();

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                AutoresCreditos = autoresCreditos
            };

            dto.AutoresCreditos.Should().HaveCount(5);
            dto.AutoresCreditos.Should().BeEquivalentTo(autoresCreditos);
        }

        [Fact(DisplayName = "DTO - Deve suportar lista vazia de arquivos")]
        public void Deve_suportar_lista_vazia_arquivos()
        {
            var arquivosVazio = new List<ArquivoCodigoNomeDTO>();

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Arquivos = arquivosVazio
            };

            dto.Arquivos.Should().NotBeNull();
            dto.Arquivos.Should().HaveCount(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar lista com múltiplos arquivos")]
        public void Deve_suportar_lista_multiplos_arquivos()
        {
            var arquivos = faker.Make(5, () => new ArquivoCodigoNomeDTO
            {
                Codigo = Guid.NewGuid(),
                Nome = faker.System.FileName()
            }).ToList();

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Arquivos = arquivos
            };

            dto.Arquivos.Should().HaveCount(5);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
        }

        [Theory(DisplayName = "DTO - Deve suportar todas as situações de empréstimo")]
        [InlineData(SituacaoEmprestimo.EMPRESTADO)]
        [InlineData(SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO)]
        [InlineData(SituacaoEmprestimo.EMPRESTADO_PRORROGACAO)]
        [InlineData(SituacaoEmprestimo.DEVOLVIDO)]
        public void Deve_suportar_todas_situacoes_emprestimo(SituacaoEmprestimo situacao)
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                SituacaoEmprestimo = situacao
            };

            dto.SituacaoEmprestimo.Should().Be(situacao);
        }

        [Theory(DisplayName = "DTO - Deve suportar todas as situações de saldo")]
        [InlineData(SituacaoSaldo.DISPONIVEL)]
        [InlineData(SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO)]
        [InlineData(SituacaoSaldo.RESERVADO)]
        [InlineData(SituacaoSaldo.EMPRESTADO)]
        public void Deve_suportar_todas_situacoes_saldo(SituacaoSaldo situacao)
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                SituacaoSaldo = situacao
            };

            dto.SituacaoSaldo.Should().Be(situacao);
        }

        [Fact(DisplayName = "DTO - Deve permitir situação de empréstimo nula")]
        public void Deve_permitir_situacao_emprestimo_nula()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                SituacaoEmprestimo = null
            };

            dto.SituacaoEmprestimo.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir data visita nula")]
        public void Deve_permitir_data_visita_nula()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                DataVisita = null
            };

            dto.DataVisita.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir data visita com valor")]
        public void Deve_permitir_data_visita_com_valor()
        {
            var dataVisita = faker.Date.Future();

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                DataVisita = dataVisita
            };

            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact(DisplayName = "DTO - Deve permitir controle de disponibilidade ativo")]
        public void Deve_permitir_controle_disponibilidade_ativo()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                TemControleDisponibilidade = true
            };

            dto.TemControleDisponibilidade.Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve permitir controle de disponibilidade inativo")]
        public void Deve_permitir_controle_disponibilidade_inativo()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                TemControleDisponibilidade = false
            };

            dto.TemControleDisponibilidade.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve permitir acervo disponível")]
        public void Deve_permitir_acervo_disponivel()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                EstaDisponivel = true
            };

            dto.EstaDisponivel.Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve permitir acervo indisponível")]
        public void Deve_permitir_acervo_indisponivel()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                EstaDisponivel = false
            };

            dto.EstaDisponivel.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve permitir alterar data visita ativo")]
        public void Deve_permitir_altera_data_visita_ativo()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                AlteraDataVisita = true
            };

            dto.AlteraDataVisita.Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve permitir alterar data visita inativo")]
        public void Deve_permitir_altera_data_visita_inativo()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                AlteraDataVisita = false
            };

            dto.AlteraDataVisita.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve suportar IDs com valores máximos")]
        public void Deve_suportar_ids_valores_maximos()
        {
            var idMaximo = long.MaxValue;
            var acervoIdMaximo = long.MaxValue;
            var acervoSolicitacaoIdMaximo = long.MaxValue;
            var situacaoIdMaximo = int.MaxValue;

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Id = idMaximo,
                AcervoId = acervoIdMaximo,
                acervoSolicitacaoId = acervoSolicitacaoIdMaximo,
                SituacaoId = situacaoIdMaximo
            };

            dto.Id.Should().Be(idMaximo);
            dto.AcervoId.Should().Be(acervoIdMaximo);
            dto.acervoSolicitacaoId.Should().Be(acervoSolicitacaoIdMaximo);
            dto.SituacaoId.Should().Be(situacaoIdMaximo);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings vazias")]
        public void Deve_suportar_strings_vazias()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                TipoAcervo = string.Empty,
                Titulo = string.Empty,
                Situacao = string.Empty,
                TipoAtendimento = string.Empty,
                SituacaoDisponibilidade = string.Empty
            };

            dto.TipoAcervo.Should().Be(string.Empty);
            dto.Titulo.Should().Be(string.Empty);
            dto.Situacao.Should().Be(string.Empty);
            dto.TipoAtendimento.Should().Be(string.Empty);
            dto.SituacaoDisponibilidade.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings longas")]
        public void Deve_suportar_strings_longas()
        {
            var stringLonga = faker.Lorem.Paragraphs(5);

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                TipoAcervo = stringLonga,
                Titulo = stringLonga,
                Situacao = stringLonga,
                TipoAtendimento = stringLonga,
                SituacaoDisponibilidade = stringLonga
            };

            dto.TipoAcervo.Should().Be(stringLonga);
            dto.Titulo.Should().Be(stringLonga);
            dto.Situacao.Should().Be(stringLonga);
            dto.TipoAtendimento.Should().Be(stringLonga);
            dto.SituacaoDisponibilidade.Should().Be(stringLonga);
        }

        [Fact(DisplayName = "DTO - Deve permitir múltiplas instâncias independentes")]
        public void Deve_permitir_multiplas_instancias_independentes()
        {
            var dto1 = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Id = 1,
                Titulo = "Titulo 1",
                SituacaoSaldo = SituacaoSaldo.DISPONIVEL
            };

            var dto2 = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Id = 2,
                Titulo = "Titulo 2",
                SituacaoSaldo = SituacaoSaldo.RESERVADO
            };

            dto1.Id.Should().Be(1);
            dto1.Titulo.Should().Be("Titulo 1");
            dto1.SituacaoSaldo.Should().Be(SituacaoSaldo.DISPONIVEL);

            dto2.Id.Should().Be(2);
            dto2.Titulo.Should().Be("Titulo 2");
            dto2.SituacaoSaldo.Should().Be(SituacaoSaldo.RESERVADO);

            dto1.Id.Should().NotBe(dto2.Id);
        }

        [Fact(DisplayName = "DTO - Deve preservar valores ao atualizar múltiplas propriedades")]
        public void Deve_preservar_valores_ao_atualizar_multiplas_propriedades()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Id = 1,
                Titulo = "Titulo Original",
                SituacaoSaldo = SituacaoSaldo.DISPONIVEL
            };

            var tituloOriginal = dto.Titulo;
            var situacaoOriginal = dto.SituacaoSaldo;
            dto.Titulo = "Titulo Modificado";
            var tituloModificado = dto.Titulo;

            tituloOriginal.Should().Be("Titulo Original");
            situacaoOriginal.Should().Be(SituacaoSaldo.DISPONIVEL);
            tituloModificado.Should().Be("Titulo Modificado");
            dto.SituacaoSaldo.Should().Be(SituacaoSaldo.DISPONIVEL);
        }

        [Fact(DisplayName = "DTO - Deve permitir arquivo com código Guid válido")]
        public void Deve_permitir_arquivo_codigo_guid_valido()
        {
            var codigoGuido = Guid.NewGuid();
            var arquivo = new ArquivoCodigoNomeDTO
            {
                Codigo = codigoGuido,
                Nome = faker.System.FileName()
            };

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Arquivos = [arquivo]
            };

            dto.Arquivos.Should().HaveCount(1);
            dto.Arquivos.First().Codigo.Should().Be(codigoGuido);
        }

        [Fact(DisplayName = "DTO - Deve permitir arquivo com nome válido")]
        public void Deve_permitir_arquivo_nome_valido()
        {
            var nomeArquivo = faker.System.FileName();
            var arquivo = new ArquivoCodigoNomeDTO
            {
                Codigo = Guid.NewGuid(),
                Nome = nomeArquivo
            };

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Arquivos = [arquivo]
            };

            dto.Arquivos.Should().HaveCount(1);
            dto.Arquivos.First().Nome.Should().Be(nomeArquivo);
        }

        [Fact(DisplayName = "DTO - Deve permitir data visita com hora")]
        public void Deve_permitir_data_visita_com_hora()
        {
            var dataComHora = faker.Date.Future();

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                DataVisita = dataComHora
            };

            dto.DataVisita.Should().Be(dataComHora);
            dto.DataVisita.Value.Hour.Should().BeGreaterThanOrEqualTo(0);
            dto.DataVisita.Value.Minute.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact(DisplayName = "DTO - Deve permitir autores com caracteres especiais")]
        public void Deve_permitir_autores_com_caracteres_especiais()
        {
            var autoresEspeciais = new[]
            {
                "José da Silva",
                "François Müller",
                "李明 (Li Ming)",
                "O'Connor"
            };

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                AutoresCreditos = autoresEspeciais
            };

            dto.AutoresCreditos.Should().HaveCount(4);
            dto.AutoresCreditos.Should().BeEquivalentTo(autoresEspeciais);
        }

        [Fact(DisplayName = "DTO - Deve permitir situação com espaços em branco")]
        public void Deve_permitir_situacao_com_espacos_branco()
        {
            var situacaoComEspacos = "  Situação com espaços  ";

            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Situacao = situacaoComEspacos,
                SituacaoDisponibilidade = situacaoComEspacos
            };

            dto.Situacao.Should().Be(situacaoComEspacos);
            dto.SituacaoDisponibilidade.Should().Be(situacaoComEspacos);
        }

        [Fact(DisplayName = "DTO - Cobertura 100% - Todos os getters e setters")]
        public void Cobertura_100_porcento_todos_getters_setters()
        {
            var dto = new AcervoSolicitacaoItemRetornoCadastroDTO();
            var idValor = faker.Random.Long(1, 1000);
            var tipoAcervoValor = faker.Lorem.Word();
            var acervoIdValor = faker.Random.Long(1, 1000);
            var tituloValor = faker.Lorem.Sentence();
            var autoresCreditosValor = faker.Make(2, () => faker.Name.FullName()).ToArray();
            var situacaoValor = faker.Lorem.Word();
            var situacaoIdValor = faker.Random.Int(1, 10);
            var tipoAtendimentoValor = faker.Lorem.Word();
            var dataVisitaValor = faker.Date.Future();
            var arquivosValor = faker.Make(1, () => new ArquivoCodigoNomeDTO 
            { 
                Codigo = Guid.NewGuid(), 
                Nome = faker.System.FileName() 
            }).ToList();
            var alteraDataVisitaValor = true;
            var situacaoEmprestrimoValor = SituacaoEmprestimo.EMPRESTADO;
            var situacaoSaldoValor = SituacaoSaldo.RESERVADO;
            var acervoSolicitacaoIdValor = faker.Random.Long(1, 1000);
            var temControleDisponibilidadeValor = true;
            var situacaoDisponibilidadeValor = faker.Lorem.Word();
            var estaDisponivelValor = true;

            dto.Id = idValor;
            dto.TipoAcervo = tipoAcervoValor;
            dto.AcervoId = acervoIdValor;
            dto.Titulo = tituloValor;
            dto.AutoresCreditos = autoresCreditosValor;
            dto.Situacao = situacaoValor;
            dto.SituacaoId = situacaoIdValor;
            dto.TipoAtendimento = tipoAtendimentoValor;
            dto.DataVisita = dataVisitaValor;
            dto.Arquivos = arquivosValor;
            dto.AlteraDataVisita = alteraDataVisitaValor;
            dto.SituacaoEmprestimo = situacaoEmprestrimoValor;
            dto.SituacaoSaldo = situacaoSaldoValor;
            dto.acervoSolicitacaoId = acervoSolicitacaoIdValor;
            dto.TemControleDisponibilidade = temControleDisponibilidadeValor;
            dto.SituacaoDisponibilidade = situacaoDisponibilidadeValor;
            dto.EstaDisponivel = estaDisponivelValor;

            dto.Id.Should().Be(idValor);
            dto.TipoAcervo.Should().Be(tipoAcervoValor);
            dto.AcervoId.Should().Be(acervoIdValor);
            dto.Titulo.Should().Be(tituloValor);
            dto.AutoresCreditos.Should().Equal(autoresCreditosValor);
            dto.Situacao.Should().Be(situacaoValor);
            dto.SituacaoId.Should().Be(situacaoIdValor);
            dto.TipoAtendimento.Should().Be(tipoAtendimentoValor);
            dto.DataVisita.Should().Be(dataVisitaValor);
            dto.Arquivos.Should().BeEquivalentTo(arquivosValor);
            dto.AlteraDataVisita.Should().Be(alteraDataVisitaValor);
            dto.SituacaoEmprestimo.Should().Be(situacaoEmprestrimoValor);
            dto.SituacaoSaldo.Should().Be(situacaoSaldoValor);
            dto.acervoSolicitacaoId.Should().Be(acervoSolicitacaoIdValor);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidadeValor);
            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidadeValor);
            dto.EstaDisponivel.Should().Be(estaDisponivelValor);
        }
    }
}

