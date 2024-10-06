using AsuroCodingChallenge.DataAccess.Database;
using AsuroCodingChallenge.DataAccess.Interfaces;
using AsuroCodingChallenge.DataAccess.Models;
using AsuroCodingChallenge.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AsuroCodingChallenge.Tests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _repository;

        public UserRepositoryTests()
        {
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            _repository = new UserRepository(_context);
        }

        public void Dispose()
        {
            // Cleanup: remove the database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _repository.GetUserAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUserSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            // Act
            await _repository.AddUserAsync(user);

            // Assert
            var addedUser = await _context.Users.FindAsync(userId);
            Assert.NotNull(addedUser);
            Assert.Equal(userId, addedUser.Id);
        }

        [Fact]
        public async Task AddUserAsync_ShouldPersistMultipleUsers()
        {
            // Arrange
            var user1 = new User { Id = Guid.Parse("f867f287-273f-4f73-abc9-edc17c32b493") };
            var user2 = new User { Id = Guid.Parse("4cc35ef7-e8e8-4c58-a96f-56890f03d55f") };

            // Act
            await _repository.AddUserAsync(user1);
            await _repository.AddUserAsync(user2);

            // Assert
            var retrievedUser1 = await _context.Users.FindAsync(Guid.Parse("f867f287-273f-4f73-abc9-edc17c32b493"));
            var retrievedUser2 = await _context.Users.FindAsync(Guid.Parse("4cc35ef7-e8e8-4c58-a96f-56890f03d55f"));

            Assert.NotNull(retrievedUser1);
            Assert.NotNull(retrievedUser2);
            Assert.Equal(Guid.Parse("f867f287-273f-4f73-abc9-edc17c32b493"), retrievedUser1.Id);
            Assert.Equal(Guid.Parse("4cc35ef7-e8e8-4c58-a96f-56890f03d55f"), retrievedUser2.Id);
        }
    }
}
