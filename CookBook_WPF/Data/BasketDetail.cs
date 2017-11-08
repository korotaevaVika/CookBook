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
    [Table("tbl_BasketDetail")]

    public class BasketDetail
    {
        [Key]
        public int nKey { get; set; }
        [Required]
        public virtual Basket nBasket { get; set; }
        [Required]
        public virtual Product nProduct { get; set; }
        [Required]
        public virtual Measure nMeasure { get; set; }
        [Required]
        public double rQuantity { get; set; }
        public bool bEdited { get; set; }

    }
}
