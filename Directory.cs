using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            DirectoryPanel = DPanel;
            ButtonPanel = BPanel;
            CaseLocations = CaseDB;
            CaseNotes = notes;
            ExternalFileManager FileManager = new ExternalFileManager();
            DropdownNumbers = Numbers;
            DropdownDistricts = Districts;

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
                DirectoryButton LetterButton = new DirectoryButton();
                LetterButton.Text = Alphabet[i].ToString();
                LetterButton.Click += OnLetterButtonPressed;

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
                NotesLabel LocationLabel = new NotesLabel();

                LocationLabel.Text = RowElements[i];
                LocationLabel.Margin = new Padding(0, 0, 0, 0);
                LocationLabel.BorderStyle = BorderStyle.FixedSingle;
                LocationLabel.Dock = DockStyle.Top;
                LocationLabel.Click += LocationSelected;

                //Check if the RowElement[i] is a key in the names dictionary
                if (CurrentlyLetterDirectory.ContainsKey(RowElements[i]))
                {
                    //For the catagories, make them stand out
                    if (RowElements[i + 1] == "")
                    {
                        LocationLabel.BackColor = Color.Black;
                        LocationLabel.ForeColor = Color.White;
                    }

                    //If it is, assign the location ID
                    LocationLabel.SetLocationID(CurrentlyLetterDirectory[RowElements[i]]);
                    LocationLabel.SetLocationName(RowElements[i]);
                    DirectoryPanel.Controls.Add(LocationLabel, i, DirectoryPanel.RowCount);
                }
                else
                {
                    if (RowElements[i] == "")
                    {
                        //For the catagories, make them stand out
                        LocationLabel.BackColor = Color.Black;
                        LocationLabel.ForeColor = Color.White;
                    }

                    LocationLabel.SetLocationID(RowElements[i]);
                    LocationLabel.SetLocationName(RowElements[i - 1]);
                    DirectoryPanel.Controls.Add(LocationLabel, i, DirectoryPanel.RowCount);
                }
            }
            //Rowcount needs to be manually updated for later use
            DirectoryPanel.RowCount = DirectoryPanel.Controls.Count;
            DirectoryPanel.ResumeLayout();
        }

        void ClearDictonaryLayout()
        {
            DirectoryPanel.SuspendLayout();

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

            bindingSource.RemoveFilter();
            return Names.ToArray();
        }

        void LocationSelected(object sender, EventArgs e)
        {
            NotesLabel SelectedLabel = (NotesLabel)sender;
            string[] LocationInfo = CaseLocations.FindLocationArray(SelectedLabel.GetLocationID());

            if (LocationInfo.Length > 0)
            {
                CaseNotes.StoreNotes(LocationInfo);
            }
            else
            {
                List<string> IrrelevantLocationInfo = new List<string>();

                if (SelectedLabel.GetLocationID() != "")
                {
                    //Add the Name, the location and the info from the location and store it in the notes
                    IrrelevantLocationInfo.Add(SelectedLabel.GetLocationName());
                    IrrelevantLocationInfo.Add(SelectedLabel.GetLocationID());
                    IrrelevantLocationInfo.Add("You find nothing of interest");
                    CaseNotes.StoreNotes(IrrelevantLocationInfo.ToArray());
                }
            }
        }

        void OnLetterButtonPressed(object sender, EventArgs e)
        {
            DirectoryButton SelectedButton = (DirectoryButton)sender;

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
                    MessageBox.Show("This location does not exist");
                }      
            }
        }
    }
}

