using QAGame.Common;
using QAGame.DataModel;
using QAGame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAGame.ViewModel
{
    public class AddCustomQuestionViewModel : Bindable
    {
        private CustomQuestionRepository _repo;
        private DelegateCommand<object> _addCustomQuestionCommand;

        public AddCustomQuestionViewModel()
        {
            this._repo = new CustomQuestionRepository();
            this.CustomQuestion = new CustomQuestion();
        }

        public CustomQuestion CustomQuestion { get; set; }
        public DelegateCommand<object> AddCustomQuestionCommand
        {
            get
            {
                return _addCustomQuestionCommand = _addCustomQuestionCommand ?? new DelegateCommand<object>(AddCustomQuestionExecutedHandler);
            }
        }
        private void AddCustomQuestionExecutedHandler(object obj)
        {
            if (!string.IsNullOrWhiteSpace(CustomQuestion.Question))
            {
                _repo.Add(CustomQuestion);
            }
        }

    }
}
