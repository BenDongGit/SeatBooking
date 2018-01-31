namespace SeatBookingSystem.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// The user
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        public User()
            : base()
        {
            Transactions = new HashSet<Transaction>();
            Meetups = new HashSet<Meetup>();
        }

        /// <summary>
        /// if set to <c>true</c> [Administror].
        /// </summary>
        public bool? IsAdministrator { get; set; }

        /// <summary>
        /// The seats
        /// </summary>
        public virtual ICollection<Transaction> Transactions { get; set; }

        /// <summary>
        /// The meetups
        /// </summary>
        public virtual ICollection<Meetup> Meetups { get; set; }

        /// <summary>
        /// Generates the user identity async.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }

    /// <summary>
    /// The meetup
    /// </summary>
    public class Meetup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Meetup"/> class
        /// </summary>
        public Meetup()
        {
            this.Seats = new HashSet<Seat>();
        }

        /// <summary>
        /// The creater identity
        /// </summary>
        public string createrId { get; set; }

        /// <summary>
        /// The meetup identiy
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The meetup time.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The meetup location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The creater.
        /// </summary>
        public virtual User Creater { get; set; }

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
        public int Id { get; set; }

        /// <summary>
        /// The seat name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The transaction Id
        /// </summary>
        public Guid? TransactionId { get; set; }

        /// <summary>
        /// The owner
        /// </summary>
        public int? OwnerId { get; set; }

        /// <summary>
        /// The meetup identity
        /// </summary>
        [Required]
        public int MeetupId { get; set; }

        /// <summary>
        /// The transation
        /// </summary>
        public virtual Transaction Transaction { get; set; }

        /// <summary>
        /// The seat owner
        /// </summary>
        public virtual Owner Owner { get; set; }

        /// <summary>
        /// The meetup
        /// </summary>
        public virtual Meetup Meetup { get; set; }
    }

    /// <summary>
    /// The seat owner
    /// </summary>
    public class Owner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Owner"/> class
        /// </summary>
        public Owner()
        {
            this.Seats = new HashSet<Seat>();
        }

        /// <summary>
        /// The meetup identiy
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The owner name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The owner email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The seats
        /// </summary>
        public virtual ICollection<Seat> Seats { get; set; }
    }

    /// <summary>
    /// The transation
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class
        /// </summary>
        public Transaction()
        {
            this.Seats = new HashSet<Seat>();
        }

        /// <summary>
        /// The transaction identity.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The accounter identity.
        /// </summary>
        public string AccounterId { get; set; }

        /// <summary>
        /// The buyer
        /// </summary>
        public virtual User Accounter { get; set; }

        /// <summary>
        /// The seats
        /// </summary>
        public virtual ICollection<Seat> Seats { get; set; }
    }
}