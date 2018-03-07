using System;

namespace DockerDemoApi.Common.Exceptions
{
    public class RegisterFailedException : Exception
    {
        public RegisterFailedException(string message) : base(message)
        {
        }
    }
}
