using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Pool Villa",
                    Details = "This is Villa 1",
                    ImageUrl = "",
                    Sqft = 220,
                    Rate = 500,
                    Occupancy = 220,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.MinValue
                },
                  new Villa
                  {
                      Id = 2,
                      Name = "Beach Villa",
                      Details = "This is Villa 2",
                      ImageUrl = "",
                      Sqft = 200,
                      Rate = 5020,
                      Occupancy = 22,
                      Amenity = "",
                      CreatedDate = DateTime.Now,
                      UpdatedDate = DateTime.MinValue
                  },
                  new Villa
                  {
                        Id = 3,
                        Name = "Royal Villa",
                        Details = "This is Villa 3",
                        ImageUrl = "",
                        Sqft = 520,
                        Rate = 200,
                        Occupancy = 220,
                        Amenity = "",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.MinValue
                  },
                  new Villa
                  {
                          Id = 4,
                          Name = "Sea Villa",
                          Details = "This is Villa 4",
                          ImageUrl = "",
                          Sqft = 220,
                          Rate = 500,
                          Occupancy = 220,
                          Amenity = "",
                          CreatedDate = DateTime.Now,
                          UpdatedDate = DateTime.MinValue
                  }
            );
        }
    }
}