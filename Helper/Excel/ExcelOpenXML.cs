using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Excel
{
    public static class ExcelOpenXML
    {
        public static void ToDataTable<T>(this IEnumerable<T> collection, string path)
        {
            DataTable dt = new DataTable("DataTable");
            Type t = typeof(T);
            PropertyInfo[] pia = t.GetProperties();

            //Inspect the properties and create the columns in the DataTable
            foreach (PropertyInfo pi in pia)
            {
                Type ColumnType = pi.PropertyType;
                if ((ColumnType.IsGenericType))
                {
                    ColumnType = ColumnType.GetGenericArguments()[0];
                }
                dt.Columns.Add(pi.Name, ColumnType);
            }

            //Populate the data table
            foreach (T item in collection)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                foreach (PropertyInfo pi in pia)
                {
                    if (pi.GetValue(item, null) != null)
                    {
                        dr[pi.Name] = pi.GetValue(item, null);
                    }
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            DataTableToExcel(dt, path);
        }
        public static void DataTableToExcel(DataTable dt, string path)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.SaveAs(path);
            }
        }
        public static void DataSetToExcel(DataSet dataSet, string filePath, bool overwiteFile = true)
        {
            try
            {
                if (System.IO.File.Exists(filePath) && overwiteFile)
                {
                    System.IO.File.Delete(filePath);
                }

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    try
                    {
                        DataTableToExcel(dataTable, filePath);
                    }
                    catch (Exception exTable)
                    {
                        Helper.LogHelper.GetInstance().PrintError(exTable);
                        throw exTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogHelper.GetInstance().PrintError(ex);
                throw ex;
            }

        }
    }
}
