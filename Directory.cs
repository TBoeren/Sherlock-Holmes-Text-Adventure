using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class Directory
    {
        private DBLayoutPanel DirectoryPanel;
        private DBLayoutPanel ButtonPanel;
        private DataTable DirectoryDatatable = new DataTable();
        private DataTable DirectoryCatagoriesDatatable = new DataTable();
        private Dictionary<string, string> CurrentlyLetterDirectory = new Dictionary<string, string>();
        private CaseLocationDatabase CaseLocations;
        private Notes CaseNotes;
        private ComboBox DropdownNumbers;
        private ComboBox DropdownDistricts;

        public Directory(DBLayoutPanel DPanel, DBLayoutPanel BPanel, CaseLocationDatabase CaseDB, Notes notes, ComboBox Numbers, ComboBox Districts)
        {
            //Set all the required variables
            DirectoryPanel = DPanel;
            ButtonPanel = BPanel;
            CaseLocations = CaseDB;
            CaseNotes = notes;
            ExternalFileManager FileManager = new ExternalFileManager();
            DropdownNumbers = Numbers;
            DropdownDistricts = Districts;

            //Populate the button table and the manual travel comboboxes. Also set the data table
            SetupManualTravel();
            CreateLetterButtons();
            DirectoryDatatable = FileManager.ConvertCSVToDatatable("London Directory.csv");
            DirectoryCatagoriesDatatable = FileManager.ConvertCSVToDatatable("London Directory Catagories.csv");
        }

        private void SetupManualTravel()
        {
            //Add the numbers 1-100
            for (int i = 1; i <= 100; i++)
            {
                DropdownNumbers.Items.Add(i);
            }

            //Set the start index so the dropdowns aren't blank
            DropdownNumbers.SelectedIndex = 0;
            DropdownDistricts.SelectedIndex = 0;
        }

        void CreateLetterButtons()
        {
            ButtonPanel.SuspendLayout();
            char[] Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            for (int i = 0; i < Alphabet.Length; i++)
            {
                Button LetterButton = new Button();
                LetterButton.Margin = new Padding(0, 0, 0, 0);
                LetterButton.Dock = DockStyle.Fill;
                LetterButton.Text = Alphabet[i].ToString();
                LetterButton.Click += OnLetterButtonPressed;
                
                //Add the button to the correct column
                if (i > (Alphabet.Length / 2) - 1)
                {
                    ButtonPanel.Controls.Add(LetterButton, 1, (i - ButtonPanel.RowCount));
                }
                else
                {
                    ButtonPanel.Controls.Add(LetterButton, 0, i);
                }
            }
            ButtonPanel.ResumeLayout();
        }

        void UpdateDirectory(string[] RowElements)
        {
            DirectoryPanel.SuspendLayout();

            //add the controls
            for (int i = 0; i < RowElements.Length; i++)
            {
                //Check if the RowElement[i] is a key in the names dictionary
                if (CurrentlyLetterDirectory.ContainsKey(RowElements[i]))
                {
                    NotesLabel LocationLabel = new NotesLabel(RowElements[i], CurrentlyLetterDirectory[RowElements[i]]);
                    LocationLabel.SuspendLayout();
                    LocationLabel.Click += LocationSelected;
                    LocationLabel.Text = RowElements[i];

                    //For the catagories, make them stand out
                    if (RowElements[i + 1] == "")
                    {
                        LocationLabel.BackColor = Color.Black;
                        LocationLabel.ForeColor = Color.White;
                    }

                    //If it is, assign the location ID
                    DirectoryPanel.Controls.Add(LocationLabel, i, DirectoryPanel.RowCount);
                    LocationLabel.ResumeLayout(false);
                }
                else
                {
                    NotesLabel LocationLabel = new NotesLabel(RowElements[i - 1], RowElements[i]);
                    LocationLabel.SuspendLayout();
                    LocationLabel.Click += LocationSelected;
                    LocationLabel.Text = RowElements[i];

                    if (RowElements[i] == "")
                    {
                        //For the catagories, make them stand out
                        LocationLabel.BackColor = Color.Black;
                        LocationLabel.ForeColor = Color.White;
                    }

                    DirectoryPanel.Controls.Add(LocationLabel, i, DirectoryPanel.RowCount);
                    LocationLabel.ResumeLayout(false);
                }
            }
            //Rowcount needs to be manually updated for later use
            DirectoryPanel.RowCount = DirectoryPanel.Controls.Count;
            DirectoryPanel.ResumeLayout();
        }

        void ClearDictonaryLayout()
        {
            DirectoryPanel.SuspendLayout();

            //Clear the layout panel except for the first entry
            int X = 1;
            while (X <= DirectoryPanel.RowCount - 2)
            {

                Control c = DirectoryPanel.Controls[DirectoryPanel.RowCount - 1];
                DirectoryPanel.Controls.Remove(c);
                c.Dispose();
                DirectoryPanel.RowCount--;
            }

            //Update the autoscroll
            DirectoryPanel.AutoScroll = false;
            DirectoryPanel.ResumeLayout(false);
            DirectoryPanel.PerformLayout();
            DirectoryPanel.AutoScroll = true;
        }

        string[] GetNamesWithLetter(string Letter)
        {
            List<string> Names = new List<string>();
            BindingSource DatatableBindingSource = new BindingSource();
            DatatableBindingSource.DataSource = DirectoryDatatable;

            //Clear the currently selected letter dictionary
            CurrentlyLetterDirectory.Clear();

            DatatableBindingSource.Filter = "name like" + "'" + Letter + "%'";
            foreach (var item in DatatableBindingSource)
            {
                DataRowView Row = (DataRowView)item;
                CurrentlyLetterDirectory.Add(Row["name"].ToString(), Row["address"].ToString());

                Names.Add(Row["name"].ToString());
                Names.Add(Row["address"].ToString());
            }

            DatatableBindingSource.RemoveFilter();
            return Names.ToArray();
        }

        string[] GetNamesWithLocation(string Location)
        {
            List<string> Names = new List<string>();
            BindingSource DatatableBindingSource = new BindingSource();
            DatatableBindingSource.DataSource = DirectoryDatatable;

            DatatableBindingSource.Filter = "address like" + "'" + Location + "%'";
            if (DatatableBindingSource.Count > 0)
            {
                DataRowView Row = (DataRowView)DatatableBindingSource[0];
                Names.Add(Row["name"].ToString());
                Names.Add(Row["address"].ToString());
                Names.Add("You find nothing of interest");
                DatatableBindingSource.RemoveFilter();
                return Names.ToArray();
            }
            else
            {
                DatatableBindingSource.RemoveFilter();
                return new string[0];
            }
        }

        string[] GetDirectoryCatagories()
        {
            List<string> Names = new List<string>();
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = DirectoryCatagoriesDatatable;

            //Clear the currently selected letter dictionary
            CurrentlyLetterDirectory.Clear();

            foreach (var item in bindingSource)
            {
                DataRowView Row = (DataRowView)item;
                CurrentlyLetterDirectory.Add(Row["name"].ToString(), Row["address"].ToString());

                Names.Add(Row["name"].ToString());
                Names.Add(Row["address"].ToString());
            }

            return Names.ToArray();
        }

        void LocationSelected(object sender, EventArgs e)
        {
            NotesLabel SelectedLabel = (NotesLabel)sender;
            string[] LocationInfo = CaseLocations.FindLocationArray(SelectedLabel.LocationID);

            //Check if the selected location is relevant to the current case. If so, add it!
            if (LocationInfo.Length > 0)
            {
                CaseNotes.StoreNotes(LocationInfo);
            }
            else
            {
                //If it is irrelevant, note the location and that it is irrelevant to the case
                List<string> IrrelevantLocationInfo = new List<string>();

                if (SelectedLabel.LocationID != "")
                {
                    //Add the Name, the location and the info from the location and store it in the notes
                    IrrelevantLocationInfo.Add(SelectedLabel.LocationName);
                    IrrelevantLocationInfo.Add(SelectedLabel.LocationID);
                    IrrelevantLocationInfo.Add("You find nothing of interest");
                    CaseNotes.StoreNotes(IrrelevantLocationInfo.ToArray());
                }
            }
        }

        void OnLetterButtonPressed(object sender, EventArgs e)
        {
            Button SelectedButton = (Button)sender;

            ClearDictonaryLayout();
            UpdateDirectory(GetNamesWithLetter(SelectedButton.Text));
        }

        public void OnCatagoryButtonPressed(object sender, EventArgs e)
        {
            ClearDictonaryLayout();
            UpdateDirectory(GetDirectoryCatagories());
        }

        public void OnManualTravelButtonPressed(object sender, EventArgs e)
        {
            //Get the set travel location
            string TravelLocation = (DropdownNumbers.Text + DropdownDistricts.Text);

            //Check the case locations if it is relevant. Otherwise check the dictionary
            string[] LocationInfo = CaseLocations.FindLocationArray(TravelLocation);

            if (LocationInfo.Length > 0)
            {
                CaseNotes.StoreNotes(LocationInfo);
            }
            else
            {
                string[] IrrelevantLocationInfo = GetNamesWithLocation(TravelLocation);
                if (IrrelevantLocationInfo.Length > 0)
                {
                    CaseNotes.StoreNotes(IrrelevantLocationInfo);
                }
                else
                {
                    MessageBox.Show("This location does not exist", "Note");
                }
            }
        }
    }
}

