namespace SeatBookingSystem.MvcExtensions
{
    using System;
    using System.Net;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// The exception result
    /// </summary>
    public class ExceptionResult : ActionResult
    {
        private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionResult"/> class
        /// </summary>
        /// <param name="e">The exception</param>
        public ExceptionResult(Exception e)
        {
            this.exception = e;
        }

        /// <summary>
        /// The execute result.
        /// </summary>
        /// <param name="context">The controller context</param>
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;

            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ContentType = MediaTypeNames.Text.Plain;

            string msg = this.exception.Message;
            if (this.exception is AggregateException)
            {
                msg += ":<br>";
                AggregateException ae = (AggregateException)this.exception;

                foreach (var e in ae.Flatten().InnerExceptions)
                {
                    msg += e.Message + "<br>";
                }
            }

            response.Write(msg);
        }
    }
}