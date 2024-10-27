using System.ComponentModel.DataAnnotations;

namespace CarModelManagementApi.Dtos
{
    public class CarImageDto
    {
        [Required]
        public byte[] ImageData { get; set; } // Storing the image data as a binary array

        [Required]
        [StringLength(255)]
        public string ImageName { get; set; }

        [Required]
        [StringLength(100)]
        public string ImageType { get; set; } // e.g., 'image/jpeg', 'image/png'

    }
}
