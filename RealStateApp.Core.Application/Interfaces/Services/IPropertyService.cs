
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyViewModel, Property>
    {
        Task<List<PropertyViewModel>> GetAllViewModelWithFilters(FilterPropertyViewModel filters);
        Task<PropertyViewModel> GetPropertyById(int Id);
        Task<int> GetPropertyCountByAgentIdAsync(string agentId);
        Task<List<PropertyViewModel>> GetAllPropertiesByAgentIdAsync(string agentId);


    }
}
