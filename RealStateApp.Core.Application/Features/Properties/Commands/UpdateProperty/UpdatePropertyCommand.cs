using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;


namespace RealStateApp.Core.Application.Features.Properties.Commands.UpdateProperty
{
    /// <summary>
    /// Parametros para actualizar propiedad
    /// </summary>
    public class UpdatePropertyCommand : IRequest<UpdatePropertyResponse>
    {
        /// <example>3</example>
        [SwaggerSchema(Description = "Id de la propiedad")]
        public int Id { get; set; }
        /// <example>394812</example>
        [SwaggerSchema(Description = "Codigo unico de la propiedad")]
        public string UniqueCode { get; set; }
        /// <example>3500000.00</example>
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
        [SwaggerSchema(Description = "Nuevas Imagenes")]
        public List<IFormFile>? Images { get; set; }
        /// <example>["./2016/04/CASA-PEQUEN%CC%83A-.002.jpg"]</example>
        [SwaggerSchema(Description = "Imagenes eliminadas")]
        public List<string>? DeletedImages { get; set; } = new List<string>();

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

    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, UpdatePropertyResponse>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository,IImprovementRepository improvementRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<UpdatePropertyResponse> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetAllQuery().Where(p => p.Id == request.Id).Include(p => p.Improvements).FirstOrDefaultAsync() ?? throw new ApiException("Property not found",(int)HttpStatusCode.BadRequest);

            foreach (var image in request.DeletedImages ?? Enumerable.Empty<string>())
            {
                if (!string.IsNullOrEmpty(image))
                {
                    FileManager.DeleteFile(image, property: true);
                    property.Images?.Remove(image);
                }
            }
            
            List<Improvement> newImprovements;
            if (request.Improvements != null && request.Improvements.Count != 0)
            {
                newImprovements = await _improvementRepository.GetAllQuery()
                .Where(improvement => request.Improvements.Contains(improvement.Id)).ToListAsync() ?? [];
                Console.WriteLine(newImprovements);
            }
            else
            {
                newImprovements = [];
            }

            var mapping = _mapper.Map<Property>(request);

            mapping.Images = property.Images?.ToList() ?? new List<string>();

            if (request.Images != null && request.Images.Count > 0)
            {
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

            await _propertyRepository.UpdateAsync(mapping, request.Id, newImprovements);
            
            return _mapper.Map<UpdatePropertyResponse>(mapping);
        }
    }
}
