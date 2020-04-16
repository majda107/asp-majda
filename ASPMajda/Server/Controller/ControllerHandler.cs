using ASPMajda.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Controller
{
    class ControllerHandler: IControllerHandler
    {
        public Dictionary<Method, ICollection<ControllerAction>> Methods { get; private set; }
        public ControllerHandler()
        {
            this.Methods = new Dictionary<Method, ICollection<ControllerAction>>();
        }

        public void Register(ControllerAction action)
        {
            if (!this.Methods.ContainsKey(action.Method))
                this.Methods.Add(action.Method, new List<ControllerAction>());

            this.Methods[action.Method].Add(action);
        }

        public bool TryFire(RequestMessage request, out ResponseMessage message)
        {
            message = ResponseMessage.Error;
            if (!this.Methods.ContainsKey(request.Method)) return false;

            foreach(var action in this.Methods[request.Method])
            {
                if (action.Path != request.Path) continue;

                message = action.Fire(request.Body);
                return true;
            }

            return false;
        }
    }
}
