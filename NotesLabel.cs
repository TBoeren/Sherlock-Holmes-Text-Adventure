using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    public partial class NotesLabel : Label
    {
        private String LocationID;
        private String LocationName;
        public NotesLabel()
        {
            InitializeComponent();

            Font = new Font("IM FELL English", 12);
            Margin = new Padding(0, 0, 0, 0);
            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Top;
            this.DoubleBuffered = true;
        }

        public String GetLocationID()
        {
            return LocationID;
        }

        public void SetLocationID(String ID)
        {
            LocationID = ID;
        }

        public String GetLocationName()
        {
            return LocationName;
        }

        public void SetLocationName(String ID)
        {
            LocationName = ID;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
