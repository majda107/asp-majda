using ASPMajda.Server.Actions;
using ASPMajda.Server.Content;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Controller
{
    class ControllerHandler : IControllerHandler
    {
        public Dictionary<string, ICollection<ControllerActionBase>> Methods { get; private set; }
        public ControllerHandler()
        {
            this.Methods = new Dictionary<string, ICollection<ControllerActionBase>>();
        }

        public void Register(ControllerActionBase action)
        {
            if (!this.Methods.ContainsKey(action.Path))
                this.Methods.Add(action.Path, new List<ControllerActionBase>());

            this.Methods[action.Path].Add(action);
        }

        public bool TryFire(RequestMessage request, out ResponseMessage message)
        {
            message = ResponseMessage.Error;
            if (request.Path == null) return false;

            if (!this.Methods.ContainsKey(request.Path)) return false;

            foreach (var action in this.Methods[request.Path])
            {
                if (action.Method != request.Method) continue;

                message = action.Fire(request.Body);
                return true;
            }

            return false;
        }
    }
}
