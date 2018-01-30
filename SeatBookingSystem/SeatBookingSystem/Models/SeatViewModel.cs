using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatBookingSystem.Models
{
    /// <summary>
    /// The seat view model
    /// </summary>
    public class SeatViewModel
    {
        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The owner.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The email
        /// </summary>
        public string Email { get; set; }
    }
}