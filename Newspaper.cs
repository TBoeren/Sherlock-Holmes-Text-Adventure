using System.Drawing;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class Newspaper
    {
        PictureBox NewsPaperImage;
        Point DragStartCoordinates;
        Point ScrollPosition;
        Panel NewspaperPanel;

        private Image NewspaperFrontSmall;
        private Image NewspaperBackSmall;
        private Image NewspaperFront;
        private Image NewspaperBack;

        public Newspaper(PictureBox NewspaperFrontImage, PictureBox NewspaperBackImage)
        {
            //Get and set the images for the newspaper
            ExternalFileManager FileManager = new ExternalFileManager();

            NewspaperFront = Image.FromFile(FileManager.GetFileLocation("NewspaperFront.jpg"));
            NewspaperFrontSmall = Image.FromFile(FileManager.GetFileLocation("NewspaperFrontSmall.jpg"));
            NewspaperBack = Image.FromFile(FileManager.GetFileLocation("NewspaperBack.jpg"));
            NewspaperBackSmall = Image.FromFile(FileManager.GetFileLocation("NewspaperBackSmall.jpg"));

            NewspaperFrontImage.Image = NewspaperFrontSmall;
            NewspaperBackImage.Image = NewspaperBackSmall;
        }

        public void UpdateMouseZoom(PictureBox selectedImage)
        {
            //If the map is not zoomed in, zoom in. Otherwise, zoom out
            if (selectedImage.Tag as string == null || selectedImage.Tag as string == "")
            {
                selectedImage.Tag = "Zoomed";
                if (selectedImage.Name == "NewspaperFront")
                {
                    selectedImage.Image = NewspaperFront;
                }
                else
                {
                    selectedImage.Image = NewspaperBack;
                }
            }
            else
            {
                selectedImage.Tag = "";
                if (selectedImage.Name == "NewspaperFront")
                {
                    selectedImage.Image = NewspaperFrontSmall;
                }
                else
                {
                    selectedImage.Image = NewspaperBackSmall;
                }
            }
        }

        public void UpdateDragState(int DragX, int DragY, Panel SelectedImagePanel, PictureBox SelectedImage)
        {
            //Set the drag boolean and update the drag start coordinates
            DragStartCoordinates = new Point(DragX, DragY);
            NewspaperPanel = SelectedImagePanel;
            NewsPaperImage = SelectedImage;
        }

        public void DragMap(int DragX, int DragY)
        {
            //Calculate the position for the auto scroll
            ScrollPosition.X = Clamp((ScrollPosition.X + DragStartCoordinates.X - DragX), 0, NewsPaperImage.Size.Width);
            ScrollPosition.Y = Clamp((ScrollPosition.Y + DragStartCoordinates.Y - DragY), 0, NewsPaperImage.Size.Height);
            NewspaperPanel.AutoScrollPosition = ScrollPosition;
        }

        int Clamp(int val, int min, int max)
        {
            //Why is there no Math.Clamp :/
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}
