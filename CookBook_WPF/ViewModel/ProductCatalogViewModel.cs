﻿using CookBook_WPF.DataAccess;
using CookBook_WPF.Helper_Classes;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Windows;
using CookBook_WPF.View;
using MahApps.Metro.Controls.Dialogs;

namespace CookBook_WPF.ViewModel
{
    public class ProductCatalogViewModel : BindableBase
    {
        private MainModel _model;
        private DataView mMaterialGroups;
        public DataView MaterialGroups
        {
            get { return mMaterialGroups; }
            set
            {
                mMaterialGroups = value;
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
                    EditGroup(null);
                }
                mDeleteGroupCommand.OnCanExecuteChanged();
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
                    EditProduct(null);
                }
                mDeleteProductCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        #region Commands
        private readonly RelayCommand mAddGroupCommand;
        private readonly RelayCommand mEditGroupCommand;
        private readonly RelayCommand mDeleteGroupCommand;
        private readonly RelayCommand mSaveGroupCommand;

        private readonly RelayCommand mAddProductCommand;
        private readonly RelayCommand mEditProductCommand;
        private readonly RelayCommand mDeleteProductCommand;
        private readonly RelayCommand mSaveProductCommand;

        private readonly RelayCommand mSaveCommand;

        public ICommand AddGroupCommand { get { return mAddGroupCommand; } }
        public ICommand EditGroupCommand { get { return mEditGroupCommand; } }
        public ICommand DeleteGroupCommand { get { return mDeleteGroupCommand; } }
        public ICommand SaveGroupCommand { get { return mSaveGroupCommand; } }
        public ICommand AddProductCommand { get { return mAddProductCommand; } }
        public ICommand EditProductCommand { get { return mEditProductCommand; } }
        public ICommand DeleteProductCommand { get { return mDeleteProductCommand; } }
        public ICommand SaveProductCommand { get { return mSaveProductCommand; } }

        public ICommand SaveCommand { get { return mSaveCommand; } }
        #endregion

        #region Material Group Properties
        private bool mIsGroupEdited;
        private int mGroupKey;
        private string mGroupName;
        private bool mIsContainsFinishedProducts;
        public bool IsGroupEdited
        {
            get { return mIsGroupEdited; }
            set
            {
                mIsGroupEdited = value;
                OnPropertyChanged();
            }
        }
        public int GroupKey
        {
            get { return mGroupKey; }
            set
            {
                mGroupKey = value;
                OnPropertyChanged();
            }
        }
        public string GroupName
        {
            get { return mGroupName; }
            set
            {
                mGroupName = value;
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

        private string mDefaultMeasure;
        public string DefaultMeasure
        {
            get { return mDefaultMeasure; }
            set
            {
                mDefaultMeasure = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Product Properties
        private int mProductKey;
        private bool mIsProductEdited;
        private bool mAutoCountEnergy;
        private string mProductName;
        private double mProtein;
        private double mFat;
        private double mCarbohydrates;
        private double mEnergy;

        public bool IsProductEdited
        {
            get { return mIsProductEdited; }
            set
            {
                mIsProductEdited = value;
                OnPropertyChanged();
            }
        }
        public bool AutoCountEnergy
        {
            get { return mAutoCountEnergy; }
            set
            {
                mAutoCountEnergy = value;
                if (mAutoCountEnergy)
                {
                    UpdateEnergy();
                }
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
        public double Protein
        {
            get { return mProtein; }
            set
            {
                mProtein = value;
                UpdateEnergy();
                OnPropertyChanged();
            }
        }
        public double Fat
        {
            get { return mFat; }
            set
            {
                mFat = value;
                UpdateEnergy();
                OnPropertyChanged();
            }
        }
        public double Carbohydrates
        {
            get { return mCarbohydrates; }
            set
            {
                mCarbohydrates = value;
                UpdateEnergy();
                OnPropertyChanged();
            }
        }
        public double Energy
        {
            get { return mEnergy; }
            set
            {
                mEnergy = value;
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

        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly MeasureProductRelationControl _dialogView = new MeasureProductRelationControl();

        public ProductCatalogViewModel()
        {
            _dialogCoordinator = new DialogCoordinator();
            // _dialogView = new MeasureProductRelationControl();

            _model = new MainModel();
            mMaterialGroups = _model.GetMaterialGroups();
            mAddGroupCommand = new RelayCommand(AddGroup);
            mEditGroupCommand = new RelayCommand(EditGroup);
            mDeleteGroupCommand = new RelayCommand(DeleteGroup, CanDeleteGroup);
            mSaveGroupCommand = new RelayCommand(SaveGroup, CanSaveGroup);
            mAddProductCommand = new RelayCommand(AddProduct);
            mEditProductCommand = new RelayCommand(EditProduct);
            mDeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
            mSaveProductCommand = new RelayCommand(SaveProduct, CanSaveProduct);
            mSaveCommand = new RelayCommand(Save);
            IsGroupEdited = false;
            IsProductEdited = false;

        }

        private string _dialogResult;
        public string DialogResult
        {
            get { return _dialogResult; }
            set
            {
                if (_dialogResult == value)
                {
                    return;
                }
                _dialogResult = value;
                OnPropertyChanged();
            }
        }

        private void ShowDialog()
        {
            //new MeasureProductRelationViewModel.MeasureProductRelationInfo
            //{
            //    productKey = 7,
            //    mainMeasureValueKey = 7,
            //    ParentViewModel = new ProductCatalogViewModel()
            //};
            var vr = string.Empty;
            MeasureProductRelationControl inputDialog = new MeasureProductRelationControl();
            if (inputDialog.ShowDialog() == true)
                vr = inputDialog.Answer;

        }

        public void ProcessUserInput(string input_message)
        {
            Console.WriteLine("Users firstname is " + input_message);

        }

        #region Methods For Commands
        private void Save(object obj)
        {
            if (IsGroupEdited && CanSaveGroup(null))
            {
                SaveGroup(null);
            }
            else if (IsProductEdited && CanSaveProduct(null))
            {
                SaveProduct(null);
            }
        }
        private bool CanSaveGroup(object obj)
        {
            return true;
        }

        private void SaveGroup(object obj)
        {
            bool mSuccess = false;
            Message = DateTime.Now.ToString() + "\t" +
                _model.SaveMaterialGroup(
                GroupKey,
                GroupName,
                IsContainsFinishedProducts,
                ref mSuccess);

            if (mSuccess)
            {
                IsGroupEdited = false;
                MaterialGroups = _model.GetMaterialGroups();
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
            Message = DateTime.Now.ToString() + "\t" +
                _model.SaveMaterial(
                ProductKey,
                ProductName,
                Protein,
                Fat,
                Carbohydrates,
                Energy,
                (int)SelectedGroup.Row["GroupKey"],
                 ref mSuccess);

            if (mSuccess)
            {
                IsProductEdited = false;
                LoadProducts();
            }
            else MessageBox.Show(Message, "Error");

        }
        private bool CanDeleteGroup(object obj)
        {
            return SelectedGroup != null;
        }
        private bool CanDeleteProduct(object obj)
        {
            return SelectedProduct != null;
        }
        private void AddGroup(object obj)
        {
            IsGroupEdited = true;
            IsProductEdited = false;
            GroupKey = 0;
            GroupName = null;
            IsContainsFinishedProducts = false;
            
        }

        private void EditGroup(object obj)
        {
            IsProductEdited = false;
            IsGroupEdited = true;
            GroupKey = (int)SelectedGroup.Row["GroupKey"];
            GroupName = (string)SelectedGroup.Row["GroupName"];
            IsContainsFinishedProducts = (bool)SelectedGroup.Row["ContainsFinishedProduct"];
        }

        private void DeleteGroup(object obj)
        {
            bool mSuccess = false;
            Message = _model.DeleteMaterialGroup((int)SelectedGroup.Row["GroupKey"], ref mSuccess);
            MaterialGroups = _model.GetMaterialGroups();
            if (mSuccess)
            {
                IsGroupEdited = false;
            }
            else MessageBox.Show(Message, "Error");

        }

        private void AddProduct(object obj)
        {
            IsProductEdited = true;
            IsGroupEdited = false;
            ProductKey = 0;
            ProductName = null;
            AutoCountEnergy = false;
            Protein = 0;
            Fat = 0;
            Carbohydrates = 0;
            Energy = 0;
            DefaultMeasure = string.Empty;
        }

        private void EditProduct(object obj)
        {
            IsProductEdited = true;
            IsGroupEdited = false;
            ProductKey = (int)SelectedProduct.Row["ProductKey"];
            ProductName = (string)SelectedProduct.Row["ProductName"];
            AutoCountEnergy = false;
            Protein = (double)SelectedProduct.Row["Protein"];
            Fat = (double)SelectedProduct.Row["Fat"];
            Carbohydrates = (double)SelectedProduct.Row["Carbohydrates"];
            if (!AutoCountEnergy)
            {
                Energy = (double)SelectedProduct.Row["Energy"];
            }

            try
            {
                DefaultMeasure = (string)_model.GetMeasures(ProductKey, 1)[0].Row["MeasureName"];
            }
            catch { DefaultMeasure = string.Empty; }
        }

        private void DeleteProduct(object obj)
        {
            bool mSuccess = false;
            Message = _model.DeleteMaterial((int)SelectedProduct.Row["ProductKey"], ref mSuccess);

            if (mSuccess)
            {
                IsProductEdited = false;
            }
            else MessageBox.Show(Message, "Error");
        }

        #endregion

        #region Other Methods
        private void UpdateEnergy()
        {
            if (AutoCountEnergy)
            {
                Energy = _model.CountEnergyValue(
                   mProtein,
                   mFat,
                   mCarbohydrates);
                ShowDialog();

            }

        }
        #endregion
        private void LoadProducts()
        {
            Products = _model.GetProducts((int)SelectedGroup.Row["GroupKey"]);
        }

    }
}
