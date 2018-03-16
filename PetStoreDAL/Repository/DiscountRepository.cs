using System;
using System.Collections.Generic;
using System.Linq;
using PetStoreDAL.Models;
using PetStoreDAL.DBContext;

namespace PetStoreDAL.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        PetDbContext db = new PetDbContext();
        public void SaveDetails(Discount disount)
        {
            db.discounts.Add(disount);
            Save();
        }
        IEnumerable<Pet> IDiscountRepository.GetPetType()
        {
            return db.pet.ToList();

        }
        public void Save()
        {
            db.SaveChanges();
        }
        public IEnumerable<Discount> FindAll()
        {
            return db.discounts.ToList();
        }
        public void DeleteDiscount(string pettype, string discount)
        {
            var typeId = db.pet.Where(p => p.PetType == pettype).Select(p => p.TypeId).FirstOrDefault();
            var discountRate = Convert.ToSingle(discount);
            var id = db.discounts.Where(d => d.TypeId == typeId && d.DiscountRate == discountRate).Select(d => d.DiscountId).FirstOrDefault();
            var discountRecord = db.discounts.Where(d => d.DiscountId == id).FirstOrDefault();
            db.discounts.Remove(discountRecord);
            Save();
        }
    }
}
