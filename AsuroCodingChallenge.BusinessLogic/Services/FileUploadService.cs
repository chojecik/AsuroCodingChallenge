using AsuroCodingChallenge.BusinessLogic.Interfaces;
using AsuroCodingChallenge.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

public class FileUploadService : IFileUploadService
{
    private readonly IFileUploadRepository _repository;
    private readonly ILogger<FileUploadService> _logger;

    public FileUploadService(IFileUploadRepository repository, ILogger<FileUploadService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string> UploadFilesAsync(string userIdString, string customerIdString, List<string> fileNames)
    {
        if (!Guid.TryParse(userIdString, out Guid userId) || userId == Guid.Empty)
        {
            _logger.LogWarning("Invalid User ID: {UserId}", userIdString);
            throw new ArgumentException("Invalid User ID.");
        }

        if (!Guid.TryParse(customerIdString, out Guid customerId) || customerId == Guid.Empty)
        {
            _logger.LogWarning("Invalid Customer ID: {CustomerId}", customerIdString);
            throw new ArgumentException("Invalid Customer ID.", nameof(customerIdString));
        }

        var trackingId = $"{userId}_{customerId}_{Guid.NewGuid()}";

        await _repository.AddFilesAsync(userId, customerId, trackingId, fileNames);

        _logger.LogInformation("Files uploaded successfully. Tracking ID: {TrackingId}", trackingId);

        return trackingId;
    }

    public async Task<bool> IsUploadCompleteAsync(string trackingId)
    {
        var record = await _repository.GetUploadRecordAsync(trackingId);
        return record?.IsComplete ?? false;
    }

    public async Task<bool> NotifyIfCompleteAsync(string userIdString, string customerIdString, string trackingId)
    {
        if (!Guid.TryParse(userIdString, out Guid userId) || userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid User ID.", nameof(userIdString));
        }

        if (!Guid.TryParse(customerIdString, out Guid customerId) || customerId == Guid.Empty)
        {
            throw new ArgumentException("Invalid Customer ID.", nameof(customerIdString));
        }

        if (await IsUploadCompleteAsync(trackingId))
        {
            // Simulate sending notification
            Console.WriteLine($"Notification sent for User: {userId}, Customer: {customerId}, Tracking ID: {trackingId}");
            return true;
        }

        return false;
    }
}
