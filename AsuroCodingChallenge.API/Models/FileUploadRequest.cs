using System.ComponentModel.DataAnnotations;

namespace AsuroCodingChallenge.API.Models
{
    public class FileUploadRequest
    {
        [Required]
        public required string UserId { get; set; }

        [Required]
        public required string CustomerId { get; set; }

        [Required]
        public required List<IFormFile> Files { get; set; }
    }
}
