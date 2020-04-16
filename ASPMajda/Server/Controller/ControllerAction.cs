using ASPMajda.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Controller
{
    class ControllerAction
    {
        public delegate ResponseMessage ControllerActionDelegate(string body);

        public string Path { get; private set; }
        public Method Method { get; private set; }
        public ControllerActionDelegate Action { get; private set; }


        public ControllerAction(Method method, string path, ControllerActionDelegate action)
        {
            this.Method = method;
            this.Path = path;

            this.Action = action;
        }

        public ResponseMessage Fire(string body = null)
        {
            if (this.Action == null) return null;

            // wrap to return response message
            return this.Action(body);
        }
    }
}
