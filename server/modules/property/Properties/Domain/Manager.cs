using System.ComponentModel.DataAnnotations;

namespace Properties.Domain;

public record Manager
{

    public Guid Id { get; set; }

    public List<Property> Properties { get; set; } = [];
};