namespace TravelRequestApi;

public static class TravelRequestEndpoints
{
    public static void MapTravelRequestEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/travelrequests");

        group.MapGet("", GetTravelRequests);

        group.MapGet("{id}", GetTravelRequest);

        group.MapPost("", CreateTravelRequest);

        group.MapPut("{id}", UpdateTravelRequest);

        group.MapDelete("{id}", DeleteTravelRequest);
    }

    static async Task<IResult> GetTravelRequests(ITravelRequestService service)
    {
        return Results.Ok(new { requests = await service.GetRequests() });
    }

    static async Task<IResult> GetTravelRequest(Guid id, ITravelRequestService service)
    {
        var request = await service.GetRequest(id);
        if (request is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(request);
    }

    static async Task<TravelRequest> CreateTravelRequest(ITravelRequestService service, InputRequest request)
    {
        return await service.CreateRequest(request.Destination, request.StartDate, request.EndDate);
    }

    static async Task<TravelRequest> UpdateTravelRequest(Guid id, ITravelRequestService service, InputRequest request)
    {
        return await service.UpdateRequest(id, request.Destination, request.StartDate, request.EndDate);
    }

    static async Task<IResult> DeleteTravelRequest(Guid id, ITravelRequestService service)
    {
        await service.DeleteRequest(id);
        return Results.Ok();
    }

    record InputRequest(string Destination, DateOnly StartDate, DateOnly EndDate);
}