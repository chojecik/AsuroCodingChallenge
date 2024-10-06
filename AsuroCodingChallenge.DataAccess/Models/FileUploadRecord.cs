namespace AsuroCodingChallenge.DataAccess.Models
{
    public class FileUploadRecord
    {
        public required string TrackingId { get; set; }
        public List<string> FileNames { get; set; } = [];
        public bool IsComplete => FileNames.Count > 0;
    }
}
