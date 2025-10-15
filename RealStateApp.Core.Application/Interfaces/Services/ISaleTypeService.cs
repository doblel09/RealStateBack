
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface ISaleTypeService : IGenericService<SaveSaleTypeViewModel, SaleTypeViewModel, SaleType>
    {

    }
}
