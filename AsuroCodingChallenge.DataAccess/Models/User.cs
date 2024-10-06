using System.ComponentModel.DataAnnotations;

namespace AsuroCodingChallenge.DataAccess.Models
{
    public class User
    {
        [Key]
        public required Guid Id { get; set; }
        public List<Customer> Customers { get; set; } = [];
    }
}
