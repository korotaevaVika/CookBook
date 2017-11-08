using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook_WPF.DataAccess
{
    public class ContextProvider
    {
        public ContextProvider()
        {
        }
        public virtual Data.CookBookModel CreateNew()
        {
            return new Data.CookBookModel();
        }
    }
}
