using FluentValidation;
using FluentValidation.Results;
using PlataformaEstagios.Application.Helpers;
using PlataformaEstagios.Application.Services.Auth;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Entities;
using PlataformaEstagios.Domain.Repositories.User;
using PlataformaEstagios.Exceptions.ExceptionBase;

namespace PlataformaEstagios.Application.UseCases.Auth.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IValidator<RequestLoginJson> _validator;
        private readonly IUserReadOnlyRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public LoginUseCase(
            IValidator<RequestLoginJson> validator,
            IUserReadOnlyRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _validator = validator;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<ResponseLoginJson> ExecuteAsync(RequestLoginJson request, CancellationToken ct = default)
        {
            await ValidateOrThrowAsync(_validator, request);

            // find user by email OR nickname (repo signature abaixo)
            var user = await _userRepository.GetByEmailOrNicknameAsync(request.EmailOrNickname, ct);
            if (user is null || !user.Active)
                throw new ErrorOnValidationException(new List<string>() { "Invalid credentials." });

            // verify password (use your PasswordHasher Verify)
            var ok = PasswordHasher.Decrypt(request.Password, user.Password);
            if (!ok)
                throw new ErrorOnValidationException(new List<string>() { "Invalid credentials." });

            var token = _jwtTokenGenerator.Generate(user, out var expUtc);

            return new ResponseLoginJson
            {
                AccessToken = token,
                ExpiresAtUtc = expUtc,
                UserIdentifier = user.UserIdentifier,
                Nickname = user.Nickname,
                Email = user.Email,
                UserType = user.UserType,
                UserTypeId = user.UserTypeId
            };
        }


        private static async Task ValidateOrThrowAsync<T>(IValidator<T> validator, T instance)
        {
            ValidationResult result = await validator.ValidateAsync(instance);
            if (!result.IsValid)
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}
