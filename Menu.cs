using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    public partial class Menu : Form
    {
        private Intro IntroTab;
        private WorldMap SherlockMap;
        private Informants InformantsTab;
        private Notes NotesTab;
        private Directory DirectoryTab;
        private CaseLocationDatabase LocationFinder;
        private Newspaper NewspaperTab;
        private EndQuestions QuestionsTab;

        private string CurrentTabPage;

        public Menu()
        {
            InitializeComponent();

            //Create classes for the tabs
            IntroTab = new Intro(CaseIntroText, CaseTitle);
            SherlockMap = new WorldMap(WorldMapImage, WorldMap);
            LocationFinder = new CaseLocationDatabase();
            InformantsTab = new Informants(tableLayoutPanel3, LocationFinder);
            NotesTab = new Notes(dbLayoutPanel27, label35);
            DirectoryTab = new Directory(dbLayoutPanel30,dbLayoutPanel31, LocationFinder, NotesTab, NumbersComboBox, DistrictComboBox);
            NewspaperTab = new Newspaper(NewspaperFront, NewspaperBack);
            QuestionsTab = new EndQuestions(FirstSeriesQPAPanel, SecondSeriesQPA, FirstSeriesAnswers, SecondSeriesAnswers, NotesTab, PointsEarnedLabel);

            //Resize the tabs to fit properly
            Menu_Resize(null, null);
            CurrentTabPage = "Intro";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.ResizeBegin += (s, evt) => { this.SuspendLayout(); };
            this.ResizeEnd += (s, evt) => { this.ResumeLayout(true); };
        }

        private void WorldMap_Click(object sender, EventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            //If the map is selected, zoom in or out based on the current state
            if (SherlockMap != null && MouseButtonPressed.Button == MouseButtons.Left)
            {
                //Update the zoom bool
                SherlockMap.UpdateMouseZoom();
            }
        }

        private void WorldMap_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            //When the right mouse button is held down on the map, you can scroll until the edges of the map are reached
            if (SherlockMap != null && MouseButtonPressed.Button == MouseButtons.Right)
            {
                SherlockMap.DragMap(e.X, e.Y);
            }
        }

        private void WorldMap_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            //When the right mouse button is released, stop the drag state
            if (SherlockMap != null && MouseButtonPressed.Button == MouseButtons.Right)
            {
                SherlockMap.UpdateDragState(0, 0);
            }
        }

        private void WorldMap_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            //When the right mouse button is pressed, initiate the drag state
            if (SherlockMap != null && MouseButtonPressed.Button == MouseButtons.Right)
            {
                SherlockMap.UpdateDragState(e.X, e.Y);
            }
        }

        private void Menu_Resize(object sender, EventArgs e)
        {
            //Resize the tabs at the top based on the new width
            if(AllTabs.Width > 0)
            {
                AllTabs.ItemSize = new Size((AllTabs.Width / AllTabs.TabCount) - 2, 0);
            }     
        }

        private void Informants_Click(object sender, EventArgs e)
        {
            //When an informant is selected on the informant tab, retrieve the information and store it
            NotesTab.StoreNotes(InformantsTab.GetSelectedInformantInformation(sender, e));
        }

        private void DirectoryCatagoryButton_Click(object sender, EventArgs e)
        {
            DirectoryTab.OnCatagoryButtonPressed(sender, e);
        }

        private void ManualTravelButton_Click(object sender, EventArgs e)
        {
            DirectoryTab.OnManualTravelButtonPressed(sender, e);
        }

        private void NewspaperImage_Click(object sender, EventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            //If the map is selected, zoom in or out based on the current state
            if (NewspaperTab != null && MouseButtonPressed.Button == MouseButtons.Left)
            {
                PictureBox SelectedNewspaper = (PictureBox)sender;
                NewspaperTab.UpdateMouseZoom(SelectedNewspaper);
            }
        }

        private void NewspaperImage_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            //When the right mouse button is pressed, initiate the drag state
            if (NewspaperTab != null && MouseButtonPressed.Button == MouseButtons.Right)
            {
                PictureBox NewspaperImage = (PictureBox)sender;
                Panel NewspaperPanel = (Panel)NewspaperImage.Parent;

                NewspaperTab.UpdateDragState(e.X, e.Y, NewspaperPanel, NewspaperImage);
            }
        }

        private void NewspaperImage_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            //When the right mouse button is released, stop the drag state
            if (NewspaperTab != null && MouseButtonPressed.Button == MouseButtons.Right)
            {
                NewspaperTab.UpdateDragState(0, 0, null, null);
            }
        }

        private void NewspaperImage_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs MouseButtonPressed = (MouseEventArgs)e;
            //When the right mouse button is held down on the map, you can scroll until the edges of the map are reached
            if (NewspaperTab != null && MouseButtonPressed.Button == MouseButtons.Right)
            {
                NewspaperTab.DragMap(e.X, e.Y);
            }
        }

        private void AllTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(AllTabs.SelectedTab == AllTabs.TabPages["EndQuestions"])
            {
                if (!QuestionsTab.IsPlayerReady())
                {
                    //If the player says they are not ready, return to the tab they were on
                    AllTabs.SelectedTab = AllTabs.TabPages[CurrentTabPage];
                }
                else
                {
                    int x = AllTabs.TabCount - 1;
                    for (int i = 0; i < x; i++)
                    {
                        AllTabs.TabPages.RemoveAt(0);
                    }
                }
            }
            else
            {
                //Update what tab the player is at
                CurrentTabPage = AllTabs.SelectedTab.Name;
            }
        }

        private void QuestionSubmitButton_Click(object sender, EventArgs e)
        {
            Button SubButton = (Button)sender;
            SubButton.Enabled = false;
            QuestionsTab.SubmitAnswers();
        }
    }
}
