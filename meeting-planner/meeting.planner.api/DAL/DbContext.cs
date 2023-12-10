using MeetingPlannerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.WebApi.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employees>? Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("Employees");
                entity.Property(e => e.BU).HasColumnName("bu");
                entity.Property(e => e.EmployeeId).HasColumnName("eid");
                entity.Property(e => e.Client).HasColumnName("client");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.EmployeeName).HasColumnName("ename");

            });

            modelBuilder.Entity<MeetingRooms>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MeetingR__3213E83F812CA79A");

                entity.HasIndex(e => e.Roomno, "UK_Password").IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Capacity).HasColumnName("capacity");
                entity.Property(e => e.IsOccupied).HasColumnName("isOccupied");
                entity.Property(e => e.Roomno).HasColumnName("roomno");
            });

            modelBuilder.Entity<MeetingsPlanned>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Meetings__3214EC076C7E898B");

                entity.ToTable("MeetingsPlanned");

                entity.Property(e => e.MeetingScheduledOn).HasColumnType("datetime");

                entity.HasOne(d => d.RoomNoNavigation).WithMany(p => p.MeetingsPlanneds)
                    .HasPrincipalKey(p => p.Roomno)
                    .HasForeignKey(d => d.RoomNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_roomNo");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("Users");
                entity.Property(e => e.uid).HasColumnName("uid").HasConversion<decimal>();
                entity.Property(e => e.user_email).HasColumnName("user_email").HasConversion<string>();
                entity.Property(e => e.user_password).HasColumnName("user_password").HasConversion<string>();

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}