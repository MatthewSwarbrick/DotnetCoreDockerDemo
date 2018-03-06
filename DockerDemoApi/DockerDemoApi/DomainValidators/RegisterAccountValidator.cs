using DockerDemoApi.CommandQuery;
using DockerDemoApi.Common.Exceptions;
using DockerDemoApi.Models;

namespace DockerDemoApi.DomainValidators
{
    public class RegisterAccountValidator
    {
        readonly CheckIfUsernameTakenQuery checkIfUsernameTakenQuery;

        public RegisterAccountValidator(CheckIfUsernameTakenQuery checkIfUsernameTakenQuery)
        {
            this.checkIfUsernameTakenQuery = checkIfUsernameTakenQuery;
        }

        public void Validate(RegisterModel model)
        {
            if (checkIfUsernameTakenQuery.Execute(model.Username.Trim().ToLower()))
            {
                throw new RegisterFailedException("Account with this username already exists");
            }
        }
    }
}
