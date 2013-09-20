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

        public SugarException SugarException { get; set; }

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
                throw new SugarException("Auth Failed did not retrieve access token");   
            }

            Token = tokenResponse.Data.access_token;
            client.AddDefaultHeader("OAuth-Token",Token);

        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            request.OnBeforeDeserialization = (resp) =>
            {
                if(((int) resp.StatusCode) >= 400)
                {
                    throw new SugarException(resp.ErrorMessage);
                }
            };

            var response = client.Execute<T>(request);
            return response.Data;
        }

        /// <summary>
        /// Executes the request
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns></returns>
        public IRestResponse Execute(IRestRequest request)
        {
            return client.Execute(request);
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
        /// Gets a Bean for the given module
        /// </summary>
        /// <typeparam name="T">Results Object to be serialized</typeparam>
        /// <param name="module">string module name</param>
        /// <returns></returns>
        public T GetBean<T>(string module) where T : new()
        {
            var request = new RestRequest(module, Method.GET);
            return Execute<T>(request);
        }

        /// <summary>
        /// Gets generic bean properties for a given ID
        /// </summary>
        /// <param name="module">Name of the module ex. ("Accounts")</param>
        /// <param name="id">string ID</param>
        /// <returns>SugarBean</returns>
        public T GetBean<T>(string module, string id) where T : new ()
        {
            var request = new RestRequest("{module}/{id}", Method.GET);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);

            return Execute<T>(request);
        }

        /// <summary>
        /// Creates a record
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="record">anonymous object</param>
        /// <returns></returns>
        public string Create(string module, object record)
        {
            var request = new RestRequest("{module}", Method.POST);
            request.AddUrlSegment("module", module);

            addParamatersFromObject(request, record);            

            Bean bean = Execute<Bean>(request);
            return bean.id;
        }

        /// <summary>
        /// Updates an individual record
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="id">Records ID</param>
        /// <param name="record">anonymous object</param>
        /// <returns></returns>
        public string Update(string module, string id, object record)
        {
            var request = new RestRequest("{module}/{id}", Method.PUT);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);

            addParamatersFromObject(request, record);

            Bean bean = Execute<Bean>(request);
            return bean.id;
        }

        /// <summary>
        /// Converts anonymous object into key value paramaters for Request
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="record">anonymous object of properties</param>
        private void addParamatersFromObject(IRestRequest request, object record)
        {
            var type = record.GetType();
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                request.AddParameter(prop.Name, prop.GetValue(record,null));
            }
        }



    }
}
