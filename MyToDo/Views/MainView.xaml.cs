using System;
using System.Windows;
using System.Windows.Input;

using MyToDo.Common.Models;
using MyToDo.Extensions;

using Prism.Events;
using Prism.Services.Dialogs;

namespace MyToDo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private IDialogHostService _hostSercice;
        public MainView(IEventAggregator aggregator, IDialogHostService hostSercice)
        {
            InitializeComponent();
            //注册提示消息
            aggregator.RegisterMessage( s =>
            {
                Snackbar.MessageQueue.Enqueue(s);
            });
            //注册等待消息窗口
            this._hostSercice= hostSercice;
            aggregator.Register(s =>
            {
                DialogHost.IsOpen = s.IsOpen;
                if(DialogHost.IsOpen)
                {
                    DialogHost.DialogContent = new ProgressView();
                }
            });
            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen= false;
            };
            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnClose.Click += async (s, e) => 
            {
                var result =  await _hostSercice.Question("温馨提示", "确定退出?");
                if (result.Result != ButtonResult.OK) return;
                this.Close(); 
            };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            };
            ColorZone.MouseMove += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            };
        }

    }
}
