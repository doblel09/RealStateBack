

using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Core.Application.ViewModels.Offer;


namespace RealStateApp.Core.Application.Services
{
    public class PropertyService : GenericService<SavePropertyViewModel, PropertyViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository propertyRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(propertyRepository, mapper) 
        {
            _propertyRepository = propertyRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public override async Task<SavePropertyViewModel> Add(SavePropertyViewModel vm)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Agent"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para agregar propiedades");
            }

            var uniqueCodes = (await _propertyRepository.GetAllListAsync()).Select(p => p.UniqueCode).ToHashSet();

            string newCode;
            do
            {
                newCode = CodeGenerator.GenerateUniqueCode().ToString();
            } while (uniqueCodes.Contains(newCode));

            vm.UniqueCode = newCode;
            vm.AgentId = _userViewModel.Id;
            vm.IsAvailable = true;

            return await base.Add(vm);
        }


        public override async Task Update(SavePropertyViewModel vm, int id)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Agent"))
            {
                throw new UnauthorizedAccessException("No tienes Permiso para actualizar los datos de esta propiedad");
            }

            await base.Update(vm, id);
        }

        public override async Task Delete(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if(property.AgentId != _userViewModel.Id)
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar esta porpiedad");
            }

            await base.Delete(id);
        }

        public async Task<PropertyViewModel> GetPropertyById(int Id)
        {
            var property = await _propertyRepository.GetByIdAsync(Id);
            var propertyVm = _mapper.Map<PropertyViewModel>(property);

            return propertyVm;
        }


        public async Task<int> GetPropertyCountByAgentIdAsync(string agentId)
        {
            return await _propertyRepository.GetAllQuery().CountAsync(p => p.AgentId == agentId);
        }

        public async Task<List<PropertyViewModel>> GetAllViewModelWithFilters(FilterPropertyViewModel filters)
        {
            
            var listProperties = _propertyRepository.GetAllQueryWithInclude(new List<string> { "PropertyType","SaleType", "Improvements" });

            if (filters.PropertyTypeId != null)
            {
               listProperties =  listProperties.Where(property => property.PropertyTypeId == filters.PropertyTypeId);
            }
            if (filters.SaleTypeId != null)
            {
                listProperties = listProperties.Where(property => property.SaleTypeId == filters.SaleTypeId);
            }
            if (filters.UniqueCode != null)
            {
                listProperties = listProperties.Where(property => property.UniqueCode == filters.UniqueCode);
            }
            if (filters.AgentId != null)
            {
                listProperties = listProperties.Where(property => property.AgentId == filters.AgentId);
            }


            var listEntities = await listProperties.ToListAsync();
            var listViewModels = _mapper.Map<List<PropertyViewModel>>(listEntities);
            return listViewModels;
        }
        public async Task<List<PropertyViewModel>> GetAllPropertiesByAgentIdAsync(string agentId)
        {
            var properties = await _propertyRepository.GetAllAsync(p => p.AgentId == agentId);

            // Mapeo de las entidades al ViewModel
            return properties.Select(p => new PropertyViewModel
            {
                Id = p.Id,
                UniqueCode = p.UniqueCode,
                Price = p.Price,
                SizeInSquareMeters = p.SizeInSquareMeters,
                RoomCount = p.RoomCount,
                BathroomCount = p.BathroomCount,
                Description = p.Description,
                Images = p.Images.ToList(),
                IsAvailable = p.IsAvailable,
                PropertyType = new PropertyTypeViewModel { Name = p.PropertyType.Name },
                SaleType = new SaleTypeViewModel { Name = p.SaleType.Name },
                Offers = p.Offers?.Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    Amount = o.Amount,
                    Date = o.Date,
                    Status = o.Status
                }).ToList(),
                Improvements = p.Improvements?.Select(i => new ImprovementViewModel
                {
                    Id = i.Id,
                    Description = i.Description,
                }).ToList(),
                AgentId = p.AgentId,
            }).ToList();
        }

    }
}
