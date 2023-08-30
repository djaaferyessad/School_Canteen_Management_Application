using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_APP
{
    public partial class loading : Form
    {
        public loading()
        {
            InitializeComponent();
        }
        private void loading_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        int startpoint = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            startpoint += 2;
            progressbar.Value = startpoint;
            if (progressbar.Value == 100)
            {
                timer1.Stop();
                progressbar.Value = 0;

                this.Hide();
                login loginForm = new login();
                loginForm.ShowDialog();
            }
        }


    }
}
