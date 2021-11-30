using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class TravelLocation : Form
    {
        private Label label1;

        public TravelLocation(string LocationText)
        {
            InitializeComponent();

            label1.Text = LocationText;
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("IM FELL English", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1206, 628);
            this.label1.TabIndex = 0;
            this.label1.Text = "“\"";
            // 
            // TravelLocation
            // 
            this.BackgroundImage = global::Sherlock_Holmes_Text_Adventure.Properties.Resources._28355_old_paper_background_4500x3090_retina;
            this.ClientSize = new System.Drawing.Size(1206, 628);
            this.Controls.Add(this.label1);
            this.Name = "TravelLocation";
            this.Text = "Location Information";
            this.ResumeLayout(false);

        }
    }
}
