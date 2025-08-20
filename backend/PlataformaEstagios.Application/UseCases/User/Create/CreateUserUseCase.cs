using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Exceptions.ExceptionBase;
using PlataformaEstagios.Domain.Repositories.User;
using FluentValidation.Results;
using AutoMapper;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Application.Helpers;


namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CreateUserUseCase(IUserWriteOnlyRepository userWriteOnlyRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(RequestCreateUserJson request)
        {
            await Validate(request);
            var newUser = _mapper.Map<Domain.Entities.User>(request);
            newUser.Password = PasswordHasher.Encrypt(request.Password);
            await _userWriteOnlyRepository.CreateUser(newUser);

            await _unitOfWork.Commit();

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
