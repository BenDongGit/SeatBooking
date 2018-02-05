namespace SeatBookingSystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using SeatBookingSystem.Common;
    using SeatBookingSystem.Entities;
    using SeatBookingSystem.MvcExtensions;
    using SeatBookingSystem.Models;
    using SeatBookingSystem.Helper;

    /// <summary>
    /// The seat controller
    /// </summary>
    [Authorize]
    [ExceptionHandler]
    public class SeatController : Controller
    {
        private Lazy<IDbContextHelper<SeatBookingContext>> lazyBookingContextHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatController"/> class
        /// </summary>
        public SeatController(IDbContextHelper<SeatBookingContext> bookingContextHelper)
        {
            lazyBookingContextHelper = new Lazy<IDbContextHelper<SeatBookingContext>>(
                () => bookingContextHelper);
            InitializeTestSeats();
        }

        /// <summary>
        /// The index entry
        /// </summary>
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// The book entry
        /// </summary>
        public ActionResult Book()
        {
            return View();
        }

        /// <summary>
        /// The realease entry
        /// </summary>
        public ActionResult Release()
        {
            return View();
        }

        /// <summary>
        /// Gets all the booked seats.
        /// </summary>
        /// <param name="meetupId">The meetup identity</param>
        /// <returns>The all booked seats</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllBookedSeats(int meetupId = 0)
        {
            return await lazyBookingContextHelper.Value.CallWithTransactionAsync<ActionResult>(
                async context =>
                {
                    var allBooked = new List<Seat>();
                    var meetup = context.Meetups.FirstOrDefault(
                        m => meetupId == 0 ? m.Location == Consts.DefaultMeetupLocation : m.Id == meetupId);
                    if (meetup == null)
                    {
                        throw new InvalidOperationException("The meetup is not existing!");
                    }

                    allBooked = meetup.Seats.Where(s => s.Owner != null).ToList();

                    await Task.FromResult(0).ConfigureAwait(false);
                    return this.NewtonsoftJsonResult(allBooked);

                }).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the current user booked seats.
        /// </summary>
        /// <param name="meetupId">The meetup identity</param>
        /// <param name="buyer">The buyer.</param>
        /// <returns>The booked seats of the accounter</returns>
        [AllowAnonymous]
        public async Task<ActionResult> GetUserBookedSeats(string buyer, int meetupId = 0)
        {
            return await lazyBookingContextHelper.Value.CallWithTransactionAsync<ActionResult>(
                async context =>
                {
                    List<Seat> seats = new List<Seat>();
                    var meetup = context.Meetups.FirstOrDefault(
                        m => meetupId == 0 ? m.Location == Consts.DefaultMeetupLocation : m.Id == meetupId);
                    if (meetup == null)
                    {
                        throw new InvalidOperationException("The meetup is not existing!");
                    }

                    var user = !string.IsNullOrEmpty(buyer) ?
                        context.Users.FirstOrDefault(u => u.UserName == buyer) : GetCurrentUser();

                    if (user != null)
                    {
                        seats = meetup.Seats.Where(
                            s => s.Transaction != null && s.Transaction.Buyer != null && s.Transaction.Buyer.UserName == user.UserName).ToList();
                    }

                    await Task.FromResult(0).ConfigureAwait(false);
                    return this.NewtonsoftJsonResult(seats);

                }).ConfigureAwait(false);
        }

        /// <summary>
        /// Books the seats.
        /// </summary>
        /// <param name="models">The seats.</param>
        /// <param name="buyer">The buyer.</param>
        /// <param name="meetupId">The meetup identity.</param>
        /// <returns>The seats booking result</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> BookSeats(SeatViewModel[] models, string buyer, int meetupId = 0)
        {
            if (models.IsNullOrEmpty())
            {
                throw new InvalidOperationException("No seats are selected!");
            }

            if (models.Length > Consts.MaxCountOfOneTransaction)
            {
                throw new InvalidOperationException("Can't book more than four seats on one transaction!");
            }

            if (models.Any(x => string.IsNullOrEmpty(x.Email) || string.IsNullOrEmpty(x.Name)))
            {
                throw new InvalidOperationException("There are seats of missing at least the followings [Owner|Email]");
            }

            return await lazyBookingContextHelper.Value.CallWithTransactionAsync<ActionResult>(
                async context =>
                {
                    var meetup = context.Meetups.FirstOrDefault(
                        m => meetupId == 0 ? m.Location == Consts.DefaultMeetupLocation : m.Id == meetupId);
                    if (meetup == null)
                    {
                        throw new InvalidOperationException("The meetup is not existing!");
                    }

                    var seats = meetup.Seats.Where(s => models.Any(m => m.Name == s.Name)).ToList();
                    if (seats.Any(s => s.Owner != null && !string.IsNullOrEmpty(s.Owner.Name)))
                    {
                        throw new InvalidOperationException(
                            string.Format("[{0}] has already been booked!",
                            string.Join(",", seats.Where(s => !string.IsNullOrEmpty(s.Owner.Name)).Select(x => x.Name))));
                    }

                    var duplicates = meetup.Seats.Where(x => x.Owner != null).ToList().Where(
                        x => models.Any(m => m.Owner.ToLower() == x.Owner.Name.ToLower() || m.Email.ToLower() == x.Owner.Email.ToLower())).ToList();
                    if (duplicates.Any())
                    {
                        string errorMsg = string.Format("[{0}] has duplicate email or name with others", string.Join(",", duplicates.Select(x => x.Owner.Name)));
                        throw new InvalidOperationException(errorMsg);
                    }

                    var user = !string.IsNullOrEmpty(buyer) ?
                                   context.Users.FirstOrDefault(u => u.UserName == buyer) : GetCurrentUser();
                    if (user == null)
                    {
                        throw new InvalidOperationException("Can't book seat since the accounter is not valid!");
                    }

                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        BuyerId = user.Id,
                        Time = DateTime.Now
                    };

                    context.Set<Transaction>().Add(transaction);

                    seats.ForEach(s => s.TransactionId = transaction.Id);
                    seats.ForEach(s =>
                    {
                        var model = models.FirstOrDefault(m => m.Name == s.Name);
                        var owner = new Owner
                        {
                            Name = model.Owner,
                            Email = model.Email
                        };

                        context.Set<Owner>().Add(owner);
                        s.Owner = owner;
                        var details = new
                        {
                            Owner = model.Owner,
                            Seat = model.Name,
                            MeetupId = meetup.Id
                        };

                        transaction.Details = details.ToJsonString();
                    });

                    context.SaveChanges();
                    await Task.FromResult(0).ConfigureAwait(false);
                    return this.NoContent();

                }).ConfigureAwait(false);
        }

        /// <summary>
        /// Books the seats.
        /// </summary>
        /// <param name="seatNames">The seats.</param>
        /// <param name="meetupId">The meetup identity.</param>
        /// <returns>The release result</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ReleaseSeats(string[] seatNames, int meetupId = 0, string accounter = null)
        {
            return await lazyBookingContextHelper.Value.CallWithTransactionAsync<ActionResult>(
                async context =>
                {
                    var meetup = context.Meetups.FirstOrDefault(
                                  m => meetupId == 0 ? m.Location == Consts.DefaultMeetupLocation : m.Id == meetupId);
                    if (meetup == null)
                    {
                        throw new InvalidOperationException("The meetup is not existing!");
                    }

                    var user = !string.IsNullOrEmpty(accounter) ?
                                   context.Users.FirstOrDefault(u => u.UserName == accounter) : GetCurrentUser();
                    if (user == null)
                    {
                        throw new InvalidOperationException("Can't release seats since the accounter is not valid!");
                    }

                    var seats = meetup.Seats.Where(x => seatNames.Contains(x.Name)).ToList();
                    foreach (var seat in seats)
                    {
                        seat.OwnerId = null;
                        seat.TransactionId = null;
                    }

                    context.SaveChanges();

                    await Task.FromResult(0).ConfigureAwait(false);
                    return this.NoContent();

                }).ConfigureAwait(false);
        }

        /// <summary>
        /// Initialize the seats, this is only for test use, and this can be removed after the feature to create meetup released
        /// </summary>
        /// <returns>The seats</returns>
        private void InitializeTestSeats()
        {
            lazyBookingContextHelper.Value.CallWithTransaction(context =>
            {
                if (context.Meetups.IsNullOrEmpty())
                {
                    var meetup = context.Meetups.FirstOrDefault();
                    if (meetup == null)
                    {
                        meetup = new Meetup
                        {
                            Location = Consts.DefaultMeetupLocation,
                            Time = DateTime.Now,
                        };

                        context.Meetups.Add(meetup);
                    }

                    List<string> seatNames = new List<string>();
                    for (int c = 0; c < Consts.SeatColNum; c++)
                    {
                        for (int r = 1; r <= Consts.SeatRowNum; r++)
                        {
                            seatNames.Add(string.Format("{0}{1}", Consts.ColSeatNames[c], r));
                        }
                    }

                    var seats = seatNames.Select(name =>
                        new Seat
                        {
                            Name = name,
                            MeetupId = meetup.Id
                        }).ToList();

                    context.Seats.AddRange(seats);
                    context.SaveChanges();
                }
            });
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>The current user</returns>
        private User GetCurrentUser()
        {
            if (this.HttpContext.User != null)
            {
                return lazyBookingContextHelper.Value.CallWithTransaction<User>(context =>
                {
                    return context.Users.Where(x => x.UserName == this.HttpContext.User.Identity.Name).FirstOrDefault();
                });
            }

            return null;
        }
    }
}
