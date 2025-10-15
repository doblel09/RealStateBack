

using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class OfferService : GenericService<SaveOfferViewModel, OfferViewModel, Offer>, IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IMapper _mapper;

        public OfferService(IOfferRepository offerRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(offerRepository, mapper) 
        {
            _offerRepository = offerRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public override async Task<SaveOfferViewModel> Add(SaveOfferViewModel vm)
        {
            //if(_userViewModel == null)
            //{
            //    throw new UnauthorizedAccessException("No tienes permiso para hacer ofertas");
            //}

            vm.Date = DateTime.Now;

            return await base.Add(vm);
        }

        public override async Task Update(SaveOfferViewModel vm, int id)
        {
            //if (_userViewModel == null)
            //{
            //    throw new UnauthorizedAccessException("No tienes Permiso para actualizar esta oferta");
            //}

            await base.Update(vm, id);
        }

        public override async Task Delete(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            //if(offer.ClientId != _userViewModel.Id)
            //{
            //    throw new UnauthorizedAccessException("No tienes permiso para eliminar esta oferta");
            //}

            await base.Delete(id);
        }

        public override async Task<List<OfferViewModel>> GetAllListViewModel()
        {
            if (_userViewModel != null)
            {
                var userId = _userViewModel.Id;
                var listOffers = await _offerRepository.GetAllListAsync();

                if (_userViewModel.Roles.Contains(Roles.Agent.ToString()))
                {
                    var filteredOffers = listOffers.Where(p => p.AgentId == userId)
                                                   .Select(p => _mapper.Map<OfferViewModel>(p))
                                                   .ToList();
                    return filteredOffers;
                }
                else if (_userViewModel.Roles.Contains(Roles.Customer.ToString()) && _userViewModel.Roles.Count == 1)
                {
                    var filteredOffers = listOffers.Where(p => p.ClientId == userId)
                                                   .Select(p => _mapper.Map<OfferViewModel>(p))
                                                   .ToList();
                    return filteredOffers;
                }
                else
                {
                    return _mapper.Map<List<OfferViewModel>>(listOffers);
                }
            }

            throw new UnauthorizedAccessException("No tienes permiso para ver ofertas");
        }



    }
}
