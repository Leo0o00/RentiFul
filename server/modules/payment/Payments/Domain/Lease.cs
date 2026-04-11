namespace Payments.Domain;

public class Lease
{
    public Guid Id { get; set; }
    public List<Payment> Payments { get; set; } = [];
}