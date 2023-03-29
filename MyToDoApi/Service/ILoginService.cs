using MyToDo.Shared.Dtos;
using MyToDoApi.Context;

namespace MyToDoApi.Service
{
    public interface ILoginService
    {
        public Task<ApiResponse> LoginAsync(string account,string password);
        public Task<ApiResponse> Register(UserDto user);
    }
}
