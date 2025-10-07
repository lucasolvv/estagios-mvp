using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.Application.Get;
using PlataformaEstagios.Application.UseCases.Application.Update;
using PlataformaEstagios.Application.UseCases.Candidate.Get;
using PlataformaEstagios.Application.UseCases.Interview.Create;
using PlataformaEstagios.Application.UseCases.Interview.Get;
using PlataformaEstagios.Application.UseCases.Vacancy.Create;
using PlataformaEstagios.Application.UseCases.Vacancy.Get;
using PlataformaEstagios.Application.UseCases.Vacancy.Update;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Api.Controllers
{
    [ApiController]
    [Route("api/enterprises")]
    public sealed class EnterpriseHomeController : ControllerBase
    {
        [Authorize(Roles = "Enterprise")]
        [HttpGet("{enterpriseId:guid}/vacancies")]
        public async Task<ActionResult<IReadOnlyList<ResponseVacancyListItem>>> GetActiveVacancies(Guid enterpriseId, [FromServices] IGetVacanciesUseCase useCase, 
            CancellationToken ct = default)
        {
            var data = await useCase.GetAllActiveVacanciesForEnterpriseAsync(enterpriseId, ct);
            return Ok(data);
        }

        [Authorize(Roles = "Enterprise")]
        [HttpGet("{enterpriseId:guid}/vacancies/{vacancyId:guid}")]
        [ProducesResponseType(typeof(ResponseGetVacancyJson), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseGetVacancyJson>> GetVacancyById(Guid enterpriseId, Guid vacancyId,
            [FromServices] IGetVacanciesUseCase useCase, CancellationToken ct = default)
        {
            var data = await useCase.GetVacancyByIdForEnterpriseAsync(enterpriseId, vacancyId, ct);
            return Ok(data);
        }
        
        [HttpPost]
        [Authorize(Roles = "Enterprise")]
        [Route("{enterpriseId:guid}/vacancies")]
        public async Task<ActionResult<ResponseCreateVacancyJson>> Create(
        Guid enterpriseId, [FromBody] RequestCreateVacancyJson body, [FromServices] ICreateVacancyUseCase useCase, CancellationToken ct)
        {
            body.EnterpriseIdentifier = enterpriseId;
            var result = await useCase.ExecuteAsync(body, ct);
            return Created();
        }

        [Authorize(Roles = "Enterprise")]
        [HttpPut("{enterpriseId:guid}/vacancies/{vacancyId:guid}")]
        public async Task<ActionResult> UpdateVacancyAsync(Guid enterpriseId, Guid vacancyId,
            [FromBody] RequestUpdateVacancyJson request, [FromServices] IUpdateVacancyUseCase useCase, CancellationToken ct = default)
        {
            var result = await useCase.UpdateVacancyAsync(enterpriseId, vacancyId, request, ct);
            return Ok("Vaga alterada com sucesso.");
        }

        [Authorize(Roles = "Enterprise")]
        [HttpPut("applications/{applicationId:guid}/status")]
        public async Task<IActionResult> UpdateApplicationStatusAsync(
            Guid applicationId,
            [FromBody] RequestUpdateApplicationStatusJson request,
            [FromServices]IUpdateApplicationUseCase useCase,
            CancellationToken ct = default)
        {
            var (ok, err) = await useCase.UpdateApplicationStatus(applicationId, request.Status, ct);
            return ok ? NoContent() : BadRequest(err);
        }

        [Authorize(Roles = "Enterprise")]
        [HttpGet("{enterpriseId:guid}/applications/all")]
        [ProducesResponseType(typeof(ResponseGetVacancyJson), 200)]
        public async Task<ActionResult<IReadOnlyList<ResponseGetApplicationJson>>> GetAllApplicationsByEnterpriseIdAsync(Guid enterpriseId,
            [FromServices] IGetApplicationUseCase useCase, CancellationToken ct = default)
        {
            var data = await useCase.GetRecentApplicationsByEnterpriseIdAsync(enterpriseId);
            return Ok(data);
        }

        [Authorize(Roles = "Enterprise")]
        [HttpGet("candidatura/{applicationId:guid}")]
        [ProducesResponseType(typeof(ResponseGetVacancyJson), 200)]

        public async Task<ActionResult<ResponseGetApplicationJson>> GetApplicationByApplicationId(Guid applicationId, 
            [FromServices] IGetApplicationUseCase useCase)
        {
            var data = await useCase.GetApplicationByApplicationIdAsync(applicationId);
            return Ok(data);
        }

        [Authorize(Roles = "Enterprise")]
        [HttpGet("candidato/{candidateId:guid}")]
        [ProducesResponseType(typeof(ResponseGetVacancyJson), 200)]
        public async Task<ActionResult<ResponseGetCandidateProfileJson>> GetCandidateProfileInfoByCandidateId(Guid candidateId,
            [FromServices] IGetCandidateUseCase useCase)
        {
            var data = await useCase.GetCandidateByIdAsync(candidateId);
            return Ok(data);
        }

        [Authorize(Roles = "Enterprise")]
        [HttpPost("applications/{applicationId:guid}/interviews")] // <-- sem espaço!
        public async Task<IActionResult> CreateInterviewAsync(
            [FromRoute] Guid applicationId,
            [FromBody] RequestCreateScheduleInterviewJson request,
            [FromServices] ICreateInterviewUseCase createInterview,
            CancellationToken ct = default)
        {
            // se ainda não extrai o EnterpriseIdentifier das claims, passe null
            var (ok, code, error) = await createInterview.ExecuteAsync(applicationId, request, /* enterpriseIdentifier: */ null);

            if (ok) return NoContent();           // 204
            if (code == 404) return NotFound(error);       // 404
            if (code == 403) return Forbid();              // 403
            if (code == 409) return Conflict(error);       // 409
            return BadRequest(error);                          // 400
        }

        [Authorize(Roles = "Enterprise")]
        [HttpGet("applications/{applicationId:guid}/interviews")]
        public async Task<ActionResult<IEnumerable<ResponseGetInterviewItemJson>>> GetInterviewsByApplicationAsync(
        [FromRoute] Guid applicationId,
        [FromServices] IGetInterviewUseCase useCase,
        CancellationToken ct = default)
        {
            var list = await useCase.GetInterviewsByApplicationIdAsync(applicationId);
            return Ok(list); // IEnumerable<ResponseGetInterviewItemJson>
        }

        [Authorize(Roles = "Enterprise")]
        [HttpGet("{enterpriseId:guid}/interviews")]
        public async Task<ActionResult<IEnumerable<ResponseGetInterviewItemJson>>> GetInterviewsByEnterpriseIdAsync(
        [FromRoute] Guid enterpriseId,
        [FromServices] IGetInterviewUseCase useCase,
        CancellationToken ct = default)
        {
            var list = await useCase.GetInterviewsByEnterpriseIdAsync(enterpriseId);
            return Ok(list); // IEnumerable<ResponseGetInterviewItemJson>
        }


    }

}
