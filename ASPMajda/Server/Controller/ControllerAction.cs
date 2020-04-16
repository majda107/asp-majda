using ASPMajda.Server.Content;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Controller
{
    class ControllerAction
    {
        public delegate ResponseMessage ControllerActionDelegate(IContent body);

        public string Path { get; private set; }
        public Method Method { get; private set; }
        public ControllerActionDelegate Action { get; private set; }


        public ControllerAction(Method method, string path, ControllerActionDelegate action)
        {
            this.Method = method;
            this.Path = path;

            this.Action = action;
        }

        public ResponseMessage Fire(IContent body = null)
        {
            if (this.Action == null) return null;

            // wrap to return response message
            return this.Action(body);
        }
    }
}
