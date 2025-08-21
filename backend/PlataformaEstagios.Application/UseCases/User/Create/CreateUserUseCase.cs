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

        // ✅ Inject validators
        private readonly IValidator<RequestCreateUserJson> _requestValidator;
        private readonly IValidator<Domain.Entities.User> _userValidator;
        private readonly IValidator<Domain.Entities.Candidate> _candidateValidator;
        private readonly IValidator<Domain.Entities.Enterprise> _enterpriseValidator;

        public CreateUserUseCase(
            IUserWriteOnlyRepository userWriteOnlyRepository,
            ICandidateWriteOnlyRepository candidateWriteOnlyRepository,
            IEnterpriseWriteOnlyRepository enterpriseWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<RequestCreateUserJson> requestValidator,
            IValidator<Domain.Entities.User> userValidator,
            IValidator<Domain.Entities.Candidate> candidateValidator,
            IValidator<Domain.Entities.Enterprise> enterpriseValidator)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _candidateWriteOnlyRepository = candidateWriteOnlyRepository;
            _enterpriseWriteOnlyRepository = enterpriseWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            _requestValidator = requestValidator;
            _userValidator = userValidator;
            _candidateValidator = candidateValidator;
            _enterpriseValidator = enterpriseValidator;
        }

        public async Task ExecuteAsync(RequestCreateUserJson request)
        {
            // 1) Request-level validation (encadeia Candidate/Enterprise/Address conforme UserType)
            await ValidateOrThrowAsync(_requestValidator, request);

            var newUser = _mapper.Map<Domain.Entities.User>(request);
            newUser.Password = PasswordHasher.Encrypt(request.Password);

            // 3) ✅ Domain-level validation (User)
            await ValidateOrThrowAsync(_userValidator, newUser);

            // 4) Stage User
            await _userWriteOnlyRepository.CreateUser(newUser);

            // 5) Stage Profile (Candidate OR Enterprise) usando o UserIdentifier já gerado
            switch (request.UserType)
            {
                case UserType.Candidate:
                    {
                        var candidate = _mapper.Map<Candidate>(request.Candidate!);
                        candidate.UserIdentifier = newUser.UserIdentifier;

                        // ✅ Domain-level validation (Candidate)
                        await ValidateOrThrowAsync(_candidateValidator, candidate);

                        await _candidateWriteOnlyRepository.CreateCandidate(candidate);
                        break;
                    }

                case UserType.Enterprise:
                    {
                        var enterprise = _mapper.Map<Enterprise>(request.Enterprise!);
                        enterprise.UserIdentifier = newUser.UserIdentifier;

                        // ✅ Domain-level validation (Enterprise)
                        await ValidateOrThrowAsync(_enterpriseValidator, enterprise);

                        await _enterpriseWriteOnlyRepository.CreateEnterprise(enterprise);
                        break;
                    }

                default:
                    throw new ErrorOnValidationException(new List<string>{ "Invalid UserType." });
            }

            // 6) ✅ Single atomic commit
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
