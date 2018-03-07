using System;
using DockerDemoApi.Specs.Helpers;
using FluentAssertions;

namespace DockerDemoApi.Specs.Steps
{
    public class SharedSteps
    {
        readonly SharedStepsContext sharedStepsContext;

        public SharedSteps(SharedStepsContext sharedStepsContext)
        {
            this.sharedStepsContext = sharedStepsContext;
        }

        public void AssertExceptionThrown<T>(string errorMessage) where T : Exception
        {
            sharedStepsContext.CaughtException.Should().BeOfType<T>();
            sharedStepsContext.CaughtException.Message.Should().Be(errorMessage);
        }
    }
}
