using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SeatBookingSystem.Attributes;
using SeatBookingSystem.Common;
using SeatBookingSystem.Entities;
using SeatBookingSystem.Models;

namespace SeatBookingSystem.Controllers
{
    [Authorize]
    public class MeetupController : Controller
    {
        /// <summary>
        /// Gets the meet up
        /// </summary>
        /// <param name="location">The meetup location</param>
        /// <param name="time">The meetup time</param>
        /// <returns>The meetup</returns>
        [AllowAnonymous]
        public ActionResult Get(string location, DateTimeOffset time)
        {
            using (SeatBookingContext context = SeatBookingContext.Create())
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

                    //Currently we think min time for meetup is day, this can be changed with new requirement 
                    meetup = context.Meetups.FirstOrDefault(
                        m => m.Location == location && m.Time.ToString("yyyymmdd") == time.DateTime.ToString("yyyymmdd"));
                }

                return this.NewtonsoftJson(meetup);
            }
        }

        /// <summary>
        /// Creates the meetup
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="time">The time</param>
        /// <returns>The meetup</returns>
        public async Task<ActionResult> Create(string location, DateTimeOffset time)
        {
            using (SeatBookingContext context = SeatBookingContext.Create())
            {
                var meetup = new Meetup
                {
                    Location = location,
                    Time = time.DateTime,
                };
                context.Meetups.Add(meetup);

                List<string> seatNames = new List<string>();
                for (int c = 0; c < Consts.SeatRowNum; c++)
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

                await Task.FromResult(0).ConfigureAwait(false);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }
    }
}