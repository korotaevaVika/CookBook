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
    [Table("tbl_MaterialGroup")]
    public class MaterialGroup
    {
        [Key]
        public int nKey { get; set; }
        [Required]
        [StringLength(255)]
        public string szGroupName { get; set; }
        public bool bContainsFinishedProduct { get; set; }
    }
}
