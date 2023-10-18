using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using OfficeOpenXml;

namespace IU
{
    public static class ExcelPackageExtensions
    {
        public static DataTable ToDataTable(this ExcelPackage package)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            //check if the worksheet is completely empty
            if (workSheet.Dimension == null)
            {
                return table;
            }

            //create a list to hold the column names
            List<string> columnNames = new List<string>();

            //needed to keep track of empty column headers
            int currentColumn = 1;

            //loop all columns in the sheet and add them to the datatable
            foreach (var cell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                string columnName = cell.Text.Trim();

                //check if the previous header was empty and add it if it was
                if (cell.Start.Column != currentColumn)
                {
                    columnNames.Add("Header_" + currentColumn);
                    table.Columns.Add("Header_" + currentColumn);
                    currentColumn++;
                }

                //add the column name to the list to count the duplicates
                columnNames.Add(columnName);

                //count the duplicate column names and make them unique to avoid the exception
                //A column named 'Name' already belongs to this DataTable
                int occurrences = columnNames.Count(x => x.Equals(columnName));
                if (occurrences > 1)
                {
                    columnName = columnName + "_" + occurrences;
                }

                //add the column to the datatable
                table.Columns.Add(columnName);

                currentColumn++;
            }

            //start adding the contents of the excel file to the datatable
            for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
            {
                var row = workSheet.Cells[i, 1, i, currentColumn - 1];
                DataRow newRow = table.NewRow();

                //loop all cells in the row
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }

                table.Rows.Add(newRow);
            }

            return table;
        

        //foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        //{
        //    table.Columns.Add(firstRowCell.Text);
        //}

        //for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //{
        //    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
        //    var newRow = table.NewRow();
        //    foreach (var cell in row)
        //    {
        //        newRow[cell.Start.Column - 1] = cell.Text;
        //    }
        //    table.Rows.Add(newRow);
        //}
        //return table;
    }
    }
}