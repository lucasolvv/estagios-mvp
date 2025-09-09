using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.Application.Create;
using PlataformaEstagios.Application.UseCases.Vacancy.Get;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

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
        public async Task<ActionResult> ApplyToVacancyAsync([FromBody] RequestCreateApplicationJson request, CancellationToken ct,
            [FromServices] ICreateApplicationUseCase useCase)
        {
            await useCase.CreateNewCandidateApplication(request.VacancyId, request.CandidateIdentifier, ct);
            return Created();
        }


    }
}
