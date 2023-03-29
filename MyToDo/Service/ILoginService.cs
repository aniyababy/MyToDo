using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyToDo.Shared.Dtos;

using MyToDoApi.Context;

namespace MyToDo.Service
{
    public interface ILoginService
    {
        Task<ApiResponse<UserDto>> LoginAsync(UserDto userdto);
        Task<ApiResponse> RegisterAsync(UserDto userdto);
    }
}
