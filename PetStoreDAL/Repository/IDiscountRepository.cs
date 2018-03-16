using PetStoreDAL.Models;
using System.Collections.Generic;

namespace PetStoreDAL.Repository
{
    public interface IDiscountRepository
    {
        IEnumerable<Pet> GetPetType();
        void SaveDetails(Discount disount);
        void Save();
        IEnumerable<Discount> FindAll();
        void DeleteDiscount(string pettype, string discount);
    }
}
