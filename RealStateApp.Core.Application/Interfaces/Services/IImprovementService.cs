

using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IImprovementService : IGenericService<SaveImprovementViewModel, ImprovementViewModel, Improvement>
    {

    }
}
