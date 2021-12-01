using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class WorldMap
    {
        PictureBox MapImage;
        Point DragStartCoordinates;
        Point ScrollPosition;
        Panel MapPanel;

        public WorldMap(PictureBox Map, Panel panel)
        {
            MapImage = Map;
            MapPanel = panel;
        }

        public void UpdateMouseZoom()
        {
            //If the map is not zoomed in, zoom in. Otherwise, zoom out
            if (MapImage.Tag as string == null || MapImage.Tag as string == "")
            {
                MapImage.Tag = "Zoomed";
                MapImage.Image = Properties.Resources.MapZoomed;
            }
            else
            {
                MapImage.Tag = "";
                MapImage.Image = Properties.Resources.Map;
            }
        }

        public void UpdateDragState(int DragX, int DragY)
        {
            //Set the drag boolean and update the drag start coordinates
            DragStartCoordinates = new Point(DragX, DragY);
        }

        public void DragMap(int DragX, int DragY)
        {
            //Calculate the position for the auto scroll
            ScrollPosition.X = Clamp((ScrollPosition.X + DragStartCoordinates.X - DragX), 0, MapImage.Size.Width);
            ScrollPosition.Y = Clamp((ScrollPosition.Y + DragStartCoordinates.Y - DragY), 0, MapImage.Size.Height);
            MapPanel.AutoScrollPosition = ScrollPosition;
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
