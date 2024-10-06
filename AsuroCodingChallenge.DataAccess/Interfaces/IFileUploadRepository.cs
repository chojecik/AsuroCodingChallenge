using AsuroCodingChallenge.DataAccess.Models;

namespace AsuroCodingChallenge.DataAccess.Interfaces
{
    public interface IFileUploadRepository
    {
        Task AddFilesAsync(Guid userId, Guid customerId, string trackingId, List<string> fileNames);
        Task<FileUploadRecord?> GetUploadRecordAsync(string trackingId);
    }
}
