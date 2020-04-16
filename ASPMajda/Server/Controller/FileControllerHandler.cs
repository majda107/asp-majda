using ASPMajda.Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server.Controller
{
    class FileControllerHandler : IControllerHandler
    {
        public string BasePath { get; private set; }
        public FileControllerHandler(string basePath)
        {
            this.BasePath = basePath;
        }
        public bool TryFire(RequestMessage request, out ResponseMessage response)
        {
            response = ResponseMessage.Error;

            var filepath = this.BasePath + request.Path;
            if (File.Exists(filepath))
            {
                //response = new StringResponseMessage(200, File.ReadAllText(filepath));
                response = new FileContentMessage(200, filepath);
                return true;
            }

            return false;
        }
    }
}
