using Common;
using PetStoreDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreBL.Services
{
    public interface IDiscountService
    {
        IEnumerable<PetDto> GetType();
        void Save(DiscountDto pdd);
        IEnumerable<DiscountDto> FindAll();
        void DeleteDiscount(string pettype, string discount);
    }
}
