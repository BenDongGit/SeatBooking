namespace SeatBookingSystem.MvcExtensions
{
    using System.Net;
    using System.Net.Mime;
    using System.Web.Mvc;

    using SeatBookingSystem.Common;

    /// <summary>
    /// The exception handler attribute
    /// </summary>
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Handles the exception
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new ExceptionResult(filterContext.Exception);
            filterContext.ExceptionHandled = true;
        }
    }
}