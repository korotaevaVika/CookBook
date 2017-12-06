using CookBook_WPF.ViewModel;
using MahApps.Metro.Controls;
using System.Collections.Generic;

namespace CookBook_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для MaintainBusketControl.xaml
    /// </summary>
    public partial class MaintainBusketControl : MetroWindow
    {
        public MaintainBusketControl()
        {
            InitializeComponent();
        }
        private void BackToParentVM(object sender, System.Windows.RoutedEventArgs e)
        {
            ((MaintainBusketViewModel)this.DataContext).BackCommand.Execute(null);
            this.DialogResult = true;
        }

        private void CreateBasket(object sender, System.Windows.RoutedEventArgs e)
        {
            ((MaintainBusketViewModel)this.DataContext).NewBusketCommand.Execute(null);
            this.DialogResult = true;
        }


        public string Answer
        {
            get { return ((MaintainBusketViewModel)this.DataContext).UserInput; }
        }

        public void SetInfo(List<int> plansIndexes)
       {
            ((MaintainBusketViewModel)this.DataContext).plansIndexes = plansIndexes;
        }

        private void AddPlansToExistingBasket(object sender, System.Windows.RoutedEventArgs e)
        {
            ((MaintainBusketViewModel)this.DataContext).ChooseBusketCommand.Execute(null);
            this.DialogResult = true;
        }
    }
}
