using MassTransit;
using SharedContracts.Commands;
using SharedContracts.Events;

namespace Applications.Sagas;

public class UpdateApplicationSaga : MassTransitStateMachine<UpdateApplicationSagaData>
{
    public State CreatingLease { get; set; }
    public State LinkingLeaseWithApprovedApplication { get; set; }
    public State LinkingTenantWithOwnedProperty { get; set; }
    public State FinishingApprovingApplicationProcess { get; set; }

    public State UnlinkingLeaseWithApprovedApplication { get; set; }
    public State RemovingCreatedLease { get; set; }
    public State RollingBackUpdateApplicationStatus { get; set; }

    #region Success Events

    public Event<ApplicationApproved> ApplicationApproved { get; set; }
    public Event<LeaseCreated> LeaseCreated { get; set; }
    public Event<LeaseLinkedToApplication> LeaseLinkedToApplication { get; set; }
    public Event<TenantOwnedPropertyAdded> TenantOwnedPropertyAdded { get; set; }

    #endregion Success Events

    #region Failure Events

    public Event<AddTenantOwnedPropertyFailed> AddTenantOwnedPropertyFailed { get; set; }
    public Event<LinkLeaseToApplicationFailed> LinkLeaseToApplicationFailed { get; set; }
    public Event<CreateLeaseFailed> CreateLeaseFailed { get; set; }

    #endregion Failure Events

    #region Compensation Transaction Events

    public Event<UnlinkedLeaseToApplication> UnlinkedLeaseToApplication { get; set; }
    public Event<LeaseRemoved> LeaseRemoved { get; set; }
    public Event<UpdateApplicationStatusRolledBack> UpdateApplicationStatusRolledBack { get; set; }

    #endregion Compensation Transaction Events



    public UpdateApplicationSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => ApplicationApproved, e => e.CorrelateById(m => m.Message.ApplicationId));
        Event(() => LeaseCreated, e => e.CorrelateById(m => m.Message.ApplicationId));
        Event(() => LeaseLinkedToApplication, e => e.CorrelateById(m => m.Message.ApplicationId));
        Event(() => TenantOwnedPropertyAdded, e => e.CorrelateById(m => m.Message.ApplicationId));

        Event(() => AddTenantOwnedPropertyFailed, e => e.CorrelateById(m => m.Message.ApplicationId));
        Event(() => LinkLeaseToApplicationFailed, e => e.CorrelateById(m => m.Message.ApplicationId));
        Event(() => CreateLeaseFailed, e => e.CorrelateById(m => m.Message.ApplicationId));

        Event(() => UnlinkedLeaseToApplication, e => e.CorrelateById(m => m.Message.ApplicationId));
        Event(() => LeaseRemoved, e => e.CorrelateById(m => m.Message.ApplicationId));
        Event(() => UpdateApplicationStatusRolledBack, e => e.CorrelateById(m => m.Message.ApplicationId));



        Initially(
            When(ApplicationApproved)
                .Then(context =>
                {
                    context.Saga.ApplicationId = context.Message.ApplicationId;
                    context.Saga.PropertyId = context.Message.PropertyId;
                    context.Saga.TenantCognitoId = context.Message.TenantCognitoId;
                })
                .TransitionTo(CreatingLease)
                .Publish(context => new CreateLease(
                        context.Message.ApplicationId,
                        context.Message.PropertyId,
                        context.Message.TenantCognitoId,
                        context.Message.ApprovedAt,
                        context.Message.ApprovedAt.AddYears(1)
                    ))
            );

        During(CreatingLease,
            When(LeaseCreated)
                .Then(context =>
                {
                    context.Saga.LeaseId = context.Message.LeaseId;
                    context.Saga.LeaseCreated = true;
                })
                .TransitionTo(LinkingLeaseWithApprovedApplication)
                .Publish(context => new LinkLeaseToApplication(context.Message.ApplicationId,
                    context.Message.LeaseId
                )),
            When(CreateLeaseFailed)
                .TransitionTo(RollingBackUpdateApplicationStatus)
                .Publish(context => new UpdateApplicationStatusRollback(context.Saga.ApplicationId)));

        During(LinkingLeaseWithApprovedApplication,
            When(LeaseLinkedToApplication)
                .Then(context => context.Saga.LeaseLinkedToApplication = true)
                .TransitionTo(LinkingTenantWithOwnedProperty)
                .Publish(context => new AddTenantOwnedProperty(context.Message.ApplicationId,
                    context.Saga.PropertyId,
                    context.Saga.TenantCognitoId)),
            When(LinkLeaseToApplicationFailed)
                .TransitionTo(RemovingCreatedLease)
                .Publish(context => new CreateLeaseRollback(context.Saga.ApplicationId, context.Saga.LeaseId))
                );

        During(LinkingTenantWithOwnedProperty,
            When(TenantOwnedPropertyAdded)
                .Then(context =>
                {
                    context.Saga.TenantOwnedPropertyAdded = true;
                    context.Saga.ApproveApplicationProcessCompleted = true;
                })
                .TransitionTo(FinishingApprovingApplicationProcess)
                .Publish(context => new ApproveApplicationProcessCompleted
                {
                    ApplicationId = context.Message.ApplicationId,
                    TenantCognitoId = context.Message.TenantCognitoId,
                    PropertyId = context.Message.PropertyId
                })
                .Finalize(),
            When(AddTenantOwnedPropertyFailed)
                .TransitionTo(UnlinkingLeaseWithApprovedApplication)
                .Publish(context => new UnlinkLeaseToApplication(context.Saga.ApplicationId, context.Saga.LeaseId)));

        During(UnlinkingLeaseWithApprovedApplication,
            When(UnlinkedLeaseToApplication)
                .Then(context =>
                {
                    context.Saga.LeaseLinkedToApplication = false;
                })
                .TransitionTo(RemovingCreatedLease)
                .Publish(context => new CreateLeaseRollback(context.Saga.ApplicationId, context.Saga.LeaseId)));

        During(RemovingCreatedLease,
            When(LeaseRemoved)
                .Then(context =>
                {
                    context.Saga.LeaseCreated = false;
                })
                .TransitionTo(RollingBackUpdateApplicationStatus)
                .Publish(context => new UpdateApplicationStatusRollback(context.Saga.ApplicationId)));

        During(RollingBackUpdateApplicationStatus,
            When(UpdateApplicationStatusRolledBack)
                .TransitionTo(Initial));
    }

}