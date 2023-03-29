using System;
using System.Collections.Generic;
using System.Text;

namespace MyToDo.Shared.Dtos
{
    /// <summary>
    /// 备忘录数据传输实体
    /// </summary>
    public class MeMoDto:BaseDto
    {
        private string title;
        private string content;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }
        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }
        
    }
}
