using AsuroCodingChallenge.DataAccess.Models;

namespace AsuroCodingChallenge.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(Guid userId);
        Task AddUserAsync(User user);
    }
}
