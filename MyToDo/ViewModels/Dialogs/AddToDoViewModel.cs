using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

using MyToDo.Common;
using MyToDo.Shared.Dtos;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels.Dialogs
{
    public class AddToDoViewModel : BindableBase,IDIalogHostAware
    {
        public AddToDoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private ToDoDto model;

        public ToDoDto Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        private void Cancel()
        {
            if(DialogHost.IsDialogOpen(DialogHostName))
            {
                //取消 传回NO
                  DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
            }
        }

        private void Save()
        {
            if(string.IsNullOrEmpty(Model.Title) || string.IsNullOrEmpty(Model.Content))
            {
                return;
            }
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                //确定 传回ok 并且传回编辑的实体
                param.Add("Value", Model);
                DialogHost.Close(DialogHostName,new DialogResult(ButtonResult.OK,param));
            }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Model = parameters.GetValue<ToDoDto>("Value");
            }
            else
            {
                Model = new ToDoDto();
            }
        }
    }
}
