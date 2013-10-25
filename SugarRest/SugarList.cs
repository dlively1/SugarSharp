using System.Collections.Generic;
using System.Linq;

namespace SugarRest
{
    public class SugarList
    {
        /// <summary>
        /// Represents SugarCRM dropdown list key (system key) value (display label)
        /// </summary>
        protected Dictionary<string, string> list;
        
        /// <summary>
        /// Create new SugarList
        /// </summary>
        /// <param name="list">Key Value dropdown list dictionary</param>
        public SugarList(Dictionary<string, string> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Retrieve list of system keys for a Sugar dropdown list
        /// </summary>
        /// <returns>Array of system keys</returns>
        public List<string> GetSystemKeys()
        {
            return list.Keys.ToList();
        }

        /// <summary>
        /// Retrieve list of display values for a dropdown
        /// </summary>
        /// <returns></returns>
        public List<string> GetDisplayValues()
        {
            return list.Values.ToList();
        }

        /// <summary>
        /// Check if a value is valid list option
        /// </summary>
        /// <param name="value">String</param>
        /// <returns>bool</returns>
        public bool KeyExists(string value)
        {
            return list.ContainsKey(value);
        }

        /// <summary>
        /// Create a SugarCRM formatted multi-select string
        /// </summary>
        /// <param name="values">List of Values</param>
        /// <returns>Formatted String</returns>
        public static string CreateMultiSelect(IList<string> values)
        {
            values = values.Select(value => "^" + value + "^").ToList();
            return string.Join(",", values);
        }

    }
}
