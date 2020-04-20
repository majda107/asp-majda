using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server.Controller
{
    class PublicFolderControllerHandler : IControllerHandler
    {
        public string EndpointPath { get; set; }
        public string DirectoryPath { get; set; }
        public PublicFolderControllerHandler(string endpointPath, string directoryPath)
        {
            this.EndpointPath = endpointPath;
            this.DirectoryPath = directoryPath;
        }
        public bool TryFire(RequestMessage request, out ResponseMessage response)
        {
            response = ResponseMessage.Error;
            if (!request.Path.StartsWith(this.EndpointPath)) return false;

            var path = this.DirectoryPath + request.Path;
            if (Directory.Exists(path))
            {
                string ul = "<ul>";
                foreach (var file in Directory.GetFiles(path))
                    ul += $"<li><a href=\"{request.Path + "/" + Path.GetFileName(file)}\">{Path.GetFileName(file)}</a></li>";

                foreach(var folder in Directory.GetDirectories(path))
                {
                    var folderName = folder.Replace(path, "");
                    ul += $"<li><a href=\"{request.Path + folderName}\">{folderName}</a></li>";
                }
                    
                ul += "</ul>";


                response = new StringResponseMessage(200, $"<html><head></head><body>{ul}</body></html>");
                return true;
            }
            if (File.Exists(path))
            {
                response = new FileContentMessage(200, path);
                return true;
            }

            return false;
        }
    }
}
