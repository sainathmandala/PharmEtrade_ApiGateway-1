using BAL.ViewModel;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IProduct
    {

        public interface IProductHelper
        {

            Task<Productviewmodel> InsertAddProduct(Productviewmodel productviewmodel);
                    
                
        }
    }
}
