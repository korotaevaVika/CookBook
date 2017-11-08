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
    [Table("tbl_Plan")]
    public class Plan
    {
        [Key]
        public int nKey { get; set; }
        [Required]
        public virtual Recipe nRecipe { get; set; }
        [Required]
        public virtual Product nProduct { get; set; }
        [Required]
        public double rQuantity { get; set; }
        [Required]
        public DateTime tDate { get; set; }


    }
}
