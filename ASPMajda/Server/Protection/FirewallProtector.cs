using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Protection
{
    class FirewallProtector: IProtector
    {
        public delegate bool RuleDelegate(RequestMessage request);
        public List<RuleDelegate> Rules { get; private set; }

        public FirewallProtector()
        {
            this.Rules = new List<RuleDelegate>();
        }

        public void AddRule(RuleDelegate rule)
        {
            this.Rules.Add(rule);
        }

        public bool Allowed(RequestMessage request)
        {
            foreach (var rule in this.Rules)
                if (rule != null && !(rule.Invoke(request))) return false;

            return true;
        }
    }
}
