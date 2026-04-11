using Ardalis.Result;
using Mediator;
using Tenants.Data.Repositories;

namespace Tenants.Features.Property.CreateProperty;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;

    public CreatePropertyCommandHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async ValueTask<Result> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var propertyToCreate = new Domain.Property(request.PropertyId);

        await _propertyRepository.Create(propertyToCreate);

        return Result.Success();
    }
}