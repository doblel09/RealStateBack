namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);
        Task UpdateAsync(Entity entity, int id);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllListAsync();
        Task<Entity> GetByIdAsync(int id);
        Task<List<Entity>> GetAllListWithIncludeAsync(List<string> properties);
        Task<List<Entity>> AddRangeAsync(List<Entity> entities);
        IQueryable<Entity> GetAllQuery();
        IQueryable<Entity> GetAllQueryWithInclude(List<string> properties);
    }
}
