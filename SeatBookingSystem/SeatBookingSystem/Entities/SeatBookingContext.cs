using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SeatBookingSystem.Entities
{
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

        ////public DbSet<Meetup> Meetups { get; set; }
        public DbSet<Seat> Seats { get; set; }

        /// <summary>
        /// On model creating
        /// </summary>
        /// <param name="builder">The model builder</param>
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Seat>().HasKey(t => t.ID);
            builder.Entity<Seat>().HasOptional(t => t.Owner).WithMany(t => t.Seats).HasForeignKey(t => t.OwnerId);

            ////Currently we don't expose the feature to support multiple meetups.

            ////builder.Entity<Seat>().HasRequired(t => t.Meetup).WithMany(t => t.Seats).HasForeignKey(t => t.MeetupId);
            ////builder.Entity<Meetup>().HasKey(x => x.MeetupId)
            ////                        .Property(x => x.MeetupId)
            ////                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
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