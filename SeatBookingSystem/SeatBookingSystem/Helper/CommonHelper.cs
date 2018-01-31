namespace SeatBookingSystem.Helper
{
    using System;
    using System.Net;
    using System.Text;
    using System.Web.Mvc;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The dashboard helper
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// Creates a MVC action result with JSON.
        /// </summary>
        /// <param name="controller">The MVC controller.</param>
        /// <param name="model">The object to serialize.</param>
        /// <returns>The MVC action result.</returns>
        public static ActionResult NewtonsoftJsonResult(this Controller controller, object model)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            var json = ToJsonString(model, Formatting.None);
            return new ContentResult() { Content = json, ContentEncoding = Encoding.UTF8, ContentType = "application/json" };
        }

        /// <summary>
        /// No content result
        /// </summary>
        /// <param name="controller">The MVC controller.</param>
        /// <returns>The MVC action result.</returns>
        public static ActionResult NoContent(this Controller controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Converts an object to JSON string.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <returns>The JSON string.</returns>
        public static string ToJsonString(this object value)
        {
            return ToJsonString(value, Formatting.Indented);
        }

        /// <summary>
        /// Converts an object to JSON string.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="formatting">The JSON format.</param>
        /// <returns>The JSON string.</returns>
        private static string ToJsonString(object value, Formatting formatting)
        {
            var result = default(string);
            var text = value as string;
            if (value != null && text == null)
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new JsonConverter[] { new StringEnumConverter() },
                    Culture = null,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    Formatting = Formatting.None,
                    DateFormatString = "yyyy-MM-dd hh:mm:ss",
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                result = JsonConvert.SerializeObject(value, formatting, settings);
            }
            else if (!string.IsNullOrEmpty(text))
            {
                result = text;
            }

            return result;
        }
    }
}