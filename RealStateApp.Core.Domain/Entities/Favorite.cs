using RealStateApp.Core.Domain.Common;


namespace RealStateApp.Core.Domain.Entities
{
    public class Favorite : AuditableBaseEntity
    {
        public string ClientId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
