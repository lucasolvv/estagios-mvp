using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.Enterprise.GetVacancies;
using PlataformaEstagios.Application.UseCases.Vacancy.Create;
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
