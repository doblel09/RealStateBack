using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;


namespace RealStateApp.Core.Application.Features.Properties.Commands.CreateProperty
{
    /// <summary>
    /// Parametros para crear una propiedad
    /// </summary>
    public class CreatePropertyCommand : IRequest<int>
    {
        [BindNever]
        [SwaggerSchema(ReadOnly = true, WriteOnly = false)]
        public string? UniqueCode { get; set; }
        [SwaggerSchema(Description = "Precio")]
        public decimal Price { get; set; }
        /// <example>85.80</example>
        [SwaggerSchema(Description = "Tamaño del terreno en metro")]
        public double SizeInSquareMeters { get; set; }
        /// <example>2</example> 
        [SwaggerSchema(Description = "Cantidad de habitaciones")]
        public int RoomCount { get; set; }
        /// <example>1</example>
        [SwaggerSchema(Description = "Cantidad de baños")]
        public int BathroomCount { get; set; }
        /// <example>Venta de propiedad de 2 habitaciones y un baño</example>
        [SwaggerSchema(Description = "Descripcion")]
        public string Description { get; set; }
        /// <example>["./2016/04/CASA-PEQUEN%CC%83A-.002.jpg"]</example>
        [SwaggerSchema(Description = "Imagenes")]
        public List<IFormFile> Images { get; set; }
        /// <example>true</example>
        [SwaggerSchema(Description = "Disponibilidad | true = disponible, false = no disponible")]
        public bool IsAvailable { get; set; } = true;
        /// <example>1</example>
        [SwaggerSchema(Description = "Id del tipo de propiedad")]
        public int PropertyTypeId { get; set; }
        /// <example>1</example>
        
        [SwaggerSchema(Description = "Id del tipo de venta")]
        public int SaleTypeId { get; set; }
        /// <example>20b56af7-8256-4916-b9fa-58ef801f91cf</example>
        [SwaggerSchema(Description = "Id del agente")]
        public string AgentId { get; set; }
        /// <example>[1,3,6]</example>
        [SwaggerSchema(Description = "Lista de mejoras por id")]
        public List<int>? Improvements { get; set; }
    }

    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, int>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IBaseAccountService _accountService;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public CreatePropertyCommandHandler(IBaseAccountService accountService,IPropertyRepository propertyRepository, IMapper mapper, IImprovementRepository improvementRepository, ISaleTypeRepository saleTypeRepository, IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _improvementRepository = improvementRepository;
            _saleTypeRepository = saleTypeRepository;
            _propertyTypeRepository = propertyTypeRepository;
            _accountService = accountService;
        }
        public async Task<int> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            _ = await _propertyTypeRepository.GetByIdAsync(request.PropertyTypeId) ?? throw new ApiException("PropertyType not found",(int)HttpStatusCode.BadRequest);

            Random random = new Random();
            var uniqueCode = random.Next(10000, 99999).ToString();

            var existingCode = await _propertyRepository.GetAllQuery().Where(p => p.UniqueCode == uniqueCode).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            while (existingCode != null)
            {
                uniqueCode = random.Next(10000, 99999).ToString();
                existingCode = await _propertyRepository.GetAllQuery().Where(p => p.UniqueCode == uniqueCode).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            }

            request.UniqueCode = uniqueCode;

            _ = await _saleTypeRepository.GetByIdAsync(request.SaleTypeId) ?? throw new ApiException("SaleType not found", (int)HttpStatusCode.BadRequest);
            _ = await _accountService.GetAgentByIdAsync(request.AgentId,true);

            var mapping = _mapper.Map<Property>(request);
            
            if (request.Improvements != null && request.Improvements.Count > 0)
            {
                var improvements = await _improvementRepository.GetAllQuery()
                    .Where(improvement => request.Improvements.Contains(improvement.Id))
                    .ToListAsync();

                mapping.Improvements = improvements;
            }

            if (request.Images != null && request.Images.Count > 0)
            {
                mapping.Images = new List<string>();
                foreach (var image in request.Images)
                {
                    var imageUrl = FileManager.UploadFile(image, "file", property: true);
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        throw new ApiException("There was an error uploading the file", (int)HttpStatusCode.BadRequest);
                    }
                    mapping.Images.Add(imageUrl);
                }
            }

            var property = await _propertyRepository.AddAsync(mapping);

            return property.Id;
        }

    }
}
