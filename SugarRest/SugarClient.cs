﻿using System;
using System.Collections.Generic;
using RestSharp;
using SugarRest.Model;
using SugarRest.Deserializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

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
        /// Oauth Refresh token for authentication
        /// </summary>
        private string RefreshToken { get; set; }

        /// <summary>
        /// RestClient to handle API interactions
        /// </summary>
        private RestClient client;

        public SugarException SugarException { get; set; }

        /// <summary>
        /// Used in serialization of unknown responses
        /// </summary>
        private DynamicJsonDeserializer dynamicSerializer;

        /// <summary>
        /// Constructor to create new API Client
        /// </summary>
        /// <param name="url">URL to rest end point</param>
        /// <param name="username">SugarCRM Username</param>
        /// <param name="password">SugarCRM Password</param>
        public SugarClient(string url, string username, string password) : this()
        {
            BaseUrl = url;
            Username = username;
            Password = password;

            client.BaseUrl = BaseUrl;
            Login();//avoid breaking change ... @todo - client creation will be moved into factory method
        }

        /// <summary>
        /// Constructor to create client given a token
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        public SugarClient(string url, string token) : this()
        {
            BaseUrl = url;
            client.BaseUrl = BaseUrl;
            Token = token;
        }

        /// <summary>
        /// Private constructor
        /// Performs base setup
        /// </summary>
        private SugarClient()
        {
            client = new RestClient();
            client.UserAgent = "SugarSharp";
            dynamicSerializer = new DynamicJsonDeserializer();
        }

        public void Login()
        {
            var request = new RestRequest("oauth2/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", "sugar");
            request.AddParameter("username", Username);
            request.AddParameter("password", Password);
            request.AddParameter("platform", "api");

            IRestResponse<TokenResponse> tokenResponse = client.Execute<TokenResponse>(request);

            if (string.IsNullOrEmpty(tokenResponse.Data.access_token))
            {
                throw new SugarException("Auth Failed did not retrieve access token");
            }

            Token = tokenResponse.Data.access_token;
            RefreshToken = tokenResponse.Data.refresh_token;
        }

        public string GetToken()
        {
            return Token;
        }


        public T Execute<T>(RestRequest request) where T : new()
        {

            if (string.IsNullOrEmpty(Token))
            {
                throw new SugarException("Invalid Token Exception");
            }

            request.AddHeader("OAuth-Token", Token);

            request.OnBeforeDeserialization = (resp) =>
            {
                if(((int) resp.StatusCode) >= 400)
                {
                    ErrorResponse error = JsonConvert.DeserializeObject<ErrorResponse>(resp.Content);
                    throw new SugarException(error.error_message);
                }
            };

            var response = client.Execute<T>(request);
            return response.Data;
        }

        /// <summary>
        /// Internal Execute Async handler
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="request">Request to be sent</param>
        /// <param name="callback">Callback Function to execute on completion</param>
        public virtual void ExecuteAsync<T>(RestRequest request, Action<bool,T> callback) where T : new()
        {
            if (string.IsNullOrEmpty(Token))
            {
                throw new SugarException("Invalid Token Exception");
            }

            request.AddHeader("OAuth-Token", Token);

            request.OnBeforeDeserialization = (resp) =>
	    {
		if (((int)resp.StatusCode) >= 400)
		{
                    //@todo -- determine better way to handle async failures
                    resp.Content = null;
                }
	    };


            client.ExecuteAsync<T>(request, (response) => { callback((int)response.StatusCode < 400, response.Data); });
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
        /// /// <summary>
        /// Sample API endpoint with token, good for testing
        /// </summary>
        /// <returns>String "pong"</returns>
        /// </summary>
        /// <returns></returns>
        public string GetPingWithToken()
        {
            var request = new RestRequest("ping", Method.GET);

            if (string.IsNullOrEmpty(Token))
            {
                throw new SugarException("Invalid Token Exception");
            }

            request.AddHeader("OAuth-Token", Token);

            IRestResponse response = Execute(request);
            return response.Content;
        }

        /// <summary>
        /// Expire the users token
        /// </summary>
        public void Logout()
        {
            var request = new RestRequest("oauth2/logout", Method.POST);
            client.Execute(request);

            Token = string.Empty;
        }

        /// <summary>
        /// Gets a list of records for the given module
        /// </summary>
        /// <typeparam name="T">Results Object to be serialized</typeparam>
        /// <param name="module">string module name</param>
        /// <returns></returns>
        public BeanListResponse<T> GetBeans<T>(string module) where T : new ()
        {
            SearchOptions options = new SearchOptions();
            options.max_num = 20;

            return GetBeans<T>(module, options);
        }

        /// <summary>
        /// Gets a list of records for the given module
        /// </summary>
        /// <typeparam name="T">Bean Type</typeparam>
        /// <param name="module">Module Name</param>
        /// <param name="options">SearchOptions object</param>
        /// <returns>next offset and list of records as BeanListResponse object</returns>
        public BeanListResponse<T> GetBeans<T>(string module, SearchOptions options) where T : new()
        {
            var request = new RestRequest(module, Method.GET);

            //Handle query string paramater options
            if (! string.IsNullOrEmpty(options.query))
                request.AddParameter("q", options.query);

            if (!string.IsNullOrEmpty(options.filter))
            {
                request.AddParameter("filter", options.filter);
            }

            if (options.max_num.HasValue && options.max_num.Value > 0)
                request.AddParameter("max_num", options.max_num);

            if (options.favorites)
                request.AddParameter("favorites", options.favorites);

            if (options.deleted)
                request.AddParameter("deleted", options.deleted);

            if (options.offset.HasValue && options.offset.Value > 0)
                request.AddParameter("offset", options.offset);

            if (! string.IsNullOrEmpty(options.fields))
                request.AddParameter("fields", options.fields);

            if (! string.IsNullOrEmpty(options.order_by))
                request.AddParameter("order_by", options.order_by);


            return Execute<BeanListResponse<T>>(request);
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
        /// Create a record async. Accepts lambda to process the results upon completion
        /// </summary>
        /// <param name="module">Name of the module</param>
        /// <param name="record">anonymous object</param>
        /// <param name="callback">Method to call upon completion. First paramater tells if request was sucessful</param>
        public void CreateAsync(string module, object record, Action<bool, String> callback)
        {
            var request = new RestRequest("{module}", Method.POST);
            request.AddUrlSegment("module", module);

            addParamatersFromObject(request, record);

            ExecuteAsync<Bean>(request, (success, response) => { callback(success, response.id); });
        }

        /// <summary>
        /// Updates an individual record
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="id">Records ID</param>
        /// <param name="record">anonymous object</param>
        /// <returns>Record Id</returns>
        public string Update(string module, string id, object record)
        {
            var request = new RestRequest("{module}/{id}", Method.PUT);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(record);

            Bean bean = Execute<Bean>(request);
            return bean.id;
        }

        /// <summary>
        /// Update a record async
        /// </summary>
        /// <param name="module">Module name</param>
        /// <param name="id">record Id</param>
        /// <param name="record">anonymous object defining record</param>
        /// <param name="callback">method called upon completion</param>
        public void UpdateAsync(string module,string id, object record, Action<bool, String> callback)
        {
            var request = new RestRequest("{module}/{id}", Method.PUT);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(record);
            ExecuteAsync<Bean>(request, (success, response) => { callback(success, response.id); });
        }

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="id">Record ID</param>
        /// <returns>ID of deleted record</returns>
        public string Delete(string module, string id)
        {
            var request = new RestRequest("{module}/{id}", Method.DELETE);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);

            Bean bean = Execute<Bean>(request);
            return bean.id;
        }

        /// <summary>
        /// Delete a record async
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="id">Record ID</param>
        /// <param name="callback">method called upon completion</param>
        public void DeleteAsync(string module, string id, Action<bool, String> callback)
        {
            var request = new RestRequest("{module}/{id}", Method.DELETE);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);

            ExecuteAsync<Bean>(request, (success, response) => { callback(success, response.id); });
        }

        /// <summary>
        /// Create a relationship between two existing records
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="parentID">Parent Record ID</param>
        /// <param name="relationshipName">Link name between modules</param>
        /// <param name="childID">Child Record ID</param>
        /// <returns>Related Record ID</returns>
        public string Link(string module, string parentID, string relationshipName, string childID)
        {
            var request = new RestRequest("{module}/{parentID}/link/{link}/{childID}", Method.POST);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("parentID", parentID);
            request.AddUrlSegment("link", relationshipName);
            request.AddUrlSegment("childID", childID);


            LinkSetResult result = Execute<LinkSetResult>(request);
            return result.related_record.id;
        }

        public string UpdateLink(string module, string parentID, string relationshipName, string childID, object relatedRecord)
        {
            var request = new RestRequest("{module}/{parentID}/link/{link}/{childID}", Method.PUT);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("parentID", parentID);
            request.AddUrlSegment("link", relationshipName);
            request.AddUrlSegment("childID", childID);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(relatedRecord);

            LinkSetResult result = Execute<LinkSetResult>(request);
            return result.related_record.id;
        }

        /// <summary>
        /// Relate record to parent record
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="parentID">Parent Record ID</param>
        /// <param name="relationshipName">Link Name</param>
        /// <param name="record">Annonoymous Object Record</param>
        /// <returns>Record ID</returns>
        public string Link(string module, string parentID, string relationshipName, object record)
        {
            var request = new RestRequest("{module}/{parentID}/link/{link}", Method.POST);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("parentID", parentID);
            request.AddUrlSegment("link", relationshipName);

            addParamatersFromObject(request, record);

            LinkSetResult result = Execute<LinkSetResult>(request);
            return result.related_record.id;
        }

        /// <summary>
        /// Remove an Existing Relationship
        /// </summary>
        /// <param name="module">Parent Module Name</param>
        /// <param name="parentID">Parent Record ID</param>
        /// <param name="relationshipName">Link Name</param>
        /// <param name="childID">Child Record ID</param>
        /// <returns>Child Record ID</returns>
        public string RemoveLink(string module, string parentID, string relationshipName, string childID)
        {
            var request = new RestRequest("{module}/{parentID}/link/{link}/{childID}", Method.DELETE);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("parentID", parentID);
            request.AddUrlSegment("link", relationshipName);
            request.AddUrlSegment("childID", childID);

            LinkSetResult result = Execute<LinkSetResult>(request);
            return result.related_record.id;
        }

        /// <summary>
        /// Gets a Sugar dropdown list
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="field">field name</param>
        /// <returns> SugarList object </returns>
        public SugarList GetList(string module, string field)
        {
            var request = new RestRequest("{module}/enum/{field}", Method.GET);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("field", field);


            var response = Execute(request);
            var list = dynamicSerializer.Deserialize<JObject>(response);

            return new SugarList(list.ToObject<Dictionary<string, string>>());
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
