using System;

namespace Common
{
    public class DiscountDto
    {
        public float DiscountRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PetType { get; set; }
    }
}
