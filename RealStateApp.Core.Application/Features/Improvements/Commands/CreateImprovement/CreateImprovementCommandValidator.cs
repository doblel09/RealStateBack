using FluentValidation;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement
{
    public class CreateImprovementCommandValidator : AbstractValidator<CreateImprovementCommand>
    {
        public CreateImprovementCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la mejora es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre de la mejora no puede exceder los 50 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción de la mejora es obligatoria.")
                .MaximumLength(250).WithMessage("La descripción de la mejora no puede exceder los 250 caracteres.");
        }
    }
}
