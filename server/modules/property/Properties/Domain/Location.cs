using Ardalis.GuardClauses;
using NetTopologySuite.Geometries;

namespace Properties.Domain;

public class Location 
{
    public Location(
        string address,
        string city,
        string state,
        string country,
        string postalCode,
        double latitude,
        double longitude)
    {
        Address = Guard.Against.NullOrWhiteSpace(address);
        City = Guard.Against.NullOrWhiteSpace(city);
        State = Guard.Against.NullOrWhiteSpace(state);
        Country = Guard.Against.NullOrWhiteSpace(country);
        PostalCode = Guard.Against.NullOrWhiteSpace(postalCode);
        Coordinates = new Point(
            Guard.Against.Null(longitude),
            Guard.Against.Null(latitude)
            )
        {
            SRID = 4326
        };
    }
    
    private Location(){}
    
    public Guid Id {get; private set; } = Guid.NewGuid();
    public string Address {get; private set; }
    public string City {get; private set; }
    public string State {get; private set; }
    public string Country {get; private set; }
    public string PostalCode {get; private set; }
    public Point Coordinates {get; private set; }

}