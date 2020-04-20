using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ASPMajda.Models.Result
{
    class MajdaView<T> : IResult
    {
        public string[] View { get; private set; }
        public T Model { get; private set; }
        public MajdaView(string viewPath, T model)
        {
            this.Model = model;

            if (!File.Exists(viewPath))
                this.View = null;
            else
                this.View = File.ReadAllLines(viewPath);
        }

        public ResponseMessage GetResponseMessage()
        {
            if (View == null)
                return ResponseMessage.Error;

            if(this.Model == null)
                return new StringResponseMessage(200, "Invalid model instance!");

            string type = String.Empty;
            var modelLine = this.View.First(line => line.StartsWith("@model"));
            var split = modelLine.Split(' ');
            if (modelLine != null && split.Length == 2)
                type = split[1];

            if (type.ToLower() != typeof(T).Name.ToString().ToLower())
                return new StringResponseMessage(200, "Invalid model type!");

            string view = String.Empty;
            foreach (var line in this.View)
            {
                var editLine = line;
                if (editLine.Contains("@model ")) continue;
                if (!editLine.Contains("@"))
                {
                    view += editLine + "\n";
                    continue;
                }

                foreach (var prop in typeof(T).GetProperties())
                    while (editLine.Contains($"@Model.{prop.Name.ToString()}"))
                        editLine = editLine.Replace($"@Model.{prop.Name.ToString()}", prop.GetValue(this.Model).ToString());

                view += editLine + "\n";
            }

            return new StringResponseMessage(200, view);
        }
    }
}
