using FluentValidation;

namespace RealStateApp.Core.Application.Features.Offers.Commands.CreateOffer
{
    public class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
    {
        public CreateOfferCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("El monto de la oferta debe ser mayor a 0.");

            RuleFor(x => x.PropertyId)
                .GreaterThan(0).WithMessage("El Id de la propiedad debe ser mayor a 0.");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("El Id del cliente es obligatorio.")
                .Must(BeAValidGuid).WithMessage("El Id del cliente debe ser un GUID válido.");

            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("El Id del agente es obligatorio.")
                .Must(BeAValidGuid).WithMessage("El Id del agente debe ser un GUID válido.");
        }

        private bool BeAValidGuid(string guid)
        {
            return Guid.TryParse(guid, out _);
        }
    }
}
