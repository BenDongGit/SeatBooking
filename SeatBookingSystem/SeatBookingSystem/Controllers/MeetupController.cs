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

        public ActionResult Create()
        {
            throw new NotImplementedException();
        }
    }
}