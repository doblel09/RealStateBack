using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.Favorite;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Features.Properties.Queries.GetPropertyById;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Controlador de agentes")]
    public class AgentController(IWebApiAccountService accountService) : BaseApiController
    {
        private readonly IWebApiAccountService _accountService = accountService;

        [HttpGet]
        [Authorize(Roles = $"Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AgentDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Listado de agentes",
            Description = "Obtiene todas los agentes inmobiliarios")]
        public async Task<IActionResult> List()
        {
           
                return Ok(await _accountService.AgentListAsync());
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Listar agente",
            Description = "Obtiene un agente por su id")]
        public async Task<IActionResult> GetById(string id)
        {

            return Ok(await _accountService.GetAgentByIdAsync(id));
        }

        [HttpGet("{id}/properties")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Listado de propiedades por agente",
            Description = "Obtiene todas las propiedades por agente con su id")]
        public async Task<IActionResult> GetAgentProperty(string id)
        {
            
                return Ok(await Mediator.Send(new GetAllPropertiesQuery { AgentId = id }));
        }

        [HttpPut("ChangeStatus")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Activar/Desactivar agente",
            Description = "Activa o desactiva un agente con su id")]
        public async Task<IActionResult> ChangeStatus([FromQuery] string id)
        {
                await _accountService.ToggleAgentStatus(id);
                return NoContent();

        }


    }
}
