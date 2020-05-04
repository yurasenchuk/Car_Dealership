using System.ComponentModel.DataAnnotations;


namespace Car_Dealership.Models
{
    public class Auto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter brand!")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Enter year!")]
        [Attributes.YearAttribute]
        public int Year { get; set; }
        [Required(ErrorMessage = "Enter Price!")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Enter color!")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Enter capacity!")]
        [Attributes.CapacityAttribute]
        public int Capacity { get; set; }
    }
}