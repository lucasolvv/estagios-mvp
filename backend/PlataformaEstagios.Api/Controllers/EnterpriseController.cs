using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.Application.Get;
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
        [HttpGet("{enterpriseId:guid}/applications/all")]
        [ProducesResponseType(typeof(ResponseGetVacancyJson), 200)]
        public async Task<ActionResult<IReadOnlyList<ResponseGetApplicationJson>>> GetAllApplicationsByEnterpriseIdAsync(Guid enterpriseId,
            [FromServices] IGetApplicationUseCase useCase, CancellationToken ct = default)
        {
            var data = await useCase.GetRecentApplicationsByEnterpriseIdAsync(enterpriseId);
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
