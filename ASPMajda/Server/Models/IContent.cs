using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server.Models
{
    interface IContent
    {
        public Stream GetStream();
    }
}
