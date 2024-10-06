using AsuroCodingChallenge.API.Models;
using AsuroCodingChallenge.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IFileUploadService _service;
    private readonly ILogger<FileUploadController> _logger;

    public FileUploadController(IFileUploadService service, ILogger<FileUploadController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFiles([FromForm] FileUploadRequest request)
    {
        try
        {
            _logger.LogInformation("Starting file upload for user {UserId} and customer {CustomerId}", request.UserId, request.CustomerId);

            if (request.Files == null || request.Files.Count == 0)
                return BadRequest("No files uploaded.");

            var fileNames = request.Files.Select(file => file.FileName).ToList();

            var trackingId = await _service.UploadFilesAsync(request.UserId, request.CustomerId, fileNames);

            var isComplete = await _service.NotifyIfCompleteAsync(request.UserId, request.CustomerId, trackingId);

            if (isComplete)
                _logger.LogInformation("File upload for user {UserId} and customer {CustomerId} finished. TrackingId: {TrackingId}", request.UserId, request.CustomerId, trackingId);

            return Ok(new UploadStatus
            {
                TrackingId = trackingId,
                IsComplete = isComplete
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation failed for upload files request.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while uploading files.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpGet("{trackingId}")]
    public async Task<IActionResult> GetUploadStatus(string trackingId)
    {
        try
        {
            if (string.IsNullOrEmpty(trackingId))
            {
                _logger.LogWarning("Invalid User ID: {TrackingId}", trackingId);
                return BadRequest("TrackingId is required");
            }

            var isComplete = await _service.IsUploadCompleteAsync(trackingId);
            return Ok(new UploadStatus
            {
                TrackingId = trackingId,
                IsComplete = isComplete
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while getting upload status.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}
