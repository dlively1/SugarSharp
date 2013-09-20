using System;
using System.Text;
using RestSharp;
using RestSharp.Extensions;
using RestSharp.Deserializers;
using SugarRest.Model;

namespace SugarRest
{
    public partial class SugarClient
    {
        /// <summary>
        /// Retrieve list of Accounts
        /// </summary>
        /// <returns></returns>
        public AccountResult GetAccounts()
        {
            var request = new RestRequest("Accounts", Method.GET);

            return Execute<AccountResult>(request);
        }

        /// <summary>
        /// Search for Accounts
        /// </summary>
        /// <param name="query">Search string for accounts if no full text search defaults to name</param>
        /// <returns></returns>
        public AccountResult GetAccounts(string query)
        {
            var request = new RestRequest("Accounts?q={query}", Method.GET);
            request.AddUrlSegment("query", query);

            return Execute<AccountResult>(request);
        }

        /// <summary>
        /// Search for Accounts
        /// </summary>
        /// <param name="query">Search string for accounts if no full text search defaults to name</param>
        /// <param name="fields">comma seperates list of fields to return</param>
        /// <returns></returns>
        public AccountResult GetAccounts(string query, string fields)
        {
            var request = new RestRequest("Accounts?q={query}&fields={fields}", Method.GET);
            request.AddUrlSegment("query", query);
            request.AddUrlSegment("fields", fields);

            return Execute<AccountResult>(request);
        }

        /// <summary>
        /// Retrieve a given Account
        /// </summary>
        /// <param name="id">string ID of the Account</param>
        /// <returns>Account Bean</returns>
        public Account GetAccount(string id)
        {
            var request = new RestRequest("Accounts/{id}", Method.GET);
            request.AddUrlSegment("id", id);

            return Execute<Account>(request);
        }

    }
}
