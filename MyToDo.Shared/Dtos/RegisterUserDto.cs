using System;
using System.Collections.Generic;
using System.Text;

namespace MyToDo.Shared.Dtos
{
    public class RegisterUserDto:BaseDto
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged(); }
        }
        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; OnPropertyChanged(); }
        }

        private string newPassWord;

        public string NewPassWord
        {
            get { return newPassWord; }
            set { newPassWord = value; OnPropertyChanged(); }
        }
    }
}
