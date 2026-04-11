using FastEndpoints;
using FluentValidation;

namespace Applications.Features.Application.ListTenantApplications.Endpoint;

public class ListTenantApplicationsRequest
{
    [QueryParam] public int Page { get; set; }
    [QueryParam] public int Limit { get; set; }

}

public class ListTenantApplicationsRequestValidator : Validator<ListTenantApplicationsRequest>
{
    public ListTenantApplicationsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 20);
    }
}