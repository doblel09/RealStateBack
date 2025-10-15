using RealStateApp.Core.Application.Dtos.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Offer
{
    public class OfferDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }

        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string AgentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
