using FastEndpoints;
using FluentValidation;
using SharedKernel;

namespace Properties.Features.Property.GetAllPropertiesInIdList.Endpoint;

public class GetAllPropertiesInIdListRequest
{
    [FromBody] public List<string> PropertiesId { get; set; } = [];
};



public class GetAllPropertiesInIdListRequestValidator : Validator<GetAllPropertiesInIdListRequest>
{
    public GetAllPropertiesInIdListRequestValidator()
    {
        When(x => x.PropertiesId.Count > 0, () =>
            {
                RuleForEach(x => x.PropertiesId)
                    .NotEmpty().WithMessage("PropertyId {CollectionIndex} cannot be null or whitespace.")
                    .MustBeUuid().WithMessage("PropertyId {CollectionIndex} must be a valid UUID value");
            });
    }
}