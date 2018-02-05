namespace SeatBookingSystem.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// The entities of seat booking 
    /// </summary>
    public class SeatBookingContext : IdentityDbContext<User>
    {
        /// <summary>
        /// The SeatBookingEntites constructor
        /// </summary>
        public SeatBookingContext()
            : base("SeatBookingConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Meetup> Meetups { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Transaction> Transations { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Transaction>().HasKey(t => t.Id);
            builder.Entity<Seat>().HasKey(t => t.Id)
                                  .Property(t => t.Name)
                                  .IsRequired();

            builder.Entity<Meetup>().HasKey(t => t.Id)
                                    .Property(t => t.Id)
                                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            builder.Entity<Owner>().HasKey(t => t.Id)
                                   .Property(t => t.Id)
                                   .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            builder.Entity<Seat>().Property(t => t.Id)
                                  .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            builder.Entity<Meetup>().HasOptional(t => t.Creater)
                                    .WithMany(t => t.Meetups)
                                    .HasForeignKey(t => t.createrId);

            builder.Entity<Transaction>().HasOptional(t => t.Buyer)
                                         .WithMany(t => t.Transactions)
                                         .HasForeignKey(t => t.BuyerId);

            builder.Entity<Seat>().HasOptional(t => t.Owner)
                                  .WithMany(t => t.Seats)
                                  .HasForeignKey(t => t.OwnerId);

            builder.Entity<Seat>().HasOptional(t => t.Transaction)
                                  .WithMany(t => t.Seats)
                                  .HasForeignKey(t => t.TransactionId);

            builder.Entity<Seat>().HasRequired(t => t.Meetup)
                                  .WithMany(t => t.Seats)
                                  .HasForeignKey(t => t.MeetupId);
        }

        /// <summary>
        /// Creates the booking context
        /// </summary>
        /// <returns></returns>
        public static SeatBookingContext Create()
        {
            return new SeatBookingContext();
        }
    }
}