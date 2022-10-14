using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AplicacionNomina
{
    public static class ReadExcelFast
    {
        public static DataTable ReadExcel(string path, int columnas = 0)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel._Worksheet)xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
            object[,] values = (object[,])xlRange.Value2;
            var dt = new DataTable();
            var totalColumnas = (columnas == 0) ? values.GetLength(1) : columnas;
            for (int i = 0; i < totalColumnas; i++)
            {
                dt.Columns.Add();
            }
            for (int i = 2; i <= values.GetLength(0); i++)
            {
                var currentValidate = 1;
                while (currentValidate <= totalColumnas)
                {
                    if (values[i, currentValidate] != null)
                    {
                        var row = values.GetRow(i, totalColumnas);
                        dt.Rows.Add(row);
                        break;
                    }
                    currentValidate++;
                }

            }
            return dt;
        }
        public static DataTable ReadExcelbk(string path, int columnas = 0)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel._Worksheet)xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
            object[,] values = (object[,])xlRange.Value2;
            var dt = new DataTable();
            var totalColumnas = (columnas == 0) ? values.GetLength(1) : columnas;
            for (int i = 0; i < totalColumnas; i++)
            {
                dt.Columns.Add();
            }
            for (int i = 2; i <= values.GetLength(0); i++)
            {
                if (values[i, 1] != null)
                {
                    var row = values.GetRow(i, totalColumnas);
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }
    }

    public static class ArrayExt
    {
        public static object[] GetRow(this object[,] array, int row, int colsMax = 0)
        {
            int cols = (colsMax == 0) ? array.GetUpperBound(1) : colsMax;
            object[] result = new object[cols];

            for (int i = 0; i < cols; i++)
            {
                result[i] = array[row, i + 1];
            }

            return result;
        }

    }
}
