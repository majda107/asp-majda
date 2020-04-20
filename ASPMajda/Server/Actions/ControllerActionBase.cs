using ASPMajda.Server.Content;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Actions
{
    abstract class ControllerActionBase
    {
        public string Path { get; private set; }
        public Method Method { get; private set; }

        public ControllerActionBase(Method method, string path)
        {
            this.Method = method;
            this.Path = path;
        }
        public abstract ResponseMessage Fire(MemoryContentBase content);
    }
}
