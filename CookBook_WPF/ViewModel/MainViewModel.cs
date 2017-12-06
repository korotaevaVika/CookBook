using CookBook_WPF.Helper_Classes;
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
        public ICommand ShowProductCatalogCommand { get { return mShowProductCatalogCommand; } }
        public ICommand ShowRecipeCatalogCommand { get { return mShowRecipeCatalogCommand; } }
        public ICommand ShowPlanCatalogCommand { get { return mShowPlanCatalogCommand; } }
        #endregion
        public MainViewModel()
        {
            CurrentViewModel = new PlanCatalogViewModel();
            //CurrentViewModel = new ProductCatalogViewModel();
           
            //CurrentViewModel = new MeasureProductRelationViewModel(
            //    new MeasureProductRelationViewModel.MeasureProductRelationInfo
            //    {
            //        productKey = 7,
            //        mainMeasureValueKey = 7,
            //        ParentViewModel = new ProductCatalogViewModel(new DialogCoordinator())
            //    });


            mShowProductCatalogCommand = new RelayCommand(ShowProductCatalog);
            mShowRecipeCatalogCommand = new RelayCommand(ShowRecipeCatalog);
            mShowPlanCatalogCommand = new RelayCommand(ShowPlanCatalog);

        }

        private void ShowRecipeCatalog(object obj)
        {
            CurrentViewModel = new RecipeCatalogViewModel();
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
