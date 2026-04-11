using FastEndpoints;
using FluentValidation;

namespace Applications.Features.Application.ListManagerApplications.Endpoint;

public class ListManagerApplicationsRequest
{
    [QueryParam] public int Page { get; set; }
    [QueryParam] public int Limit { get; set; }

}

public class ListApplicationsRequestValidator : Validator<ListManagerApplicationsRequest>
{
    public ListApplicationsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 20);
    }
}