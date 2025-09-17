// PlataformaEstagios.Application/UseCases/Candidate/UpdateProfile/UpdateCandidateProfileUseCase.cs
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.Candidate;
using PlataformaEstagios.Exceptions.ExceptionBase;
using System.Text;

namespace PlataformaEstagios.Application.UseCases.Candidate.UpdateProfile
{
    public class UpdateCandidateProfileUseCase : IUpdateCandidateProfileUseCase
    {
        private readonly ICandidateReadOnlyRepository _candidateReadOnlyRepository;
        private readonly IFileStorage _fileStorage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<RequestUpdateCandidateProfileJson> _validator;

        public UpdateCandidateProfileUseCase(
            ICandidateReadOnlyRepository candidateReadOnlyRepository,
            IFileStorage fileStorage,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<RequestUpdateCandidateProfileJson> validator)
        {
            _candidateReadOnlyRepository = candidateReadOnlyRepository;
            _fileStorage = fileStorage;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task ExecuteAsync(Guid candidateId, RequestUpdateCandidateProfileJson request, CancellationToken ct = default)
        {
            await ValidateOrThrowAsync(_validator, request);

            var candidate = await _candidateReadOnlyRepository.GetCandidateByIdAsync(candidateId);
            if (candidate is null)
                throw new ErrorOnValidationException(new List<string> { "Candidato não encontrado." });

            // update parcial
            _mapper.Map(request, candidate);

            // base do nome: slug do candidato
            var slug = Slugify(candidate.Name) ?? candidate.CandidateIdentifier.ToString("N");

            // Foto (pública)
            if (!string.IsNullOrWhiteSpace(request.ProfilePictureBase64))
            {
                var publicPath = await _fileStorage.SavePublicProfilePictureBase64Async(
                    candidate.CandidateIdentifier,
                    request.ProfilePictureBase64!,
                    slug,   // nome base
                    ct);

                candidate.SetProfilePicture(publicPath);
            }

            // Currículo (privado)
            if (!string.IsNullOrWhiteSpace(request.ResumeBase64))
            {
                var privatePath = await _fileStorage.SavePrivateResumeBase64Async(
                    candidate.CandidateIdentifier,
                    request.ResumeBase64!,
                    slug,   // nome base
                    ct);

                candidate.SetResume(privatePath); // ou SetResumePath
            }

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

        private static string Slugify(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(capacity: normalized.Length);

            foreach (var ch in normalized)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            var ascii = sb.ToString().Normalize(NormalizationForm.FormC)
                            .ToLowerInvariant()
                            .Replace(' ', '-');

            // remove tudo que não for [a-z0-9-_]
            var clean = new string(ascii.Where(c =>
                (c >= 'a' && c <= 'z') ||
                (c >= '0' && c <= '9') ||
                c == '-' || c == '_').ToArray());

            // fallback
            return string.IsNullOrWhiteSpace(clean) ? "candidate" : clean;
        }
    }
}
