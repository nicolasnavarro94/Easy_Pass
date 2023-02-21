using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class alertForm : Form
    {
        private int count = 0;
        private double opacity = 0.2;
        private string _type = "";
        public alertForm(string message,string type)
        {
            InitializeComponent();
            Opacity = 0;
            txtMessage.Text = message;
            _type = type;
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Opacity+= opacity;
            if (Opacity == 1)
            {
                count++;
                if (count  == 5)
                {
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void alertForm_Load(object sender, EventArgs e)
        {
            this.PointToScreen(new Point(0,0));
            ActiveControl = panel1;

            switch (_type)
            {
                case "success":
                    pictureBox1.Image = Properties.Resources._059_success;
                    break;

                case "warning":
                    pictureBox1.Image = Properties.Resources._060_warning;
                    break;

                case "error":
                    pictureBox1.Image = Properties.Resources._058_error;
                    break;

                case "delete":
                    pictureBox1.Image = Properties.Resources._265_garbage;
                    break;

            }
        }
    }
}
