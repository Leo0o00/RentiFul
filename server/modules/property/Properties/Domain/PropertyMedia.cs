namespace Properties.Domain;

public class PropertyMedia
{
    public PropertyMedia(
        Guid propertyId,
        string photoKey
    )
    {
        PropertyId = propertyId;
        PhotoKey = photoKey;
    }

    public int Id { get; set; }
    public Guid PropertyId {get; set;}
    public string PhotoKey {get; set;}
};