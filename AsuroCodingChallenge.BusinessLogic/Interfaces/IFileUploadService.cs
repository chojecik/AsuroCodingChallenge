namespace AsuroCodingChallenge.BusinessLogic.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadFilesAsync(string userIdString, string customerIdString, List<string> fileNames);
        Task<bool> IsUploadCompleteAsync(string trackingId);
        Task<bool> NotifyIfCompleteAsync(string userIdString, string customerIdString, string trackingId);
    }
}
