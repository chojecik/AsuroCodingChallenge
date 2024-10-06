using System.ComponentModel.DataAnnotations;

namespace AsuroCodingChallenge.DataAccess.Models
{
    public class Customer
    {
        [Key]
        public required Guid Id { get; set; }
        public List<FileUploadRecord> Uploads { get; set; } = [];
    }
}
