using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class Notes
    {
        private Dictionary<string, string> LocationInformation = new Dictionary<string, string>();
        private DBLayoutPanel NotesPanel;
        private Label InformationTextLabel;

        public Notes(DBLayoutPanel panel, Label label)
        {
            NotesPanel = panel;
            InformationTextLabel = label;
        }

        public void StoreNotes(string[] RowElements)
        {
            //Check if you have already been to this location
            if (!LocationInformation.ContainsKey(RowElements[1]))
            {
                if (NotesPanel.ColumnCount != RowElements.Length - 1)
                {
                    throw new Exception("Elements number doesn't match!");
                }

                //increase panel rows count by one and add the visited locations to the dictionary
                NotesPanel.RowCount++;
                LocationInformation.Add(RowElements[1], RowElements[2]);

                //add the controls
                for (int i = 0; i < RowElements.Length - 1; i++)
                {
                    //Create a new label, assign a location ID to the label and add it to the cell
                    NotesLabel label = new NotesLabel();
                    label.Text = RowElements[i];
                    label.SetLocationID(RowElements[1]);
                    label.Click += ShowLocationInformation;
                    NotesPanel.Controls.Add(label, i, NotesPanel.RowCount - 1);
                }

                TravelLocation NewLocation = new TravelLocation(RowElements[2]);
                NewLocation.Show();
            }
            else
            {
                Console.WriteLine("You have already been here");
            }
        }

        void ShowLocationInformation(object sender, EventArgs e)
        {
            //When selecting the row, print the information of that location on the other split panel
            NotesLabel selectedLabel = (NotesLabel)sender;
            InformationTextLabel.Text = LocationInformation[selectedLabel.GetLocationID()];
        }
    }
}
