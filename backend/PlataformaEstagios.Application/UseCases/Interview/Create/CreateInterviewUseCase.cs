using AutoMapper;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Domain.Enums;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.Application;
using PlataformaEstagios.Domain.Repositories.Interview;
using PlataformaEstagios.Infrastructure.Repositories;

namespace PlataformaEstagios.Application.UseCases.Interview.Create
{
    public class CreateInterviewUseCase : ICreateInterviewUseCase
    {
        private readonly IInterviewWriteOnlyRepository _interviewsWrite;
        private readonly IInterviewReadOnlyRepository _interviewsRead;
        private readonly IApplicationReadOnlyRepository _applications;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateInterviewUseCase(
            IInterviewWriteOnlyRepository interviewsWrite,
            IInterviewReadOnlyRepository interviewsRead,
            IApplicationReadOnlyRepository applications,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _interviewsWrite = interviewsWrite;
            _interviewsRead = interviewsRead;
            _applications = applications;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(bool Success, int Code, string? Error)> ExecuteAsync(
          Guid applicationId,
          RequestCreateScheduleInterviewJson request,
          Guid? enterpriseIdentifier)
        {
            // 1) Validações básicas do request
            if (request is null) return (false, 400, "Requisição inválida.");
            if (request.DurationMinutes <= 0) return (false, 400, "Duração deve ser maior que zero.");
            if (string.IsNullOrWhiteSpace(request.Location) && string.IsNullOrWhiteSpace(request.MeetingLink))
                return (false, 400, "Informe um local presencial ou um link de reunião.");

            var now = DateTimeOffset.UtcNow;
            if (request.StartAt <= now) return (false, 400, "Data/hora deve ser futura.");

            // 2) Carrega a candidatura
            var app = await _applications.GetByIdAsync(applicationId);
            if (app is null) return (false, 404, "Candidatura não encontrada.");

            // 3) Regras de status (no MVP, apenas bloqueia quando Rejected)
            if (app.Status == ApplicationStatus.Rejected)
                return (false, 409, "Não é possível agendar entrevista para candidatura rejeitada.");

            // 4) (Opcional) Autorização do dono da vaga
            if (enterpriseIdentifier.HasValue)
            {
                var belongs = await _applications.BelongsToEnterpriseAsync(applicationId, enterpriseIdentifier.Value);
                if (!belongs) return (false, 403, "Você não tem permissão para esta candidatura.");
            }

            // 5) (Opcional) Conflito de horário por Application (evita duplicado exato)
            var hasOverlap = await _interviewsRead.ExistsSameStartAsync(applicationId, request.StartAt);
            if (hasOverlap) return (false, 409, "Já existe entrevista marcada neste horário para esta candidatura.");

            
            var entity = _mapper.Map<Domain.Entities.Interview>(request);
            entity.InterviewIdentifier = Guid.NewGuid();
            entity.ApplicationIdentifier = applicationId;
            entity.StartAt = request.StartAt.ToUniversalTime(); // garante offset 0
            // 7) Persiste
            await _interviewsWrite.AddAsync(entity);
            await _unitOfWork.Commit();

            return (true, 204, null);
        }


    }
}
