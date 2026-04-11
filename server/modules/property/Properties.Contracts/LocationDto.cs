namespace Properties.Contracts;

public class LocationDto
{
    public string Address {get; set;}
    public string City {get; set;}
    public CoordinatesDto Coordinates {get; set;}
}

public class CoordinatesDto
{
    public double Latitude {get; set;}
    public double Longitude {get; set;}
}