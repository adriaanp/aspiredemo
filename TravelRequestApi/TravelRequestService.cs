using Microsoft.EntityFrameworkCore;

namespace TravelRequestApi;

public interface ITravelRequestService
{
    Task<TravelRequest> CreateRequest(string destination, DateOnly startDate, DateOnly endDate);
    Task<TravelRequest?> GetRequest(Guid id);
    Task<IEnumerable<TravelRequest>> GetRequests();
    Task<TravelRequest> UpdateRequest(Guid id, string destination, DateOnly startDate, DateOnly endDate);
    Task DeleteRequest(Guid id);
}

public class TravelRequestService : ITravelRequestService
{
    private static readonly List<TravelRequest> Requests = new();
    private AppDbContext _dbContext;

    public TravelRequestService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TravelRequest> CreateRequest(string destination, DateOnly startDate, DateOnly endDate)
    {
        var travelRequest = new TravelRequest(Guid.NewGuid(), destination, startDate, endDate);
        _dbContext.TravelRequests.Add(travelRequest);
        await _dbContext.SaveChangesAsync();
        return travelRequest;
    }
    
    public async Task<TravelRequest?> GetRequest(Guid id)
    {
        var travelRequest = await _dbContext.TravelRequests.FindAsync(id);
        return travelRequest;
    }
    
    public async Task<IEnumerable<TravelRequest>> GetRequests()
    {
        var travelRequests = await _dbContext.TravelRequests
            .ToListAsync();
        return travelRequests;
    }
    
    public async Task<TravelRequest> UpdateRequest(Guid id, string destination, DateOnly startDate, DateOnly endDate)
    {
        var request = await _dbContext.TravelRequests.FindAsync(id);
        if (request is null)
        {
            throw new KeyNotFoundException();
        }

        request.Destination = destination;
        request.StartDate = startDate;
        request.EndDate = endDate;
        _dbContext.TravelRequests.Update(request);
        await _dbContext.SaveChangesAsync();
        return request;
    }
    
    public async Task DeleteRequest(Guid id)
    {
        var result = await _dbContext.TravelRequests.Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }
}

public class TravelRequest(Guid id, string destination, DateOnly startDate, DateOnly endDate)
{
    public Guid Id { get; set; } = id;
    public string Destination { get; set; } = destination;
    public DateOnly StartDate { get; set; } = startDate;
    public DateOnly EndDate { get; set; } = endDate;
};
