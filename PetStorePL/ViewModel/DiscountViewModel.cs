using System;
using System.ComponentModel.DataAnnotations;

namespace PetStorePL.ViewModel
{
    public class DiscountViewModel
    {
        public string PetType { get; set; }
        [Required,Display(Name ="Discount Rate")]
        public float DiscountRate { get; set; }
        [Required,Display(Name ="Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [Required, Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }
    }
}