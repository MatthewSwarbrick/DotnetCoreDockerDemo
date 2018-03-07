using System.Linq;
using DockerDemoApi.Domain;
using DockerDemoApi.Orm;

namespace DockerDemoApi.CommandQuery
{
    public class CheckIfUsernameTakenQuery
    {
        readonly ISession session;

        public CheckIfUsernameTakenQuery(ISession session)
        {
            this.session = session;
        }

        public bool Execute(string username)
        {
            return session.Query<User>(@"select * from AspNetUsers where UserName = @username", new { username }).Any();
        }
    }
}
