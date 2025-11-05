using System.Data;
using Npgsql;

namespace SME.CDEP.Infra.Dados;

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
