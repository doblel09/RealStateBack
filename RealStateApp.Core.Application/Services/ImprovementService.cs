

using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class ImprovementService : GenericService<SaveImprovementViewModel, ImprovementViewModel, Improvement>, IImprovementService
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IMapper _mapper;

        public ImprovementService(IImprovementRepository improvementRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(improvementRepository, mapper) 
        {
            _improvementRepository = improvementRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public override async Task<SaveImprovementViewModel> Add(SaveImprovementViewModel vm)
        {
            //if (_userViewModel == null)
            //{
            //    throw new UnauthorizedAccessException("No tienes permiso para agregar mejoras");
            //}

            return await base.Add(vm);
        }

        public override Task Update(SaveImprovementViewModel vm, int id)
        {
            //if (_userViewModel == null)
            //{
            //    throw new UnauthorizedAccessException("No tienes Permiso para actualizar mejoras");
            //}
            return base.Update(vm, id);
        }

        public override async Task Delete(int id)
        {
            //if (_userViewModel == null)
            //{
            //    throw new UnauthorizedAccessException("No tienes permiso para eliminar mejoras");
            //}

            await base.Delete(id);
        }

        public override async Task<List<ImprovementViewModel>> GetAllListViewModel()
        {
            //if (_userViewModel == null || _userViewModel.Roles.Count == 1)
            //{
            //    throw new UnauthorizedAccessException("No tienes permiso para ver las mejoras");
            //}

            var listImprovement = await _improvementRepository.GetAllListAsync();
            return _mapper.Map<List<ImprovementViewModel>>(listImprovement);
        }
    }
}
