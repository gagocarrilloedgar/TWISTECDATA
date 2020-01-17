using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwistecData.UI.ViewModel
{
    public class MessageViewModel: ViewModelBase
    {
        #region Properties
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }
        #endregion  

        public MessageViewModel()
        {

        }
    }
}
