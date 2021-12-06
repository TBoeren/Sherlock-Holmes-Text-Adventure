using System;
using System.Data;
using System.IO;
using System.Text;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class ExternalFileManager
    {
        public DataTable ConvertCSVToDatatable(string FileName)
        {
            string Filepath = GetFileLocation(FileName);
            DataTable DatatableToStore = new DataTable();

            //Get the lines from the CSV
            string[] Lines = File.ReadAllLines(Filepath, Encoding.Default);
            string[] Fields;
            Fields = Lines[0].Split(new char[] { ';' });
            int Cols = Fields.GetLength(0);

            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
            {
                DatatableToStore.Columns.Add(Fields[i].ToLower(), typeof(string));
            }

            //Add the rows
            DataRow Row;
            for (int i = 1; i < Lines.GetLength(0); i++)
            {
                Fields = Lines[i].Split(new char[] { ';' });
                Row = DatatableToStore.NewRow();
                for (int f = 0; f < Cols; f++)
                {
                    //Add a new line in place of the @ for text formatting purposes
                    Fields[f] = Fields[f].Replace("@", Environment.NewLine);
                    Row[f] = Fields[f];
                }
                DatatableToStore.Rows.Add(Row);
            }

            return DatatableToStore;
        }

        public string GetFileLocation(string FileName)
        {
            string FullFilePath = AppContext.BaseDirectory + @"Resources\" + FileName;
            return FullFilePath;
        }

        public string ReadTextFile(string FileName)
        {
            string Filepath = GetFileLocation(FileName);
            return File.ReadAllText(Filepath, Encoding.Default);
        }
    }
}