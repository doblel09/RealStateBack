using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypes;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de tipos de propiedades")]
    public class PropertyTypeController : BaseApiController
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyTypeDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de tipos de propiedades",
            Description = "Obtiene todas los tipos de propiedades")]
        public async Task<IActionResult> List()
        {
                return Ok(await Mediator.Send(new GetAllPropertyTypesQuery()));
        }


        [HttpGet("{id}")]
        [Authorize(Roles = $"Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listar tipo de venta",
            Description = "Obtiene un tipo de venta filtrado por su id")]
        public async Task<IActionResult> GetById(int id)
        {
           
                return Ok(await Mediator.Send(new GetPropertyTypeByIdQuery { Id = id }));
           
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Crear tipo de venta",
            Description = "Recibe los parametros para crear un tipo de venta")]
        [Authorize(Roles = $"Admin")]
        public async Task<IActionResult> Create([FromBody] CreatePropertyTypeCommand command)
        {
           
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                await Mediator.Send(command);
                return Created(string.Empty, new { Success = true });
           
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Actualizar tipo de venta",
            Description = "Recibe los parametros para actualizar un tipo de venta")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyTypeCommand command)
        {
           
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != command.Id) {
                    return BadRequest();
                }
                return Ok(await Mediator.Send(command));
           
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin,Developer")]
        [SwaggerOperation(
            Summary = "Eliminar tipo de venta",
            Description = "Elimina un tipo de venta con su id")]
        public async Task<IActionResult> Delete(int id)
        {
            
                await Mediator.Send(new DeletePropertyTypeByIdCommand { Id = id});
                return NoContent();
           
        }
    }
}
