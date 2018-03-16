using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using PetStoreDAL.Models;
using PetStoreDAL.Repository;

namespace PetStoreBL.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository discountRepository;
        public DiscountService(IDiscountRepository discountRepo)
        {
            discountRepository = discountRepo;
        }
        public void DeleteDiscount(string pettype, string discount)
        {
            discountRepository.DeleteDiscount(pettype, discount);
        }

        public IEnumerable<DiscountDto> FindAll()
        {
            IEnumerable<Discount> discount = discountRepository.FindAll();
            IEnumerable<DiscountDto> pt = from g in discount
                                            select new DiscountDto
                                            {
                                                DiscountRate=g.DiscountRate,
                                                PetType=g.pet.PetType,
                                                StartDate=g.StartDate,
                                                EndDate=g.EndDate
                                            };
            return pt.ToList();
        }
        public void Save(DiscountDto pdd)
        {
            Discount disount = new Discount
            {
                DiscountRate = pdd.DiscountRate,
                StartDate = pdd.StartDate,
                EndDate = pdd.EndDate,
                TypeId=Convert.ToInt32(pdd.PetType)
            };
            discountRepository.SaveDetails(disount);
        }
        IEnumerable<PetDto> IDiscountService.GetType()
        {
            IEnumerable<Pet> petdetails = discountRepository.GetPetType();
            IEnumerable<PetDto> pt = from g in petdetails
                                     select new PetDto
                                     {
                                         PetType = g.PetType,
                                         TypeId = g.TypeId
                                     };
            return pt.ToList();
        }
    }
}
