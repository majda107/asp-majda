using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Models
{
    class ResponseMessage
    {
        public static ResponseMessage Error = new StringResponseMessage(404, "<html><body><h1>Couldn't find endpoint...</h1></body></html>");

        public int StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public virtual IContent Content { get => null; set { value = null; } }

        public ResponseMessage(int statusCode)
        {
            this.StatusCode = statusCode;
            this.Headers = new Dictionary<string, string>();
        }
    }
}
