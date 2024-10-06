using AsuroCodingChallenge.API.Models;
using AsuroCodingChallenge.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuroCodingChallenge.Tests.API
{
    public class FileUploadControllerTests
    {
        private readonly Mock<IFileUploadService> _fileUploadServiceMock;
        private readonly Mock<ILogger<FileUploadController>> _loggerMock;
        private readonly FileUploadController _controller;

        public FileUploadControllerTests()
        {
            _fileUploadServiceMock = new Mock<IFileUploadService>();
            _loggerMock = new Mock<ILogger<FileUploadController>>();
            _controller = new FileUploadController(_fileUploadServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task UploadFiles_ShouldReturnOk_WhenFilesAreUploadedSuccessfully()
        {
            // Arrange
            var request = new FileUploadRequest
            {
                UserId = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                Files = new List<IFormFile>
                {
                    new FormFile(new MemoryStream(), 0, 0, "file1.txt", "file1.txt")
                }
            };

            var trackingId = "tracking1";

            _fileUploadServiceMock
                .Setup(s => s.UploadFilesAsync(request.UserId, request.CustomerId, It.IsAny<List<string>>()))
                .ReturnsAsync(trackingId);
            _fileUploadServiceMock
                .Setup(s => s.NotifyIfCompleteAsync(request.UserId, request.CustomerId, trackingId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UploadFiles(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var uploadStatus = Assert.IsType<UploadStatus>(result.Value);
            Assert.Equal(trackingId, uploadStatus.TrackingId);
            Assert.True(uploadStatus.IsComplete);
        }

        [Fact]
        public async Task UploadFiles_ShouldReturnBadRequest_WhenNoFilesUploaded()
        {
            // Arrange
            var request = new FileUploadRequest
            {
                UserId = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                Files = null
            };

            // Act
            var result = await _controller.UploadFiles(request) as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("No files uploaded.", result.Value);
        }

        [Fact]
        public async Task UploadFiles_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
        {
            // Arrange
            var request = new FileUploadRequest
            {
                UserId = null,
                CustomerId = Guid.NewGuid().ToString(),
                Files = new List<IFormFile>
                {
                    new FormFile(new MemoryStream(), 0, 0, "file1.txt", "file1.txt")
                }
            };

            _fileUploadServiceMock
                .Setup(s => s.UploadFilesAsync(request.UserId, request.CustomerId, It.IsAny<List<string>>()))
                .ThrowsAsync(new ArgumentException("Invalid User ID."));

            // Act
            var result = await _controller.UploadFiles(request) as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid User ID.", result.Value);
        }

        [Fact]
        public async Task GetUploadStatus_ShouldReturnOk_WhenTrackingIdIsValid()
        {
            // Arrange
            var trackingId = "tracking1";
            _fileUploadServiceMock
                .Setup(s => s.IsUploadCompleteAsync(trackingId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.GetUploadStatus(trackingId) as OkObjectResult;

            // Assert
            Assert.Equal(200, result.StatusCode);
            var uploadStatus = Assert.IsType<UploadStatus>(result.Value);
            Assert.Equal(trackingId, uploadStatus.TrackingId);
            Assert.True(uploadStatus.IsComplete);
        }

        [Fact]
        public async Task GetUploadStatus_ShouldReturnBadRequest_WhenTrackingIdIsNull()
        {
            // Act
            var result = await _controller.GetUploadStatus(null) as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("TrackingId is required", result.Value);
        }
    }
}
