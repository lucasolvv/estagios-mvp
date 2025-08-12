using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public interface ICreateUserUseCase
    {
        Task ExecuteAsync(ResquestCreateUserJson request);
    }
}
