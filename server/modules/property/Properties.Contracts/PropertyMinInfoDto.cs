namespace Properties.Contracts;

public class PropertyMinInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal PricePerMonth { get; set; }
    public decimal SecurityDeposit { get; set; }

    public string PhotoKey { get; set; }

    public LocationMinInfoDto Location { get; set; }
}