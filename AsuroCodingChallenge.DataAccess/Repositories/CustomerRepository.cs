using AsuroCodingChallenge.DataAccess.Database;
using AsuroCodingChallenge.DataAccess.Interfaces;
using AsuroCodingChallenge.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AsuroCodingChallenge.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerAsync(Guid customerId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
    }
}
