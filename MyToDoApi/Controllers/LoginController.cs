using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MyToDo.Shared.Dtos;
using MyToDoApi.Context;
using MyToDoApi.Service;

namespace MyToDoApi.Controllers
{
    /// <summary>
    /// 账号控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<Service.ApiResponse> Register([FromBody] UserDto user)
        {
            return await _loginService.Register(user);
        }
        [HttpPost]
        public async Task<Service.ApiResponse> Login([FromBody] UserDto user)
        {
            return await _loginService.LoginAsync(user.Account, user.Password);
        }
    }
}
