using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels
{
	public class ToDoViewModel: NavigationViewModel
    {
		private readonly IToDoService _toDoService;
		private readonly IDialogHostService _dialogHost;

        public ToDoViewModel(IToDoService toDoService, IContainerProvider provider) : base(provider)
        {
			_dialogHost = provider.Resolve<IDialogHostService>();
            _toDoService = toDoService;
            ToDoDtos = new ObservableCollection<ToDoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand  =new DelegateCommand<ToDoDto>(Delete);
        }



        private async void Excute(string obj)
        {
			switch(obj)
			{
				case "新增":Add();break;
				case "查询":await GetDataAsync();break;
				case "保存":Save();break;
			}
        }

		
        private int selectedIndex;

		/// <summary>
		/// 下拉列表选择状态值
		/// </summary>
		public int SelectedIndex
        {
			get { return selectedIndex; }
			set { selectedIndex = value; RaisePropertyChanged(); }
		}


		private bool isRightDrawerOpen;

		/// <summary>
		/// 右侧编辑窗口是否展开
		/// </summary>
		public bool IsRightDrawerOpen
        {
			get { return isRightDrawerOpen; }
			set { isRightDrawerOpen = value;RaisePropertyChanged(); }
		}
		private ToDoDto currentDto;

		/// <summary>
		/// 编辑选中对象/新增的对象
		/// </summary>
		public ToDoDto CurrentDto
		{
			get { return currentDto; }
			set { currentDto = value; RaisePropertyChanged(); }
		}
		private string search;

		public string Search
		{
			get { return search; }
			set { search = value; RaisePropertyChanged(); }
		}


		/// <summary>
		/// 添加待办
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		private void Add()
        {
			CurrentDto= new ToDoDto();
			IsRightDrawerOpen = true;
        }
		/// <summary>
		/// 删除待办
		/// </summary>
		/// <param name="obj"></param>
		/// <exception cref="NotImplementedException"></exception>
        private async void Delete(ToDoDto obj)
        {
			IDialogResult? dialogResult = await _dialogHost.Question("温馨提示", $"确定删除{obj.Title}?");
			if (dialogResult.Result != ButtonResult.OK) return;
			var deleteResult = await _toDoService.DeleteAsync(obj.Id);
			if (deleteResult.Status)
			{
				var model = ToDoDtos.FirstOrDefault(s=>s.Id== obj.Id);
				if(model!=null) 
					ToDoDtos.Remove(model);
			}
        }
        /// <summary>
        /// 保存待办
        /// </summary>
        private async void Save()
        {
			if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content))
				{
				return;
			}
			else
			{
				UpdateLoading(true);
				if (CurrentDto.Id > 0)
				{
				  var result = await _toDoService.UpdateAsync(CurrentDto);
					if (result.Status)
					{
						var todo = ToDoDtos.FirstOrDefault(t=>t.Id== CurrentDto.Id);
						if (todo != null)
						{
							todo.Title= CurrentDto.Title;
							todo.Content= CurrentDto.Content;
							todo.Status = CurrentDto.Status;
                            UpdateLoading(false);

                        }
                    }
				}
				else
				{
					var addresult = await _toDoService.AddAsync(CurrentDto);
					if(addresult.Status)
					{
						
						ToDoDtos.Add(addresult.Result);
						IsRightDrawerOpen= false;
					}
				}
				UpdateLoading(false);
			}
        }
        private async void Selected(ToDoDto obj)
        {
			
				var todomodel = await _toDoService.GetFirstOrDefaultAsync(obj.Id);
				if (todomodel.Status)
				{
					CurrentDto = todomodel.Result;
					IsRightDrawerOpen = true;
				}
			
        }


        private ObservableCollection<ToDoDto> toDoDtos;
		public DelegateCommand<string> ExcuteCommand { get;private set; }
		public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
		public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }

        public ObservableCollection<ToDoDto> ToDoDtos
		{
			get { return toDoDtos; }
			set { toDoDtos = value;}
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		public async Task GetDataAsync()
		{
			UpdateLoading(true);
            int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;
            var todoResult = await _toDoService.GetAllFilterAsync(new ToDoParameter()
			{
				PageIndex = 0,
				PageSize = 20,
				Search = Search,
				Status = Status,
			});
			if (todoResult.Status)
			{
				ToDoDtos.Clear();
				foreach(var item in todoResult.Result.Items)
				{
					ToDoDtos.Add(item);
				}
			}
			UpdateLoading(false);
		}
        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {

            base.OnNavigatedTo(navigationContext);
			if(navigationContext.Parameters.ContainsKey("Value"))
				SelectedIndex = navigationContext.Parameters.GetValue<int>("Value");
			else
				SelectedIndex= 0;
			await GetDataAsync();
        }
    }
}
