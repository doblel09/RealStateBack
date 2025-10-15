using FluentValidation;

namespace RealStateApp.Core.Application.Features.Favorites.Queries.GetAllFavorites
{
    public class GetAllFavoritesQueryValidator : AbstractValidator<GetAllFavoritesQuery>
    {
        public GetAllFavoritesQueryValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("El ClientId es obligatorio.");     
        }

    }
}
