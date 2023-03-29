using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace MyToDo.Shared.Dtos
{
    /// <summary>
    /// 汇总
    /// </summary>
    public class SummaryDto:BaseDto
    {
        private int sum;
        private int complatedCount;
        private int memoCount;
        private string complatedRadio;

        /// <summary>
        /// 待办事项总数
        /// </summary>
        public int Sum
        {
            get { return sum; }
            set { sum = value;OnPropertyChanged(); }
        }
        /// <summary>
        /// 完成待办事项数量
        /// </summary>
        public int ComplatedCount
        {
            get { return complatedCount; }
            set { complatedCount = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 备忘录数量
        /// </summary>
        public int MemoCount
        {
            get { return memoCount; }
            set { memoCount = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 完成比例
        /// </summary>
        public string ComplatedRadio
        {
            get { return complatedRadio; }
            set { complatedRadio = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> todoList;
        private ObservableCollection<MeMoDto> memoList;
        
        /// <summary>
        /// 待办事项列表
        /// </summary>
        public ObservableCollection<ToDoDto> ToDoList
        {
            get { return todoList; }
            set { todoList = value;OnPropertyChanged(); }
        }
        /// <summary>
        /// 备忘录列表
        /// </summary>
        public ObservableCollection<MeMoDto> MemoList
        {
            get { return memoList; }
            set { memoList = value; OnPropertyChanged(); }
        }

    }
}
