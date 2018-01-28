using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SeatBookingSystem.Entities
{
    /// <summary>
    /// The user
    /// </summary>
    public class User : IdentityUser
    {
        public User()
            : base()
        {
            Seats = new HashSet<Seat>();
        }

        /// <summary>
        /// The seats
        /// </summary>
        public virtual ICollection<Seat> Seats { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    /// <summary>
    /// The meetup
    /// </summary>
    public class Meetup 
    {
        /// <summary>
        /// The meetup constructor
        /// </summary>
        public Meetup()
        {
            this.Seats = new HashSet<Seat>();
        }

        /// <summary>
        /// The meetup identiy
        /// </summary>
        public int MeetupId { get; set; }

        /// <summary>
        /// The meetup time.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The meetup location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The seats
        /// </summary>
        public virtual ICollection<Seat> Seats { get; set; }
    }

    /// <summary>
    /// The seat
    /// </summary>
    public class Seat 
    {
        /// <summary>
        /// The seat identity
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The owner identity
        /// </summary>
        public string OwnerId { get; set; }

        /////// <summary>
        /////// The meetup identity
        /////// </summary>
        ////public int MeetupId { get; set; }

        /// <summary>
        /// The owner
        /// </summary>
        public virtual User Owner { get; set; }

        /////// <summary>
        /////// The meetup
        /////// </summary>
        ////public virtual Meetup Meetup { get; set; }
    }
}