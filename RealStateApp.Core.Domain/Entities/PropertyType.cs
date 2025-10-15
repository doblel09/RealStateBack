using RealStateApp.Core.Domain.Common;


namespace RealStateApp.Core.Domain.Entities
{
    public class PropertyType : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation Properties
        public ICollection<Property> Properties { get; set; }
    }
}
