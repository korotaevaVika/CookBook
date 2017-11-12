using CookBook_WPF.DataAccess;
using CookBook_WPF.Helper_Classes;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Windows;
using CookBook_WPF.Data;
using System.Collections.Generic;
using System.Linq;

namespace CookBook_WPF.ViewModel
{
    public class RecipeCatalogViewModel : BindableBase
    {
        private MainModel _model;

        #region OutputProductsCollection
        private List<Product> mOutputProductsCollection;
        public List<Product> OutputProductsCollection
        {
            get { return mOutputProductsCollection; }
            set
            {
                mOutputProductsCollection = value;
                OnPropertyChanged();
            }
        }
        #endregion

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

        //private DataView mProducts;
        //public DataView Products
        //{
        //    get { return mProducts; }
        //    set
        //    {
        //        mProducts = value;
        //        LoadMeasures();
        //        OnPropertyChanged();
        //    }
        //}

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
                    EditRecipe(null);
                }
                //mDeleteRecipeCommand.OnCanExecuteChanged();
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
                    //  LoadMeasures();
                }
                //mDeleteIngredientCommand.OnCanExecuteChanged();
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
                    //LoadMeasures();
                }
                //mDeleteIngredientCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        #region Commands
        private readonly RelayCommand mAddRecipeCommand;
        private readonly RelayCommand mEditRecipeCommand;
        private readonly RelayCommand mDeleteRecipeCommand;
        private readonly RelayCommand mSaveRecipeCommand;

        private readonly RelayCommand mAddIngredientCommand;
        private readonly RelayCommand mEditIngredientCommand;
        private readonly RelayCommand mDeleteIngredientCommand;
        private readonly RelayCommand mSaveIngredientCommand;

        private readonly RelayCommand mSaveCommand;

        public ICommand AddRecipeCommand { get { return mAddRecipeCommand; } }
        public ICommand EditRecipeCommand { get { return mEditRecipeCommand; } }
        public ICommand DeleteRecipeCommand { get { return mDeleteRecipeCommand; } }
        public ICommand SaveRecipeCommand { get { return mSaveRecipeCommand; } }
        public ICommand AddIngredientCommand { get { return mAddIngredientCommand; } }
        public ICommand EditIngredientCommand { get { return mEditIngredientCommand; } }
        public ICommand DeleteIngredientCommand { get { return mDeleteIngredientCommand; } }
        public ICommand SaveProductCommand { get { return mSaveIngredientCommand; } }

        public ICommand SaveCommand { get { return mSaveCommand; } }
        #endregion

        #region Recipe Properties
        private bool mIsRecipeEdited;
        private int mRecipeKey;
        private string mRecipeName;
        private string mDescription;
        private double mPortion;
        private double mQuantity;
        private string mMeasure;
        private Product mOutputProduct;

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
        public string Description
        {
            get { return mDescription; }
            set
            {
                mDescription = value;
                OnPropertyChanged();
            }
        }
        public double Portion
        {
            get { return mPortion; }
            set
            {
                mPortion = value;
                OnPropertyChanged();
            }
        }
        public double Quantity
        {
            get { return mQuantity; }
            set
            {
                mQuantity = value;
                OnPropertyChanged();
            }
        }
        public string Measure
        {
            get { return mMeasure; }
            set
            {
                mMeasure = value;
                OnPropertyChanged();
            }
        }

        public Product OutputProduct
        {
            get { return mOutputProduct; }
            set
            {
                mOutputProduct = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Product Properties
        private int mIngredientKey;
        private int mProductKey;
        private string mProductName;
        private double mIngredientQuantity;

        private bool mIsIngredientEdited;

        public bool IsIngredientEdited
        {
            get { return mIsIngredientEdited; }
            set
            {
                mIsIngredientEdited = value;
                OnPropertyChanged();
            }
        }


        public int ProductKey
        {
            get { return mProductKey; }
            set
            {
                mProductKey = value;
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
            LoadOutputProducts();
            mAddRecipeCommand = new RelayCommand(AddRecipe);
            mEditRecipeCommand = new RelayCommand(EditRecipe);
            mDeleteRecipeCommand = new RelayCommand(DeleteRecipe, CanDeleteRecipe);
            mSaveRecipeCommand = new RelayCommand(SaveRecipe, CanSaveRecipe);
            mAddIngredientCommand = new RelayCommand(AddProduct);
            mEditIngredientCommand = new RelayCommand(EditProduct);
            mDeleteIngredientCommand = new RelayCommand(DeleteIngredient, CanDeleteIngredient);
            mSaveIngredientCommand = new RelayCommand(SaveProduct, CanSaveProduct);
            mSaveCommand = new RelayCommand(Save);

            IsRecipeEdited = false;
            IsIngredientEdited = false;
        }

        #region Methods For Commands
        private void Save(object obj)
        {
            if (IsRecipeEdited && CanSaveRecipe(null))
            {
                SaveRecipe(null);
            }
            else if (IsIngredientEdited && CanSaveProduct(null))
            {
                SaveProduct(null);
            }
        }
        private bool CanSaveRecipe(object obj)
        {
            return OutputProduct != null && (Portion != 0 || Quantity != 0);
        }

        private void SaveRecipe(object obj)
        {
            bool mSuccess = false;
            Message = DateTime.Now.ToString() + "\t" +
                _model.SaveRecipe(
                RecipeKey,
                RecipeName,
                OutputProduct.nKey,
                Portion, 
                Quantity,
                Description,
                ref mSuccess);

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
                IsIngredientEdited = false;
            }
            else MessageBox.Show(Message, "Error");

        }
        private bool CanDeleteRecipe(object obj)
        {
            return SelectedRecipe != null;
        }
        private bool CanDeleteIngredient(object obj)
        {
            return SelectedProduct != null;
        }
        private void AddRecipe(object obj)
        {
            IsRecipeEdited = true;
            IsIngredientEdited = false;
            RecipeKey = 0;
            RecipeName = string.Empty;
            OutputProduct = null;
            Portion = 0;
            Quantity = 0;
            Description = string.Empty;
        }

        private void EditRecipe(object obj)
        {
            IsIngredientEdited = false;
            IsRecipeEdited = true;
            RecipeKey = (int)SelectedRecipe.Row["RecipeKey"];
            RecipeName = (string)SelectedRecipe.Row["RecipeName"];
            Description = (string)SelectedRecipe.Row["Description"];
            Portion = (double)SelectedRecipe.Row["Portion"];
            Quantity = (double)SelectedRecipe.Row["Quantity"];

            OutputProduct = OutputProductsCollection.FirstOrDefault(
                x => x.nKey == (int)SelectedRecipe.Row["ProductKey"]);
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
            IsIngredientEdited = true;
            IsRecipeEdited = false;
            mProductKey = 0;
            mIngredientKey = 0;
            ProductName = null;

        }

        private void EditProduct(object obj)
        {
            IsIngredientEdited = true;
            IsRecipeEdited = false;
            mIngredientKey = (int)SelectedProduct.Row["IngredientKey"];
            mProductKey = (int)SelectedProduct.Row["ProductKey"];
            ProductName = (string)SelectedProduct.Row["ProductName"];

        }

        private void DeleteIngredient(object obj)
        {
            bool mSuccess = false;
            //Message = _model.DeleteIngredient((int)SelectedProduct.Row["IngredientKey"], ref mSuccess);

            if (mSuccess)
            {
                IsIngredientEdited = false;
            }
            else MessageBox.Show(Message, "Error");
        }

        #endregion

        #region Other Methods

        #endregion
        //private void LoadProducts()
        //{
        //    Products = _model.GetProducts((int)SelectedGroup.Row["GroupKey"]);
        //}
        private void LoadIngredients()
        {
            //Products = _model.GetIngredients((int)SelectedRecipe.Row["RecipeKey"]);

        }
        private void LoadOutputProducts()
        {
            OutputProductsCollection = _model.GetOutputProducts();
            //MeasureProducts = _model.GetMeasures((int)SelectedProduct.Row["RecipeKey"], 0);
        }
    }
}
