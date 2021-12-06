using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class CaseLocationDatabase
    {
        private DataTable CaseLocationsDatatable = new DataTable();

        public CaseLocationDatabase()
        {
            //Get the locations for the case from the csv and port it to a datatable
            ExternalFileManager FilesManager = new ExternalFileManager();
            CaseLocationsDatatable = FilesManager.ConvertCSVToDatatable("SherlockLocations.csv");
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
