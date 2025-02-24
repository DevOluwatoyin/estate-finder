using Microsoft.AspNetCore.Mvc;

namespace EstateFInder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            var result = await _accountService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticationRequest request)
        {
            var result = await _accountService.AuthenticateAsync(request);
            return Ok(result);
        }

    }
}
