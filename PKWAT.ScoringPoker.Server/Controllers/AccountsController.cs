namespace PKWAT.ScoringPoker.Server.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PKWAT.ScoringPoker.Contracts.Accounts;
    using PKWAT.ScoringPoker.Server.Data;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterRequest model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return BadRequest(new RegisterResponse { Success = false, Errors = result.Errors.Select(x => x.Description)});
            }

            return Ok(new RegisterResponse { Success = true });
        }
    }
}
