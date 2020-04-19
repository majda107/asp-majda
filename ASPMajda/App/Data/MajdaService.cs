using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.App.Data
{
    // A SIMPLE TESTING SERVICE
    class MajdaService
    {
        public string Name { get; set; }
        public MajdaService(string name)
        {
            this.Name = name;
        }
    }
}
