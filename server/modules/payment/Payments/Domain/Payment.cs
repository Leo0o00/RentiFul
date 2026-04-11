namespace Payments.Domain;

public class Payment
{
    public Payment(
        decimal amountDue,
        decimal amountPaid,
        DateTime dueDate,
        DateTime paymentDate,
        Guid leaseId,
        PaymentStatus paymentStatus = PaymentStatus.Pending
    )
    {
        AmountDue = amountDue;
        AmountPaid = amountPaid;
        DueDate = dueDate;
        PaymentDate = paymentDate;
        PaymentStatus = paymentStatus;
        LeaseId = leaseId;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public decimal AmountDue { get; private set; }
    public decimal AmountPaid { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public Guid LeaseId { get; private set; }
    public Lease Lease { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; private set; }
}

public enum PaymentStatus
{
    Pending,
    Paid,
    PartiallyPaid,
    Overdue
}