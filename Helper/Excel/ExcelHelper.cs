using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Helper
{
    public static class ExcelHelper
    {
        
        private static string GetConnectionString(string dataSource)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = dataSource;

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }
        public static void DataTabletoExcel(this DataTable dataTable, string filePath, bool overwiteFile = true)
        {
            if (System.IO.File.Exists(filePath) && overwiteFile)
            {
                System.IO.File.Delete(filePath);
            }

            var conn = GetConnectionString(filePath);
            using (OleDbConnection connection = new OleDbConnection(conn))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand())
                {
                    command.Connection = connection;

                    List<string> columnNames = new List<string>();
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        columnNames.Add(dataColumn.ColumnName);
                    }

                    string tableName = !string.IsNullOrWhiteSpace(dataTable.TableName) ? dataTable.TableName : Guid.NewGuid().ToString();
                    command.CommandText = $"CREATE TABLE [{tableName}] ({string.Join(",", columnNames.Select(c => $"[{c}] VARCHAR").ToArray())});";
                    command.ExecuteNonQuery();


                    foreach (DataRow row in dataTable.Rows)
                    {
                        List<string> rowValues = new List<string>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            rowValues.Add((row[column] != null && row[column] != DBNull.Value) ? row[column].ToString() : string.Empty);
                        }
                        command.CommandText = $"INSERT INTO [{tableName}]({string.Join(",", columnNames.Select(c => $"[{c}]"))}) VALUES ({string.Join(",", rowValues.Select(r => $"'{r.Replace("'", "''")}'").ToArray())});";
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }
        public static void DataSetToExcel(this DataSet dataSet, string filePath, bool overwiteFile = true)
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
                        DataTabletoExcel(dataTable, filePath, false);
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
        public static DataTable ExcelToDataTable(String FilePath, int MaxColumnToRead = 0)
        {
            DataTable dt = null;

            if (!System.IO.File.Exists(FilePath))
            {
                throw new Exception("El archivo especificado NO existe. "+ FilePath);
            }

            try
            {

                IWorkbook workbook = null;  //IWorkbook determina se es xls o xlsx              
                ISheet worksheet = null;
                string first_sheet_name = "";

                workbook = WorkbookFactory.Create(FilePath); //Abre tanto XLS como XLSX
                worksheet = workbook.GetSheetAt(0); //Obtener Hoja por indice
                try
                {
                    first_sheet_name = worksheet.SheetName;  //Obtener el nombre de la Hoja

                    dt = new DataTable(first_sheet_name);
                    dt.Rows.Clear();
                    dt.Columns.Clear();
                    try
                    {
                        Helper.LogHelper.GetInstance().PrintError("Se crean columnas del DataTable.");
                        //CreateColumns(ref dt, worksheet);
                        CreateColumns(ref dt, worksheet, MaxColumnToRead);

                        //AddData(ref dt, worksheet);
                        AddData(ref dt, worksheet, MaxColumnToRead);
                    }
                    catch (Exception ex)
                    {
                        Helper.LogHelper.GetInstance().PrintError(ex);
                    }
                    dt.AcceptChanges();
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    workbook.Close();
                    Helper.LogHelper.GetInstance().PrintError(ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Helper.LogHelper.GetInstance().PrintError(ex);
                throw ex;
            }
            return dt;
        }
        private static void CreateColumns(ref DataTable dt, ISheet worksheet, int MaxColumnToRead = 0)
        {

            IRow rowColumnName = worksheet.GetRow(0);
            IRow rowColumnType = worksheet.GetRow(1);

            //Leer cada celda de la fila
            int cellIndex = 0;


            foreach (ICell cell in rowColumnType.Cells)
            {
                try
                {
                    //ECC - Limita la lectura de columnas hasta la celda MaxColumnToRead o todas si no viene el parámetro
                    if(cell.ColumnIndex <= MaxColumnToRead || MaxColumnToRead == 0)
                    {

                        string cellType = "";
                        string nameColumn = rowColumnName.Cells[cellIndex].StringCellValue;

                        // ECC - se comenta la definicion de tipo de columna para tratar todos los datos como string
                        //
                        // Determino el tipo de dato de la celda:
                        //switch (cell.CellType)
                        //{
                        //    case CellType.Blank:
                        //        cellType = "System.String";
                        //        break;

                        //    case CellType.Boolean:
                        //        cellType = "System.Boolean";
                        //        break;
                        //    case CellType.String:
                        //        cellType = "System.String";
                        //        break;
                        //    case CellType.Numeric:
                        //        if (HSSFDateUtil.IsCellDateFormatted(cell))
                        //        {
                        //            cellType = "System.DateTime";
                        //        }
                        //        else
                        //        {
                        //            cellType = "System.Double";
                        //        }
                        //        break;
                        //    case CellType.Formula:
                        //        switch (cell.CachedFormulaResultType)
                        //        {
                        //            case CellType.Boolean:
                        //                cellType = "System.Boolean";
                        //                break;
                        //            case CellType.String:
                        //                cellType = "System.String";
                        //                break;
                        //            case CellType.Numeric:
                        //                if (HSSFDateUtil.IsCellDateFormatted(cell))
                        //                {
                        //                    cellType = "System.DateTime";
                        //                }
                        //                else
                        //                {
                        //                    cellType = "System.Double";
                        //                }
                        //                break;
                        //            default: // ECC
                        //                cellType = "System.String";
                        //                break;
                        //        }
                        //        break;
                        //    default:
                        //        cellType = "System.String";
                        //        break;
                        //}

                        DataColumn column = null;
                        // ECC tratamiento de todas las celdas como string
                        cellType = "System.String";
                        column = new DataColumn(nameColumn, System.Type.GetType(cellType));
                        dt.Columns.Add(column);

                        cellIndex++;
                    }
                    
                }
                catch (Exception e)
                {
                    LogHelper.GetInstance().PrintError(e);
                }
            }
        }
        private static void AddData(ref DataTable dt, ISheet worksheet, int MaxColumnToRead = 0)
        {
            bool isDataRow = false;
            try
            {
                Helper.LogHelper.GetInstance().PrintError("Se comienza a cargar linea por linea.");
                // Leer Fila por fila desde la primera
                for (int rowIndex = 1; rowIndex <= worksheet.LastRowNum; rowIndex++)
                {
                    try
                    {
                        Helper.LogHelper.GetInstance().PrintError("Inicio Linea: " + rowIndex.ToString());
                        DataRow NewReg = null;
                        IRow row = worksheet.GetRow(rowIndex);

                        // Salteo filas vacias
                        if (row == null)
                        {
                            continue;
                        }


                        NewReg = dt.NewRow();
                        isDataRow = true;


                        //Leer cada celda de la fila
                        int cellIndex = 0;

                        foreach (ICell cell in row.Cells)
                        {
                            //ECC- Limita la lectura de columnas hasta la celda MaxColumnToRead o todas si no viene el parámetro
                            if (cell.ColumnIndex <= MaxColumnToRead || MaxColumnToRead == 0)
                            {
                                Helper.LogHelper.GetInstance().PrintError("Obteneniendo Fila: "+ cell.RowIndex + " Columna: "+ cell.ColumnIndex);
                                object cellValue = null;

                                // Determina el tipo de dato de la celda:
                                switch (cell.CellType)
                                {
                                    case CellType.Blank:
                                        cellValue = string.Empty;
                                        break;

                                    case CellType.Boolean:
                                        cellValue = cell.BooleanCellValue;
                                        break;
                                    case CellType.String:
                                        cellValue = cell.StringCellValue;
                                        break;
                                    case CellType.Numeric:
                                        if (HSSFDateUtil.IsCellDateFormatted(cell))
                                        {
                                            cellValue = cell.DateCellValue;
                                        }
                                        else
                                        {
                                            cellValue = cell.NumericCellValue;
                                        }
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.Boolean:
                                                cellValue = cell.BooleanCellValue;
                                                break;
                                            case CellType.String:
                                                cellValue = cell.StringCellValue;
                                                break;
                                            case CellType.Numeric:
                                                if (HSSFDateUtil.IsCellDateFormatted(cell))
                                                {
                                                    cellValue = cell.DateCellValue;
                                                }
                                                else
                                                {
                                                    cellValue = cell.NumericCellValue;
                                                }
                                                break;
                                        }
                                        break;
                                    case CellType.Error:
                                        cellValue = string.Empty;
                                        break;
                                    default:
                                        try
                                        {
                                            cellValue = cell.StringCellValue;
                                        }
                                        catch (Exception e)
                                        {
                                            Helper.LogHelper.GetInstance().PrintError("Error en obtener tipo de valor. Fila: " + cell.RowIndex + " Columna: " + cell.ColumnIndex);
                                            LogHelper.GetInstance().PrintError(e);
                                            cellValue = string.Empty;
                                        }
                                        break;
                                }

                                // Agrega el dato de la celda en la columna del registro
                                try
                                {
                                        NewReg[cell.ColumnIndex] = cellValue;
                                }
                                catch (Exception e)
                                {
                                    Helper.LogHelper.GetInstance().PrintError("Error en crear nuevo registro. Fila: " + cell.RowIndex + " Columna: " + cell.ColumnIndex);
                                    LogHelper.GetInstance().PrintError(e);
                                }
                                cellIndex++;
                            }
 
                        }
                        if (isDataRow)
                        {
                            try
                            {
                                dt.Rows.Add(NewReg);
                            }
                            catch (Exception ex)
                            {
                                Helper.LogHelper.GetInstance().PrintError("Error en agregar nuevo registro." );
                                LogHelper.GetInstance().PrintError(ex);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        LogHelper.GetInstance().PrintError(ex);
                    }
                }//end for
                Helper.LogHelper.GetInstance().PrintError("Fin de carga Linea por linea.");
            }
            catch (Exception ex)
            {
                Helper.LogHelper.GetInstance().PrintError("Error al tratar de cargar DataTable.");
                LogHelper.GetInstance().PrintError(ex);
            }
        }
    }
}
