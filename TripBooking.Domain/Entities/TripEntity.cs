namespace TripBooking.Domain.Entities;

using System;
using System.Collections.Generic;

public class TripEntity
{
    public string Name { get; init; } = null!;

    public string Country { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public DateTime Start { get; set; }

    public int NumberOfSeats { get; set; }

    public List<TripRegistrationEntity> Registrations { get; init; } = new List<TripRegistrationEntity>();
}