using Applications.Domain;

namespace Applications.Data.Repositories;

public interface IPropertyRepository
{
    
    Task Create(Property property);
}