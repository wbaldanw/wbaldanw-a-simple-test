using Last.Simple.App.Domain.Contracts.UseCases.Users;
using Last.Simple.App.Domain.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Last.Simple.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICreateUserUC createUserUC;
        private readonly ISignInUC signInUC;

        public AccountController(ICreateUserUC createUserUC, ISignInUC signInUC) 
        {
            this.createUserUC = createUserUC;
            this.signInUC = signInUC;
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<ActionResult<long>> CreateUser([FromBody]CreateUserRequest request) 
        {
            var userId = await createUserUC.CreateUser(request);

            return Created(string.Empty, new {id = userId});
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult<string>> SignIn([FromBody]SignInRequest request)
        {
            var token = await signInUC.SignIn(request);
            return Ok(new { token });
        }
    }
}
