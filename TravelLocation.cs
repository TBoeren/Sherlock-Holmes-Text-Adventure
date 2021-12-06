using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class TravelLocation : Form
    {
        private DBLayoutPanel dbLayoutPanel1;
        private System.ComponentModel.IContainer components;
        private Label label2;
        private Label label1;

        public TravelLocation(string LocationText, string LeadsFollowed)
        {
            InitializeComponent();

            label1.Text = LocationText;
            label2.Text = LeadsFollowed;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TravelLocation));
            this.dbLayoutPanel1 = new Sherlock_Holmes_Text_Adventure.DBLayoutPanel(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dbLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dbLayoutPanel1
            // 
            this.dbLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.dbLayoutPanel1.ColumnCount = 1;
            this.dbLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dbLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.dbLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.dbLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.dbLayoutPanel1.Name = "dbLayoutPanel1";
            this.dbLayoutPanel1.RowCount = 2;
            this.dbLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.dbLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.dbLayoutPanel1.Size = new System.Drawing.Size(1206, 628);
            this.dbLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("IM FELL English", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 471);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1200, 157);
            this.label2.TabIndex = 2;
            this.label2.Text = "“\"";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("IM FELL English", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1200, 471);
            this.label1.TabIndex = 1;
            this.label1.Text = "“\"";
            // 
            // TravelLocation
            // 
            this.BackgroundImage = global::Sherlock_Holmes_Text_Adventure.Properties.Resources._28355_old_paper_background_4500x3090_retina;
            this.ClientSize = new System.Drawing.Size(1206, 628);
            this.Controls.Add(this.dbLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TravelLocation";
            this.Text = "Location Information";
            this.dbLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
