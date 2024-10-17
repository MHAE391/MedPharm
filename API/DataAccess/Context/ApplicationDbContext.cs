using API.DataAccess.Authentication;
using API.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    { 

        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicinePharmacy> MedicinePharmacies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<Customer>(c => c.Id);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Pharmacy)
                .WithOne(p => p.User)
                .HasForeignKey<Pharmacy>(p => p.Id);

            modelBuilder.Entity<MedicinePharmacy>().HasKey(m => new {m.PharmacyId , m.MedicineId} );

            modelBuilder.Entity<MedicinePharmacy>().HasOne(m => m.Medicine).WithMany(mp => mp.MedicinePharmacies)
                .HasForeignKey(m => m.MedicineId);


            modelBuilder.Entity<MedicinePharmacy>().HasOne(ph => ph.Pharmacy).WithMany(mp => mp.PharmacyMedicines)
            .HasForeignKey(ph => ph.PharmacyId);

        }
    }
}
