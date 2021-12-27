namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HotelCaliforniaModel : DbContext
    {
        public HotelCaliforniaModel()
            : base("name=HotelCaliforniaModel")
        {
        }

        public virtual DbSet<client> client { get; set; }
        public virtual DbSet<reservation> reservation { get; set; }
        public virtual DbSet<room> room { get; set; }
        public virtual DbSet<roomType> roomType { get; set; }
        public virtual DbSet<service> service { get; set; }
        public virtual DbSet<service_string> service_string { get; set; }
        public virtual DbSet<serviceType> serviceType { get; set; }
        public virtual DbSet<status> status { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<client>()
                .Property(e => e.full_name)
                .IsUnicode(false);

            modelBuilder.Entity<client>()
                .Property(e => e.phone_number)
                .IsUnicode(false);

            modelBuilder.Entity<client>()
                .Property(e => e.client_document)
                .IsUnicode(false);

            modelBuilder.Entity<client>()
                .HasMany(e => e.reservation)
                .WithRequired(e => e.client1)
                .HasForeignKey(e => e.client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<reservation>()
                .HasMany(e => e.service_string)
                .WithRequired(e => e.reservation1)
                .HasForeignKey(e => e.reservation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<room>()
                .HasMany(e => e.reservation)
                .WithRequired(e => e.room1)
                .HasForeignKey(e => e.room)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<roomType>()
                .Property(e => e.category)
                .IsUnicode(false);

            modelBuilder.Entity<roomType>()
                .HasMany(e => e.room)
                .WithRequired(e => e.roomType)
                .HasForeignKey(e => e.room_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<service>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<service>()
                .HasMany(e => e.service_string)
                .WithRequired(e => e.service1)
                .HasForeignKey(e => e.service)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<serviceType>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<serviceType>()
                .HasMany(e => e.service)
                .WithRequired(e => e.serviceType)
                .HasForeignKey(e => e.service_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<status>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<status>()
                .HasMany(e => e.reservation)
                .WithRequired(e => e.status1)
                .HasForeignKey(e => e.status)
                .WillCascadeOnDelete(false);
        }
    }
}
