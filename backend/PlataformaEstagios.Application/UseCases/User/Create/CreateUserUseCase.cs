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
            await ValidateOrThrowAsync(_requestValidator, request);

            // Mapeia o User (Guid já é conhecido ao adicionar; se preferir, atribua manualmente)
            var newUser = _mapper.Map<Domain.Entities.User>(request);
            newUser.Password = PasswordHasher.Encrypt(request.Password);

            switch (request.UserType)
            {
                case UserType.Candidate:
                    var candidate = _mapper.Map<Candidate>(request.Candidate!);

                    // Garanta que o Id exista agora (normalmente já existe; se não, force):
                    if (candidate.CandidateIdentifier == Guid.Empty)
                        candidate.CandidateIdentifier = Guid.NewGuid();

                    candidate.UserIdentifier = newUser.UserIdentifier; // FK
                    newUser.UserTypeId = candidate.CandidateIdentifier; // <- já define aqui

                    await _userWriteOnlyRepository.CreateUser(newUser);
                    await _candidateWriteOnlyRepository.CreateCandidate(candidate);
                    break;

                case UserType.Enterprise:
                    var enterprise = _mapper.Map<Enterprise>(request.Enterprise!);

                    if (enterprise.EnterpriseIdentifier == Guid.Empty)
                        enterprise.EnterpriseIdentifier = Guid.NewGuid();

                    enterprise.UserIdentifier = newUser.UserIdentifier;
                    newUser.UserTypeId = enterprise.EnterpriseIdentifier;

                    await _userWriteOnlyRepository.CreateUser(newUser);
                    await _enterpriseWriteOnlyRepository.CreateEnterprise(enterprise);
                    break;

                default:
                    throw new ErrorOnValidationException(new List<string> { "Invalid UserType." });
            }

            await _unitOfWork.Commit(); // único commit/SaveChanges (transacional)
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
