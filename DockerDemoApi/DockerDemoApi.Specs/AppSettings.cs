using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace DockerDemoApi.Specs
{
    public class AppSettings
    {
        public static string ConnectionString
        {
            get
            {
                var configurationBuilder = new ConfigurationBuilder();
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");
                configurationBuilder.AddJsonFile(path, false);

                var root = configurationBuilder.Build();
                return root.GetConnectionString("DockerDemoApiSpecs");
            }
        }
    }
}
