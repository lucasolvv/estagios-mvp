using PlataformaEstagios.Domain.Enums;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.Application;

namespace PlataformaEstagios.Application.UseCases.Application.Update
{
    public class UpdateApplicationUseCase : IUpdateApplicationUseCase
    {
        private readonly IApplicationReadOnlyRepository _applicationRepository;
        private readonly IApplicationWriteOnlyRepository _applicationWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateApplicationUseCase(
            IApplicationReadOnlyRepository applicationRepository,
            IApplicationWriteOnlyRepository applicationWriteOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _applicationRepository = applicationRepository;
            _applicationWriteOnlyRepository = applicationWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool Success, string? Error)> UpdateApplicationStatus(
            Guid applicationId,
            string status,
            CancellationToken ct = default)
        {
            // 1) Validar entrada
            if (string.IsNullOrWhiteSpace(status))
                return (false, "Status não pode ser vazio.");

            if (!Enum.TryParse<ApplicationStatus>(status, ignoreCase: true, out var newStatus))
                return (false, "Status inválido. Use: Pending, Approved ou Rejected.");

            // 2) Carregar candidatura
            var app = await _applicationRepository.GetByIdAsync(applicationId, ct);
            if (app is null)
                return (false, "Candidatura não encontrada.");

            var current = app.Status; // propriedade atual na entidade

            // 3) Regras de transição (MVP)
            // - Pending -> Approved | Rejected
            // - Approved, Rejected -> terminais
            if (current == newStatus)
                return (true, null); // nada a fazer

            if (current is ApplicationStatus.Approved or ApplicationStatus.Rejected)
                return (false, "A candidatura já foi finalizada e não pode mais ter o status alterado.");

            if (current == ApplicationStatus.Pending &&
                newStatus is not (ApplicationStatus.Approved or ApplicationStatus.Rejected))
                return (false, "Transição inválida a partir de 'Pending'. Use 'Approved' ou 'Rejected'.");

            // 4) Persistir
            // Se sua entidade tiver método, prefira: app.SetStatus(newStatus);
            // Aqui delegamos ao repositório de escrita (implemente nele).
            await _applicationWriteOnlyRepository.UpdateStatusAsync(applicationId, newStatus, ct);

            await _unitOfWork.Commit();
            return (true, null);
        }
    }
}
