using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("/usuario")]
        public async Task<IActionResult> CreateUser([FromBody] ResquestCreateUserJson request, [FromServices] ICreateUserUseCase useCase)
        {
            await useCase.ExecuteAsync(request);
            return Ok();
        }
    }
}
