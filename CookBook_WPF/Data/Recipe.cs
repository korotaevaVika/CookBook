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
    [Table("tbl_Recipe")]
    public class Recipe
    {
        [Key]
        public int nKey { get; set; }
        [Required]
        [StringLength(255)]
        public string szRecipeName { get; set; }
        public virtual Product nProduct { get; set; }
        public double rPortion { get; set; }
        public double rQuantity { get; set; }
        public string szDescription { get; set; }
    }
}
