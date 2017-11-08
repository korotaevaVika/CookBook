using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
