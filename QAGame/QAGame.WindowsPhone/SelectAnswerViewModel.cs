using QAGame.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAGame
{
    public class SelectAnswerViewModel : Bindable
    {
        public SelectAnswerViewModel() { }

        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public string SelectedAnswer { get; set; }

        private DelegateCommand<object> _selectAnswerCommand;
        public DelegateCommand<object> SelectAnswerCommand
        {
            get
            {
                return _selectAnswerCommand = _selectAnswerCommand ?? new DelegateCommand<object>(SelectAnswerExecutedHandler);
            }
        }
        private void SelectAnswerExecutedHandler(object obj)
        {
            OnPropertyChanged("SelectedAnswer");
        }
    }
}
