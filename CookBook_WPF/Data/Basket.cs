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
    [Table("tbl_Basket")]
    public class Basket
    {
        [Key]
        public int nKey { get; set; }
        public DateTime tDate { get; set; }
        public string szDescription { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
    }
}
