using System.Collections.Generic;
using System.Linq;
using PetStoreDAL.Models;
using PetStoreDAL.DBContext;
using System.Data.Entity;

namespace PetStoreDAL.Repository
{
    public class PetRepository :IPetRepository
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
        public IEnumerable<PetDetails> SortByPetType(string type)
        {
            return db.petdetails.Where(p =>p.pet.PetType==type).ToList();
        }
        /// <summary>
        /// To sort according to breed
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<PetDetails> SortByBreed(string type)
        {
            return db.petdetails.Where(p => p.BreedType==type).ToList();
        }
        /// <summary>
        /// Delete pet record
        /// </summary>
        /// <param name="id"></param>
        public void DeletePetRecord(int id)
        {
            var pet = db.petdetails.Where(p => p.PetId==id).FirstOrDefault();
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
    }
}
