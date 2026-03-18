using Microsoft.EntityFrameworkCore;
using RoomMaintenanceAPI.Models;

namespace RoomMaintenanceAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Existing tables
        public DbSet<TrnRequest> TrnRequest { get; set; }
        public DbSet<TrnRequestDetails> TrnRequestDetails { get; set; }
        // Facility Master table
        public DbSet<FacilityMaster> FacilityMaster { get; set; }
        // Location Master table
        public DbSet<LocationMaster> LocationMaster { get; set; }
        // Apartment Master table
        public DbSet<ApartmentMaster> ApartmentMaster { get; set; }
        // Category Master table
        public DbSet<CategoryMaster> CategoryMaster { get; set; }
        // SubCategory Master table
        public DbSet<SubCategoryMaster> SubCategoryMaster { get; set; }
        //Error table
        public DbSet<ErrorLog> ErrorLog { get; set; }
        //Login table
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -----------------------------
            // TrnRequest (Parent Table)
            // -----------------------------
            modelBuilder.Entity<TrnRequest>()
                .ToTable("trnrequest", "rvp")
                .HasKey(r => r.RequestId);

            // -----------------------------
            // TrnRequestDetails (Child Table)
            // -----------------------------
            modelBuilder.Entity<TrnRequestDetails>()
                .ToTable("trnrequestdetails", "rvp")
                .HasKey(d => d.RequestId);

            // -----------------------------
            // 1-to-1 Relationship Mapping
            // -----------------------------
            modelBuilder.Entity<TrnRequest>()
                .HasOne(r => r.Details)                 // A Request has ONE Details record
                .WithOne(d => d.Request)                // A Details record belongs to ONE Request
                .HasForeignKey<TrnRequestDetails>(d => d.RequestId); // FK lives in Details table

            // -----------------------------
            // FacilityMaster Table Mapping
            // -----------------------------
            modelBuilder.Entity<FacilityMaster>()
                .ToTable("FacilityMaster", "rvp");

            modelBuilder.Entity<FacilityMaster>()
                .Property(f => f.FacilityName)
                .IsRequired();

            modelBuilder.Entity<FacilityMaster>()
                .Property(f => f.IsActive)
                .HasDefaultValue(true);

            // -----------------------------
            // LocationMaster Table Mapping
            // -----------------------------
            modelBuilder.Entity<LocationMaster>()
                .ToTable("LocationMaster", "rvp");

            modelBuilder.Entity<LocationMaster>()
                .Property(f => f.LocationName)
                .IsRequired();

            modelBuilder.Entity<LocationMaster>()
                .Property(f => f.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<LocationMaster>()
                .HasOne(l => l.Facility)
                .WithMany(f => f.Locations)
                .HasForeignKey(l => l.FacilityID);

            // -----------------------------
            // ApartmentMaster Table Mapping
            // -----------------------------
            modelBuilder.Entity<ApartmentMaster>()
                .ToTable("ApartmentMaster", "rvp");

            modelBuilder.Entity<ApartmentMaster>()
                .Property(f => f.ApartmentName)
                .IsRequired();

            modelBuilder.Entity<ApartmentMaster>()
                .Property(f => f.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<ApartmentMaster>()
            .HasOne(a => a.Location)
            .WithMany(l => l.Apartments)
            .HasForeignKey(a => a.LocationID);

            // -----------------------------
            // CategoryMaster Table Mapping
            // -----------------------------
            modelBuilder.Entity<CategoryMaster>()
                .ToTable("CategoryMaster", "rvp");

            modelBuilder.Entity<CategoryMaster>()
                .Property(f => f.CategoryName)
                .IsRequired();

            modelBuilder.Entity<CategoryMaster>()
                .Property(f => f.IsActive)
                .HasDefaultValue(true);

            // -----------------------------
            // SubCategoryMaster Table Mapping
            // -----------------------------
            modelBuilder.Entity<SubCategoryMaster>()
                .ToTable("SubCategoryMaster", "rvp");

            modelBuilder.Entity<SubCategoryMaster>()
                .Property(f => f.SubCategoryName)
                .IsRequired();

            modelBuilder.Entity<SubCategoryMaster>()
                .Property(f => f.IsActive)
                .HasDefaultValue(true);

            // -----------------------------
            // ErrorLog Table Mapping
            // -----------------------------
            modelBuilder.Entity<ErrorLog>().
                ToTable("trnErrorLog", "rvp");

            // -----------------------------
            // EmployeeMaster Table Mapping
            // -----------------------------
            modelBuilder.Entity<Users>()
             .ToTable("EmployeeMaster", "rvp")
             .HasKey(u => u.EmpId);


        }
    }
}