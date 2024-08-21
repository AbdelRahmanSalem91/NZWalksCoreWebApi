using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code can not be less than 3 letters")]
        [MaxLength(3, ErrorMessage = "Code can not be more than 3 letters")]
        public string Code { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Name can not be less than 2 letters")]
        [MaxLength(50, ErrorMessage = "Name can not be more than 50 letters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
