using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookBook_WPF.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        DataAccess.MainModel _model;
        public MainVM()
        {
            _model = new DataAccess.MainModel();
            _model.GetMaterialGroups();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
