using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server.Content
{
    class StringContent : IContent
    {
        public string Value { get; set; }
        public StringContent(string value)
        {
            this.Value = value;
        }
        public Stream GetStream()
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(this.Value);
            sw.Flush();
            ms.Position = 0;
            return ms;
        }
    }
}
