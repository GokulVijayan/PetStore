using System.ComponentModel.DataAnnotations;


namespace PetStorePL.ViewModel
{
    public class PetDetailsViewModel
    {
        [Required]
        [Display(Name = "Pet Name")]
        public string PetName { get; set; }
        [Display(Name ="Image")]
        [RegularExpression("(.*?).(jpg | jpeg | png | gif | JPG | JPEG | PNG | GIF)$", ErrorMessage = "Please select a Image")]
        public string ImagePath { get; set; }
        [Required]
        [Range(0, 100)]
        public int Age { get; set; }
        [Required]
        public float Price { get; set; }
        [Required, Display(Name = "Breed Type")]
        public string BreedType { get; set; }
        [Required]
        [Display(Name = "Pet Type")]
        public string PetType { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

    }
}