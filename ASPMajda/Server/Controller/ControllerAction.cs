using ASPMajda.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Controller
{
    class ControllerAction
    {
        public string Path { get; private set; }
        public Method Method { get; private set; }
        public Action Action { get; private set; }

        public ResponseMessage Response { get; private set; }

        public ControllerAction(Method method, string path, Action action)
        {
            this.Method = method;
            this.Path = path;

            this.Action = action;
        }

        public ResponseMessage Fire()
        {
            if (this.Action == null) return null;

            // wrap to return response message
            this.Action();

            return this.Response;
        }
    }
}
