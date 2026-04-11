namespace SharedContracts.Events;

public class PropertyCreated
{
    public Guid Id { get; set; }
    public Guid ManagerId { get; set; }
    public decimal PricePerMonth { get; set; }
    public decimal SecurityDeposit { get; set; }
}