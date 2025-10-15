
using RealStateApp.Core.Application.ViewModels.Favorite;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IFavoriteService : IGenericService<SaveFavoriteViewModel, FavoriteViewModel, Favorite>
    {

    }
}
