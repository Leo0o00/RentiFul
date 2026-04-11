using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Properties.Contracts;
using Properties.Features.Property.GetAllPropertiesPaged.Handler;

namespace Properties.Features.Property.GetAllPropertiesPaged.Endpoint;



public class GetAllPropertiesPaged : Endpoint<GetAllPropertiesPagedRequest, Result<PropertiesListDto>>
{
    private readonly IMediator _mediator;

    public GetAllPropertiesPaged(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get($"/{Constants.ModuleName}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllPropertiesPagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetAllPropertiesPagedQuery(
            Location: request.Location,
            PriceMin: request.PriceMin,
            PriceMax: request.PriceMax,
            Beds: request.Beds,
            Baths: request.Baths,
            PropertyType: request.PropertyType,
            SquareFeetMin: request.SquareFeetMin,
            SquareFeetMax: request.SquareFeetMax,
            Amenities: request.Amenities,
            AvailableFrom: request.AvailableFrom,
            FavoriteIds: request.FavoriteIds,
            Latitude: request.Latitude,
            Longitude: request.Longitude
            );
        
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendOkAsync(result.Value,
                cancellation: cancellationToken
                );
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
        }

    }
}