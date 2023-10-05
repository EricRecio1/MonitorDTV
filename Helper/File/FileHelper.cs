using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.File
{
    public class FileHelper
    {
        public static bool GuardarFileString(string ruta, string text)
        {
            bool result = false;
            try
            {
                System.IO.File.WriteAllText(ruta, text);
                result = true;
                Helper.LogHelper.GetInstance().PrintDebug("GuardarFileString log : " + ruta);
            }
            catch (Exception ex)
            {
                Helper.LogHelper.GetInstance().PrintError(ex);
            }
            return result;
        }
        public static bool GuardarFileXml(string ruta, DataSet data)
        {
            bool result = false;
            try
            {
                //string PathLog = Helper.ConfigurationHelper.GetValue("Configuration", "PathLog");
                //PathLog += DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_" + nroHDR + "" + ".ds";
                data.WriteXml(ruta);
                //Helper.Log.ExcelExport.DataSetToExcel(result, PathLog, true);
                result = true;
                Helper.LogHelper.GetInstance().PrintDebug("GuardarFileXml log : " + ruta);
            }
            catch (Exception ex)
            {
                Helper.LogHelper.GetInstance().PrintError(ex);
            }
            return result;
        }

    }
}
