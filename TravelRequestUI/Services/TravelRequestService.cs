using TravelRequestUI.Models;

namespace TravelRequestUI.Services;

public class TravelRequestService
{

    public TravelRequestService(HttpClient client)
    {
        _client = client;
    }
    private readonly List<TravelRequest> _travelRequests = new();
    private readonly HttpClient _client;

    class GetTravelRequestsResponse
    {
        public List<TravelRequest> Requests { get; set; } = new();
    }
    
    public async Task<List<TravelRequest>> GetAll()
    {
        var response = await _client.GetFromJsonAsync<GetTravelRequestsResponse>("");
        return response?.Requests ?? [];
    }

    public async Task<TravelRequest?> Add(TravelRequest request)
    {
        var response = await _client.PostAsJsonAsync("", request);

        return await response.Content.ReadFromJsonAsync<TravelRequest>();
    }

    public async Task<TravelRequest?> Update(TravelRequest request)
    {
        var response = await _client.PutAsJsonAsync($"{request.Id}", request);
        return await response.Content.ReadFromJsonAsync<TravelRequest>();
    }

    public async Task Delete(Guid id)
    {
        await _client.DeleteAsync($"{id}");
    }
}