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
        /// BaseURL of SugarCRM Instance
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// SugarCRM Username
        /// </summary>
        private string Username { get; set; }

        /// <summary>
        /// SugarCRM Password
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// Oauth Token for authentication on requests to the SugarCRM API
        /// </summary>
        private string Token { get; set; }

        /// <summary>
        /// RestClient to handle API interactions
        /// </summary>
        private RestClient client;

        /// <summary>
        /// Constructor to create new API Client
        /// </summary>
        /// <param name="url">URL to rest end point</param>
        /// <param name="username">SugarCRM Username</param>
        /// <param name="password">SugarCRM Password</param>
        public SugarClient(string url, string username, string password)
        {
            BaseUrl = url;
            Username = username;
            Password = password;

            client = new RestClient();
            client.UserAgent = "SugarSharp";
            client.BaseUrl = BaseUrl;

            var request = new RestRequest("oauth2/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", "sugar");
            request.AddParameter("username", Username);
            request.AddParameter("password", Password);

            IRestResponse<TokenResponse> tokenResponse = client.Execute<TokenResponse>(request);

            if(string.IsNullOrEmpty(tokenResponse.Data.access_token))
            {
                //@todo - handle exception here
            }

            Token = tokenResponse.Data.access_token;
            client.AddDefaultHeader("OAuth-Token",Token);

        }

        /// <summary>
        /// Sample API endpoint, good for testing
        /// </summary>
        /// <returns>String "pong"</returns>
        public string GetPing()
        {
            var request = new RestRequest("ping", Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content;    
        }

        /// <summary>
        /// Retrieves collection of basic bean properties for the given module
        /// </summary>
        /// <param name="module">Name of the module ex. ("Accounts")</param>
        /// <returns>List of Bean records</returns>
        public BeanResponse GetBean(string module) 
        {
            var request = new RestRequest(module, Method.GET);
            IRestResponse<BeanResponse> response = client.Execute<BeanResponse>(request);
            return response.Data;
        }

        /// <summary>
        /// Gets generic bean properties for a given ID
        /// </summary>
        /// <param name="module">Name of the module ex. ("Accounts")</param>
        /// <param name="id">string ID</param>
        /// <returns>SugarBean</returns>
        public Bean GetBean(string module, string id)
        {
            var request = new RestRequest("{module}/{id}", Method.GET);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);
            IRestResponse<Bean> response = client.Execute<Bean>(request);
            return response.Data;
        }

    }
}
