using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels
{
    public class MemoViewModel:NavigationViewModel
    {
        private readonly IMeMoService _memoService;
        private readonly IDialogHostService _hostService;
        public MemoViewModel(IMeMoService meMoService, IContainerProvider provider) : base(provider)
        {
            _hostService=provider.Resolve<IDialogHostService>();
            _memoService = meMoService;
            MemoDtos = new ObservableCollection<MeMoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            SelectedCommand = new DelegateCommand<MeMoDto>(Selected);
            DeleteCommand = new DelegateCommand<MeMoDto>(Delete);
        }



        private async void Excute(string obj)
        {
            switch (obj)
            {
                case "新增": Add(); break;
                case "查询": await GetDataAsync(); break;
                case "保存": Save(); break;
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
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }
        private MeMoDto currentDto;

        /// <summary>
        /// 编辑选中对象/新增的对象
        /// </summary>
        public MeMoDto CurrentDto
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
        /// 添加备忘录
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void Add()
        {
            CurrentDto = new MeMoDto();
            IsRightDrawerOpen = true;
        }
        /// <summary>
        /// 删除备忘录
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void Delete(MeMoDto obj)
        {
            UpdateLoading(true);
            IDialogResult? dialogResult = await _hostService.Question("温馨提示", $"确定删除{obj.Title}?");
            if (dialogResult.Result != ButtonResult.OK) return;
            var deleteResult = await _memoService.DeleteAsync(obj.Id);
            if (deleteResult.Status)
            {
                var model = MemoDtos.FirstOrDefault(s => s.Id == obj.Id);
                if (model != null)
                    MemoDtos.Remove(model);
            }
            UpdateLoading(false);

        }
        /// <summary>
        /// 保存备忘录
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
                    var result = await _memoService.UpdateAsync(CurrentDto);
                    if (result.Status)
                    {
                        var todo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                        }
                        IsRightDrawerOpen = false;
                    }
                }
                else
                {
                    var addresult = await _memoService.AddAsync(CurrentDto);
                    if (addresult.Status)
                    {
                        MemoDtos.Add(addresult.Result);
                        UpdateLoading(false);

                        IsRightDrawerOpen = false;
                    }
                }
                UpdateLoading(false);
            }
        }
        private async void Selected(MeMoDto obj)
        {

            var todomodel = await _memoService.GetFirstOrDefaultAsync(obj.Id);
            if (todomodel.Status)
            {
                CurrentDto = todomodel.Result;
                IsRightDrawerOpen = true;
            }

        }


        private ObservableCollection<MeMoDto> memeDtos;
        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<MeMoDto> SelectedCommand { get; private set; }
        public DelegateCommand<MeMoDto> DeleteCommand { get; private set; }

        public ObservableCollection<MeMoDto> MemoDtos
        {
            get { return memeDtos; }
            set { memeDtos = value; }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public async Task GetDataAsync()
        {
            UpdateLoading(true);
            var todoResult = await _memoService.GetAllAsync(new QueryParameters()
            {
                PageIndex = 0,
                PageSize = 20,
                Search = Search,
            });
            if (todoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in todoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }
        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            await GetDataAsync();
        }
    }
}
