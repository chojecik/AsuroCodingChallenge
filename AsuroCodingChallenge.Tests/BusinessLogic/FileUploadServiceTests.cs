using AsuroCodingChallenge.BusinessLogic.Interfaces;
using AsuroCodingChallenge.DataAccess.Interfaces;
using AsuroCodingChallenge.DataAccess.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace AsuroCodingChallenge.Tests
{
    public class FileUploadServiceTests
    {
        private readonly Mock<IFileUploadRepository> _repositoryMock;
        private readonly Mock<ILogger<FileUploadService>> _loggerMock;
        private readonly IFileUploadService _service;

        public FileUploadServiceTests()
        {
            _repositoryMock = new Mock<IFileUploadRepository>();
            _loggerMock = new Mock<ILogger<FileUploadService>>();
            _service = new FileUploadService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task UploadFilesAsync_ShouldThrowArgumentException_WhenUserIdIsNull()
        {
            // Arrange
            string userId = null;
            var customerId = "73423ca5-6da4-494e-9e96-dc74010cdfc7";
            var fileNames = new List<string> { "file1.txt" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UploadFilesAsync(userId, customerId, fileNames));
            Assert.Equal("Invalid User ID.", exception.Message);
        }

        [Fact]
        public async Task UploadFilesAsync_ShouldCallRepository_WhenValidInputsAreProvided()
        {
            // Arrange
            var userId = "42d73b2f-c3f1-45e4-9d19-45ea3e283f24";
            var customerId = "73423ca5-6da4-494e-9e96-dc74010cdfc7";
            var fileNames = new List<string> { "file1.txt", "file2.txt" };

            // Act
            await _service.UploadFilesAsync(userId, customerId, fileNames);

            // Assert
            _repositoryMock.Verify(repo => repo.AddFilesAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>()), Times.Once);
        }

        [Fact]
        public async Task IsUploadCompleteAsync_ShouldReturnTrue_WhenUploadIsComplete()
        {
            // Arrange
            var trackingId = "trackingId";
            var record = new FileUploadRecord 
            { 
                TrackingId = trackingId,
                FileNames = ["file1.txt"]
            };

            _repositoryMock.Setup(repo => repo.GetUploadRecordAsync(It.IsAny<string>()))
                .ReturnsAsync(record);

            // Act
            var result = await _service.IsUploadCompleteAsync(trackingId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsUploadCompleteAsync_ShouldReturnFalse_WhenUploadIsNotComplete()
        {
            // Arrange
            var trackingId = "tracking1";
            var record = new FileUploadRecord 
            { 
                TrackingId = trackingId, 
                FileNames = [] 
            };

            _repositoryMock.Setup(repo => repo.GetUploadRecordAsync(It.IsAny<string>()))
                .ReturnsAsync(record);

            // Act
            var result = await _service.IsUploadCompleteAsync(trackingId);

            // Assert
            Assert.False(result);
        }
    }
}
