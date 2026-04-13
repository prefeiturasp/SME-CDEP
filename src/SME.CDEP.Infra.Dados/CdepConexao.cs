using System.Data;
using System.Diagnostics.CodeAnalysis;
using Npgsql;

namespace SME.CDEP.Infra.Dados;

[ExcludeFromCodeCoverage]
public class CdepConexao : ICdepConexao
{
        private readonly IDbConnection _conexao; 
        
        public CdepConexao(string stringConexao)
        {
            _conexao = new NpgsqlConnection(stringConexao);
            Abrir();
        }

        public CdepConexao(IDbConnection conexao)
        {
            _conexao = conexao;
        }
        
        public void Dispose()
        {
            if (_conexao.State == ConnectionState.Open)
                _conexao.Close();
            
            GC.SuppressFinalize(this);
        }
    
        public void Abrir()
        {
            if (_conexao.State != ConnectionState.Open)
                _conexao.Open();
        }

        public void Fechar()
        {
            if (_conexao.State != ConnectionState.Closed)
            {
                _conexao.Close();
            }
        }

        public IDbConnection Obter()
        {
            return _conexao;
        }
}
