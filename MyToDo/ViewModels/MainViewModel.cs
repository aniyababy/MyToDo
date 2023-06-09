﻿using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;

using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace MyToDo.ViewModels
{
    public class MainViewModel:BindableBase, IConfigureService
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public MainViewModel(IRegionManager regionManager, IContainerProvider containerProvider)
        {
           
            this.MenuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            this.regionManager = regionManager;
            this.containerProvider = containerProvider;
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal.CanGoBack&&journal!=null)
                {
                    journal.GoBack();
                }
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal.CanGoForward && journal != null)
                {
                    journal.GoForward();
                }
            });
            LoginOutCommand = new DelegateCommand(() =>
            {
                App.LoginOut(containerProvider);
            });
        }

        public void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace)) 
                return;

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
            
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand LoginOutCommand { get; private set; }
        private ObservableCollection<MenuBar> menuBars;
        private readonly IRegionManager regionManager;
        private readonly IContainerProvider containerProvider;
        private IRegionNavigationJournal journal;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }


        public void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "待办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            UserName = AppSession.UserName;
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
