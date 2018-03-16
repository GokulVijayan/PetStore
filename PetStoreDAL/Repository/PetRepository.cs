using System.Collections.Generic;
using System.Linq;
using PetStoreDAL.Models;
using PetStoreDAL.DBContext;
using System.Data.Entity;
using System;
using Common;

namespace PetStoreDAL.Repository
{
    public class PetRepository : IPetRepository
    {
        PetDbContext db = new PetDbContext();
        /// <summary>
        /// To find all pet records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PetDetails> FindAll()
        {
            return db.petdetails.ToList();
        }
        /// <summary>
        /// To save pet record
        /// </summary>
        /// <param name="pdd"></param>
        public void SaveDetails(PetDetails pdd)
        {
            db.petdetails.Add(pdd);
            Save();
        }
        public void Save()
        {
            db.SaveChanges();
        }
        IEnumerable<Pet> IPetRepository.GetType()
        {
            return db.pet.ToList();
        }
        /// <summary>
        /// To sort pets according to pettype
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<PetDetails> SortByPetType(string type, Page page, out int totalCount)
        {
            var petId = Convert.ToInt32(type);
            var pett = db.petdetails.Where(p => p.pet.TypeId == petId).OrderBy(p => p.PetName).Skip(page.PageSize * page.PageNumber).Take(page.PageSize).ToList();
            totalCount = db.petdetails.Count();
            return pett;
        }
        /// <summary>
        /// Delete pet record
        /// </summary>
        /// <param name="id"></param>
        public void DeletePetRecord(int id)
        {
            var pet = db.petdetails.Where(p => p.PetId == id).FirstOrDefault();
            db.petdetails.Remove(pet);
            Save();
        }
        /// <summary>
        /// To get petid
        /// </summary>
        /// <param name="petName"></param>
        /// <param name="breedType"></param>
        /// <returns></returns>
        public int GetPetId(string petName, string breedType)
        {
            var id = db.petdetails.Where(p => p.PetName == petName && p.BreedType == breedType).Select(p => p.PetId).FirstOrDefault();
            return id;

        }
        /// <summary>
        /// To return petdetails according to id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PetDetails GetPetById(int id)
        {
            return db.petdetails.Find(id);
        }
        /// <summary>
        /// To edit pet record
        /// </summary>
        /// <param name="pd"></param>
        public void EditPet(PetDetails pd)
        {
            db.Entry(pd).State = EntityState.Modified;
            Save();
        }
        public IEnumerable<PetDetails> GetPetDetails(string pettype, string breedtype, string age, string price, Page pp, out int totalcount)
        {

            var typeId = Convert.ToInt32(pettype);
            if (string.IsNullOrEmpty(age) && string.IsNullOrEmpty(price))
            {
                var pet = db.petdetails.Where(p => p.pet.TypeId == typeId && p.BreedType == breedtype).OrderBy(p => p.PetName).Skip(pp.PageSize * pp.PageNumber).Take(pp.PageSize).ToList();
                totalcount = db.petdetails.Count();
                return pet;
            }
            else if (string.IsNullOrEmpty(breedtype) && string.IsNullOrEmpty(age))
            {
                var petPrice = Convert.ToSingle(price);
                var pet = db.petdetails.Where(p => p.pet.TypeId == typeId && p.Price <= petPrice).OrderBy(b => b.PetName).Skip(pp.PageSize * pp.PageNumber).Take(pp.PageSize).ToList();
                totalcount = db.petdetails.Count();
                return pet;
            }
            else if (string.IsNullOrEmpty(breedtype) && string.IsNullOrEmpty(price))
            {
                var petAge = Convert.ToInt32(age);
                var pet = db.petdetails.Where(p => p.pet.TypeId == typeId && p.Age <= petAge).OrderBy(b => b.PetName).Skip(pp.PageSize * pp.PageNumber).Take(pp.PageSize).ToList();
                totalcount = db.petdetails.Count();
                return pet;

            }
            else if (string.IsNullOrEmpty(age))
            {
                var petPrice = Convert.ToSingle(price);
                var pet = db.petdetails.Where(p => p.pet.TypeId == typeId && p.BreedType == breedtype && p.Price <= petPrice).OrderBy(b => b.PetName).Skip(pp.PageSize * pp.PageNumber).Take(pp.PageSize).ToList();
                totalcount = db.petdetails.Count();
                return pet;

            }
            else if (string.IsNullOrEmpty(breedtype))
            {
                var petAge = Convert.ToInt32(age);
                var petPrice = Convert.ToSingle(price);
                var pet = db.petdetails.Where(p => p.pet.TypeId == typeId && p.Age<=petAge && p.Price <= petPrice).OrderBy(b => b.PetName).Skip(pp.PageSize * pp.PageNumber).Take(pp.PageSize).ToList();
                totalcount = db.petdetails.Count();
                return pet;

            }
            else if (string.IsNullOrEmpty(price))
            {
                var petAge = Convert.ToInt32(age);
                var pet = db.petdetails.Where(p => p.pet.TypeId == typeId && p.BreedType == breedtype && p.Age <= petAge).OrderBy(b => b.PetName).Skip(pp.PageSize * pp.PageNumber).Take(pp.PageSize).ToList();
                totalcount = db.petdetails.Count();
                return pet;
            }
            else
            {
                var petAge = Convert.ToInt32(age);
                var petPrice = Convert.ToSingle(price);
                var pet = db.petdetails.Where(p => p.pet.TypeId == typeId && p.Price <= petPrice && p.Age <= petAge && p.BreedType == breedtype).OrderBy(b => b.PetName).Skip(pp.PageSize * pp.PageNumber).Take(pp.PageSize).ToList(); ;
                totalcount = db.petdetails.Count();
                return pet;
            }
        }
    }
}
