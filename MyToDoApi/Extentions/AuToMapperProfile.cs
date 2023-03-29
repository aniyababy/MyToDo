using AutoMapper;

using MyToDo.Shared.Dtos;

using MyToDoApi.Context;
using MyToDoApi.Migrations;

namespace MyToDoApi.Extentions
{
    public class AuToMapperProfile:MapperConfigurationExpression
    {
        public AuToMapperProfile()
        {
            CreateMap<ToDo,ToDoDto>().ReverseMap();
            CreateMap<Memo,MeMoDto>().ReverseMap();
            CreateMap<User,UserDto>().ReverseMap();
        }
    }
}
