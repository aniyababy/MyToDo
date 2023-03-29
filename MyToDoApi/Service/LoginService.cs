using System.Security.Principal;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MyToDo.Shared.Dtos;
using MyToDo.Shared.Extensions;
using MyToDoApi.Context;
using MyToDoApi.Context.UnitOfWork;

namespace MyToDoApi.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LoginService(IUserService userService, IUnitOfWork unitOfWork = null, IMapper mapper = null)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(string account, string password)
        {
            password =  password.GetMD5();
            var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate:e => e.Password == password && e.Account == account);
            if(user == null)
            {
                return new ApiResponse("用户名或密码错误",false);
            }
            else
            {
                return new ApiResponse(true, user);
            }
        }
        public async Task<ApiResponse> Register([FromBody]UserDto user)
        {
            var usermodel = _mapper.Map<User>(user);
            var result = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: e => e.Account== usermodel.Account);
            if (result != null)
            {
                return new ApiResponse("当前账号已存在");
            }
            else
            {
                usermodel.CreateTime = DateTime.Now;
                usermodel.Password = usermodel.Password.GetMD5();
                await _unitOfWork.GetRepository<User>().InsertAsync(usermodel);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse(true, usermodel);
            }
        }
    }
}
