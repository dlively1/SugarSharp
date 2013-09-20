using System;
using System.Collections.Generic;

namespace SugarRest.Model
{
    public class Bean
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime date_entered { get; set; }
        public DateTime date_modified { get; set; }
        public string modified_user_id { get; set; }
        public string modified_by_name { get; set; }
        public string created_by { get; set; }
        public string created_by_name { get; set; }
        public string description { get; set; }
        public bool deleted { get; set; }
        public string assigned_user_id { get; set; }
        public string assigned_user_name { get; set; }
        public List<Team> team_name { get; set; }

    }

}
