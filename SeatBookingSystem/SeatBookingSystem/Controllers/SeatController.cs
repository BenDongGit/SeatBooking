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

namespace SeatBookingSystem.Controllers
{
    /// <summary>
    /// The seat controller
    /// </summary>
    [Authorize]
    [ExceptionHandler]
    public class SeatController : Controller
    {
        private static SeatBookingContext context = SeatBookingContext.Create();
        private static Lazy<EntityRepository<Seat>> lazySeatRepo = new Lazy<EntityRepository<Seat>>(() => EntityRepository<Seat>.Create(context));

        public SeatController()
        {
            if (lazySeatRepo.Value.GetAll().Count < 1)
            {
                InitSeats(Consts.SeatRowNum, Consts.SeatRowNum);
            }
        }

        /// <summary>
        /// The index entry
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the booked seats.
        /// </summary>
        /// <returns>The all seats</returns>
        [HttpGet]
        public async Task<ActionResult> GetBookedSeats()
        {
            var allBookedSeats = lazySeatRepo.Value.GetEntities(seat => !string.IsNullOrEmpty(seat.OwnerId));
            string userId = GetCurrentUserId();
            var seatsOfUser = allBookedSeats.Where(seat => seat.OwnerId == userId).ToList(); ;
            await Task.FromResult(0).ConfigureAwait(false);

            var result = new
            {
                SeatsBookedByOthers = allBookedSeats.Except(seatsOfUser).ToList(),
                SeatsBookedByCurrentUser = seatsOfUser
            };

            return this.NewtonsoftJson(result);
        }

        /// <summary>
        /// Books the seats.
        /// </summary>
        /// <param name="bookingSeatIds">The seats that are being booked.</param>
        /// <param name="releasingSeatIds">The seats that are being released.</param>
        /// <returns>The update result</returns>
        [HttpPost]
        public async Task<ActionResult> Update(string[] bookingSeatIds, string[] releasingSeatIds)
        {
            var booingSeats = new List<Seat>();
            var releasingSeats = new List<Seat>();

            if (bookingSeatIds != null && bookingSeatIds.Length > 0)
            {
                string userId = this.GetCurrentUserId();
                booingSeats = lazySeatRepo.Value.GetEntities(seat => bookingSeatIds.Contains(seat.ID)).ToList();
                if (booingSeats.Any(seat => seat.OwnerId != userId && !string.IsNullOrEmpty(seat.OwnerId)))
                {
                    var names = new StringBuilder();
                    var seatsOfOthers = booingSeats.Where(seat => seat.OwnerId != userId).ToList();
                    seatsOfOthers.ForEach(s => names.Append(string.Format("[{0}]", s.ID)));
                    string errorMsg = string.Format("{0} has/have already been booked by others, please book other seats!", booingSeats.ToString());
                    throw new InvalidOperationException(errorMsg);
                }

                booingSeats.ForEach(seat => seat.OwnerId = userId);
            }

            if (releasingSeatIds != null && releasingSeatIds.Length > 0)
            {
                releasingSeats = lazySeatRepo.Value.GetEntities(seat => releasingSeatIds.Contains(seat.ID)).ToList();
                releasingSeats.ForEach(seat => seat.OwnerId = null);
            }

            context.SaveChanges();

            await Task.FromResult(0).ConfigureAwait(false);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        /// <summary>
        /// Initialize the seats
        /// </summary>
        /// <returns>The seats</returns>
        private void InitSeats(int seatRow, int seatColumn)
        {
            List<string> seatIds = new List<string>();
            for (int c = 0; c < seatColumn; c++)
            {
                for (int r = 1; r <= seatRow; r++)
                {
                    seatIds.Add(string.Format("{0}{1}", Consts.ColSeatNames[c], r));
                }
            }

            var seats = seatIds.Select(id => new Seat { ID = id }).ToList();
            lazySeatRepo.Value.AddEntites(seats);
        }

        /// <summary>
        /// Gets the current user identity.
        /// </summary>
        /// <returns>The current user identity</returns>
        private string GetCurrentUserId()
        {
            var userName = this.HttpContext.User.Identity.Name;
            var userId = context.Users.Where(x => x.UserName == userName).FirstOrDefault().Id;

            return userId;
        }
    }
}