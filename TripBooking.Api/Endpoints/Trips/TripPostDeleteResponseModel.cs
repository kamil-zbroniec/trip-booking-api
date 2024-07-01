namespace TripBooking.Api.Endpoints.Trips;

using Hateoas;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public record TripPostDeleteResponseModel
{
    [JsonPropertyName("_links")]
    public IReadOnlyCollection<Link> Links { get; init; } = Array.Empty<Link>();
}