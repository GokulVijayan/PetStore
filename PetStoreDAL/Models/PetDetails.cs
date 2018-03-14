using System.ComponentModel.DataAnnotations;

namespace PetStoreDAL.Models
{
    public class PetDetails
    {
        [Key]
        public int PetId { get; set; }
        public string PetName { get; set; }
        public string ImagePath { get; set; }
        public int Age { get; set; }
        public float Price { get; set; }
        public string BreedType { get; set; }
        public string Gender { get; set; }

        public int TypeId { get; set; }
        public virtual Pet pet { get; set; }


    }
}
