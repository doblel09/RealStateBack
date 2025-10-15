using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealStateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById;
using RealStateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealStateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements;
using RealStateApp.Core.Application.Features.Improvements.Queries.GetImprovementById;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Improvement;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;


namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de mejoras")]
    public class ImprovementController : BaseApiController
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImprovementDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de mejoras",
            Description = "Obtiene todas las mejoras")]
        public async Task<IActionResult> List()
        {              
            return Ok(await Mediator.Send(new GetAllImprovementsQuery()));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin,Developer")]
        [SwaggerOperation(
            Summary = "Listar mejora",
            Description = "Obtiene una mejora filtrando su id")]
        public async Task<IActionResult> GetById(int id)
        {
            
                return Ok(await Mediator.Send(new GetImprovementByIdQuery { Id = id}));
            
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Crear mejora",
            Description = "Recibe los parametros para crear una mejora")]
        public async Task<IActionResult> Create([FromBody] CreateImprovementCommand command)
        {
            
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                await Mediator.Send(command);
                return Created(string.Empty, new { Success = true });
            
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Actualizar mejora",
            Description = "Recibe los parametros para actualizar una mejora")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateImprovementCommand command)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != command.Id) return BadRequest();
                return Ok(await Mediator.Send(command));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Eliminar mejora",
            Description = "Recibe el id para eliminar una mejora")]
        public async Task<IActionResult> Delete(int id)
        {
                await Mediator.Send(new DeleteImprovementByIdCommand { Id = id});
                return NoContent();
            
        }
    } 
}
