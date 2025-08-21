using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using PlataformaEstagios.Application.Helpers;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Domain.Entities;
using PlataformaEstagios.Domain.Enums;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.Candidate;
using PlataformaEstagios.Domain.Repositories.Enterprise;
using PlataformaEstagios.Domain.Repositories.User;
using PlataformaEstagios.Exceptions.ExceptionBase;

namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly ICandidateWriteOnlyRepository _candidateWriteOnlyRepository;
        private readonly IEnterpriseWriteOnlyRepository _enterpriseWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<RequestCreateUserJson> _requestValidator;

        public CreateUserUseCase(
            IUserWriteOnlyRepository userWriteOnlyRepository,
            ICandidateWriteOnlyRepository candidateWriteOnlyRepository,
            IEnterpriseWriteOnlyRepository enterpriseWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<RequestCreateUserJson> requestValidator)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _candidateWriteOnlyRepository = candidateWriteOnlyRepository;
            _enterpriseWriteOnlyRepository = enterpriseWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestValidator = requestValidator;
        }

        public async Task ExecuteAsync(RequestCreateUserJson request)
        {
            // 1) Valida somente o request (inclui User + Candidate/Enterprise via FluentValidation)
            await ValidateOrThrowAsync(_requestValidator, request);

            // 2) Cria usuário base
            var newUser = _mapper.Map<Domain.Entities.User>(request);
            newUser.Password = PasswordHasher.Encrypt(request.Password);

            await _userWriteOnlyRepository.CreateUser(newUser);

            // 3) Cria perfil relacionado (Candidate ou Enterprise)
            switch (request.UserType)
            {
                case UserType.Candidate:
                    var candidate = _mapper.Map<Candidate>(request.Candidate!);
                    candidate.UserIdentifier = newUser.UserIdentifier;
                    await _candidateWriteOnlyRepository.CreateCandidate(candidate);
                    break;

                case UserType.Enterprise:
                    var enterprise = _mapper.Map<Enterprise>(request.Enterprise!);
                    enterprise.UserIdentifier = newUser.UserIdentifier;
                    await _enterpriseWriteOnlyRepository.CreateEnterprise(enterprise);
                    break;

                default:
                    throw new ErrorOnValidationException(new List<string> { "Invalid UserType." });
            }

            // 4) Commit único e atômico
            await _unitOfWork.Commit();
        }

        private static async Task ValidateOrThrowAsync<T>(IValidator<T> validator, T instance)
        {
            ValidationResult result = await validator.ValidateAsync(instance);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
