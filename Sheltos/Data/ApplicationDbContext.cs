using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data.Enum;
using Sheltos.Models;

namespace Sheltos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Property>Properties { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyFeature> PropertyFeatures { get; set; }
        public DbSet<Feature> Features { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<PropertyFeature>()
                .HasKey(pf => new { pf.PropertyId, pf.FeatureId });
            modelBuilder.Entity<PropertyFeature>()
                .HasOne(pf => pf.Property)
                .WithMany(p => p.Features)
                .HasForeignKey(pf => pf.PropertyId);
            modelBuilder.Entity<PropertyFeature>()
                .HasOne(pf => pf.Feature)
                .WithMany()
                .HasForeignKey(pf => pf.FeatureId);
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Address)
                .WithMany()
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Property>()
                .HasOne(p => p.Agent)
                .WithMany()
                .HasForeignKey(p => p.AgentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = 1,  City = "ojo", State = "Lagos", Country = "Nigeria" },
                new Address { AddressId = 2, City = "New heaven", State = "Enugu", Country = "Brazil" },
                new Address { AddressId = 3, City = "Tempsite", State = "Anambra", Country = "Nigeria" },
                new Address { AddressId = 4, City = "Festac", State = "Lagos", Country = "Nigeria" },
                new Address { AddressId = 5, City = "Mile2", State = "Abuja", Country = "Nigeria" },
                new Address { AddressId = 6, City = "Thinkers Corner", State = "Edo", Country = "Germany" }

            );

            
            modelBuilder.Entity<Agent>().HasData(
                new Agent { AgentId = 1, FullName = "John Doe", PhoneNumber = "08012345678", Email = "john@example.com", AddressId = 1 },
                new Agent { AgentId = 2, FullName = "Gennie Doe", PhoneNumber = "09057247888", Email = "nwekeblessing06@gmail.com", AddressId = 2 }

            );

            
            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    Id = 1,
                    Title = "Luxury Beachfront Villa",
                    Type = "Duplex",
                    Description = "Residences can be classified by and how they are connected residences and land. Different types of housing tenure can be used for the same physical type.",
                    AddressId = 3,
                    Price = 125000.00,
                    Beds = 4,
                    Bathrooms = 3,
                    DateTime = DateTime.UtcNow,
                    PropertyStatus = PropertyStatus.Rent,
                    AgentId = 1
                }
                 new Property
                 {
                     Id = 1,
                     Title = "Luxury Beachfront Villa",
                     Type = "Duplex",
                     Description = "Residences can be classified by and how they are connected residences and land. Different types of housing tenure can be used for the same physical type.",
                     AddressId = 3,
                     Price = 125000.00,
                     Beds = 4,
                     Bathrooms = 3,
                     DateTime = DateTime.UtcNow,
                     PropertyStatus = PropertyStatus.Rent,
                     AgentId = 1
                 }
            );

            
            modelBuilder.Entity<PropertyImage>().HasData(
                new PropertyImage { Id = 1, PropertyId = 1, ImageUrl = "/uploads/properties/villa1.jpg" },
                new PropertyImage { Id = 2, PropertyId = 1, ImageUrl = "/uploads/properties/villa2.jpg" },
                new PropertyImage { Id = 3, PropertyId = 1, ImageUrl = "/uploads/properties/villa3.jpg" }
            );

           
            modelBuilder.Entity<Feature>().HasData(
                new Feature { Id = 1, Name = "Free Wi-Fi" },
                new Feature { Id = 2, Name = "Power Supply" },
                new Feature { Id = 3, Name = "Constant Water Supply" }
            );

            
            modelBuilder.Entity<PropertyFeature>().HasData(
                new PropertyFeature { PropertyId = 1, FeatureId = 1 },
                new PropertyFeature { PropertyId = 1, FeatureId = 2 }
            );

        }

    }
}
