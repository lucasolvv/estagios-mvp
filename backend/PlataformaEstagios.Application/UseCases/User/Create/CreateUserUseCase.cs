using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Domain.Repositories.User;
using System.Net;

namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        public CreateUserUseCase(IUserWriteOnlyRepository userWriteOnlyRepository)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
        }

        public async Task ExecuteAsync(ResquestCreateUserJson request)
        {
            var validator = new CreateUserValidator();

            await validator.Validate(request);


        }
    }
}
