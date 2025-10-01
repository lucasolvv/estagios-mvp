using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.Application.Create;
using PlataformaEstagios.Application.UseCases.Application.Get;
using PlataformaEstagios.Application.UseCases.Candidate.Get;
using PlataformaEstagios.Application.UseCases.Candidate.UpdateProfile;
using PlataformaEstagios.Application.UseCases.Vacancy.Get;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Repositories.Candidate;

namespace PlataformaEstagios.Api.Controllers
{
    [ApiController]
    [Route("api/candidate")]
    public sealed class CandidateController : ControllerBase
    {
        [Authorize(Roles = "Candidate")]
        [HttpGet("open-vacancies")]
        public async Task<ActionResult<IReadOnlyList<ResponseVacancyListItem>>> GetAllActiveVacancies([FromServices] IGetVacanciesUseCase useCase,
            CancellationToken ct = default)
        {
            var data = await useCase.GetAllActiveVacanciesForCandidateAsync(ct);
            return Ok(data);
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("vacancies/{vacancyId:guid}")]
        [ProducesResponseType(200, Type = typeof(ResponseGetVacancyToApplicationJson))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseGetVacancyToApplicationJson>> GetVacancyById(Guid vacancyId,[FromServices] IGetVacanciesUseCase useCase, 
            CancellationToken ct = default)
        {
            var data = await useCase.GetVacancyByIdForCandidateAsync(vacancyId, ct);
            return Ok(data);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost("application")]
        [ProducesResponseType(201, Type = typeof(CreatedResult))]
        public async Task<ActionResult> ApplyToVacancy([FromBody] RequestCreateApplicationJson request, CancellationToken ct,
            [FromServices] ICreateApplicationUseCase useCase)
        {
            await useCase.CreateNewCandidateApplication(request.VacancyId, request.CandidateIdentifier, ct);
            return Created();
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("applications/{candidateId:guid}")]
        [ProducesResponseType(200, Type = typeof(Ok))]
        public async Task<ActionResult<IReadOnlyList<ResponseGetApplicationJson>>> GetRecentApplications([FromRoute] Guid candidateId,
            [FromServices] IGetApplicationUseCase useCase)
        {
            var data = await useCase.GetRecentApplicationsByCandidateIdAsync(candidateId);
            return Ok(data);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPut("{candidateId:guid}/profile")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateProfile(
            [FromRoute] Guid candidateId,
            [FromBody] RequestUpdateCandidateProfileJson request,
            [FromServices] IUpdateCandidateProfileUseCase useCase,
            CancellationToken ct = default)
        {
            await useCase.ExecuteAsync(candidateId, request, ct);
            return NoContent();
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("{candidateId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseGetCandidateProfileJson>> GetCandidateById(
        [FromRoute] Guid candidateId,
        [FromServices] IGetCandidateUseCase useCase,
        CancellationToken ct = default)
        {
            var candidate = await useCase.GetCandidateByIdAsync(candidateId);
            if (candidate is null) return NotFound();
            return Ok(candidate);
        }
    }
}
