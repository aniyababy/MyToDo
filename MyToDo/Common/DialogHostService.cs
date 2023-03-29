using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

using MyToDo.Common.Models;

using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDo.Common
{
    public class DialogHostService :DialogService, IDialogHostService
    {
        private readonly IContainerExtension _containerExtension;
        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            _containerExtension = containerExtension;
        }

        /// <summary>
        /// 对话主机服务(自定义)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dialogHostName"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<IDialogResult>? ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root")
        {
            if(parameters == null)
            {
                parameters = new DialogParameters();
            }
            //从容器当中取出弹出窗口的实例
            var content = _containerExtension.Resolve<object>(name);
            //验证实例的有效性
            if(!(content is FrameworkElement dialogConent)) 
            {
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");
            }
            if(dialogConent is FrameworkElement view &&view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }
            if(!(dialogConent.DataContext is IDIalogHostAware viewModel))
            {
                throw new NullReferenceException("A dialog's ViewModel must implemennt the IDIalogAware Interface");
            }
            viewModel.DialogHostName= dialogHostName;
            DialogOpenedEventHandler eventHandler = (sender, eventArgs) =>
            {
                if (viewModel is IDIalogHostAware aware)
                {
                    aware.OnDialogOpend(parameters);
                }
                eventArgs.Session.UpdateContent(content);
            };
            return await DialogHost.Show(dialogConent, viewModel.DialogHostName, eventHandler) as IDialogResult;
        }
    }
}
