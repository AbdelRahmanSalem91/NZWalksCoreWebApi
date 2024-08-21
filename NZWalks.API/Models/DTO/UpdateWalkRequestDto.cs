using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name can not be more than 100 letters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Name can not be more than 500 letters")]
        public string Description { get; set; }

        [Required]
        [Range(1, 50)]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
