using CookBook_WPF.DataAccess;
using CookBook_WPF.Helper_Classes;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Windows;

namespace CookBook_WPF.ViewModel
{
    public class RecipeCatalogViewModel : BindableBase
    {
        private MainModel _model;
        private DataView mRecipes;
        public DataView Recipes
        {
            get { return mRecipes; }
            set
            {
                mRecipes = value;
                OnPropertyChanged();
            }
        }

        private DataView mIngredients;
        public DataView Ingredients
        {
            get { return mIngredients; }
            set
            {
                mIngredients = value;
                OnPropertyChanged();
            }
        }

        private DataView mGroups;
        public DataView Groups
        {
            get { return mGroups; }
            set
            {
                mGroups = value;
                OnPropertyChanged();
            }
        }

        private DataView mProducts;
        public DataView Products
        {
            get { return mProducts; }
            set
            {
                mProducts = value;
                LoadMeasures();
                OnPropertyChanged();
            }
        }

        private DataRowView mSelectedRecipe;
        public DataRowView SelectedRecipe
        {
            get { return mSelectedRecipe; }
            set
            {
                mSelectedRecipe = value;
                if (value != null)
                {
                    LoadIngredients();
                    // EditRecipe(null);
                }
                //mDeleteRecipeCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private DataRowView mSelectedGroup;
        public DataRowView SelectedGroup
        {
            get { return mSelectedGroup; }
            set
            {
                mSelectedGroup = value;
                if (value != null)
                {
                    LoadProducts();
                }
                OnPropertyChanged();
            }
        }

        private DataRowView mSelectedProduct;
        public DataRowView SelectedProduct
        {
            get { return mSelectedProduct; }
            set
            {
                mSelectedProduct = value;
                if (value != null)
                {
                    LoadMeasures();
                }
                //mDeleteProductCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        private DataView mMeasureProducts;
        public DataView MeasureProducts
        {
            get { return mMeasureProducts; }
            set
            {
                mMeasureProducts = value;
                OnPropertyChanged();
            }
        }

        private DataRowView mSelectedMeasureProduct;
        public DataRowView SelectedMeasureProduct
        {
            get { return mSelectedMeasureProduct; }
            set
            {
                mSelectedMeasureProduct = value;
                if (value != null)
                {
                    LoadMeasures();
                }
                //mDeleteProductCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        #region Commands
        private readonly RelayCommand mAddRecipeCommand;
        private readonly RelayCommand mEditRecipeCommand;
        private readonly RelayCommand mDeleteRecipeCommand;
        private readonly RelayCommand mSaveRecipeCommand;

        private readonly RelayCommand mAddProductCommand;
        private readonly RelayCommand mEditProductCommand;
        private readonly RelayCommand mDeleteProductCommand;
        private readonly RelayCommand mSaveProductCommand;

        private readonly RelayCommand mSaveCommand;

        public ICommand AddRecipeCommand { get { return mAddRecipeCommand; } }
        public ICommand EditRecipeCommand { get { return mEditRecipeCommand; } }
        public ICommand DeleteRecipeCommand { get { return mDeleteRecipeCommand; } }
        public ICommand SaveRecipeCommand { get { return mSaveRecipeCommand; } }
        public ICommand AddProductCommand { get { return mAddProductCommand; } }
        public ICommand EditProductCommand { get { return mEditProductCommand; } }
        public ICommand DeleteProductCommand { get { return mDeleteProductCommand; } }
        public ICommand SaveProductCommand { get { return mSaveProductCommand; } }

        public ICommand SaveCommand { get { return mSaveCommand; } }
        #endregion

        #region Material Group Properties
        private bool mIsRecipeEdited;
        private int mRecipeKey;
        private string mRecipeName;
        private bool mIsContainsFinishedProducts;
        public bool IsRecipeEdited
        {
            get { return mIsRecipeEdited; }
            set
            {
                mIsRecipeEdited = value;
                OnPropertyChanged();
            }
        }
        public int RecipeKey
        {
            get { return mRecipeKey; }
            set
            {
                mRecipeKey = value;
                OnPropertyChanged();
            }
        }
        public string RecipeName
        {
            get { return mRecipeName; }
            set
            {
                mRecipeName = value;
                OnPropertyChanged();
            }
        }
        public bool IsContainsFinishedProducts
        {
            get { return mIsContainsFinishedProducts; }
            set
            {
                mIsContainsFinishedProducts = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Product Properties
        private int mIngredientKey;
        private int mProductKey;
        private string mProductName;
        private double mIngredientQuantity;

        private bool mIsProductEdited;

        public bool IsProductEdited
        {
            get { return mIsProductEdited; }
            set
            {
                mIsProductEdited = value;
                OnPropertyChanged();
            }
        }

        public string ProductName
        {
            get { return mProductName; }
            set
            {
                mProductName = value;
                OnPropertyChanged();
            }
        }

        public double IngredientQuantity
        {
            get { return mIngredientQuantity; }
            set
            {
                mIngredientQuantity = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Other Properties
        private string mMessage;
        public string Message
        {
            get { return mMessage; }
            set
            {
                mMessage = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public RecipeCatalogViewModel()
        {
            _model = new MainModel();
            mRecipes = _model.GetRecipes();
            mAddRecipeCommand = new RelayCommand(AddRecipe);
            mEditRecipeCommand = new RelayCommand(EditRecipe);
            mDeleteRecipeCommand = new RelayCommand(DeleteRecipe, CanDeleteRecipe);
            mSaveRecipeCommand = new RelayCommand(SaveRecipe, CanSaveRecipe);
            mAddProductCommand = new RelayCommand(AddProduct);
            mEditProductCommand = new RelayCommand(EditProduct);
            mDeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
            mSaveProductCommand = new RelayCommand(SaveProduct, CanSaveProduct);
            mSaveCommand = new RelayCommand(Save);

            IsRecipeEdited = false;
            IsProductEdited = false;
        }

        #region Methods For Commands
        private void Save(object obj)
        {
            if (IsRecipeEdited && CanSaveRecipe(null))
            {
                SaveRecipe(null);
            }
            else if (IsProductEdited && CanSaveProduct(null))
            {
                SaveProduct(null);
            }
        }
        private bool CanSaveRecipe(object obj)
        {
            return true;
        }

        private void SaveRecipe(object obj)
        {
            bool mSuccess = false;
            //Message = DateTime.Now.ToString() + "\t" +
            //    _model.SaveRecipe(
            //    RecipeKey,
            //    RecipeName,
            //    ref mSuccess);

            if (mSuccess)
            {
                IsRecipeEdited = false;
                Recipes = _model.GetRecipes();
            }
            else MessageBox.Show(Message, "Error");


        }
        private bool CanSaveProduct(object obj)
        {
            return true;
        }

        private void SaveProduct(object obj)
        {
            bool mSuccess = false;
            //Message = DateTime.Now.ToString() + "\t" +
            //    _model.SaveIngredient(
            //    ProductKey,
            //    ProductName,

            //    (int)SelectedRecipe.Row["RecipeKey"],
            //     ref mSuccess);

            if (mSuccess)
            {
                IsProductEdited = false;
                LoadProducts();
            }
            else MessageBox.Show(Message, "Error");

        }
        private bool CanDeleteRecipe(object obj)
        {
            return SelectedRecipe != null;
        }
        private bool CanDeleteProduct(object obj)
        {
            return SelectedProduct != null;
        }
        private void AddRecipe(object obj)
        {
            IsRecipeEdited = true;
            IsProductEdited = false;
            RecipeKey = 0;
            RecipeName = null;
        }

        private void EditRecipe(object obj)
        {
            IsProductEdited = false;
            IsRecipeEdited = true;
            RecipeKey = (int)SelectedRecipe.Row["RecipeKey"];
            RecipeName = (string)SelectedRecipe.Row["RecipeName"];
        }

        private void DeleteRecipe(object obj)
        {
            bool mSuccess = false;
            // Message = _model.DeleteRecipe((int)SelectedRecipe.Row["RecipeKey"], ref mSuccess);
            Recipes = _model.GetRecipes();
            if (mSuccess)
            {
                IsRecipeEdited = false;
            }
            else MessageBox.Show(Message, "Error");

        }

        private void AddProduct(object obj)
        {
            IsProductEdited = true;
            IsRecipeEdited = false;
            mProductKey = 0;
            mIngredientKey = 0;
            ProductName = null;
           
        }

        private void EditProduct(object obj)
        {
            IsProductEdited = true;
            IsRecipeEdited = false;
            mIngredientKey = (int)SelectedProduct.Row["IngredientKey"];
            mProductKey = (int)SelectedProduct.Row["ProductKey"];
            ProductName = (string)SelectedProduct.Row["ProductName"];
           
        }

        private void DeleteProduct(object obj)
        {
            bool mSuccess = false;
            //Message = _model.DeleteIngredient((int)SelectedProduct.Row["IngredientKey"], ref mSuccess);

            if (mSuccess)
            {
                IsProductEdited = false;
            }
            else MessageBox.Show(Message, "Error");
        }

        #endregion

        #region Other Methods

        #endregion
        private void LoadProducts()
        {
            Products = _model.GetProducts((int)SelectedGroup.Row["GroupKey"]);
        }
        private void LoadIngredients()
        {
            //Products = _model.GetIngredients((int)SelectedRecipe.Row["RecipeKey"]);
        }
        private void LoadMeasures()
        {
            //MeasureProducts = _model.GetMeasures((int)SelectedProduct.Row["RecipeKey"], 0);
        }

    }
}
