

using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaveSaleTypeViewModel, SaleTypeViewModel, SaleType>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IMapper _mapper;

        public SaleTypeService(ISaleTypeRepository saleTypeRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(saleTypeRepository, mapper) 
        {
            _saleTypeRepository = saleTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public override async Task<SaveSaleTypeViewModel> Add(SaveSaleTypeViewModel vm)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para agregar tipos de ventas");
            }

            return await base.Add(vm);
        }

        public override Task Update(SaveSaleTypeViewModel vm, int id)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("No tienes Permiso para actualizar los tipos de ventas");
            }
            return base.Update(vm, id);
        }

        public override async Task Delete(int id)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar tipos de ventas");
            }

            await base.Delete(id);
        }

        public override async Task<List<SaleTypeViewModel>> GetAllListViewModel()
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin") && !_userViewModel.Roles.Contains("Agent"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para ver los tipos ventas");
            }

            var listTypes = await _saleTypeRepository.GetAllListAsync();

            var saleTypeViewModels = _mapper.Map<List<SaleTypeViewModel>>(listTypes);

            foreach (var saleType in saleTypeViewModels)
            {
                var saleCount = listTypes
                    .First(st => st.Id == saleType.Id)
                    .Properties?.Count ?? 0; 

                saleType.PropertiesQuantity = saleCount;
            }

            return saleTypeViewModels;
        }
    }
}
