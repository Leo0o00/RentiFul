using Applications.Data.Repositories;
using Ardalis.Result;
using Mediator;

namespace Applications.Features.Property.CreateProperty;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;

    public CreatePropertyCommandHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async ValueTask<Result> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var propertyToCreate = new Domain.Property
        {
            Id = request.PropertyId,
            ManagerId = request.ManagerId
        };

        await _propertyRepository.Create(propertyToCreate);

        return Result.Success();
    }
}