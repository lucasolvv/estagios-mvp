using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.Enterprise.UpdateVacancies;
using PlataformaEstagios.Application.UseCases.Vacancy.Create;
using PlataformaEstagios.Application.UseCases.Vacancy.GetVacancies;
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
        public async Task<ActionResult<IReadOnlyList<ResponseVacancyListItem>>> GetActive(Guid enterpriseId, [FromServices] IGetVacanciesUseCase useCase, 
            CancellationToken ct = default)
        {
            var data = await useCase.ExecuteAsync(enterpriseId, ct);
            return Ok(data);
        }

        [Authorize(Roles = "Enterprise")]
        [HttpGet("{enterpriseId:guid}/vacancies/{vacancyId:guid}")]
        [ProducesResponseType(typeof(ResponseGetVacancyJson), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseGetVacancyJson>> GetByIdAsync(Guid enterpriseId, Guid vacancyId,
            [FromServices] IGetVacanciesUseCase useCase, CancellationToken ct = default)
        {
            var data = await useCase.GetByIdAsync(enterpriseId, vacancyId, ct);
            return Ok(data);
        }

        [Authorize(Roles = "Enterprise")]
        [HttpPut("{enterpriseId:guid}/vacancies/{vacancyId:guid}")]
        public async Task<ActionResult> UpdateAsync(Guid enterpriseId, Guid vacancyId,
            [FromBody] RequestUpdateVacancyJson request, [FromServices] IUpdateVacancyUseCase useCase, CancellationToken ct = default)
        {
            var result = await useCase.ExecuteAsync(enterpriseId, vacancyId, request, ct);
            return Ok("Vaga alterada com sucesso.");
        }

        //[HttpGet("stats")]
        //public async Task<ActionResult<EnterpriseHomeStatsResponse>> GetStats(CancellationToken ct)
        //{
        //    var enterpriseId = _user.EnterpriseIdentifier;
        //    var data = await _queries.GetStatsAsync(enterpriseId, ct);
        //    return Ok(data);
        //}

        //[HttpGet("recent-candidates")]
        //public async Task<ActionResult<IReadOnlyList<RecentCandidateResponse>>> GetRecent(
        //    [FromQuery] int take = 10, CancellationToken ct = default)
        //{
        //    var enterpriseId = _user.EnterpriseIdentifier;
        //    var data = await _queries.GetRecentCandidatesAsync(enterpriseId, take, ct);
        //    return Ok(data);
        //}
    }

}
