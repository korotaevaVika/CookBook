using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBook_WPF.Data
{
    [Table("tbl_MeasureProductRelation")]

    public class MeasureProductRelation
    {
        [Key]
        public int nKey { get; set; }
        public virtual Product nProduct { get; set; }
        public virtual Measure nMeasure { get; set; }
        public bool bIsDefault { get; set; }
        [Required]
        public double rQuantity { get; set; }
    }
}
