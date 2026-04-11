using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;

namespace Properties.Features.Property.CreateProperty.Endpoint;

public class CreateProperty : Endpoint<CreatePropertyRequest>
{
    private readonly IMediator _mediator;
    private readonly CreatePropertyMapper _mapper;

    public CreateProperty(IMediator mediator, CreatePropertyMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post($"/{Constants.ModuleName}");
        Roles("manager");
        AllowFileUploads();
    }

    public override async Task HandleAsync(CreatePropertyRequest request, CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map(request);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendCreatedAtAsync($"/{Constants.ModuleName}/{result.Value.PropertyId}",cancellation: cancellationToken);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
        }

    }
}