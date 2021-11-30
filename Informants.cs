using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class Informants
    {
        private Dictionary<Point, string[]> InformantDictionary = new Dictionary<Point, string[]>();
        private DBLayoutPanel InformantsTable;
        private CaseLocationDatabase LocationFinder;

        public Informants(DBLayoutPanel InformantsLayoutTable, CaseLocationDatabase finder)
        {
            //Set the layout table for calculations
            InformantsTable = InformantsLayoutTable;
            LocationFinder = finder;

            //Create the dictionary which is used when the informants images are selected
            InformantDictionary.Add(new Point(0, 0), LocationFinder.FindLocationArray("5WC")); 
            InformantDictionary.Add(new Point(1, 0), LocationFinder.FindLocationArray("42NW"));
            InformantDictionary.Add(new Point(2, 0), LocationFinder.FindLocationArray("5SW"));
            InformantDictionary.Add(new Point(3, 0), LocationFinder.FindLocationArray("38EC"));
            InformantDictionary.Add(new Point(4, 0), LocationFinder.FindLocationArray("2SW"));
            InformantDictionary.Add(new Point(0, 1), LocationFinder.FindLocationArray("13SW"));
            InformantDictionary.Add(new Point(1, 1), LocationFinder.FindLocationArray("30EC"));
            InformantDictionary.Add(new Point(2, 1), LocationFinder.FindLocationArray("17WC"));
            InformantDictionary.Add(new Point(3, 1), LocationFinder.FindLocationArray("22SW"));
            InformantDictionary.Add(new Point(4, 1), LocationFinder.FindLocationArray("52EC"));
        }

        public string[] GetSelectedInformantInformation(object sender, EventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            Control SelectedInformant = (Control)sender;

            //Determine where the user clicked and use it to determine the selected informant
            Point ScreenLocation = SelectedInformant.PointToScreen(new Point(MouseButtonPressed.X, MouseButtonPressed.Y));
            Point RelativeLocation = InformantsTable.PointToClient(ScreenLocation);
            Point SelectedCell = new Point(RelativeLocation.X / (InformantsTable.Width / InformantsTable.ColumnCount), RelativeLocation.Y / (InformantsTable.Height / InformantsTable.RowCount));

            if (InformantDictionary.ContainsKey(SelectedCell))
            {
                //If the selected cell is found in the dictionary and pass the information to the notes
                return InformantDictionary[SelectedCell];
            }
            else
            {
                return null;
            }
        }
    }
}
