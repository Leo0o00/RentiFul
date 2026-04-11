using Amazon.S3;
using Amazon.S3.Model;
using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Options;
using Properties.Contracts;
using Properties.Data.Repositories;

namespace Properties.Features.Property.GetPropertyMinInfo.Handler;

public class GetPropertyMinInfoQueryHandler : IRequestHandler<GetPropertyMinInfoQuery, Result<PropertyMinInfoDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<S3Settings> _s3Settings;

    public GetPropertyMinInfoQueryHandler(IPropertyRepository propertyRepository, IOptions<S3Settings> s3Settings, IAmazonS3 s3Client)
    {
        _propertyRepository = propertyRepository;
        _s3Settings = s3Settings;
        _s3Client = s3Client;
    }

    public async ValueTask<Result<PropertyMinInfoDto>> Handle(GetPropertyMinInfoQuery request, CancellationToken ct)
    {
        var property = await _propertyRepository.GetPropertyMinimalInfoById(request.PropertyId);

        if (property is null)
        {
            return Result.NotFound();
        }

        return property;

    }
}