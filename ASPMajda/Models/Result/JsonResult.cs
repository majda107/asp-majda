using ASPMajda.Server.Content;
using ASPMajda.Server.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Models.Result
{
    class JsonResult : IResult
    {
        public object Instance { get; private set; }
        public JsonResult(object instance)
        {
            this.Instance = instance;
        }

        public ResponseMessage GetResponseMessage()
        {
            var content = new JsonContent(JsonConvert.SerializeObject(this.Instance));

            return new ContentResponseMessage(200, content);
        }
    }
}
