using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Data;

public partial class PetStoreDbContext : DbContext
{
    public PetStoreDbContext()
    {
    }

    public PetStoreDbContext(DbContextOptions<PetStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<GroomingService> GroomingServices { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetCategory> PetCategories { get; set; }

    public virtual DbSet<PetFood> PetFoods { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Vaccination> Vaccinations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=petstore;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__addresse__CAA247C8A280BCD1");

            entity.ToTable("addresses");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("street");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("zip_code");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__customer__CD65CB856C887B23");

            entity.ToTable("customers");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .HasDefaultValue("");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__customers__addre__02084FDA");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__employee__C52E0BA8C7556B13");

            entity.ToTable("employees");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .HasDefaultValue("");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("position");

            entity.HasOne(d => d.Address).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__employees__addre__5EBF139D");

            entity.HasMany(d => d.Pets).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeePetRelationship",
                    r => r.HasOne<Pet>().WithMany()
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__employee___pet_i__7B5B524B"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__employee___emplo__7A672E12"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "PetId").HasName("PK__employee__36BEC7F7F9D4599D");
                        j.ToTable("employee_pet_relationship");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
                        j.IndexerProperty<int>("PetId").HasColumnName("pet_id");
                    });
        });

        modelBuilder.Entity<GroomingService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__grooming__3E0DB8AFC7AB162A");

            entity.ToTable("grooming_services");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.PetId).HasName("PK__pets__390CC5FE4D4B018B");

            entity.ToTable("pets");

            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Breed)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("breed");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Category).WithMany(p => p.Pets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__pets__category_i__68487DD7");

            entity.HasMany(d => d.Foods).WithMany(p => p.Pets)
                .UsingEntity<Dictionary<string, object>>(
                    "PetFoodRelationship",
                    r => r.HasOne<PetFood>().WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__pet_food___food___73BA3083"),
                    l => l.HasOne<Pet>().WithMany()
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__pet_food___pet_i__72C60C4A"),
                    j =>
                    {
                        j.HasKey("PetId", "FoodId").HasName("PK__pet_food__ABF80123CE22AD68");
                        j.ToTable("pet_food_relationship");
                        j.IndexerProperty<int>("PetId").HasColumnName("pet_id");
                        j.IndexerProperty<int>("FoodId").HasColumnName("food_id");
                    });

            entity.HasMany(d => d.Services).WithMany(p => p.Pets)
                .UsingEntity<Dictionary<string, object>>(
                    "PetGroomingRelationship",
                    r => r.HasOne<GroomingService>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__pet_groom__servi__778AC167"),
                    l => l.HasOne<Pet>().WithMany()
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__pet_groom__pet_i__76969D2E"),
                    j =>
                    {
                        j.HasKey("PetId", "ServiceId").HasName("PK__pet_groo__DAEC1E743568229D");
                        j.ToTable("pet_grooming_relationship");
                        j.IndexerProperty<int>("PetId").HasColumnName("pet_id");
                        j.IndexerProperty<int>("ServiceId").HasColumnName("service_id");
                    });

            entity.HasMany(d => d.Suppliers).WithMany(p => p.Pets)
                .UsingEntity<Dictionary<string, object>>(
                    "PetSupplierRelationship",
                    r => r.HasOne<Supplier>().WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__pet_suppl__suppl__7F2BE32F"),
                    l => l.HasOne<Pet>().WithMany()
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__pet_suppl__pet_i__7E37BEF6"),
                    j =>
                    {
                        j.HasKey("PetId", "SupplierId").HasName("PK__pet_supp__AFE29CB08E90EF56");
                        j.ToTable("pet_supplier_relationship");
                        j.IndexerProperty<int>("PetId").HasColumnName("pet_id");
                        j.IndexerProperty<int>("SupplierId").HasColumnName("supplier_id");
                    });

            entity.HasMany(d => d.Vaccinations).WithMany(p => p.Pets)
                .UsingEntity<Dictionary<string, object>>(
                    "PetVaccinationRelationship",
                    r => r.HasOne<Vaccination>().WithMany()
                        .HasForeignKey("VaccinationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__pet_vacci__vacci__6FE99F9F"),
                    l => l.HasOne<Pet>().WithMany()
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__pet_vacci__pet_i__6EF57B66"),
                    j =>
                    {
                        j.HasKey("PetId", "VaccinationId").HasName("PK__pet_vacc__57544F00801F72F5");
                        j.ToTable("pet_vaccination_relationship");
                        j.IndexerProperty<int>("PetId").HasColumnName("pet_id");
                        j.IndexerProperty<int>("VaccinationId").HasColumnName("vaccination_id");
                    });
        });

        modelBuilder.Entity<PetCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__pet_cate__D54EE9B4E01FBAEB");

            entity.ToTable("pet_categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PetFood>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__pet_food__2F4C4DD83275F8AA");

            entity.ToTable("pet_food");

            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("brand");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__supplier__6EE594E8BB34685F");

            entity.ToTable("suppliers");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contact_person");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .HasDefaultValue("");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone_number");

            entity.HasOne(d => d.Address).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__suppliers__addre__619B8048");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__transact__85C600AF928A2DC4");

            entity.ToTable("transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("transaction_status");

            entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__transacti__custo__05D8E0BE");

            entity.HasOne(d => d.Pet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FK__transacti__pet_i__06CD04F7");
        });

        modelBuilder.Entity<Vaccination>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__vaccinat__E588AFE7B4018746");

            entity.ToTable("vaccinations");

            entity.Property(e => e.VaccinationId).HasColumnName("vaccination_id");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
