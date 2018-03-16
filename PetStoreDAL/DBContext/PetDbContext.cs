using PetStoreDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreDAL.DBContext
{
    public class PetDbContext:DbContext
    {
        public PetDbContext() : base(nameOrConnectionString: "Default")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public"); 
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PetDetails>().Property(p => p.PetName).HasColumnType("varchar").HasMaxLength(50);
            modelBuilder.Entity<PetDetails>().Property(p => p.Gender).HasColumnType("varchar").HasMaxLength(50);
            modelBuilder.Entity<PetDetails>().Property(p => p.BreedType).HasColumnType("varchar").HasMaxLength(50);
            modelBuilder.Entity<Pet>().Property(p => p.PetType).HasColumnType("varchar").HasMaxLength(50);

        }
        public DbSet<PetDetails>petdetails { get; set; }
        public DbSet<Pet>pet { get; set; }
        public DbSet<Discount> discounts { get; set; }
    }
}
