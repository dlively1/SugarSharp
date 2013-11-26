using System;
using System.Text;
using System.IO;
using RestSharp;
using RestSharp.Extensions;
using RestSharp.Deserializers;
using SugarRest.Model;
using SugarRest.Exceptions;

namespace SugarRest
{
    public partial class SugarClient
    {


        public bool UploadFile(string module, string id, string filePath, string fileName)
        {
            return UploadFile(module, id, filePath, fileName, false);
        }

        public bool UploadFile(string module, string id, string filePath, string fileName, bool deleteOnFailure)
        {
            var request = new RestRequest("{module}/{id}/file/{fileName}", Method.POST);
            request.AddUrlSegment("module", module);
            request.AddUrlSegment("id", id);
            request.AddUrlSegment("fileName", fileName);

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

            FileUploadResponse fileResponse = Execute<FileUploadResponse>(request);

            return !string.IsNullOrEmpty(fileResponse.name);
        }

    }
}