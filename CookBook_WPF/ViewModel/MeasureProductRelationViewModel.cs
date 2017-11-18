using CookBook_WPF.DataAccess;
using CookBook_WPF.Helper_Classes;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Windows;
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
        public string MainMeasureValueName { get { return _info.mainMeasureValueName; } }

        #region Commands
        private readonly RelayCommand mSaveCommand;
        private readonly RelayCommand mBackToParentViewModelCommand;

        public ICommand SaveCommand { get { return mSaveCommand; } }
        public ICommand BackToParentViewModelCommand { get { return mBackToParentViewModelCommand; } }
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
            // _info = info;

            //mSaveCommand = new RelayCommand(Save);
            mBackToParentViewModelCommand = new RelayCommand(BackToParentViewModel);
            UserInput = "fuy";
        }


        public MeasureProductRelationViewModel(MeasureProductRelationInfo info)
        {
            _model = new MainModel();
            _info = info;

            // mSaveCommand = new RelayCommand(Save);
            mBackToParentViewModelCommand = new RelayCommand(BackToParentViewModel);


        }

        #region Methods For Commands
        private void BackToParentViewModel(object obj)
        {
            //Not strictly MVVM but prefer the simplicity of using code-behind for this

            //switch (e.Key)
            //{

            //    case Key.Enter:
            //        if (this.DataContext != null) (dynamic)this.DataContext.butOK();
            //        break;
            Cancel = true;
        }





        private void Save(object obj)
        {
            bool mSuccess = false;
            mSuccess = true;
            //Message = DateTime.Now.ToString() + "\t" +
            //    _model.SaveMeasureProductRelations(
            //        MeasureProductWrappers,
            //        ref mSuccess);

            if (mSuccess)
            {
                LoadData();
            }
            else MessageBox.Show(Message, "Error");

        }


        #endregion

        #region Other Methods

        #endregion
        private void LoadData()
        {
            MeasureProductWrappers = _model.GetMeasureRelations(_info.productKey);
        }

    }
}
