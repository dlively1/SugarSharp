using System;
using System.Text;
using System.IO;
using RestSharp;
using RestSharp.Extensions;
using RestSharp.Deserializers;
using SugarRest.Model;
using SugarRest.Exceptions;
using Newtonsoft.Json.Linq;

namespace SugarRest
{
    public partial class SugarClient
    {
        /// <summary>
        /// Upload a file to a note record. 
        /// </summary>
        /// <param name="module">Module name (Notes)</param>
        /// <param name="id">record ID</param>
        /// <param name="filePath">Path to file on local disk</param>
        /// <returns>Bool</returns>
        public bool UploadFile(string module, string id, string filePath)
        {
            return UploadFile(module, id, filePath, false);
        }

        /// <summary>
        /// Uploads a file, if the request fails this overload will delete the note record
        /// </summary>
        /// <param name="module">Notes</param>
        /// <param name="id">Record ID</param>
        /// <param name="filePath">Path to file on local disk</param>
        /// <param name="deleteOnFailure">Bool: deletes record if upload fails</param>
        /// <returns>Bool</returns>
        public bool UploadFile(string module, string id, string filePath, bool deleteOnFailure)
        {
            var request = new RestRequest("{module}/{id}/file/filename", Method.POST);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);

            request.AddParameter("format", "sugar-html-json");
            request.AddParameter("delete_if_fails", deleteOnFailure);

            if (deleteOnFailure)
            {
                request.AddParameter("oauth_token", Token);
            }

            if (!File.Exists(filePath))
            {
                //@todo create a SugarFileException class
                throw new SugarException("Can not locate file path. Path attempted = " + filePath);
            }

            request.AddFile("filename", filePath);//?


            FileUploadResponse response = Execute<FileUploadResponse>(request);

            return ! string.IsNullOrEmpty(response.filename.name);
        }

    }
}