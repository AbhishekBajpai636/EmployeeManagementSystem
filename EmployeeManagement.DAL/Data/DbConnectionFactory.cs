using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbConnectionFactory> _logger;
        public DbConnectionFactory(IConfiguration configuration,ILogger<DbConnectionFactory> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public IDbConnection CreateConnection()
        {
            try
            {
                _logger.LogInformation("Creating a new database connection.");
                return new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));
            }
            finally
            {
                _logger.LogInformation("Database connection created successfully.");
            }

        }
    }
}
