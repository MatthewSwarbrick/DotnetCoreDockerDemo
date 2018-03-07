using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DockerDemoApi.Specs.Helpers
{
    public class Catch
    {
        [DebuggerStepThrough]
        public static void Exception(Action throwingAction)
        {
            Exception<Exception>(throwingAction);
        }

        [DebuggerStepThrough]
        public static Exception ExceptionAsync(Func<Task> throwingAction)
        {
            return ExceptionAsync<Exception>(throwingAction).Result;
        }

        public static Exception Exception<T>(Action throwingAction) where T : Exception
        {
            try
            {
                throwingAction();
            }
            catch (T exception)
            {
                return exception;
            }

            return null;
        }

        public static async Task<Exception> ExceptionAsync<T>(Func<Task> throwingAction) where T : Exception
        {
            try
            {
                await throwingAction();
            }
            catch (T exception)
            {
                return exception;
            }

            return null;
        }
    }
}
