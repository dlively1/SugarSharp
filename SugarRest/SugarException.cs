using System;
using System.Net;
using RestSharp;

namespace SugarRest
{
    public class SugarException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public IRestResponse Response { get; private set; }

        public SugarException() { }

        public SugarException(string message)
            : base(message) { }

        public SugarException(IRestResponse response)
        {
            Response = response;
            StatusCode = response.StatusCode;
        }


    }
}
