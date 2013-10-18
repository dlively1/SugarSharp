using System;
using System.Text;
using RestSharp;
using RestSharp.Extensions;
using RestSharp.Deserializers;
using SugarRest.Model;
using SugarRest.Exceptions;

namespace SugarRest
{
    public partial class SugarClient
    {

        /// <summary>
        /// Favorite a record for the current user
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="id">Record ID</param>
        /// <returns>Record ID</returns>
        public string Favorite(string module, string id)
        {
            var request = new RestRequest("{module}/{id}/favorite", Method.PUT);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);

            Bean bean = Execute<Bean>(request);
            return bean.id;
        }

        /// <summary>
        /// Remove record as favorite
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="id">Record ID</param>
        /// <returns>Record ID</returns>
        public string UnFavorite(string module, string id)
        {
            var request = new RestRequest("{module}/{id}/favorite", Method.DELETE);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);

            Bean bean = Execute<Bean>(request);
            return bean.id;
        }


    }
}
