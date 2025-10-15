


using RealStateApp.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Offer
{
    public class SaveOfferViewModel
    {
        
        public int? Id { get; set; }
        [Required]
        public int PropertyId { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string AgentId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public OfferStatus Status { get; set; }
    }
}
