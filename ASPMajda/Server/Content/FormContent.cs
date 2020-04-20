using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ASPMajda.Server.Content
{
    class FormContent : MemoryContentBase
    {
        private readonly string _content;
        public Dictionary<string, string> Form { get; private set; }
        public FormContent(string content)
        {
            this._content = content;
            this.Form = new Dictionary<string, string>();

            this.Parse(content);
        }

        public FormContent()
        {
            this.Form = new Dictionary<string, string>();
        }

        private void Parse(string content)
        {
            var kvp = content.Split('&');
            if (kvp.Length <= 0) return;

            foreach(var val in kvp)
            {
                var split = val.Split('=');
                if (split.Length <= 1) continue;

                if (!this.Form.ContainsKey(split[0]))
                    this.Form.Add(split[0], "");

                this.Form[split[0]] = split[1];
            }
        }

        public string this[string key]
        {
            get
            {
                if (this.Form.ContainsKey(key)) return this.Form[key];
                return "";
            }
        }
        public override string GetMime()
        {
            return "x-www-form-urlencoded";
        }

        public override MemoryStream GetStream()
        {
            var content = String.Empty;

            foreach (var kvp in this.Form)
                content += $"{kvp.Key}={kvp.Value}&";

            if (content.Length > 0) content = content.Substring(0, content.Length - 1);

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.WriteLine(content);
            sw.Flush();

            ms.Position = 0;
            return ms;
        }

        public override object GetMvcResult(MethodInfo action, object controllerInstance)
        {
            if (!this.MvcMethodValid(action, typeof(FormContent))) return null;

            return action.Invoke(controllerInstance, new object[] { this });
        }
    }
}
