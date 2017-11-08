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
    [Table("tbl_Measure")]
    public class Measure
    {
        [Key]
        public int nKey { get; set; }
        [Required]
        [StringLength(255)]
        public string szMeasureName { get; set; }
        public bool bIsDefault { get; set; }
    }
}
