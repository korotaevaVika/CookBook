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
    [Table("tbl_Product")]

    public class Product
    {
        [Key]
        public int nKey { get; set; }

        [Required]
        [StringLength(255)]
        public string szMaterialName { get; set; }
        public double rProtein { get; set; }
        public double rFat { get; set; }
        public double rCarbohydrate { get; set; }
        public double rEnergy { get; set; }
        [Required]
        public virtual MaterialGroup nMaterialGroup { get; set; }
    }
}
