namespace TripBooking.Api.Endpoints.Trips;

using Hateoas;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public record TripResponseModel
{
    public string Name { get; init; } = null!;

    public string Country { get; init; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    public DateTime Start { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int NumberOfSeats { get; init; }
    
    [JsonPropertyName("_links")]
    public IReadOnlyCollection<Link> Links { get; init; } = Array.Empty<Link>();
}