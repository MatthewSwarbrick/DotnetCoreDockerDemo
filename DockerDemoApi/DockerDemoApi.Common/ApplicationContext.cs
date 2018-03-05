using SimpleInjector;

namespace DockerDemoApi.Common
{
    public class ApplicationContext
    {
        static Container Container;

        static ApplicationContext()
        {
            Container = new Container();
        }

        public static Container GetContainer()
        {
            return Container;
        }

        public static void SetContainer(Container container)
        {
            Container = container;
        }

        public static T Resolve<T>() where T : class
        {
            return Container.GetInstance<T>();
        }
    }
}
