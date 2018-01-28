using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SeatBookingSystem.Common
{
    /// <summary>
    /// The dashboard helper
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// The default JSON serialization settings.
        /// </summary>
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings()
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

        /// <summary>
        /// Creates a MVC action result with JSON.
        /// </summary>
        /// <param name="controller">The MVC controller.</param>
        /// <param name="model">The object to serialize.</param>
        /// <param name="settings">The JSON settings.</param>
        /// <returns>The MVC action result.</returns>
        public static ActionResult NewtonsoftJson(this Controller controller, object model, JsonSerializerSettings settings = null)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            var json = ToJsonString(model, Formatting.None, settings);
            return new ContentResult() { Content = json, ContentEncoding = Encoding.UTF8, ContentType = "application/json" };
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
        /// <param name="jsonSettings">The JSON settings.</param>
        /// <returns>The JSON string.</returns>
        private static string ToJsonString(object value, Formatting formatting, JsonSerializerSettings jsonSettings = null)
        {
            var result = default(string);
            var text = value as string;
            if (value != null && text == null)
            {
                var settings = jsonSettings == null ? DefaultJsonSerializerSettings : jsonSettings;
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