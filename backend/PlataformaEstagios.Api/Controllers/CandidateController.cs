using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.Vacancy.GetVacancies;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Api.Controllers
{
    [ApiController]
    [Route("api/candidate")]
    public sealed class CandidateController : ControllerBase
    {
        [Authorize(Roles = "Candidate")]
        [HttpGet("open-vacancies")]
        public async Task<ActionResult<IReadOnlyList<ResponseVacancyListItem>>> GetActive([FromServices] IGetVacanciesUseCase useCase,
     CancellationToken ct = default)
        {
            var data = await useCase.ExecuteAsync(ct);
            return Ok(data);
        }
    }
}
