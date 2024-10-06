using AsuroCodingChallenge.DataAccess.Database;
using AsuroCodingChallenge.DataAccess.Models;
using AsuroCodingChallenge.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AsuroCodingChallenge.Tests
{
    public class CustomerRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly CustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            _repository = new CustomerRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldAddCustomerSuccessfully()
        {
            // Arrange
            var customer = new Customer { Id = Guid.Parse("29b7c78e-f46b-4b95-88ea-171f10a0351d") };

            // Act
            await _repository.AddCustomerAsync(customer);

            // Assert
            var savedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == Guid.Parse("29b7c78e-f46b-4b95-88ea-171f10a0351d"));
            Assert.NotNull(savedCustomer);
            Assert.Equal(Guid.Parse("29b7c78e-f46b-4b95-88ea-171f10a0351d"), savedCustomer.Id);
        }

        [Fact]
        public async Task GetCustomerAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer { Id = Guid.Parse("2274d606-5cce-404b-8fc5-27509596bbe3") };
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetCustomerAsync(Guid.Parse("2274d606-5cce-404b-8fc5-27509596bbe3"));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Guid.Parse("2274d606-5cce-404b-8fc5-27509596bbe3"), result.Id);
        }

        [Fact]
        public async Task GetCustomerAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Act
            var result = await _repository.GetCustomerAsync(Guid.Parse("78da5fd0-0422-4655-9cba-9e6151b4cfcc"));

            // Assert
            Assert.Null(result);
        }
    }
}
