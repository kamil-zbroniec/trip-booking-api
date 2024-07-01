namespace TripBooking.Api.Endpoints.TripRegistrations;

using Hateoas;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public record TripRegistrationResponseModel
{
    public string TripName { get; init; } = null!;
    
    public string UserEmail { get; init; } = null!;

    [JsonPropertyName("_links")]
    public IReadOnlyCollection<Link> Links { get; init; } = Array.Empty<Link>();
}