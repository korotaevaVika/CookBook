using CookBook_WPF.Helper_Classes;
using CookBook_WPF.View;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookBook_WPF.ViewModel
{
    public class MainViewModel : BindableBase
    {

        private BindableBase mCurrentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return mCurrentViewModel; }
            set { mCurrentViewModel = value; OnPropertyChanged(); }
        }

        #region Commands
        private readonly RelayCommand mShowProductCatalogCommand;
        private readonly RelayCommand mShowRecipeCatalogCommand;
        public ICommand ShowProductCatalogCommand { get { return mShowProductCatalogCommand; } }
        public ICommand ShowRecipeCatalogCommand { get { return mShowRecipeCatalogCommand; } }
        #endregion
        public MainViewModel()
        {
            CurrentViewModel = new ProductCatalogViewModel();
            //CurrentViewModel = new MeasureProductRelationViewModel(
            //    new MeasureProductRelationViewModel.MeasureProductRelationInfo
            //    {
            //        productKey = 7,
            //        mainMeasureValueKey = 7,
            //        ParentViewModel = new ProductCatalogViewModel(new DialogCoordinator())
            //    });
           // CurrentViewModel = new ProductCatalogViewModel();

            //DialogWindow wn = new DialogWindow();
            
            mShowProductCatalogCommand = new RelayCommand(ShowProductCatalog);
            mShowRecipeCatalogCommand = new RelayCommand(ShowRecipeCatalog);

        }

        private void ShowRecipeCatalog(object obj)
        {
            CurrentViewModel = new RecipeCatalogViewModel();
        }

        private void ShowProductCatalog(object obj)
        {
            CurrentViewModel = new ProductCatalogViewModel();
        }
    }
}
