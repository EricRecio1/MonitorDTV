using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;

namespace Helper
{
    public class ConfigurationHelper
    {
        public static string GetValue(string seccion , string value)
        {
            NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(seccion);
            return config.Get(value);
        }
        //private static NameValueCollection GetValues(String seccion)
        //{
        //    return (NameValueCollection)ConfigurationManager.GetSection(seccion);
        //}
        //private static Dictionary<string, string> GetDiccionary(String seccion)
        //{
        //    return (Dictionary<string, string>)ConfigurationManager.GetSection(seccion);
        //}
    }
}
