

using RealStateApp.Core.Domain.Entities;
using System.Linq.Expressions;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task UpdateAsync(Property newProperty, int propertyId, List<Improvement> improvements);
        Task UpdateAsync(Property property);
        Task<List<Property>> GetAllAsync(Expression<Func<Property, bool>> predicate = null);

        IQueryable<Property> GetAll();

    }
}
