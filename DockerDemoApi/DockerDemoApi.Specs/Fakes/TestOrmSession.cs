using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DockerDemoApi.Orm;

namespace DockerDemoApi.Specs.Fakes
{
    public class TestOrmSession : ISession
    {
        IDbConnection connection;
        IDbTransaction transaction;

        public void Begin()
        {
            connection = new SqlConnection(AppSettings.ConnectionString);
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        public void Execute(string query, params object[] parameters)
        {
            connection.Execute(query, parameters, transaction);
        }

        public IEnumerable<TEntity> Query<TEntity>(string query, object parameters = null) where TEntity : class
        {
            return connection.Query<TEntity>(query, parameters, transaction);
        }

        public T ExecuteScalar<T>(string query, object parameters = null)
        {
            return connection.ExecuteScalar<T>(query, parameters, transaction);
        }

        public void Rollback()
        {
            transaction.Rollback();
            connection.Close();
        }
    }
}
