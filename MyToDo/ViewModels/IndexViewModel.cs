using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;

using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels
{
    public class IndexViewModel:NavigationViewModel
    {
        private readonly IDialogHostService dialog;
        private readonly IToDoService _toDoService;
        private readonly IMeMoService _meMoService;
        private readonly IRegionManager _regionManager;
        public IndexViewModel(IDialogHostService dialog, IContainerProvider provider) : base(provider)
        {
            Title = $"你好{AppSession.UserName}，现在是{DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";

            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.dialog = dialog;
            CreateTaskBars();
            _meMoService = provider.Resolve<IMeMoService>();
            _toDoService = provider.Resolve<IToDoService>();
            _regionManager = provider.Resolve<IRegionManager>();
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MeMoDto>(AddMemo);
            ToDoComplatedCommand = new DelegateCommand<ToDoDto>(Complated);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
        }


        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<ToDoDto> ToDoComplatedCommand { get; private set; }
        public DelegateCommand<MeMoDto> EditMemoCommand { get; private set; }
        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }

        private void Navigate(TaskBar obj)
        {
            if(string.IsNullOrEmpty(obj.Target))
            {
                return;
            }
            NavigationParameters param = new NavigationParameters();
            if (obj.Title == "已完成")
            {
                param.Add("Value", 2);
            }
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, param);

        }

        private async void Complated(ToDoDto obj)
        {
            UpdateLoading(true);
            var updateResult =  await _toDoService.UpdateAsync(obj);
            if (updateResult.Status)
            {
                var todo = summaryDto.ToDoList.FirstOrDefault(i => i.Id == obj.Id);
                if (todo != null)
                {
                    summaryDto.ToDoList.Remove(todo);
                    summaryDto.ComplatedCount += 1;
                    summaryDto.ComplatedRadio = (summaryDto.ComplatedCount / (double)summaryDto.Sum).ToString("0%");
                   
                    Reflush();
                }
                aggregator.SendMessage("已完成!");
            }
            UpdateLoading(false);
        }

        private void Execute(string obj)
        {  
            switch(obj)
            {
                case "新增待办":AddToDo(null); break;
                case "新增备忘录": AddMemo(null); break;
            }
        }
        /// <summary>
        /// 添加备忘录
        /// </summary>
        private async void AddMemo(MeMoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
            {
                param.Add("Value", model);
            }
            
            var dialogResult = await dialog.ShowDialog("AddMemoView", param);
             
            if (dialogResult.Result == ButtonResult.OK)
            {
                var memo = dialogResult.Parameters.GetValue<MeMoDto>("Value");
                if (memo.Id > 0)
                {
                    var updateResult = await _meMoService.UpdateAsync(memo);
                    if (updateResult.Status)
                    {
                        var todoModel = SummaryDto.MemoList.FirstOrDefault(t => t.Id == memo.Id);
                        if (todoModel != null)
                        {
                            todoModel.Title = memo.Title;
                            todoModel.Content = memo.Content;
                        }
                    }
                }
                else
                {
                    var addResult = await _meMoService.AddAsync(memo);
                    if (addResult.Status)
                    {
                        SummaryDto.MemoList.Add(addResult.Result);
                    }
                }
            }
        }
        /// <summary>
        /// 添加待办
        /// </summary>
        /// </summary>
        private async void AddToDo(ToDoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
            {
                param.Add("Value", model);
            }
            var dialogResult =  await dialog.ShowDialog("AddToDoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                if (todo.Id > 0)
                {
                   var updateResult = await _toDoService.UpdateAsync(todo);
                    if (updateResult.Status)
                    {
                       var todoModel = summaryDto.ToDoList.FirstOrDefault(t => t.Id == todo.Id);
                        if (todoModel != null)
                        {
                            todoModel.Title = todo.Title;
                            todoModel.Content = todo.Content;
                        }
                    }
                }
                else
                {
                    var addResult =await _toDoService.AddAsync(todo);
                    if (addResult.Status)
                    {
                        summaryDto.Sum += 1;
                        summaryDto.ComplatedRadio = (summaryDto.ComplatedCount / (double)summaryDto.Sum).ToString("0%");
                        summaryDto.ToDoList.Add(addResult.Result);
                        Reflush();
                    }
                }
            }
        }

        #region 属性
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value;RaisePropertyChanged(); }
        }

        public SummaryDto summaryDto;
        /// <summary>
        /// 首页统计
        /// </summary>
        public SummaryDto SummaryDto
        {
            get { return summaryDto; }
            set { summaryDto = value; RaisePropertyChanged(); }
        }
        #endregion
        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Color = "#FF02C6DC", Target = "" });
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = "MemoView" });
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateLoading(true);
            var summaryResult = await _toDoService.SummaryAsync();
            if (summaryResult.Status)
            {
                SummaryDto = summaryResult.Result;
                Reflush();
            }
            UpdateLoading(false);

            base.OnNavigatedTo(navigationContext);
        }
        void Reflush()
        {
            TaskBars[0].Content = SummaryDto.Sum.ToString();
            TaskBars[1].Content = SummaryDto.ComplatedCount.ToString();
            TaskBars[2].Content = SummaryDto.ComplatedRadio;
            TaskBars[3].Content = SummaryDto.MemoCount.ToString();
        }

    }
}
