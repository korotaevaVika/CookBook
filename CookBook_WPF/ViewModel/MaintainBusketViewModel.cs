using CookBook_WPF.DataAccess;
using CookBook_WPF.Helper_Classes;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using CookBook_WPF.Helper_Classes.DataWrappers;
using System.Data;

namespace CookBook_WPF.ViewModel
{
    public class MaintainBusketViewModel : BindableBase
    {
        private MainModel _model;
        internal List<int> plansIndexes;

        private DateTime mSelectedDate;
        public DateTime SelectedDate
        {
            get { return mSelectedDate; }
            set { mSelectedDate = value; OnPropertyChanged(); }
        }
        private string mDescription;
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; OnPropertyChanged(); }
        }
        private DataTable mBuskets;
        public DataTable Buckets
        {
            get { return mBuskets; }
            set { mBuskets = value; OnPropertyChanged(); }
        }
        private DataRowView mSelectedBasket;
        public DataRowView SelectedBasket
        {
            get { return mSelectedBasket; }
            set { mSelectedBasket = value; OnPropertyChanged(); }
        }

        #region Commands
        private readonly RelayCommand mNewBusketCommand;
        public ICommand NewBusketCommand { get { return mNewBusketCommand; } }
        private readonly RelayCommand mChooseBusketCommand;
        public ICommand ChooseBusketCommand { get { return mChooseBusketCommand; } }

        //private readonly RelayCommand mSaveCommand;
        //public ICommand SaveCommand { get { return mSaveCommand; } }
        private readonly RelayCommand mBackCommand;
        public ICommand BackCommand { get { return mBackCommand; } }
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
        private string mUserInput;
        public string UserInput
        {
            get { return mUserInput; }
            set
            {
                mUserInput = value;
                OnPropertyChanged();
            }
        }
        private bool mCancel;

        public bool Cancel
        {
            get { return mCancel; }
            set
            {
                mCancel = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public MaintainBusketViewModel()
        {
            _model = new MainModel();
            mNewBusketCommand = new RelayCommand(MaintainBusket, CanMaintainBusket);
            mChooseBusketCommand = new RelayCommand(ChooseBusket, CanChooseBusket);
            //mSaveCommand = new RelayCommand(Save, CanSave);
            mBackCommand = new RelayCommand(Back);
            SelectedDate = DateTime.Today;
            Description = null;
            Buckets = new DataTable();
            LoadData();
        }

        private void Back(object obj)
        {
            UserInput = "Изменения не сохранены. Планы не добавлены в корзину";
        }

        #region Methods For Commands
        //private void Save(object obj)
        //{
        //    bool mSuccess = false;
        //    mSuccess = true;
        //    //Message = DateTime.Now.ToString() + "\t" +
        //    //    _model.SaveBasket(
        //    //        plansIndexes,
        //    //        basketKey,
        //    //        ref mSuccess);

        //    if (mSuccess)
        //    {
        //        LoadData();
        //        UserInput = "Success";
        //    }
        //    else UserInput = "Error";

        //}
        //private bool CanSave(object obj)
        //{
        //    return true;
        //}

        private bool CanChooseBusket(object obj)
        {
            return true;
        }

        private void ChooseBusket(object obj)
        {
            bool mSuccess = false;
            UserInput = _model.SaveBasket(
                (int)SelectedBasket["BasketKey"],
                (DateTime)SelectedBasket["Date"],
                (string)SelectedBasket["Description"],
                plansIndexes, ref mSuccess);

        }

        private bool CanMaintainBusket(object obj)
        {
            return true;
        }

        private void MaintainBusket(object obj)
        {
            bool mSuccess = false;
            UserInput = _model.SaveBasket(0, SelectedDate, Description, plansIndexes, ref mSuccess);
        }
        #endregion

        private void LoadData()
        {
            try
            {
                Buckets = _model.GetBuskets();
            }
            catch { }
        }
    }
}
