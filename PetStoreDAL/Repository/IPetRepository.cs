using Common;
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
        IEnumerable<PetDetails> SortByPetType(string type,Page page, out int totalCount);
        void DeletePetRecord(int id);
        int GetPetId(string petName, string breedType);
        PetDetails GetPetById(int id);
        void EditPet(PetDetails pd);
        IEnumerable<PetDetails> GetPetDetails(string pettype, string breedtype, string age, string price,Page p,out int totalcount);
    }
}
