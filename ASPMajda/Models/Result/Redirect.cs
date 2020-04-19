using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Models.Result
{
    class Redirect : IResult
    {
        public string Url { get; set; }
        public Redirect(string url)
        {
            this.Url = url;
        }
        public ResponseMessage GetResponseMessage()
        {
            var response = new ResponseMessage(301);
            response.Headers.SetHeader("Location", this.Url);
            response.Headers.SetHeader("Content-Type", "text/html");

            return response;
        }
    }
}
