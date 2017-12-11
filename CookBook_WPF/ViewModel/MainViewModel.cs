using CookBook_WPF.Helper_Classes;
using CookBook_WPF.View;
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
        private readonly RelayCommand mShowPlanCatalogCommand;
        private readonly RelayCommand mPrintBasketCommand;
        
        public ICommand ShowProductCatalogCommand { get { return mShowProductCatalogCommand; } }
        public ICommand ShowRecipeCatalogCommand { get { return mShowRecipeCatalogCommand; } }
        public ICommand ShowPlanCatalogCommand { get { return mShowPlanCatalogCommand; } }
        public ICommand PrintBasketCommand { get { return mPrintBasketCommand; } }
        #endregion
        public MainViewModel()
        {
             CurrentViewModel = new LoginViewModel();
           // CurrentViewModel = null;

            mShowProductCatalogCommand = new RelayCommand(ShowProductCatalog);
            mShowRecipeCatalogCommand = new RelayCommand(ShowRecipeCatalog);
            mShowPlanCatalogCommand = new RelayCommand(ShowPlanCatalog);
            mPrintBasketCommand = new RelayCommand(PrintBasket);

        }

        private void ShowRecipeCatalog(object obj)
        {
            CurrentViewModel = new RecipeCatalogViewModel();
        }
        private void PrintBasket(object obj)
        {
            try
            {
                PrintBasketControl inputDialog = new PrintBasketControl();
                inputDialog.ShowDialog();
            }
            catch { MessageBox.Show("Список пуст"); }
        }

        private void ShowProductCatalog(object obj)
        {
            CurrentViewModel = new ProductCatalogViewModel();
        }
        private void ShowPlanCatalog(object obj)
        {
            CurrentViewModel = new PlanCatalogViewModel();
        }
    }
}
