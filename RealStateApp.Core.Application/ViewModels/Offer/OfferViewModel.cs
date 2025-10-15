

using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.ViewModels.Offer
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string ClientId { get; set; }
        public string AgentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public OfferStatus Status { get; set; }
    }
}
