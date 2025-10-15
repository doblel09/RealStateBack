using RealStateApp.Core.Domain.Common;
using System.Threading.Tasks;

namespace RealStateApp.Core.Domain.Entities
{
    public class Offer : AuditableBaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public OfferStatus Status { get; set; } // Enum: Pending, Rejected, Accepted

        // Navigation Properties
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string ClientId { get; set; }
        public string AgentId { get; set; }
    }

    public enum OfferStatus
    {
        Pending,
        Rejected,
        Accepted
    }
    
}
