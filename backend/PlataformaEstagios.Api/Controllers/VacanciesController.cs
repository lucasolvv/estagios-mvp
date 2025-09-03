using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.Vacancy.Create;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Api.Controllers
{
    [ApiController]
    public class VacanciesController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Enterprise")]
        [Route("api/enterprises/{enterpriseId:guid}/vacancies")]
        public async Task<ActionResult<ResponseCreateVacancyJson>> Create(
        Guid enterpriseId,
        [FromBody] RequestCreateVacancyJson body,
        [FromServices] ICreateVacancyUseCase useCase,
        CancellationToken ct)
        {
            body.EnterpriseIdentifier = enterpriseId;
            var result = await useCase.ExecuteAsync(body, ct);
            return Created();
        }

        //// GET: aberto para leitura (ajuste conforme sua política)
        //[HttpGet]
        //[AllowAnonymous] // ou [Authorize(Roles = "Candidate,Enterprise")]
        //[Route("api/enterprises/{enterpriseId:guid}/vacancies")]
        //public async Task<ActionResult<IReadOnlyList<ResponseCreateVacancyJson>>> GetByEnterprise(
        //    Guid enterpriseId,
        //    [FromServices] PlataformaEstagios.Domain.Repositories.Vacancy.IVacancyReadOnlyRepository repo,
        //    [FromServices] AutoMapper.IMapper mapper,
        //    CancellationToken ct)
        //{
        //    var items = await repo.ListByEnterpriseAsync(enterpriseId, ct);
        //    return Ok(mapper.Map<IReadOnlyList<ResponseCreateVacancyJson>>(items));
        //}
    }
}
