using FluentValidation;

namespace RealStateApp.Core.Application.Features.Favorites.Commands.CreateFavorite
{
    public class CreateFavoriteCommandValidator : AbstractValidator<CreateFavoriteCommand>
    {
        public CreateFavoriteCommandValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("El Id del cliente no puede estar vacío.");

            RuleFor(x => x.PropertyId)
                .GreaterThan(0).WithMessage("El Id de la propiedad debe ser mayor que 0.");
        }

    }
}
