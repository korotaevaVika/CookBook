namespace CookBook_WPF.Helper_Classes.DataWrappers
{
    public class MeasureProductWrapper
    {
        public int IngredientKey { get; set; }
        public double? Proportion { get; set; }
        public double? CurrentMeasureQuantity { get; set; }
        public double? MainMeasureQuantity { get; set; }
        public int MeasureKey { get; set; }
        public string MeasureName { get; set; }
        public bool IsChanged { get; set; }
        public bool IsSaved { get; set; }
    }
}
