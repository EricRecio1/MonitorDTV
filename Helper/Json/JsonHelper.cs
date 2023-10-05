using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Helper.Json
{
    public class JsonHelper
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            //MemoryStream ms = new MemoryStream();
            //ser.WriteObject(ms, obj);
            //string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            //ms.Close();
            ////Replace Json Date String
            //string p = @"\\/Date\((\d+)\+\d+\)\\/";
            //MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            //Regex reg = new Regex(p);
            //jsonString = reg.Replace(jsonString, matchEvaluator);
            //;
        }

        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            ////Convert "yyyy-MM-dd HH:mm:ss" String as "\/Date(1319266795390+0800)\/"
            //string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            //MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            //Regex reg = new Regex(p);
            //jsonString = reg.Replace(jsonString, matchEvaluator);
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            //T obj = ((T)ser.ReadObject(ms));
            //return obj;
            T obj = JsonConvert.DeserializeObject<T>(jsonString);
            return obj;
        }

        /// <summary>
        /// Convert Serialization Time /Date(1319266795390+0800) as String
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = String.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// Convert Date String as Json Time
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = String.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = String.Format(@"\/Date({0}+0800)\/", ts.TotalMilliseconds);
            return result;
        }
    }
}
