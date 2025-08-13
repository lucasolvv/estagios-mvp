using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Exceptions.ExceptionBase;
using PlataformaEstagios.Domain.Repositories.User;
using FluentValidation.Results;


namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        public CreateUserUseCase(IUserWriteOnlyRepository userWriteOnlyRepository)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
        }

        public async Task ExecuteAsync(RequestCreateUserJson request)
        {

            await Validate(request);
        }

        private async Task Validate(RequestCreateUserJson request)
        {
            var validator = new CreateUserValidator();

            ValidationResult result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            } 
        }
    }
}
