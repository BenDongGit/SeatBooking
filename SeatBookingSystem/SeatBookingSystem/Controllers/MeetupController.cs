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
    using SeatBookingSystem.Helper;

    [Authorize]
    [ExceptionHandler]
    public class MeetupController : Controller
    {
        private Lazy<IDbContextHelper<SeatBookingContext>> lazyBookingContextHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatController"/> class
        /// </summary>
        public MeetupController(IDbContextHelper<SeatBookingContext> bookingContextHelper)
        {
            lazyBookingContextHelper = new Lazy<IDbContextHelper<SeatBookingContext>>(() => bookingContextHelper);
        }

        /// <summary>
        /// Gets the meet up
        /// </summary>
        /// <param name="location">The meetup location</param>
        /// <param name="time">The meetup time</param>
        /// <returns>The meetup</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Get(string location, DateTimeOffset time)
        {
            return await lazyBookingContextHelper.Value.CallWithTransactionAsync<ActionResult>(
                async context =>
                {
                    var meetup = default(Meetup);
                    if (!string.IsNullOrEmpty(location) && location == Consts.DefaultMeetupLocation)
                    {
                        meetup = context.Meetups.FirstOrDefault(m => m.Location == Consts.DefaultMeetupLocation);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(location))
                        {
                            throw new InvalidOperationException("The location is not given!");
                        }

                        //Currently we set the min time of meetup is day, this can be changed with new requirement 
                        meetup = context.Meetups.FirstOrDefault(
                            m => m.Location == location && m.Time.ToString("yyyymmdd") == time.DateTime.ToString("yyyymmdd"));
                    }

                    await Task.FromResult(0).ConfigureAwait(false);
                    return this.NewtonsoftJsonResult(meetup);

                }).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the meetup
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="time">The time</param>
        /// <param name="creater">The creater</param>
        /// <param name="row">The seat rows of meetup.</param>
        /// <param name="col">The seat cols of meetup.</param>
        /// <returns>The meetup</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Create(string location, DateTimeOffset time, string creater, int row = 0, int col = 0)
        {
            if (string.IsNullOrEmpty(creater))
            {
                throw new InvalidOperationException("The meetup should be created by administrator");
            }

            if (col > Consts.MaxRowNum)
            {
                throw new InvalidOperationException(string.Format("No more than [{0}] seat rows can be created", Consts.MaxRowNum));
            }

            return await lazyBookingContextHelper.Value.CallWithTransactionAsync<ActionResult>(
                async context =>
                {
                    var meetup = new Meetup
                    {
                        Location = location,
                        Time = time.DateTime,
                    };

                    context.Meetups.Add(meetup);

                    row = row == 0 ? Consts.SeatRowNum : row;
                    col = col == 0 ? Consts.SeatColNum : col;
                    List<string> seatNames = new List<string>();
                    for (int c = 0; c < col; c++)
                    {
                        for (int r = 1; r <= row; r++)
                        {
                            seatNames.Add(string.Format("{0}{1}", Consts.ColSeatNames[c], r));
                        }
                    }

                    var seats = seatNames.Select(name =>
                    new Seat { Name = name, MeetupId = meetup.Id }).ToList();
                    context.Seats.AddRange(seats);
                    context.SaveChanges();

                    await Task.FromResult(0).ConfigureAwait(false);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);

                }).ConfigureAwait(false);
        }
    }
}