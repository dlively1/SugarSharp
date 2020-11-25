using System;
using System.Collections.Generic;

namespace SugarRest
{
    /// <summary>
    /// https://support.sugarcrm.com/Documentation/Sugar_Developer/Sugar_Developer_Guide_10.2/Integration/Web_Services/REST_API/Endpoints/modulefilter_GET/
    /// </summary>
    public class SearchOptions
    {
        /// <summary>
        /// Search Query String
        /// </summary>
        public string query { get; set; }

        /// <summary>
        /// Filter expression
        /// </summary>
        public string filter { get; set; }

        /// <summary>
        /// Max number of results returned
        /// </summary>
        public int? max_num { get; set; }

        /// <summary>
        /// Offset of request (used in pagination)
        /// </summary>
        public int? offset { get; set; }

        /// <summary>
        /// Only retrieve favorite records
        /// </summary>
        public bool favorites { get; set; }

        /// <summary>
        /// Comma seperated list of fields to return
        /// </summary>
        public string fields { get; set; }

        /// <summary>
        /// Return delete records (records marked with soft-delete flag)
        /// </summary>
        public bool deleted { get; set; }

        /// <summary>
        /// comma delimited list with the direction appended to the column name after a colon 
        /// example first_name:DESC
        /// </summary>
        public string order_by { get; set; }
    }
}
