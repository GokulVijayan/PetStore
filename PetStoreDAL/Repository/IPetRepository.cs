using PetStoreDAL.Models;
using System.Collections.Generic;

namespace PetStoreDAL.Repository
{
    public interface IPetRepository
    {
        void SaveDetails(PetDetails pdd);
        IEnumerable<PetDetails> FindAll();
        void Save();
        IEnumerable<Pet> GetType();
        IEnumerable<PetDetails> SortByPetType(string type);
        IEnumerable<PetDetails> SortByBreed(string type);
        void DeletePetRecord(int id);
        int GetPetId(string petName, string breedType);
        PetDetails GetPetById(int id);
        void EditPet(PetDetails pd);
    }
}
