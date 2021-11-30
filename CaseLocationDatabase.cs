using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class CaseLocationDatabase
    {
        private DataTable CaseLocationsDatatable = new DataTable();

        public CaseLocationDatabase()
        {
            ConvertCSVToDatatable("D:/Thijs Huiswerk/Sherlock Holmes Text Adventures/Sherlock Holmes Text Adventure/Resources/SherlockLocations.csv", CaseLocationsDatatable);
        }

        void ConvertCSVToDatatable(string Filepath, DataTable DataTableToStore)
        {
            //Get the lines from the CSV
            string[] Lines = File.ReadAllLines(Filepath,Encoding.Default);
            string[] Fields;
            Fields = Lines[0].Split(new char[] { ';' });
            int Cols = Fields.GetLength(0);

            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
            {
                DataTableToStore.Columns.Add(Fields[i].ToLower(), typeof(string));
            }

            //Add the rows
            DataRow Row;
            for (int i = 1; i < Lines.GetLength(0); i++)
            {
                Fields = Lines[i].Split(new char[] { ';' });
                Row = DataTableToStore.NewRow();
                for (int f = 0; f < Cols; f++)
                {
                    Fields[f] = Fields[f].Replace("@", System.Environment.NewLine);
                    Row[f] = Fields[f];
                }
                DataTableToStore.Rows.Add(Row);
            }
        }

        public string[] FindLocationArray(string Location)
        {
            //Set the local variables
            List<string> LocationInfo = new List<string>();
            BindingSource DatatableBindingSource = new BindingSource();
            DatatableBindingSource.DataSource = CaseLocationsDatatable;

            //Use the location to find the row
            int Index = DatatableBindingSource.Find("Location", Location);
            if (Index > -1)
            {
                //if the row is found, add it to the list and return as a array
                DataRowView Row = (DataRowView)DatatableBindingSource[Index];
                LocationInfo.Add(Row["Inhabitant"].ToString());
                LocationInfo.Add(Row["Location"].ToString());
                LocationInfo.Add(Row["Information"].ToString());

                return LocationInfo.ToArray();
            }
            else
            {
                //This location is not part of the case
                return LocationInfo.ToArray();
            }
        }
    }
}
