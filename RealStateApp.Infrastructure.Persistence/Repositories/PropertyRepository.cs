using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using System.Linq.Expressions;


namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task UpdateAsync(Property newProperty, int propertyId, List<Improvement> improvements)
        {
            var existingProperty = await _dbContext.Properties.Include(p => p.Improvements).FirstOrDefaultAsync(p => p.Id == propertyId) ?? throw new InvalidOperationException($"Property with ID {propertyId} not found."); ;
            _dbContext.Entry(existingProperty).CurrentValues.SetValues(newProperty);

            existingProperty.Improvements?.Clear();

                // Add new property improvements if any
                if (improvements != null && improvements.Count > 0)
                {
                    foreach(var improvement in improvements)
                    {
                        existingProperty.Improvements?.Add(improvement);
                    }
                }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Property property)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Call base update method for the property entity
                _dbContext.Set<Property>().Update(property);
                await _dbContext.SaveChangesAsync();
                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback transaction on error
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<Property>> GetAllAsync(Expression<Func<Property, bool>> predicate = null)
        {
            IQueryable<Property> query = _dbContext.Set<Property>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }

        public IQueryable<Property> GetAll()
        {
            IQueryable<Property> query = _dbContext.Set<Property>();

            return query;
        }

    }
}
