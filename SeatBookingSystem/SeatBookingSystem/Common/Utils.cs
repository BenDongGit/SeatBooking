using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatBookingSystem.Common
{
    public static class Utils
    {
        public static bool IsNullOrEmpty(this IEnumerable<object> collections)
        {
            return collections == null || !collections.Any();
        }
    }
}