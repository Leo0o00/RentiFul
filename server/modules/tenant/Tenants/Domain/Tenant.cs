using Ardalis.GuardClauses;
using Ardalis.Result;

namespace Tenants.Domain;

public class Tenant
{
    #region Constructors

    public Tenant(string cognitoId, string name, string email, string phoneNumber)
    {
        CognitoId = cognitoId;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    private Tenant(){} // EF
    
    #endregion Constructors
    
    #region Public Properties
    
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string CognitoId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public IReadOnlyCollection<Property> FavoriteProperties => _favoriteProperties.AsReadOnly();
    public IReadOnlyCollection<Property> OwnedProperties => _ownedProperties.AsReadOnly();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    #endregion Public Properties
    
    #region Private Properties
    
    private readonly List<Property> _favoriteProperties = [];
    private readonly List<Property> _ownedProperties = [];
    
    #endregion Private Properties
    
    #region Public Tenant Methods

    public void Update(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }
    
    #endregion Public Tenant Methods
    
    #region Public Property Methods
    
    public Result AddPropertyToFavorites(Property property)
    {
        Guard.Against.Null(property);

        var existingProperty = _favoriteProperties.SingleOrDefault(p => p.Id == property.Id);
        if (existingProperty != null)
        {
            return Result.Conflict();
        }
        
        _favoriteProperties.Add(property);
        
        return Result.Success();
    }

    public Result RemovePropertyFromFavorites(Guid propertyId)
    {
        Guard.Against.Null(propertyId);
        var existingProperty = _favoriteProperties.SingleOrDefault(p => p.Id == propertyId);
        if (existingProperty is null)
        {
            return Result.NotFound();
        }
        _favoriteProperties.Remove(existingProperty);
        return Result.Success();
    }
    public Result AddPropertyToOwnedProperties(Property property)
    {
        Guard.Against.Null(property);

        var existingProperty = _ownedProperties.SingleOrDefault(p => p.Id == property.Id);
        if (existingProperty != null)
        {
            return Result.Conflict();
        }

        _ownedProperties.Add(property);

        return Result.Success();
    }

    public Result RemovePropertyFromOwnedProperties(Guid propertyId)
    {
        Guard.Against.Null(propertyId);
        var existingProperty = _ownedProperties.SingleOrDefault(p => p.Id == propertyId);
        if (existingProperty is null)
        {
            return Result.NotFound();
        }
        _ownedProperties.Remove(existingProperty);
        return Result.Success();
    }



    #endregion Public Property Methods
    
}