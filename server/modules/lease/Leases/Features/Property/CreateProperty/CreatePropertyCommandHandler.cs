using Ardalis.Result;
using Leases.Data.Repositories;
using Mediator;

namespace Leases.Features.Property.CreateProperty;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;

    public CreatePropertyCommandHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async ValueTask<Result> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var propertyToCreate = new Domain.Property(
            id: request.PropertyId,
            pricePerMonth:  request.PricePerMonth,
            securityDeposit:  request.SecurityDeposit
            );

        await _propertyRepository.Create(propertyToCreate);

        return Result.Success();
    }
}