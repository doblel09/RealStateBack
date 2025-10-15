using RealStateApp.Core.Application.Dtos.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Agent
{
    public class AgentDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PropertyCount { get; set; }
        public string Email {  get; set; }
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }
        public List<PropertyDto> Properties { get; set; } = new List<PropertyDto>();
    }
}
