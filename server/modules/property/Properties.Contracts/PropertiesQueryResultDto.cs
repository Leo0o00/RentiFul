namespace Properties.Contracts;

public class PropertiesQueryResultDto
{
    public int count {get; set;}
    public IEnumerable<PropertyDto> properties {get; set;}
}