using System.Collections.Generic;
using System.Data;
using Dapper;

namespace DockerDemoApi.Orm
{
    public class OrmSession : ISession
    {
        readonly IDbConnection connection;

        public OrmSession(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(string query, params object[] parameters)
        {
            connection.Execute(query, parameters);
        }

        public IEnumerable<TEntity> Query<TEntity>(string query, object parameters = null) where TEntity : class
        {
            return connection.Query<TEntity>(query, parameters);
        }

        public T ExecuteScalar<T>(string query, object parameters = null)
        {
            return connection.ExecuteScalar<T>(query, parameters);
        }
    }
}
