using FluentValidation;

namespace SharedKernel;

public static class CustomValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeUuid<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return ruleBuilder.Must(value => Guid.TryParse(value, out Guid _)).WithMessage("{PropertyName} must be a valid UUID string.");
    }

    public static IRuleBuilderOptions<T, string> MustBeEnumName<T, TEnum>(
        this IRuleBuilder<T, string> ruleBuilder,
        bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        return ruleBuilder.Must(value =>
                !string.IsNullOrWhiteSpace(value) &&
                Enum.TryParse<TEnum>(value.Trim(), ignoreCase, out _)
            )
            .WithMessage("{PropertyName} is invalid.");
    }

    public static IRuleBuilderOptions<T, string[]?> MustBeUniqueNormalized<T>(
        this IRuleBuilder<T, string[]?> ruleBuilder)
    {
        return ruleBuilder
            .Must(arr =>
            {
                if (arr is null) return true;
                var normalized = arr
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim().ToUpperInvariant())
                    .ToArray();

                return normalized.Distinct().Count() == normalized.Length;
            })
            .WithMessage("{PropertyName} must contain unique values.");
    }
}