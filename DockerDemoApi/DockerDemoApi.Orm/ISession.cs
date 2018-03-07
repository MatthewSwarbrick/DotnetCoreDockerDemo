using System.Collections.Generic;

namespace DockerDemoApi.Orm
{
    public interface ISession
    {
        IEnumerable<TEntity> Query<TEntity>(string query, object parameters = null) where TEntity : class;
        void Execute(string query, params object[] parameters);
        T ExecuteScalar<T>(string query, object parameters = null);
    }
}
