using FluentValidation;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Features.Offers.Commands.UpdateOffer
{
    public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
    {
        public UpdateOfferCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id debe ser mayor que 0.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("El monto debe ser mayor que 0.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("El estado de la oferta es obligatorio.")
                .Must(BeAValidStatus).WithMessage("El estado debe ser 'Pending', 'Rejected' o 'Accepted'.");

            RuleFor(x => x.PropertyId)
                .GreaterThan(0).WithMessage("El Id de la propiedad debe ser mayor que 0.");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("El Id del cliente es obligatorio.");

            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("El Id del agente es obligatorio.");
        }

        private bool BeAValidStatus(string status)
        {
            return Enum.TryParse(typeof(OfferStatus), status, true, out _);
        }
    }
}
