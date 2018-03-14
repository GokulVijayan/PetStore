﻿using Common;
using System.Collections.Generic;


namespace PetStoreBL.Services
{
    public interface IPetService
    {
        IEnumerable<PetDetailsDto>FindAll();
        void Save(PetDetailsDto pdd);
        IEnumerable<PetDto> GetType();
        IEnumerable<PetDetailsDto> SortByPetType(string type);
        IEnumerable<PetDetailsDto> SortByBreed(string type);
        void DeletePetRecord(int id);
        int GetPetId(string petName, string breedType);
        PetDetailsDto GetPetById(int id);
        void EditPet(PetDetailsDto pd,int petId);
    }
}