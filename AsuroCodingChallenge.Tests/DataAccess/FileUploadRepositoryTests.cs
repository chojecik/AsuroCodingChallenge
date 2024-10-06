using AsuroCodingChallenge.DataAccess.Database;
using AsuroCodingChallenge.DataAccess.Interfaces;
using AsuroCodingChallenge.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AsuroCodingChallenge.Tests
{
    public class FileUploadRepositoryTests : IDisposable
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly AppDbContext _context;
        private readonly FileUploadRepository _repository;

        public FileUploadRepositoryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            _repository = new FileUploadRepository(_userRepositoryMock.Object, _customerRepositoryMock.Object, _context);
        }

        public void Dispose()
        {
            // Cleanup: remove the database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddFilesAsync_ShouldAddUser_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var trackingId = "tracking1";
            var fileNames = new List<string> { "file1.txt", "file2.txt" };

            _userRepositoryMock.Setup(repo => repo.GetUserAsync(userId)).ReturnsAsync(null as User);

            // Act
            await _repository.AddFilesAsync(userId, customerId, trackingId, fileNames);

            // Assert
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(u => u.Id == userId)), Times.Once);
        }

        [Fact]
        public async Task AddFilesAsync_ShouldAddCustomer_WhenCustomerDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var trackingId = "tracking1";
            var fileNames = new List<string> { "file1.txt", "file2.txt" };

            var user = new User { Id = userId };
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(userId)).ReturnsAsync(user);
            _customerRepositoryMock.Setup(repo => repo.GetCustomerAsync(customerId)).ReturnsAsync(null as Customer);

            // Act
            await _repository.AddFilesAsync(userId, customerId, trackingId, fileNames);

            // Assert
            _customerRepositoryMock.Verify(repo => repo.AddCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task AddFilesAsync_ShouldAddFilesSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var trackingId = "tracking1";
            var fileNames = new List<string> { "file1.txt", "file2.txt" };

            // Create and add User and Customer to the context
            var user = new User { Id = userId };
            var customer = new Customer
            {
                Id = customerId,
            };

            // Set up the relationship
            user.Customers.Add(customer);

            // Add User and Customer to the in-memory database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Setup the user repository mock
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(userId)).ReturnsAsync(user);
            _customerRepositoryMock.Setup(repo => repo.GetCustomerAsync(customerId)).ReturnsAsync(customer);

            // Act
            await _repository.AddFilesAsync(userId, customerId, trackingId, fileNames);

            // Assert
            var addedRecord = await _context.FileUploadRecords.FirstOrDefaultAsync(r => r.TrackingId == trackingId);
            Assert.NotNull(addedRecord);  // Ensure the upload record is found
            Assert.Equal(2, addedRecord.FileNames.Count);  // Ensure the files count is correct
            Assert.Contains("file1.txt", addedRecord.FileNames);  // Check file names
            Assert.Contains("file2.txt", addedRecord.FileNames);  // Check file names
        }
    }
}
