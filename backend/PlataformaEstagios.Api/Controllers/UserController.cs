using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Application.UseCases.User.Create;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("new-user")]
        public async Task<IActionResult> CreateUser([FromBody] RequestCreateUserJson request, [FromServices] ICreateUserUseCase useCase)
        {
            await useCase.ExecuteAsync(request);
            return Ok();
        }
    }
}
