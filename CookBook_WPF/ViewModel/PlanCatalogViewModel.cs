using CookBook_WPF.DataAccess;
using CookBook_WPF.Helper_Classes;
using System.Data;
using System.Windows.Input;
using System;
using System.Windows;
using CookBook_WPF.Data;
using System.Collections.Generic;
using System.Linq;
using CookBook_WPF.View;
using System.Collections.ObjectModel;
using CookBook_WPF.Helper_Classes.DataWrappers;

namespace CookBook_WPF.ViewModel
{
    public class PlanCatalogViewModel : BindableBase
    {
        private MainModel _model;
        private List<PlanWrapper> mPlans;

        private List<int> selectedIndexes;
        public List<PlanWrapper> Plans
        {
            get { return mPlans; }
            set { mPlans = value; OnPropertyChanged(); }
        }

        private PlanWrapper mSelectedPlan;
        public PlanWrapper SelectedPlan
        {
            get { return mSelectedPlan; }
            set
            {
                mSelectedPlan = value;
                if (value != null)
                {
                    EditPlan(null);
                }
                if (SelectedPlan != null && SelectedPlan.HasBasket == 1)
                {
                    AllowPlanEditing = false;
                }
                else
                { AllowPlanEditing = true; }
                mDeletePlanCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private bool mAllowPlanEditing;
        public bool AllowPlanEditing
        {
            get
            {
                return mAllowPlanEditing;
            }
            set
            {
                mAllowPlanEditing = value;
                OnPropertyChanged();
            }
        }

        private DateTime mDateFrom;
        private DateTime mDateTill;
        private bool mIsPlanEdited;

        public DateTime DateFrom
        {
            get { return mDateFrom; }
            set { mDateFrom = value; OnPropertyChanged(); LoadPlans(); }
        }
        public DateTime DateTill
        {
            get { return mDateTill; }
            set { mDateTill = value; OnPropertyChanged(); LoadPlans(); }
        }
        public bool IsPlanEdited
        {
            get { return mIsPlanEdited; }
            set { mIsPlanEdited = value; OnPropertyChanged(); }
        }


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
        private Product mSelectedProduct;
        public Product SelectedProduct
        {
            get { return mSelectedProduct; }
            set
            {
                mSelectedProduct = value;
                if (value != null)
                {
                    LoadRecipes(SelectedProduct.nKey);
                    SelectedRecipe = mSelectedPlan != null ? RecipesCollection?.FirstOrDefault(
                       x => x.nKey == mSelectedPlan.RecipeKey) :
                       RecipesCollection?.FirstOrDefault();
                }
                OnPropertyChanged();
            }
        }

        private List<Recipe> mRecipesCollection;
        public List<Recipe> RecipesCollection
        {
            get { return mRecipesCollection; }
            set
            {
                mRecipesCollection = value;
                OnPropertyChanged();
            }
        }

        private Recipe mSelectedRecipe;
        public Recipe SelectedRecipe
        {
            get { return mSelectedRecipe; }
            set
            {
                mSelectedRecipe = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void LoadRecipes(int productKey)
        {
            DataTable dt = _model.GetRecipes(productKey)?.Table;
            var test = new List<Recipe>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var obj = new Recipe()
                    {
                        szRecipeName = (string)row["RecipeName"],
                        nKey = (int)row["RecipeKey"],
                        rQuantity = (double)row["Quantity"],
                        rPortion = (double)row["Portion"]
                    };
                    test.Add(obj);
                };
            }
            RecipesCollection = test;
        }

        #region Commands
        private readonly RelayCommand mAddPlanCommand;
        private readonly RelayCommand mEditPlanCommand;
        private readonly RelayCommand mDeletePlanCommand;

        private readonly RelayCommand mSaveCommand;
        private readonly RelayCommand mCreateBusketCommand;

        public ICommand AddPlanCommand { get { return mAddPlanCommand; } }
        public ICommand EditPlanCommand { get { return mEditPlanCommand; } }
        public ICommand DeletePlanCommand { get { return mDeletePlanCommand; } }
        public ICommand SaveCommand { get { return mSaveCommand; } }
        public ICommand CreateBusketCommand { get { return mCreateBusketCommand; } }

        #endregion

        #region Product Properties
        private int mPlanKey;
        private DateTime mDate;
        private double mQuantity;

        public double Quantity
        {
            get { return mQuantity; }
            set
            {
                mQuantity = value;
                mSaveCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get { return mDate; }
            set
            {
                mDate = value;
                mSaveCommand.OnCanExecuteChanged();
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


        public PlanCatalogViewModel()
        {
            _model = new MainModel();

            DateFrom = DateTime.Today.Date;
            DateTill = DateTime.Today.Date.AddDays(1).AddSeconds(-1);
            LoadPlans();
            mAddPlanCommand = new RelayCommand(AddPlan);
            mEditPlanCommand = new RelayCommand(EditPlan);
            mDeletePlanCommand = new RelayCommand(DeletePlan, CanDeletePlan);

            mSaveCommand = new RelayCommand(Save, CanSave);
            mCreateBusketCommand = new RelayCommand(CreateBusket);
            LoadOutputProducts();
            IsPlanEdited = false;
        }

        private void CreateBusket(object obj)
        {
            bool isPlansSelected = CanCreateBusket(null);
            if (isPlansSelected)
            {
                var vr = string.Empty;
                MaintainBusketControl inputDialog = new MaintainBusketControl();
                inputDialog.SetInfo(selectedIndexes);

                if (inputDialog.ShowDialog() == true)
                    vr = inputDialog.Answer;
            }
        }

        private bool CanCreateBusket(object obj)
        {
            selectedIndexes = new List<int>();
            if (Plans != null)
            {
                selectedIndexes = Plans.Where(x => x.IsSelected).Select(x => x.PlanKey).ToList();
                return selectedIndexes.Count() != 0;

            }
            else return false;
        }

        private void LoadPlans()
        {
            Plans = _model.GetPlans(DateFrom, DateTill);
        }

        private bool CanSave(object obj)
        {
            return IsPlanEdited &&
                SelectedProduct != null &&
                Quantity != 0 &&
                SelectedRecipe != null &&
                Date != null;
        }

        private void LoadMeasures()
        {
            DataTable dt = _model.GetMeasures(0, 0)?.Table;
            var test = new List<Measure>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var obj = new Measure()
                    {
                        szMeasureName = (string)row["MeasureName"],
                        nKey = (int)row["MeasureKey"],
                        bIsDefault = (bool)row["IsDefault"]
                    };
                    test.Add(obj);
                };
            }
        }

        #region Methods For Commands
        private void Save(object obj)
        {

            bool mSuccess = false;
            Message = DateTime.Now.ToString() + "\t" +
                _model.SavePlan(
                mPlanKey,
                mSelectedProduct?.nKey ?? 0,
                mSelectedRecipe?.nKey ?? 0,
                Date,
                mQuantity,
                ref mSuccess);

            if (mSuccess)
            {
                IsPlanEdited = false;
                LoadPlans();
            }
            else MessageBox.Show(Message, "Error");

        }

        private bool CanDeletePlan(object obj)
        {
            return SelectedPlan != null;
        }

        private void AddPlan(object obj)
        {
            SelectedPlan = null;
            IsPlanEdited = true;
            mPlanKey = 0;
            SelectedProduct = null;
            RecipesCollection = new List<Recipe>();
            Date = DateTime.Today;
            Quantity = 0;
        }

        private void EditPlan(object obj)
        {
            IsPlanEdited = true;
            //mPlanKey = (int)mSelectedPlan.Row["PlanKey"];
            //SelectedProduct = OutputProductsCollection.FirstOrDefault(
            //    x => x.nKey == (int)mSelectedPlan.Row["ProductKey"]);
            //Quantity = (double)mSelectedPlan.Row["rQuantity"];
            //Date = (DateTime)mSelectedPlan.Row["tDate"];
            mPlanKey = mSelectedPlan.PlanKey;
            SelectedProduct = OutputProductsCollection.FirstOrDefault(
                x => x.nKey == mSelectedPlan.ProductKey);
            Quantity = mSelectedPlan.rQuantity;
            Date = mSelectedPlan.tDate;
        }

        private void DeletePlan(object obj)
        {
            bool mSuccess = false;
            Message = _model.DeletePlan(
                mSelectedPlan.PlanKey,
                ref mSuccess);
            if (mSuccess)
            {
                IsPlanEdited = false;
                LoadPlans();
            }
            else MessageBox.Show(Message, "Error");
        }

        #endregion

        private void LoadOutputProducts()
        {
            OutputProductsCollection = _model.GetOutputProducts();
        }
    }
}
