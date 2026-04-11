using MassTransit;

namespace Applications.Sagas;

public class UpdateApplicationSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; }

    public Guid ApplicationId { get; set; }

    public Guid TenantCognitoId { get; set; }

    public Guid PropertyId { get; set; }
    public Guid LeaseId { get; set; }

    public bool LeaseCreated { get; set; }

    public bool LeaseLinkedToApplication { get; set; }

    public bool TenantOwnedPropertyAdded { get; set; }

    public bool ApproveApplicationProcessCompleted { get; set; }
}