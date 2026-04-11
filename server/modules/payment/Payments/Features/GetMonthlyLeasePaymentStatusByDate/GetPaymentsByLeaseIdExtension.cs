using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Payments.Features.GetMonthlyLeasePaymentStatusByDate.Handler;

namespace Payments.Features.GetMonthlyLeasePaymentStatusByDate;

public static class GetPaymentsByLeaseIdExtension
{
    public static IServiceCollection AddGetPaymentsByLeaseId(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<GetMonthlyLeasePaymentStatusByDateQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}