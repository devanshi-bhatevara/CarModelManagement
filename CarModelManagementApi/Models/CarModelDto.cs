using System.ComponentModel.DataAnnotations;

namespace CarModelManagementApi.Dtos
{
    public class CarModelDto
    {
        public int CarModelId { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required]
        [StringLength(100)]
        public string Class { get; set; }

        [Required]
        [StringLength(100)]
        public string ModelName { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]{10}$", ErrorMessage = "Model Code must be 10 alphanumeric characters.")]
        public string ModelCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Features { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateOfManufacturing { get; set; }
    }
}
