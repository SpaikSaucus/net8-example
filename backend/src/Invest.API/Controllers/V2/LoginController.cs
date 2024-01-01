using Asp.Versioning;
using Invest.API.ViewModels;
using Invest.Application.UserCases.Login.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Invest.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LoginController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest req)
        {
            var cmd = new UserLoginCommand()
            {
                UserName = req.UserName,
                Password = req.Password
            };
            var result = await this.mediator.Send(cmd);
            if (string.IsNullOrEmpty(result))
                return Unauthorized();

            return Ok(result);
        }
    }
}
