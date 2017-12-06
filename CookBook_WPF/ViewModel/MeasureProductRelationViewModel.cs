using CookBook_WPF.DataAccess;
using CookBook_WPF.Helper_Classes;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using CookBook_WPF.Helper_Classes.DataWrappers;

namespace CookBook_WPF.ViewModel
{
    public class MeasureProductRelationViewModel : BindableBase
    {
        public class MeasureProductRelationInfo
        {
            public int productKey { get; set; }
            public int mainMeasureValueKey { get; set; }
            public string mainMeasureValueName { get; set; }
            public BindableBase ParentViewModel { get; set; }
        }
        private MeasureProductRelationInfo _info;
        public MeasureProductRelationInfo Info
        {
            get { return _info; }
            set
            {
                _info = value;
                if (value != null)
                {
                    LoadData();
                }
                OnPropertyChanged();
            }
        }
        private MainModel _model;
        private List<MeasureProductWrapper> mMeasureProductWrappers;
        public List<MeasureProductWrapper> MeasureProductWrappers
        {
            get { return mMeasureProductWrappers; }
            set
            {
                mMeasureProductWrappers = value;
                OnPropertyChanged();
            }
        }
        public string MainMeasureValueName { get { return _info?.mainMeasureValueName; } }
        private MeasureProductWrapper mSelectedMeasureProductWrapper;
        public MeasureProductWrapper SelectedMeasureProductWrapper
        {
            get { return mSelectedMeasureProductWrapper; }
            set
            {
                mSelectedMeasureProductWrapper = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        private readonly RelayCommand mSaveCommand;

        public ICommand SaveCommand { get { return mSaveCommand; } }
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
        public MeasureProductRelationViewModel()
        {
            _model = new MainModel();
            mSaveCommand = new RelayCommand(Save);
        }

        #region Methods For Commands
        private void Save(object obj)
        {
            bool mSuccess = false;
            mSuccess = true;
            Message = DateTime.Now.ToString() + "\t" +
                _model.SaveMeasureProductRelations(
                    MeasureProductWrappers,
                    Info.productKey,
                    ref mSuccess);

            if (mSuccess)
            {
                LoadData();
                UserInput = "Success";
            }
            else UserInput = "Error";

        }

        #endregion

        private void LoadData()
        {
            try
            {
                bool mSuccess = false;
                mSuccess = true;
                string mErrorMessage = null;
                MeasureProductWrappers = _model.GetMeasureRelations(_info.productKey, ref mErrorMessage, ref mSuccess);
                if (!mSuccess)
                {
                    Message = DateTime.Now.ToString() + "\t" + mErrorMessage;
                }
            }
            catch { }

        }

    }
}
