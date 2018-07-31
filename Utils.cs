using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SampleDataGenerator
{
    public static class Utils
    {
        public static readonly string ConnectionString =
            XElement.Load("C:\\SF.Code\\C54\\RecruitHub\\Sabio.Web\\Web.config")
            .Element("connectionStrings")
            .Elements("add")
            .Where(e => e.Attribute("name").Value == "DefaultConnection")
            .Select(e => e.Attribute("connectionString").Value)
            .First();
    }
}

