using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace DapperCrudUsingStoredProcedure.Context
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
        
    }
}
