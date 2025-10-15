using FluentValidation;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement
{
    public class UpdateImprovementCommandValidator : AbstractValidator<UpdateImprovementCommand>
    {
        public UpdateImprovementCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id de la mejora debe ser mayor a 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la mejora es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre de la mejora no puede exceder los 50 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción de la mejora es obligatoria.")
                .MaximumLength(250).WithMessage("La descripción de la mejora no puede exceder los 250 caracteres.");
        }
    }
}
