using System.Text.RegularExpressions;
using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoUsuario : IServicoUsuario
    {
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoAcessos servicoAcessos;
        private readonly IMapper mapper;
        
        public ServicoUsuario(IRepositorioUsuario repositorioUsuario,IServicoAcessos servicoAcessos, IMapper mapper) 
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoAcessos = servicoAcessos ?? throw new ArgumentNullException(nameof(servicoAcessos));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(UsuarioDTO usuarioDto)
        {
            var usuario = mapper.Map<Usuario>(usuarioDto);
            return await repositorioUsuario.Inserir(usuario);
        }

        public async Task<IList<UsuarioDTO>> ObterTodos()
        {
            return (await repositorioUsuario.ObterTodos()).ToList().Select(s=> mapper.Map<UsuarioDTO>(s)).ToList();
        }

        public async Task<UsuarioDTO> Alterar(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<Usuario>(usuarioDTO);
            return mapper.Map<UsuarioDTO>(await repositorioUsuario.Atualizar(usuario));
        }

        public async Task<UsuarioDTO> ObterPorId(long usuarioId)
        {
            return mapper.Map<UsuarioDTO>(await repositorioUsuario.ObterPorId(usuarioId));
        }
        
        public async Task<UsuarioDTO> ObterPorLogin(string login)
        {
            return mapper.Map<UsuarioDTO>(await repositorioUsuario.ObterPorLogin(login));
        }

        public async Task<bool> CadastrarUsuarioExterno(UsuarioExternoDTO usuarioExternoDto)
        {
            usuarioExternoDto.Cpf = usuarioExternoDto.Cpf.Replace(".","").Replace("-","");
            ValidarSenha(usuarioExternoDto.Senha, usuarioExternoDto.ConfirmarSenha);
            
            var usuarioAcervo = await ObterPorLogin(usuarioExternoDto.Cpf);
            if (usuarioAcervo != null)
                throw new NegocioException(MensagemNegocio.VOCE_JA_POSSUI_LOGIN_ACERVO);
            
            var usuarioCoreSSO = await servicoAcessos.UsuarioCadastradoCoreSSO(usuarioExternoDto.Cpf);
            if (usuarioCoreSSO)
                throw new NegocioException(MensagemNegocio.VOCE_JA_POSSUI_LOGIN_CORESSO);

            var retornoCoreSSO = await servicoAcessos.CadastrarUsuarioCoreSSO(usuarioExternoDto.Cpf, usuarioExternoDto.Nome, usuarioExternoDto.Email, usuarioExternoDto.Senha);

            if (!retornoCoreSSO)
                throw new NegocioException(MensagemNegocio.NAO_FOI_POSSIVEL_CADASTRAR_USUARIO_EXTERNO_NO_CORESSO);
                
            var retorno = await repositorioUsuario.Inserir(new Usuario()
            {
                Login = usuarioExternoDto.Cpf, Nome = usuarioExternoDto.Nome, UltimoLogin = DateTimeExtension.HorarioBrasilia().Date,
                Telefone = usuarioExternoDto.Telefone, Endereco = usuarioExternoDto.Endereco, Numero = usuarioExternoDto.Numero,
                Complemento = usuarioExternoDto.Complemento, Cidade = usuarioExternoDto.Cidade, Estado = usuarioExternoDto.Estado,
                Cep = usuarioExternoDto.Cep, TipoUsuario = usuarioExternoDto.TipoUsuario, Bairro = usuarioExternoDto.Bairro
            });

            return retorno != 0;
        }
        
        public async Task<DadosUsuarioDTO> ObterMeusDados(string login)
        {
            var dadosUsuarioCoreSSO = await servicoAcessos.ObterMeusDados(login);

            var dadosusuarioAcervo = await repositorioUsuario.ObterPorLogin(login);
            if (dadosusuarioAcervo.EhCadastroExterno())
            {
                dadosUsuarioCoreSSO.Telefone = dadosusuarioAcervo.Telefone;
                dadosUsuarioCoreSSO.Endereco = dadosusuarioAcervo.Endereco;
                dadosUsuarioCoreSSO.Numero = dadosusuarioAcervo.Numero.ToString();
                dadosUsuarioCoreSSO.Complemento = dadosusuarioAcervo.Complemento;
                dadosUsuarioCoreSSO.Bairro = dadosusuarioAcervo.Bairro;
                dadosUsuarioCoreSSO.Cep = dadosusuarioAcervo.Cep;
                dadosUsuarioCoreSSO.Cidade = dadosusuarioAcervo.Cidade;
                dadosUsuarioCoreSSO.Estado = dadosusuarioAcervo.Estado;
            }
            return dadosUsuarioCoreSSO;
        }

        public Task<bool> AlterarSenha(string login, string senhaAtual, string senhaNova, string confirmarSenha)
        {
            ValidarSenha(senhaNova, confirmarSenha);
            var retorno = servicoAcessos.AlterarSenha(login, senhaAtual, senhaNova);
            
            if (!retorno.Result)
                throw new NegocioException(MensagemNegocio.LOGIN_OU_SENHA_ATUAL_NAO_COMFEREM);
            
            return retorno;
        }

        public Task<bool> AlterarEmail(string login, string email)
        {
            var retorno = servicoAcessos.AlterarEmail(login, email);
            
            return retorno;
        }

        public async Task<bool> AlterarEndereco(EnderecoTelefoneUsuarioExternoDTO enderecoTelefoneUsuarioExternoDto)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(enderecoTelefoneUsuarioExternoDto.Login);
            if (usuario == null)
                throw new NegocioException(MensagemNegocio.LOGIN_NAO_ENCONTRADO);

            if (!usuario.EhCadastroExterno())
                throw new NegocioException(MensagemNegocio.SO_EH_PERMITIDO_ALTERAR_ENDERECO_TELEFONE_DE_USUARIOS_EXTERNOS);
                
            usuario.Telefone = enderecoTelefoneUsuarioExternoDto.Telefone;
            usuario.Endereco = enderecoTelefoneUsuarioExternoDto.Endereco;
            usuario.Numero = enderecoTelefoneUsuarioExternoDto.Numero;
            usuario.Complemento = enderecoTelefoneUsuarioExternoDto.Complemento;
            usuario.Cidade = enderecoTelefoneUsuarioExternoDto.Cidade;
            usuario.Estado = enderecoTelefoneUsuarioExternoDto.Estado;
            usuario.Cep = enderecoTelefoneUsuarioExternoDto.Cep;
            await repositorioUsuario.Atualizar(usuario);
            
            return true;
        }
        
        public async Task<bool> AlterarTelefone(EnderecoTelefoneUsuarioExternoDTO enderecoTelefoneUsuarioExternoDto)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(enderecoTelefoneUsuarioExternoDto.Login);
            if (usuario == null)
                throw new NegocioException(MensagemNegocio.LOGIN_NAO_ENCONTRADO);

            if (!usuario.EhCadastroExterno())
                throw new NegocioException(MensagemNegocio.SO_EH_PERMITIDO_ALTERAR_ENDERECO_TELEFONE_DE_USUARIOS_EXTERNOS);
                
            usuario.Telefone = enderecoTelefoneUsuarioExternoDto.Telefone;
            await repositorioUsuario.Atualizar(usuario);
            
            return true;
        }

        private void ValidarSenha(string senhaNova, string confirmarSenha)
        {
            var erros = new List<string>();
            
            if (!senhaNova.Equals(confirmarSenha))
                erros.Add(MensagemNegocio.CONFIRMACAO_SENHA_DEVE_SER_IGUAL_A_SENHA);
            
            if (senhaNova.Length < 8)
                erros.Add(MensagemNegocio.A_SENHA_DEVE_TER_NO_MÍNIMO_8_CARACTERES);

            if (senhaNova.Length > 12)
                erros.Add(MensagemNegocio.A_SENHA_DEVE_TER_NO_MÁXIMO_12_CARACTERES);

            if (senhaNova.Contains(" "))
                erros.Add(MensagemNegocio.A_SENHA_NAO_PODE_CONTER_ESPACOS_EM_BRANCO);

            var regexSenha = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d|\W)[^áàâãéèêíïóôõöúçñÁÀÂÃÉÈÊÍÏÓÔÕÖÚÇÑ]{8,12}$");

            if (!regexSenha.IsMatch(senhaNova))
                erros.Add(MensagemNegocio.A_SENHA_DEVE_CONTER_SOMENTE);

            if (erros.Any())
                throw new NegocioException(erros);
        }

        public async Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
        {
            var retorno = await servicoAcessos.Autenticar(login, senha);

            await ManutencaoUsuario(login, retorno);
            
            return retorno;
        }

        private async Task ManutencaoUsuario(string login, UsuarioAutenticacaoRetornoDTO retorno)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(login);
            if (usuario != null)
            {
                usuario.UltimoLogin = DateTimeExtension.HorarioBrasilia().Date;
                usuario.Nome = retorno.Nome;
                await repositorioUsuario.Atualizar(usuario);
            }
            else
            {
                await repositorioUsuario.Inserir(new Usuario()
                {
                    Login = retorno.Login, Nome = retorno.Nome, UltimoLogin = DateTimeExtension.HorarioBrasilia().Date,
                });
            }
        }
    }
}
