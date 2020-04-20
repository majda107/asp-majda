using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Models.Result
{
    class Ok : IResult
    {
        public string Status { get; private set; }
        public Ok(string status = "OK")
        {
            this.Status = status;
        }
        public ResponseMessage GetResponseMessage()
        {
            return new ResponseMessage(200, this.Status);
        }
    }
}
