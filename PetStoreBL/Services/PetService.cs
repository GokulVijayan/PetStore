﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using PetStoreDAL.Models;
using PetStoreDAL.Repository;

namespace PetStoreBL.Services
{
    public class PetService:IPetService
    {
        private readonly IPetRepository petRepository;
        public PetService(IPetRepository petRepo)
        {
            petRepository = petRepo;
        }
        /// <summary>
        /// To delete pet record
        /// </summary>
        /// <param name="id"></param>
        public void DeletePetRecord(int id)
        {
            petRepository.DeletePetRecord(id);
        }
        public void EditPet(PetDetailsDto pd,int petId)
        {
            PetDetails pett = new PetDetails
            {
                Age = pd.Age,
                PetId=petId,
                ImagePath = pd.ImagePath,
                BreedType = pd.BreedType,
                PetName = pd.PetName,
                Price = pd.Price,
                TypeId =Convert.ToInt32(pd.PetType),
                Gender=pd.Gender

                
            };
            petRepository.EditPet(pett);
        }
        public IEnumerable<PetDetailsDto> FindAll()
        {
            IEnumerable<PetDetails>petdetails = petRepository.FindAll();
            IEnumerable<PetDetailsDto> pt =from g in petdetails select new PetDetailsDto
            {
                Age=g.Age,
                BreedType=g.BreedType,
                PetName=g.PetName,
                ImagePath=g.ImagePath,
                Price=g.Price,
                PetType=g.pet.PetType,
                Gender=g.Gender

            };
            return pt.ToList();
        }
        /// <summary>
        /// To get pet details according to id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PetDetailsDto GetPetById(int id)
        {
            PetDetails petdetails = petRepository.GetPetById(id);
            PetDetailsDto pett = new PetDetailsDto
            {
                Age = petdetails.Age,
                ImagePath = petdetails.ImagePath,
                BreedType = petdetails.BreedType,
                PetName = petdetails.PetName,
                Price = petdetails.Price,
                PetType=petdetails.pet.PetType,
                Gender=petdetails.Gender
            };
            return pett;
        }
        /// <summary>
        /// To get pet by petname and breedtype
        /// </summary>
        /// <param name="petName"></param>
        /// <param name="breedType"></param>
        /// <returns></returns>
        public int GetPetId(string petName, string breedType)
        {
            var id = petRepository.GetPetId(petName, breedType);
            return id;
        }
        public void Save(PetDetailsDto pet)
        {
            PetDetails pett = new PetDetails
            {
                Age = pet.Age,
                ImagePath = pet.ImagePath,
                BreedType = pet.BreedType,
                PetName = pet.PetName,
                Price = pet.Price,
                TypeId = Convert.ToInt32(pet.PetType),
                Gender=pet.Gender
                
            };
            petRepository.SaveDetails(pett);
        }
        /// <summary>
        /// To sort pet by pet type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<PetDetailsDto> SortByPetType(string type,Page page, out int totalCount)
        {
            IEnumerable<PetDetails> petdetails = petRepository.SortByPetType(type, page, out totalCount);
            var pet = ListPets(petdetails);
            return pet;
        }
        /// <summary>
        /// To list all pets
        /// </summary>
        /// <param name="petdetails"></param>
        /// <returns></returns>
        IEnumerable <PetDetailsDto>ListPets(IEnumerable<PetDetails> petdetails)
        {
            IEnumerable<PetDetailsDto> pt = from g in petdetails
                                            select new PetDetailsDto
                                            {
                                                Age = g.Age,
                                                BreedType = g.BreedType,
                                                PetName = g.PetName,
                                                ImagePath = g.ImagePath,
                                                Price = g.Price,
                                                PetType = g.pet.PetType,
                                                Gender=g.Gender
                                            };
            return pt.ToList();
        }
        /// <summary>
        /// To get all pet types
        /// </summary>
        /// <returns></returns>
        IEnumerable<PetDto> IPetService.GetType()
        {
            IEnumerable<Pet> petdetails = petRepository.GetType();
            IEnumerable<PetDto> pt = from g in petdetails
                                            select new PetDto
                                            {
                                                PetType=g.PetType,
                                                TypeId=g.TypeId
                                            };
            return pt.ToList();

        }
        public IEnumerable<PetDetailsDto> GetPetDetails(string pettype, string breedtype, string age, string price,Page p,out int totalcount)
        {
            IEnumerable<PetDetails> petdetails = petRepository.GetPetDetails(pettype,breedtype,age,price,p,out totalcount);
            var pet = ListPets(petdetails);
            return pet;
        } 
    }
}
