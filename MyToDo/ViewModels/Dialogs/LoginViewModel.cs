using System;

using Microsoft.Win32;

using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels.Dialogs
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly ILoginService _loginService;
        private readonly IEventAggregator _aggregator;
        public LoginViewModel(ILoginService loginService, IEventAggregator aggregator)
        {
            UserDto = new RegisterUserDto();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            _loginService = loginService;
            _aggregator = aggregator;
        }


        void Excute(string arg)
        {
            switch (arg)
            {
                case "Login": Login(); break;
                case "LoginOut": LoginOut(); break;
                case "Go": SelectIndex = 1; break;//跳转注册页面
                case "Register": Register(); break;//注册账号
                case "Return": SelectIndex = 0; break;//返回登录页面
            }
        }
        public string Title { get; set; } = "ToDO";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            LoginOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public DelegateCommand<string> ExcuteCommand { get;private set; }

        private int selectIndex;

        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
        }


        private string account;

        public string Account
        {
            get { return account; }
            set { account = value;RaisePropertyChanged(); }
        }
        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value;RaisePropertyChanged(); }
        }

        private RegisterUserDto userDto;

        public RegisterUserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; RaisePropertyChanged(); }
        }


       

        private async void Register()
        {
            if (string.IsNullOrEmpty(UserDto.Account) || string.IsNullOrEmpty(UserDto.UserName)|| string.IsNullOrEmpty(UserDto.NewPassWord)|| string.IsNullOrEmpty(UserDto.PassWord))
            {
                _aggregator.SendMessage("信息不能为空");
                return;
            }
            if(UserDto.PassWord!=UserDto.NewPassWord)
            {
                _aggregator.SendMessage("两次密码不一致");
                return;
            }
            var RegisterResut = await _loginService.RegisterAsync(new UserDto()
            {
                Account= UserDto.Account,
                UserName= UserDto.UserName,
                Password= UserDto.PassWord,
            });
            if(RegisterResut!= null&&RegisterResut.Status==true)
            {
                //注册成功
                SelectIndex= 0;
                _aggregator.SendMessage("注册成功");

            }

            //注册失败 给出提示
            _aggregator.SendMessage("error");
        }

        async void Login() 
        {
            if(string.IsNullOrEmpty(Account)||string.IsNullOrEmpty(PassWord)) 
            {
                _aggregator.SendMessage("用户名或密码不能为空");
                return;
            }
            var loginResult = await _loginService.LoginAsync(new UserDto()
            {
                Account = Account,
                Password = PassWord,
                //UserName= UserDto.UserName,
            });
            if(loginResult.Status&&loginResult!=null)
            {
                AppSession.UserName = loginResult.Result.UserName;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                return;
            }
            else
            {
            //登录失败....
            _aggregator.SendMessage("error");
            }


        }

        void LoginOut() 
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }
    }
}
