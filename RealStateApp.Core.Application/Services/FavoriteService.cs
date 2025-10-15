

using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Favorite;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class FavoriteService : GenericService<SaveFavoriteViewModel, FavoriteViewModel, Favorite>, IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IMapper _mapper;

        public FavoriteService(IFavoriteRepository favoriteRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(favoriteRepository, mapper) 
        {
            _favoriteRepository = favoriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public override async Task<SaveFavoriteViewModel> Add(SaveFavoriteViewModel vm)
        {
            //if(_userViewModel == null)
            //{
            //    throw new UnauthorizedAccessException("No tienes permiso para agregar favoritos");
            //}
            //vm.ClientId = _userViewModel.Id;

            return await base.Add(vm);
        }

        public override async Task Delete(int id)
        {
            var favorite = await _favoriteRepository.GetByIdAsync(id);
            //if(favorite.ClientId != _userViewModel.Id)
            //{
            //    throw new UnauthorizedAccessException("No tienes permiso para quitar este favorito");
            //}

            await base.Delete(id);
        }

        public override async Task<List<FavoriteViewModel>> GetAllListViewModel()
        {
            //if (_userViewModel == null)
            //{
            //    throw new UnauthorizedAccessException("No tienes permiso para ver favoritos");
            //}

            var listFavotites = await _favoriteRepository.GetAllListAsync();
            var filteredFavorites = listFavotites.Where(p => p.ClientId == _userViewModel.Id)
                                           .Select(p => _mapper.Map<FavoriteViewModel>(p))
                                           .ToList();
            return filteredFavorites;

        }



    }
}
