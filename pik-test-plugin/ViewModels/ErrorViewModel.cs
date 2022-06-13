using PikTestPlugin.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PikTestPlugin.ViewModels
{
    public class ErrorViewModel : INotifyPropertyChanged
    {
        public ErrorViewModel(ErrorModel errorModel)
        {
            _errorModel = errorModel;
        }
        private ErrorModel _errorModel;

        public string ErrorText 
        { 
            get { return _errorModel.ErrorMessage; } 
            set
            {
                _errorModel.ErrorMessage = value;
                OnPropertyChanged(nameof(ErrorText));
            } 
        }

        public string InfoText
        {
            get { return _errorModel.InfoMessage; }
            set
            {
                _errorModel.InfoMessage = value;
                OnPropertyChanged(nameof(InfoText));
            }
        }

        public string ButtonText
        {
            get { return _errorModel.ButtonText; }
            set
            {
                _errorModel.ButtonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}