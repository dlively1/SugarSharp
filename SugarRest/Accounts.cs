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
        public AccountResult GetAccount()
        {
            var request = new RestRequest("Accounts", Method.GET);
            IRestResponse<AccountResult> response = client.Execute<AccountResult>(request);
            return response.Data;
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
            IRestResponse<Account> response = client.Execute<Account>(request);
            return response.Data;
        }

    }
}
