using CookBook_WPF.ViewModel;

namespace CookBook_WPF.Helper_Classes.DataWrappers
{
    public class MeasureProductWrapper : BindableBase
    {
        public int MeasureProductKey { get; set; }

        private double? mProportion;
        public double? Proportion { get { return mProportion; } set { mProportion = value; IsChanged = true; OnPropertyChanged(); } }

        private double? mCurrentMeasureQuantity;
        public double? CurrentMeasureQuantity
        {
            get { return mCurrentMeasureQuantity; }
            set
            {
                mCurrentMeasureQuantity = value;
                if (mCurrentMeasureQuantity != null && mMainMeasureQuantity != null)
                {
                    IsChanged = true;
                    Proportion = mCurrentMeasureQuantity / MainMeasureQuantity;
                }

            }
        }

        private double? mMainMeasureQuantity;
        public double? MainMeasureQuantity
        {
            get { return mMainMeasureQuantity; }
            set
            {
                mMainMeasureQuantity = value;
                if (mCurrentMeasureQuantity != null && mMainMeasureQuantity != null)
                {
                    IsChanged = true;
                    Proportion = mCurrentMeasureQuantity / MainMeasureQuantity;
                }
            }
        }


        public int MeasureKey { get; set; }
        public string MeasureName { get; set; }

        public string MainMeasureName { get; set; }
        public int MainMeasureKey { get; set; }

        public bool IsChanged { get; set; }
        private bool mIsSaved;
        public bool IsSaved
        {
            get { return mIsSaved; }
            set
            {
                mIsSaved = value;
                OnPropertyChanged();
            }
        }

    }
}
