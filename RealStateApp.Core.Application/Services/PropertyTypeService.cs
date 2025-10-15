

using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class PropertyTypeService : GenericService<SavePropertyTypeViewModel, PropertyTypeViewModel, PropertyType>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IMapper _mapper;

        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(propertyTypeRepository, mapper) 
        {
            _propertyTypeRepository = propertyTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public override async Task<SavePropertyTypeViewModel> Add(SavePropertyTypeViewModel vm)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para agregar tipos de propiedades");
            }

            return await base.Add(vm);
        }

        public override Task Update(SavePropertyTypeViewModel vm, int id)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("No tienes Permiso para actualizar los tipos de propiedades");
            }
            return base.Update(vm, id);
        }

        public override async Task Delete(int id)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar tipos porpiedades");
            }

            await base.Delete(id);
        }

        public override async Task<List<PropertyTypeViewModel>> GetAllListViewModel()
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin") && !_userViewModel.Roles.Contains("Agent"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para ver los tipos de propiedades");
            }

            var listTypes = await _propertyTypeRepository.GetAllListAsync();

            var propertyTypeViewModels = _mapper.Map<List<PropertyTypeViewModel>>(listTypes);

            foreach (var propertyType in propertyTypeViewModels)
            {
                var propertyCount = listTypes
                    .First(pt => pt.Id == propertyType.Id)
                    .Properties?.Count ?? 0; 

                propertyType.PropertiesQuantity = propertyCount;
            }

            return propertyTypeViewModels;
        }


    }
}
