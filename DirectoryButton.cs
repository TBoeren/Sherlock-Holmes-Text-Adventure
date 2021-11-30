using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    public partial class DirectoryButton : Button
    {
        public DirectoryButton()
        {
            InitializeComponent();

            //Set the style
            Margin = new Padding(0, 0, 0, 0);
            Dock = DockStyle.Fill;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
