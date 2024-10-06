using AsuroCodingChallenge.DataAccess.Database;
using AsuroCodingChallenge.DataAccess.Interfaces;
using AsuroCodingChallenge.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

public class FileUploadRepository : IFileUploadRepository
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly AppDbContext _context;

    public FileUploadRepository(IUserRepository userRepository, ICustomerRepository customerRepository, AppDbContext context)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _context = context;
    }

    public async Task AddFilesAsync(Guid userId, Guid customerId, string trackingId, List<string> fileNames)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (user == null)
        {
            user = new User { Id = userId };
            await _userRepository.AddUserAsync(user);
        }

        var customer = await _customerRepository.GetCustomerAsync(customerId);
        if (customer == null)
        {
            customer = new Customer { Id = customerId };
            user.Customers.Add(customer);
            await _customerRepository.AddCustomerAsync(customer);
        }

        var uploadRecord = customer.Uploads.FirstOrDefault(r => r.TrackingId == trackingId);
        if (uploadRecord == null)
        {
            uploadRecord = new FileUploadRecord{TrackingId = trackingId};
            customer.Uploads.Add(uploadRecord);
        }

        uploadRecord.FileNames.AddRange(fileNames);

        await _context.SaveChangesAsync();
    }


    public async Task<FileUploadRecord?> GetUploadRecordAsync(string trackingId) => 
        await _context.FileUploadRecords.FirstOrDefaultAsync(x => x.TrackingId == trackingId);
}
