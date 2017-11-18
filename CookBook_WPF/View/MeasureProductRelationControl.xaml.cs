using CookBook_WPF.ViewModel;
using MahApps.Metro.Controls;

namespace CookBook_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для MeasureProductRelationControl.xaml
    /// </summary>
    public partial class MeasureProductRelationControl : MetroWindow
    {
        public MeasureProductRelationControl()
        {
            InitializeComponent();
        }

        private void BackToParentVM(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string Answer
        {
            get { return ((MeasureProductRelationViewModel)this.DataContext).UserInput; }
        }
    }
}
