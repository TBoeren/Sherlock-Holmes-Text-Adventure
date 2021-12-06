using System.Drawing;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    public partial class NotesLabel : Label
    {
        public string LocationID { get; }

        public string LocationName { get; }

        public NotesLabel(string name, string location)
        {
            InitializeComponent();
            LocationName = name;
            LocationID = location;

            Font = new Font("IM FELL English", 12);
            Margin = new Padding(0, 0, 0, 0);
            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Top;
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
