namespace Properties.Domain;

public class Property
{
    #region Constructor

    private Property()
    {
        
    }

    public Property(
        string name,
        string description,
        decimal pricePerMonth,
        decimal securityDeposit,
        decimal applicationFee,
        List<Amenity> amenities,
        List<Highlight> highlights,
        PropertyType propertyType,
        bool isPetsAllowed,
        bool isParkingIncluded,
        int beds,
        double baths,
        double squareFeet,
        Location propertyLocation,
        Manager? managerAssigned)
    {
        Name = name;
        Description = description;
        PricePerMonth = pricePerMonth;
        SecurityDeposit = securityDeposit;
        ApplicationFee = applicationFee;
        Amenities = amenities;
        Highlights = highlights;
        Type = propertyType;
        IsPetsAllowed = isPetsAllowed;
        IsParkingIncluded = isParkingIncluded;
        Beds = beds;
        Baths = baths;
        SquareFeet = squareFeet;
        PropertyLocation = propertyLocation;
        ManagerId = managerAssigned?.Id;
    }

    #endregion Constructor
    
    #region Public Properties

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal PricePerMonth { get; private set; }
    public decimal SecurityDeposit { get; private set; }
    public decimal ApplicationFee { get; private set; }
    
    public List<PropertyMedia> PhotoKeys { get; private set; } = new List<PropertyMedia>();
    
    public List<Amenity> Amenities { get; private set; } = [];

    public List<Highlight>  Highlights { get; private set; } = [];
    
    public PropertyType Type { get; private set; }
    
    public bool IsPetsAllowed { get; private set; } = false;
    public bool IsParkingIncluded { get; private set; } = false;
    public int Beds { get; private set; }
    public double Baths { get; private set; }
    public double SquareFeet { get; private set; }
    
    
    public DateTime? PostedAt { get; private set; } = DateTime.UtcNow;
    public double? AverageRating { get; private set; } = 0;
    public int? NumberOfReviews { get; private set; } = 0;
    public Location PropertyLocation { get; private set; }
    public Manager? ManagerAssigned { get; private set; }
    public Guid? ManagerId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public void AddPhotoKey(string photoKey)
    {
        var media = new PropertyMedia(Id, photoKey);

        PhotoKeys.Add(media);
    }

    #endregion Public Methods

}