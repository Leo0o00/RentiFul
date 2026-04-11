using Ardalis.Result;
using Mediator;
using Tenants.Contracts;

namespace Tenants.Features.Property.AddOwnedProperty.Handler;

public record AddOwnedPropertyCommand(Guid TenantCognitoId, Guid PropertyId) : IRequest<Result<AddOwnedPropertyResponse>>;