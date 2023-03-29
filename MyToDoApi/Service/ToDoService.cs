using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection.Metadata;

using AutoMapper;

using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using MyToDoApi.Context;
using MyToDoApi.Context.UnitOfWork;
using System.Collections.ObjectModel;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.JSInterop;

namespace MyToDoApi.Service
{
    /// <summary>
    /// 待办事项 服务实现
    /// </summary>
    public class ToDoService : IToDoService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ToDoService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
                var todo = _mapper.Map<ToDo>(model);
                await _unitOfWork.GetRepository<ToDo>().InsertAsync(todo);
                if(await _unitOfWork.SaveChangesAsync()>0)
                {
                    return new ApiResponse(true, todo);
                }
                else
                {
                    return new ApiResponse("添加数据失败");
                }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var model = await repository.GetFirstOrDefaultAsync(predicate:s=>s.Id==id);
            if(model!=null)
            {
                repository.Delete();
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse("success",true);
            }
            return new ApiResponse("failed");
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameters parameter)
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            if (string.IsNullOrEmpty(parameter.Search))
            {
                var models = await repository.GetAll().OrderByDescending(t=>t.Id).ToPagedListAsync(parameter.PageIndex, parameter.PageSize);
                return new ApiResponse(true, models);
            }
            else
            {
                var models = await repository.GetPagedListAsync(predicate: x => x.Title.Contains(parameter.Search), pageIndex: parameter.PageIndex, pageSize: parameter.PageSize);
                return new ApiResponse(true, models);
            }
        }

        public async Task<ApiResponse> GetAllAsync(ToDoParameter parameter)
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            if (string.IsNullOrEmpty(parameter.Search)&&parameter.Status!=null)
            {
                var models = await repository.GetAll(p => p.Status == parameter.Status).ToPagedListAsync(parameter.PageIndex, parameter.PageSize);
                return new ApiResponse(true, models);
            }else if (string.IsNullOrEmpty(parameter.Search) && parameter.Status == null)
            {
                var models = await repository.GetAll().ToPagedListAsync(parameter.PageIndex, parameter.PageSize);
                return new ApiResponse(true, models);
            }
            else
            {
                var models = await repository.GetPagedListAsync(predicate: x => (x.Title.Contains(parameter.Search) && x.Status == parameter.Status), pageIndex: parameter.PageIndex, pageSize: parameter.PageSize);
                return new ApiResponse(true, models);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var model = await repository.GetFirstOrDefaultAsync(predicate:(s)=>s.Id==id);
            if (model != null)
            {
                return new ApiResponse(true, model);
            }
            return new ApiResponse("没有该用户");
        }

        public async Task<ApiResponse> Summary()
        {
            ///待办事项结果
            var todos = await _unitOfWork.GetRepository<ToDo>().GetAllAsync(orderBy:s=>s.OrderByDescending(t=>t.CreateTime));
            ///备忘录结果
            var memos = await _unitOfWork.GetRepository<Memo>().GetAllAsync(orderBy:s=>s.OrderByDescending(t=>t.CreateTime));
            SummaryDto summary = new SummaryDto();
            ///待办事项数量
            summary.Sum = todos.Count();
            ///统计完成数量
            summary.ComplatedCount = todos.Where(t => t.Status == 1).Count();
            ///统计完成比例
            summary.ComplatedRadio = (summary.ComplatedCount / (double)summary.Sum).ToString("0%");
            ///备忘录数量
            summary.MemoCount= memos.Count();
            ///待办事项列表
            summary.ToDoList = new ObservableCollection<ToDoDto>(_mapper.Map<List<ToDoDto>>(todos.Where(t=>t.Status==0)));
            ///备忘录列表
            summary.MemoList = new ObservableCollection<MeMoDto>(_mapper.Map<List<MeMoDto>>(memos));
            return new ApiResponse(true,summary);
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {

                var todo = _mapper.Map<ToDo>(model);

                var repository = _unitOfWork.GetRepository<ToDo>();
                var todomodel = await repository.GetFirstOrDefaultAsync(predicate: s => s.Id == todo.Id);
                if (todomodel != null)
                {
                    todomodel.Title = todo.Title;
                    todomodel.UpdateTime = DateTime.Now;
                    todomodel.Content = todo.Content;
                    todomodel.Status = todo.Status;
                    repository.Update(todomodel);
                    await _unitOfWork.SaveChangesAsync();
                    return new ApiResponse(true, todo);
                }
                else return new ApiResponse("异常！请检查您输入的id是否正确");
            }
            catch(Exception ex) 
            {
                return new ApiResponse(ex.Message);
            }
        }

    }
}
