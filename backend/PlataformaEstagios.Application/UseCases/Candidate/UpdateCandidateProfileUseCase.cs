// PlataformaEstagios.Application/UseCases/Candidates/UpdateCandidateProfile/UpdateCandidateCandidateProfileUseCase.cs
using AutoMapper;
using PlataformaEstagios.Application.Contracts;
using PlataformaEstagios.Communication.Requests.Candidates;
using PlataformaEstagios.Communication.Responses.Candidates;
using PlataformaEstagios.Domain.Entities;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Infrastructure.Repositories;

namespace PlataformaEstagios.Application.UseCases.Candidates.UpdateCandidateProfile
{
    public interface IUpdateCandidateProfileUseCase
    {
        Task<ResponseCandidateProfileJson> ExecuteAsync(
            RequestUpdateCandidateProfileJson request,
            Stream? profilePictureStream, string? profilePictureFileName, long? profilePictureLength,
            Stream? resumeStream, string? resumeFileName, long? resumeLength,
            CancellationToken ct);
    }

    public sealed class UpdateCandidateProfileUseCase : IUpdateCandidateProfileUseCase
    {
        private readonly ICandidateRepository _candidates;
        private readonly IUnitOfWork _uow;
        private readonly IFileStorage _storage;
        private readonly IMapper _mapper;

        public UpdateCandidateProfileUseCase(
            ICandidateRepository candidates,
            IUnitOfWork uow,
            IFileStorage storage,
            IMapper mapper)
        {
            _candidates = candidates;
            _uow = uow;
            _storage = storage;
            _mapper = mapper;
        }

        public async Task<ResponseCandidateProfileJson> ExecuteAsync(
            RequestUpdateCandidateProfileJson req,
            Stream? profilePictureStream, string? profilePictureFileName, long? profilePictureLength,
            Stream? resumeStream, string? resumeFileName, long? resumeLength,
            CancellationToken ct)
        {
            // Leitura pelo identificador de negócio (CandidateIdentifier)
            var candidate = await _candidates.GetByCandidateIdentifierAsync(req.CandidateIdentifier, ct)
                ?? throw new Exception("Candidato não encontrado.");

            // Atualizações textuais simples
            if (!string.IsNullOrWhiteSpace(req.Name))
                candidate.Name = req.Name;

            if (req.BirthDate.HasValue)
                candidate.BirthDate = DateOnly.FromDateTime(req.BirthDate.Value.Date);

            if (!string.IsNullOrWhiteSpace(req.CourseName))
                candidate.CourseName = req.CourseName;

            if (req.Address is not null)
            {
                candidate.Address ??= new Address();
                if (!string.IsNullOrWhiteSpace(req.Address.Street)) candidate.Address.Street = req.Address.Street;
                if (!string.IsNullOrWhiteSpace(req.Address.Complement)) candidate.Address.Complement = req.Address.Complement;
                if (!string.IsNullOrWhiteSpace(req.Address.Neighborhood)) candidate.Address.Neighborhood = req.Address.Neighborhood;
                if (!string.IsNullOrWhiteSpace(req.Address.City)) candidate.Address.City = req.Address.City;
                if (!string.IsNullOrWhiteSpace(req.Address.UF)) candidate.Address.UF = req.Address.UF;
                if (!string.IsNullOrWhiteSpace(req.Address.Cep)) candidate.Address.CEP = req.Address.Cep;
            }

            // Foto (pública)
            if (profilePictureStream is not null && (profilePictureLength ?? 0) > 0 && !string.IsNullOrWhiteSpace(profilePictureFileName))
            {
                var picPath = await _storage.SavePublicProfilePictureAsync(
                    candidate.CandidateIdentifier, profilePictureStream, profilePictureFileName!, ct);
                candidate.SetProfilePicture(picPath);
            }

            // Currículo (privado)
            if (resumeStream is not null && (resumeLength ?? 0) > 0 && !string.IsNullOrWhiteSpace(resumeFileName))
            {
                var resumePath = await _storage.SavePrivateResumeAsync(
                    candidate.CandidateIdentifier, resumeStream, resumeFileName!, ct);
                candidate.SetResume(resumePath);
            }

            await _uow.CommitAsync(ct);

            var resp = _mapper.Map<ResponseCandidateProfileJson>(candidate);
            resp.HasResume = !string.IsNullOrWhiteSpace(candidate.ResumePath);
            return resp;
        }
    }
}
