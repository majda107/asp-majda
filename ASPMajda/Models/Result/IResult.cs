using ASPMajda.Server.Content;
using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Models.Result
{
    interface IResult
    {
        public ResponseMessage GetResponseMessage();
    }
}
