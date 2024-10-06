using AsuroCodingChallenge.DataAccess.Models;

namespace AsuroCodingChallenge.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomerAsync(Guid customerId);
        Task AddCustomerAsync(Customer customer);
    }
}
