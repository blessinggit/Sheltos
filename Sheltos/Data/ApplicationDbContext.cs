using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data.Enum;
using Sheltos.Models;

namespace Sheltos.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<ShoppingCartItems> ShoppingCartItems { get; set; }
        public DbSet<AgentApplication> AgentApplications { get; set; }
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
                .WithMany(f => f.Properties)
                .HasForeignKey(pf => pf.FeatureId);
           
            
            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = 1,  City = "ojo", State = "Lagos", Country = "Nigeria"  },
                new Address { AddressId = 2, City = "New heaven", State = "Enugu", Country = "Brazil" },
                new Address { AddressId = 3, City = "Tempsite", State = "Anambra", Country = "Nigeria" }
            );

            
            modelBuilder.Entity<Agent>().HasData(
                new Agent { AgentId = 1, FullName = "John Doe", PhoneNumber = "08012345678",ImageUrl= "/assets/images/avatar/3.jpg", Email = "john@example.com",
                    Address = "iba,ojo,lagos",Qualifications = "M.sc Holder",NinNo = "423578983873",Gender="Male",DateOfBirth=new DateTime(2002,3,23)},
                new Agent { AgentId = 2, FullName = "Gennie Doe", PhoneNumber = "09057247888", ImageUrl = "/assets/images/avatar/3.jpg", Email = "nwekeblessing06@gmail.com", 
                    Address = "New heaven,enugu", Qualifications = "O`Level Holder", NinNo = "423578983873",
                    Gender = "Male",
                    DateOfBirth = new DateTime(2002, 3, 23)
                }

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
                    PropertySize = 5000,
                    DateTime = DateTime.UtcNow,
                    PropertyStatus = PropertyStatus.Rent,
                    AgentId = 1
                },
                new Property
                {
                    Id = 2,
                    Title = "Hidden Spring Hideway",
                    Type = "Apartment",
                    Description = "An interior designer is someone who plans,researches,coordinates,management and manages such enhancement projects.",
                    AddressId = 2,
                    Price = 300000000.00,
                    Beds = 3,
                    Bathrooms = 3,
                    PropertySize = 4000,

                    DateTime = DateTime.UtcNow,
                    PropertyStatus = PropertyStatus.Sale,
                    AgentId = 1
                },
                new Property
                {
                    Id = 3,
                    Title = "Modern City Apartment",
                    Type = "Apartment",
                    Description = "An apartment is a self-contained housing unit that occupies only part of a building, typically on a single level.",
                    AddressId = 1,
                    Price = 75000.00,
                    Beds = 2,
                    Bathrooms = 1,
                    PropertySize = 7000,

                    DateTime = DateTime.UtcNow,
                    PropertyStatus = PropertyStatus.Rent,
                    AgentId = 2
                }
            );

            
            modelBuilder.Entity<PropertyImage>().HasData(
                new PropertyImage { Id = 1, PropertyId = 1, ImageUrl = "/assets/images/property/10.jpg" },
                new PropertyImage { Id = 2, PropertyId = 1, ImageUrl = "/assets/images/property/5.jpg" },
                new PropertyImage { Id = 3, PropertyId = 1, ImageUrl = "/assets/images/property/3.jpg" },
                new PropertyImage { Id = 4, PropertyId = 1, ImageUrl = "/assets/images/property/4.jpg" },
                new PropertyImage { Id = 5, PropertyId = 2, ImageUrl = "/assets/images/property/10.jpg" },
                new PropertyImage { Id = 6, PropertyId = 2, ImageUrl = "/assets/images/property/5.jpg" },
                new PropertyImage { Id = 7, PropertyId = 2, ImageUrl = "/assets/images/property/3.jpg" },
                new PropertyImage { Id = 8, PropertyId = 2, ImageUrl = "/assets/images/property/4.jpg" },
                new PropertyImage { Id = 9, PropertyId = 3, ImageUrl = "/assets/images/property/10.jpg" },
                new PropertyImage { Id = 10, PropertyId = 3, ImageUrl = "/assets/images/property/5.jpg" },
                new PropertyImage { Id = 11, PropertyId = 3, ImageUrl = "/assets/images/property/3.jpg" },
                new PropertyImage { Id = 12, PropertyId = 3, ImageUrl = "/assets/images/property/4.jpg" }

            );

           
            modelBuilder.Entity<Feature>().HasData(
                new Feature { Id = 1, Name = "Free Wi-Fi" },
                new Feature { Id = 2, Name = "Power Supply" },
                new Feature { Id = 3, Name = "Constant Water Supply" },
                new Feature { Id = 4, Name = "Security Guard" },
                new Feature { Id = 5, Name = "Elevator lift" },
                new Feature { Id = 6, Name = "CCTV" },
                new Feature { Id = 7, Name = "Laundry Service" }

            );

            
            modelBuilder.Entity<PropertyFeature>().HasData(
                new PropertyFeature { PropertyId = 1, FeatureId = 1 },
                new PropertyFeature { PropertyId = 1, FeatureId = 2 },
                new PropertyFeature { PropertyId = 1, FeatureId = 5 },
                new PropertyFeature { PropertyId = 2, FeatureId = 2 },
                new PropertyFeature { PropertyId = 2, FeatureId =3 },
                new PropertyFeature { PropertyId = 2, FeatureId = 6 },
                new PropertyFeature { PropertyId = 2, FeatureId = 4},
                new PropertyFeature { PropertyId = 3, FeatureId = 1 },
                new PropertyFeature { PropertyId = 3, FeatureId = 2 },
                new PropertyFeature { PropertyId = 3, FeatureId = 4 }
            );

        }

    }
}
