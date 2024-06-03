namespace TripBooking.Api.Hateoas;

using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

public readonly struct Link
{
    public Link(string href, string rel, HttpMethod method)
    {
        Href = href;
        Rel = rel;
        Method = method.ToString("G").ToUpperInvariant();
    }

    public string Href { get; }

    public string Rel { get; }

    public string Method { get; }
}