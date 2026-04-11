namespace Managers.Domain;

public class Manager
{
    #region Constructors

    public Manager(string cognitoId, string name, string email, string phoneNumber)
    {
        CognitoId = cognitoId;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    private Manager(){} // EF
    
    #endregion Constructors
    
    #region Public Properties
    
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string CognitoId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    #endregion Public Properties
    
    #region Public Manager Methods

    public void Update(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }
    
    #endregion Public Manager Methods
}