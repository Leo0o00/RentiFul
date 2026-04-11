
namespace Properties.Contracts;

public class PropertiesListDto
{
    public int count {get; set;}
    public IEnumerable<PropertyResponseDto> properties {get; set;}
}