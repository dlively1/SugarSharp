using System;
using System.Collections.Generic;
using System.Linq;

namespace SugarRest.Extensions
{
    /// <summary>
    /// Class for SugarCRM extension methods
    /// </summary>
    public static class SugarExtensions
    {

        /// <summary>
        /// Formats a DateTime object into a date string to be inserted through the REST API
        /// </summary>
        /// <param name="dt">A DateTime obeject</param>
        /// <returns>Formatted Date String [yyyy-MM-dd]</returns>
        public static string ToSugarDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Converts a DateTime object to UTC time and creates proper datetime string for REST API
        /// </summary>
        /// <param name="dt">DateTime object</param>
        /// <returns>UTC formatted string</returns>
        public static string ToSugarDateTimeString(this DateTime dt)
        {
            return dt.ToUniversalTime().ToString("o");
        }

        /// <summary>
        /// Format a list of multi-select options into string for Sugar REST API
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>SugarCRM formatted string representation of multi-select</returns>
        public static string ToSugarMultiSelect(this IList<string> values)
        {
            values = values.Select(value => "^" + value + "^").ToList();
            return string.Join(",", values);
        }

    }
}
