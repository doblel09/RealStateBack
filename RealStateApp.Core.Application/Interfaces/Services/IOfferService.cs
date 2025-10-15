
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IOfferService : IGenericService<SaveOfferViewModel, OfferViewModel, Offer>
    {

    }
}
