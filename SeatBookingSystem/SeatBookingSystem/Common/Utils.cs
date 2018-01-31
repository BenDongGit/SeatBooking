namespace SeatBookingSystem.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// The utilities.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Check if the collections is null or empty
        /// </summary>
        /// <param name="collections">The collections</param>
        public static bool IsNullOrEmpty(this IEnumerable<object> collections)
        {
            return collections == null || !collections.Any();
        }

        /// <summary>
        /// Runs async
        /// </summary>
        public static async Task RunAsync()
        {
            await Task.FromResult(0).ConfigureAwait(false);
        }
    }
}