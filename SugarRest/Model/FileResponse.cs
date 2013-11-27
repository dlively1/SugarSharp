using System;

namespace SugarRest.Model
{
    public class FileResponse
    {
        public string content_type { get; set; }
        public string content_length { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
    }
}
