using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyToDo.Shared.Dtos;

using MyToDoApi.Context;

namespace MyToDo.Service
{
    public class LoginService : ILoginService
    {
        private readonly HttpRestClient _client;
        private readonly string serviceName = "Login";
        public LoginService(HttpRestClient client)
        {
            _client = client;
        }

        public async Task<ApiResponse<UserDto>> LoginAsync(UserDto userdto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Login";
            userdto.UserName = " ";
            request.Parameter = userdto;
            return await _client.ExecuteAsync<UserDto>(request);
        }

        public async Task<ApiResponse> RegisterAsync(UserDto userdto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Register";
            request.Parameter = userdto;
            return await _client.ExecuteAsync(request);
        }
    }
}
