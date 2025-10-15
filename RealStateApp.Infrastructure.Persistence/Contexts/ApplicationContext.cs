using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        #region Tables
        public DbSet<Property> Properties { get; set; }
        public DbSet<Improvement> Improvements { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }
        
        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region ToTable
            
            modelBuilder.Entity<Improvement>().ToTable("Improvements");
            modelBuilder.Entity<Favorite>().ToTable("Favorites");
            modelBuilder.Entity<PropertyType>().ToTable("PropertyTypes");
            modelBuilder.Entity<SaleType>().ToTable("SaleTypes");

            #endregion

            #region Properties


            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Properties");
                entity.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
                entity.HasIndex(p => p.UniqueCode)
                .IsUnique();
                entity.Property(p => p.AgentId)
                .IsRequired();
                
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("Offers");
                entity.Property(o => o.ClientId)
                 .IsRequired();
                entity.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
            });


            modelBuilder.Entity<Favorite>()
            .Property(f => f.ClientId)
            .IsRequired();


            #endregion

            #region Relations
            

            modelBuilder.Entity<Property>()
                .HasMany(p => p.Offers)
                .WithOne(o => o.Property)
                .HasForeignKey(o => o.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.PropertyType)
                .WithMany(pt => pt.Properties)
                .HasForeignKey(p => p.PropertyTypeId);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.SaleType)
                .WithMany(st => st.Properties)
                .HasForeignKey(p => p.SaleTypeId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Property)
                .WithMany()
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

        }



    }
}


