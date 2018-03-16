using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreDAL.Models
{
    public class Discount
    {
        [Key]
        public int DiscountId { get; set; }
        public float DiscountRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TypeId { get; set; }
        public virtual Pet pet { get; set; }

    }
}
