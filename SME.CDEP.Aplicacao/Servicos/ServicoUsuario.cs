using System.Text.RegularExpressions;
using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoUsuario : IServicoUsuario
    {
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoAcessos servicoAcessos;
        private readonly IMapper mapper;
        private readonly IServicoPerfilUsuario servicoPerfilUsuario;
        private readonly IContextoAplicacao contextoAplicacao;
        
        public ServicoUsuario(IRepositorioUsuario repositorioUsuario,IServicoAcessos servicoAcessos, 
            IMapper mapper,IServicoPerfilUsuario servicoPerfilUsuario,IContextoAplicacao contextoAplicacao) 
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoAcessos = servicoAcessos ?? throw new ArgumentNullException(nameof(servicoAcessos));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.servicoPerfilUsuario = servicoPerfilUsuario ?? throw new ArgumentNullException(nameof(servicoPerfilUsuario));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public async Task<long> Inserir(UsuarioDTO usuarioDto)
        {
            var usuario = mapper.Map<Usuario>(usuarioDto);
            return await repositorioUsuario.Inserir(usuario);
        }

        public async Task<IEnumerable<UsuarioDTO>> ObterTodos()
        {
            return (await repositorioUsuario.ObterTodos()).ToList().Select(s=> mapper.Map<UsuarioDTO>(s));
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

        public async Task<bool> InserirUsuarioExterno(UsuarioExternoDTO usuarioExternoDto)
        {
            usuarioExternoDto.Cpf = usuarioExternoDto.Cpf.Replace(".","").Replace("-","");
            ValidarSenha(usuarioExternoDto.Senha, usuarioExternoDto.ConfirmarSenha);
            
            await ValidarCpfEmUsuarioAcervoECoreSSO(usuarioExternoDto.Cpf);

            var retornoCoreSSO = await servicoAcessos.CadastrarUsuarioCoreSSO(usuarioExternoDto.Cpf, usuarioExternoDto.Nome, usuarioExternoDto.Email, usuarioExternoDto.Senha);

            if (!retornoCoreSSO)
                throw new NegocioException(MensagemNegocio.NAO_FOI_POSSIVEL_CADASTRAR_USUARIO_EXTERNO_NO_CORESSO);
                
            var retorno = await repositorioUsuario.Inserir(new Usuario()
            {
                Login = usuarioExternoDto.Cpf, Nome = usuarioExternoDto.Nome,
                Telefone = usuarioExternoDto.Telefone, Endereco = usuarioExternoDto.Endereco, Numero = usuarioExternoDto.Numero,
                Complemento = usuarioExternoDto.Complemento, Cidade = usuarioExternoDto.Cidade, Estado = usuarioExternoDto.Estado,
                Cep = usuarioExternoDto.Cep, TipoUsuario = usuarioExternoDto.Tipo, Bairro = usuarioExternoDto.Bairro
            });

            return retorno != 0;
        }

        private async Task<bool> ValidarCpfEmUsuarioAcervoECoreSSO(string cpf)
        {
            var usuarioAcervo = await ObterPorLogin(cpf);
            if (usuarioAcervo.NaoEhNulo())
                throw new NegocioException(MensagemNegocio.VOCE_JA_POSSUI_LOGIN_ACERVO);

            var usuarioCoreSSO = await servicoAcessos.UsuarioCadastradoCoreSSO(cpf);
            if (usuarioCoreSSO)
                throw new NegocioException(MensagemNegocio.VOCE_JA_POSSUI_LOGIN_CORESSO);

            return false;
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
                dadosUsuarioCoreSSO.Tipo = (int)dadosusuarioAcervo.TipoUsuario;
            }
            return dadosUsuarioCoreSSO;
        }

        public async Task<bool> AlterarSenha(string login, AlterarSenhaUsuarioDTO alterarSenhaUsuarioDto)
        {
            ValidarSenha(alterarSenhaUsuarioDto.SenhaNova, alterarSenhaUsuarioDto.ConfirmarSenha);
            var retorno = await servicoAcessos.AlterarSenha(login, alterarSenhaUsuarioDto.SenhaAtual, alterarSenhaUsuarioDto.SenhaNova);
            
            if (!retorno)
                throw new NegocioException(MensagemNegocio.LOGIN_OU_SENHA_ATUAL_NAO_COMFEREM);
            
            return retorno;
        }

        public Task<bool> AlterarEmail(string login, string email)
        {
            var retorno = servicoAcessos.AlterarEmail(login, email);
            
            return retorno;
        }

        public async Task<bool> AlterarEndereco(string login, EnderecoUsuarioExternoDTO enderecoUsuarioExternoDto)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(login);
            
            ValidarUsuarioExterno(usuario);
                
            usuario.Endereco = enderecoUsuarioExternoDto.Endereco;
            usuario.Numero = enderecoUsuarioExternoDto.Numero;
            usuario.Complemento = enderecoUsuarioExternoDto.Complemento;
            usuario.Cidade = enderecoUsuarioExternoDto.Cidade;
            usuario.Estado = enderecoUsuarioExternoDto.Estado;
            usuario.Cep = enderecoUsuarioExternoDto.Cep;
            usuario.Bairro = enderecoUsuarioExternoDto.Bairro;
            await repositorioUsuario.Atualizar(usuario);
            
            return true;
        }
        
        public async Task<bool> AlterarTelefone(string login, string telefone)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(login);
            
            ValidarUsuarioExterno(usuario);
            
            usuario.Telefone = telefone;
            await repositorioUsuario.Atualizar(usuario);
            
            return true;
        }

        private void ValidarUsuarioExterno(Usuario usuario)
        {
            if (usuario.EhNulo())
                throw new NegocioException(MensagemNegocio.LOGIN_NAO_ENCONTRADO);

            if (!usuario.EhCadastroExterno())
                throw new NegocioException(MensagemNegocio.SO_EH_PERMITIDO_ALTERAR_ENDERECO_TELEFONE_DE_USUARIOS_EXTERNOS);
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

        public async Task<RetornoPerfilUsuarioDTO> Autenticar(string login, string senha)
        {
            var retornoAutenticacao = await servicoAcessos.Autenticar(login, senha);
            
            if (retornoAutenticacao.Login.NaoEstaPreenchido())
                throw new NegocioException(MensagemNegocio.USUARIO_OU_SENHA_INVALIDOS);

            await ManutencaoUsuario(login, retornoAutenticacao);
            
            return await servicoPerfilUsuario.ObterPerfisUsuario(login);
        }

        private async Task ManutencaoUsuario(string login, UsuarioAutenticacaoRetornoDTO retorno)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(login);
            if (usuario.NaoEhNulo())
            {
                usuario.UltimoLogin = DateTimeExtension.HorarioBrasilia();
                usuario.Nome = retorno.Nome;
                await repositorioUsuario.Atualizar(usuario);
            }
            else
            {
                await repositorioUsuario.Inserir(new Usuario()
                {
                    Login = retorno.Login, Nome = retorno.Nome, UltimoLogin = DateTimeExtension.HorarioBrasilia(),
                });
            }
        }
        
        public async Task<string> SolicitarRecuperacaoSenha(string login)
        {
            var loginRecuperar = login.Replace(" ", "");
            var retorno = await servicoAcessos.SolicitarRecuperacaoSenha(loginRecuperar);
            if (retorno.IndexOf('@') < 0)
                throw new NegocioException(MensagemNegocio.LOGIN_NAO_ENCONTRADO);
            
            var inicioServidor = retorno.LastIndexOf('@');
            var emailTratrado = retorno.Substring(0, 3) + new string('*', inicioServidor - 3) + retorno.Substring(inicioServidor);
                
            return string.Format(MensagemNegocio.AS_ORIENTAÇÕES_PARA_RECUPERAÇÃO_DE_SENHA_FORAM_ENVIADOS_PARA_EMAIL_VERIFIQUE_SUA_CAIXA_DE_ENTRADA, emailTratrado);
        }

        public Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token)
        {
            return servicoAcessos.TokenRecuperacaoSenhaEstaValido(token);
        }

        public async Task<RetornoPerfilUsuarioDTO> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
            var login = await servicoAcessos.AlterarSenhaComTokenRecuperacao(recuperacaoSenhaDto);
            return await servicoPerfilUsuario.ObterPerfisUsuario(login);
        }

        public IEnumerable<Permissao> ObterPermissoes()
        {
            var claims = contextoAplicacao.ObterVariavel(Constantes.CLAIMS).Where(a => a.Item1 == Constantes.CLAIM_PERMISSAO);
            var retorno = new List<Permissao>();

            if (claims.Any())
                retorno.AddRange(claims.Select(claim => (Permissao)Enum.Parse(typeof(Permissao), claim.Item2)));
            
            return retorno;
        }
        
        public async Task<bool> AlterarTipoUsuario(string login, TipoUsuarioExternoDTO tipoUsuario)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(login);
            
            ValidarUsuarioExterno(usuario);
            
            usuario.TipoUsuario = (TipoUsuario)tipoUsuario.Tipo;
            await repositorioUsuario.Atualizar(usuario);
            
            return true;
        }

        public async Task<bool> ValidarCpfExistente(string cpf)
        {
            return await ValidarCpfEmUsuarioAcervoECoreSSO(cpf);
        }
    }
}
